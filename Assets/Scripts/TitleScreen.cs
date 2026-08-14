using UnityEngine;

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
    AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
  }

  public void ExitGame()
  {
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
#if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
  }
}
