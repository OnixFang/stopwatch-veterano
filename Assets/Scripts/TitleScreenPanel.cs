using UnityEngine;

public class TitleScreenPanel : MonoBehaviour
{
  [SerializeField] GameObject tournamentMode;
  [SerializeField] GameObject creditsPanel;

  void Start()
  {
    Canvas.ForceUpdateCanvases();
    AudioManager.Instance.PlayMusic(Music.MainTheme);
  }

  public void PlayGame()
  {
    gameObject.SetActive(false);
    tournamentMode.SetActive(true);
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
