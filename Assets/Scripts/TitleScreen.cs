using UnityEngine;

public class TitleScreen : MonoBehaviour
{
  [SerializeField] GameObject tournamentSettingsPanel;
  [SerializeField] GameObject creditsPanel;

  void Start()
  {
    Canvas.ForceUpdateCanvases();
    AudioManager.Instance.PlayMusic(Music.MainTheme);
  }

  public void PlayGame()
  {
    gameObject.SetActive(false);
    tournamentSettingsPanel.SetActive(true);
    AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
  }

  public void OpenCredits()
  {
    gameObject.SetActive(false);
    creditsPanel.SetActive(true);
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
