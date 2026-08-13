using UnityEngine;
using UnityEngine.EventSystems;

public class MenuIndicator : MonoBehaviour
{
  [SerializeField] Vector2 offset = new(0f, 0f);
  [SerializeField] GameObject arrowIndicator;

  void Update()
  {
    // Fetch the currently navigated GameObject from the Event System
    GameObject selected = EventSystem.current.currentSelectedGameObject;

    // If nothing is selected, hide the arrow
    if (selected == null)
    {
      if (arrowIndicator.activeSelf)
      {
        arrowIndicator.SetActive(false);
      }
    }
    else
    {
      if (!arrowIndicator.activeSelf)
      {
        arrowIndicator.SetActive(true);
      }

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

    // corners[0] = bottom-left
    // corners[1] = top-left
    // corners[2] = top-right
    // corners[3] = bottom-right

    Vector3 leftCenter = (corners[0] + corners[1]) / 2f;

    arrowIndicator.GetComponent<RectTransform>().position = leftCenter + (Vector3)offset;
  }
}
