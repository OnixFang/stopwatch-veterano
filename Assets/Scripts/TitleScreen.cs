using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TitleScreen : MonoBehaviour
{
  [SerializeField] GameObject tournamentSettingsPanel;

  void Start()
  {
    Canvas.ForceUpdateCanvases();
  }

  public void PlayGame()
  {
    gameObject.SetActive(false);
    tournamentSettingsPanel.SetActive(true);
  }
}
