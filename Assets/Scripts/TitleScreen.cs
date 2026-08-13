using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
  [SerializeField] GameObject tournamentSettingsPanel;
  [SerializeField] InputActionReference navigateAction;
  [SerializeField] GameObject firstMenuObject;

  void Start()
  {
    Canvas.ForceUpdateCanvases();
  }

  void OnEnable()
  {
    EventSystem.current.SetSelectedGameObject(firstMenuObject);
    navigateAction.action.performed += OnNavigate;
  }

  void OnDisable()
  {
    navigateAction.action.performed -= OnNavigate;
  }

  void OnNavigate(InputAction.CallbackContext context)
  {
    Vector2 direction = context.ReadValue<Vector2>();

    if (direction.y == 0)
    {
      return;
    }

    StartCoroutine(ReselectFirstMenuObject());
  }

  IEnumerator ReselectFirstMenuObject()
  {
    yield return null;
    if (EventSystem.current.currentSelectedGameObject == null)
    {
      EventSystem.current.SetSelectedGameObject(firstMenuObject);
    }
  }

  public void PlayGame()
  {
    gameObject.SetActive(false);
    tournamentSettingsPanel.SetActive(true);
  }
}
