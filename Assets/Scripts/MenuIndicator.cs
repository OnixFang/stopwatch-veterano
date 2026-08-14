using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuIndicator : MonoBehaviour
{
  [SerializeField] Vector2 offset = new(0f, 0f);
  [SerializeField] GameObject arrowIndicator;
  [SerializeField] GameObject firstMenuObject;
  [SerializeField] InputActionReference navigateAction;

  bool initialized = false;
  GameObject currentSelected;

  void Update()
  {
    // Fetch the currently navigated GameObject from the Event System
    GameObject selected = EventSystem.current.currentSelectedGameObject;

    // If nothing is selected, hide the arrow
    if (selected == null)
    {
      if (arrowIndicator.activeSelf)
        arrowIndicator.SetActive(false);

      return;
    }

    if (selected != currentSelected && selected.GetComponent<Selectable>() != null)
    {
      currentSelected = selected;

      // Move the arrow to the active item's position
      MoveArrowToTarget(selected.GetComponent<RectTransform>());
    }
  }

  void MoveArrowToTarget(RectTransform buttonRect)
  {
    // Create an empty array of Vector3. The corners of an object is in Vector3 and are returned as 4 Vector3s
    Vector3[] corners = new Vector3[4];

    // Add the Vector3s to our array
    buttonRect.GetWorldCorners(corners);
    Vector3 leftCenter = (corners[0] + corners[1]) / 2f;

    // Ensure arrow indicator is visible
    if (!arrowIndicator.activeSelf)
      arrowIndicator.SetActive(true);

    arrowIndicator.GetComponent<RectTransform>().position = leftCenter + (Vector3)offset;

    // Play audio only after this has ben ran once
    if (initialized)
      AudioManager.Instance.PlaySFX(SoundEffect.MenuSelect);

    initialized = true;
  }

  void OnNavigate(InputAction.CallbackContext context)
  {
    Vector2 direction = context.ReadValue<Vector2>();

    if (direction.y == 0)
      return;

    StartCoroutine(ReselectFirstMenuObject());
  }

  IEnumerator ReselectFirstMenuObject()
  {
    yield return null;
    if (EventSystem.current.currentSelectedGameObject == null)
    {
      EventSystem.current.SetSelectedGameObject(firstMenuObject);
      if (currentSelected == firstMenuObject)
        MoveArrowToTarget(firstMenuObject.GetComponent<RectTransform>());
    }
  }

  void OnEnable()
  {
    EventSystem.current.SetSelectedGameObject(firstMenuObject);
    navigateAction.action.performed += OnNavigate;
  }

  void OnDisable()
  {
    initialized = false;
    navigateAction.action.performed -= OnNavigate;
  }
}
