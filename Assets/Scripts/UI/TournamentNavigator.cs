using UnityEngine;

public class TournamentNavigator : MonoBehaviour
{
  [SerializeField] TournamentController tournamentController;

  [Header("Settings Panels")]
  [SerializeField] GameObject titleScreenPanel;
  [SerializeField] GameObject playerInputPanel;
  [SerializeField] GameObject timerInputPanel;

  [Header("Panel Groups")]
  [SerializeField] GameObject tournamentGamePanels;

  // Default screens when enabled
  void OnEnable()
  {
    // Enable first state of settings
    playerInputPanel.SetActive(true);
    timerInputPanel.SetActive(false);
    tournamentGamePanels.SetActive(false);
  }

  // Player Input Panel back button click handler
  public void PlayerInputBackHandler()
  {
    gameObject.SetActive(false);
    titleScreenPanel.SetActive(true);
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  // Player Input Panel configure time button click handler
  public void PlayerInputConfigureTimeHandler()
  {
    if (tournamentController.CanConfigureTime())
    {
      playerInputPanel.SetActive(false);
      timerInputPanel.SetActive(true);
      AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
    }
    else
    {
      Debug.Log("Insuficient players to play.");
      AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
    }
  }

  // Timer Input Panel back button click handler
  public void TimerInputBackHandler()
  {
    timerInputPanel.SetActive(false);
    playerInputPanel.SetActive(true);
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  // Timer Input Panel start game click handler
  public void TimerInputStarGameHandler()
  {
    if (tournamentController.CanStartGame())
    {
      timerInputPanel.SetActive(false);
      tournamentGamePanels.SetActive(true);
      AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
      tournamentController.StartTournament();
    }
    else
    {
      Debug.Log("Insuficient players or invalid timer.");
      AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
    }
  }

  public void StopwatchNewGameHandler()
  {
    tournamentGamePanels.SetActive(false);
    tournamentController.ResetSettings();
    playerInputPanel.SetActive(true);
  }
}
