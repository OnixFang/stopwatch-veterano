using System;
using System.Collections.Generic;
using UnityEngine;

public class TournamentController : MonoBehaviour
{
  [SerializeField] GameObject titleScreenPanel;
  [SerializeField] PlayerInputPanel playerInputPanel;
  [SerializeField] GameObject timerInputPanel;
  [SerializeField] PlayerListPanel playerListPanel;

  [SerializeField] TournamentMode tournamentPanel;

  // Tournament data
  readonly List<Player> players = new();
  TimeSpan timer = TimeSpan.FromSeconds(3); // default time is 3 seconds

  // Events
  public event Action<Player> PlayerAdded;
  public event Action<Player> PlayerRemoved;
  public event Action<TimeSpan> TimeChanged;

  void Awake()
  {
    playerInputPanel.CreatePlayerRequested += AddPlayer;
    playerListPanel.RemovePlayerRequested += RemovePlayer;
  }

  void AddPlayer(string name)
  {
    if (players.Count >= 15)
    {
      Debug.Log("Cannot create player: Max players reached.");
      return;
    }

    foreach (var savedPlayer in players)
    {
      if (savedPlayer.Name.ToLower() == name.ToLower())
      {
        Debug.Log("Cannot create player: Duplicate player name.");
        return;
      }
    }

    Player player = new(name);
    players.Add(player);
    PlayerAdded?.Invoke(player);
  }

  void RemovePlayer(Player player)
  {
    players.Remove(player);
    PlayerRemoved?.Invoke(player);
  }

  // Navigation
  public void ShowTitleScreenPanel()
  {
    gameObject.SetActive(false);
    titleScreenPanel.SetActive(true);
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  public void ShowTimeInputPanel()
  {
    // Going from the first screen to next screen, needs confirmation on player count
    if (players.Count > 1)
    {
      playerInputPanel.gameObject.SetActive(false);
      timerInputPanel.SetActive(true);
      playerListPanel.DeactivateRemoveButtons();
      AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
    }
    else
    {
      Debug.Log("Insuficient players to play.");
    }
  }

  public void ShowPlayerEntryPanel()
  {
    // Going back from second screen to first screen, no validation needed
    timerInputPanel.SetActive(false);
    playerInputPanel.gameObject.SetActive(true);
    playerListPanel.ActivateRemoveButtons();
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  public void StartGame()
  {
    if (players.Count > 1 && timer > TimeSpan.Zero)
    {
      TournamentData data = GetTournamentData();
      tournamentPanel.StartTournament(data);
      timerInputPanel.SetActive(false);
      tournamentPanel.gameObject.SetActive(true);
      AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
      AudioManager.Instance.LowerMusic();
    }
    else
    {
      Debug.Log("Insuficient players or invalid timer.");
    }
  }

  // Gameplay
  public TournamentData GetTournamentData()
  {
    return new(players, timer);
  }

  public void ResetSettings()
  {
    players.Clear();
    timer = TimeSpan.FromSeconds(3);

    playerListPanel.ResetList();
  }
}
