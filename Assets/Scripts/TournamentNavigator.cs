using UnityEngine;

public class TournamentNavigator : MonoBehaviour
{
  [SerializeField] TournamentController tournamentController;

  [Header("Settings Panels")]
  [SerializeField] GameObject titleScreenPanel;
  [SerializeField] GameObject playerInputPanel;
  [SerializeField] GameObject timerInputPanel;
  [SerializeField] PlayerListPanel playerListPanel;

  [Header("Panel Groups")]
  [SerializeField] GameObject tournamentGamePanels;
  [SerializeField] GameObject tournamentSettingsPanels;


  // Default screens when enabled
  void OnEnable()
  {
    // Enable first state of settings
    tournamentSettingsPanels.SetActive(true);
    playerListPanel.gameObject.SetActive(true);
    playerInputPanel.SetActive(true);

    timerInputPanel.SetActive(false);
    tournamentGamePanels.SetActive(false);
  }

  // Player Input Panel back button click handler
  public void ShowTitleScreenPanel()
  {
    gameObject.SetActive(false);
    titleScreenPanel.SetActive(true);
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  // Player Input Panel configure time button click handler
  public void ShowTimeInputPanel()
  {
    if (tournamentController.CanConfigureTime())
    {
      playerInputPanel.SetActive(false);
      timerInputPanel.SetActive(true);
      playerListPanel.DeactivateRemoveButtons();
      AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
    }
    else
    {
      Debug.Log("Insuficient players to play.");
      AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
    }
  }

  // Timer Input Panel back button click handler
  public void ShowPlayerInputPanel()
  {
    timerInputPanel.SetActive(false);
    playerInputPanel.SetActive(true);
    playerListPanel.ActivateRemoveButtons();
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  // Timer Input Panel start game click handler
  public void ShowTournamentGamePanels()
  {
    if (tournamentController.CanStartGame())
    {
      tournamentSettingsPanels.SetActive(false);
      tournamentGamePanels.SetActive(true);
      AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
      AudioManager.Instance.LowerMusic();
    }
    else
    {
      Debug.Log("Insuficient players or invalid timer.");
      AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
    }
  }
}
