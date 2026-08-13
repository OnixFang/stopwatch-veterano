using UnityEngine;

public class TitleScreen : MonoBehaviour
{
  [SerializeField] GameObject tournamentSettingsPanel;

  public void PlayGame()
  {
    gameObject.SetActive(false);
    tournamentSettingsPanel.SetActive(true);
  }
}
