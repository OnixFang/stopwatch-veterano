using UnityEngine;

public class CreditsPanel : MonoBehaviour
{
  [SerializeField] GameObject titleScreenPanel;

  public void BackToTitleScreen()
  {
    gameObject.SetActive(false);
    titleScreenPanel.SetActive(true);
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }
}
