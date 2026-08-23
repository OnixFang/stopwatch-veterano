using System;
using System.Collections.Generic;
using UnityEngine;

public class TournamentController : MonoBehaviour
{
  [SerializeField] PlayerInputPanel playerInputPanel;
  [SerializeField] GameObject timerInputPanel;
  [SerializeField] PlayerListPanel playerListPanel;

  // Tournament data
  readonly List<Player> players = new();
  TimeSpan timer = TimeSpan.FromSeconds(3); // default time is 3 seconds

  // Events
  public event Action<Player> PlayerAdded;
  public event Action<Player> PlayerRemoved;
  public event Action<TimeSpan> TimeChanged;
  public event Action SettingsReset;

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
      AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
      return;
    }

    foreach (var savedPlayer in players)
    {
      if (savedPlayer.Name.ToLower() == name.ToLower())
      {
        Debug.Log("Cannot create player: Duplicate player name.");
        AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
        return;
      }
    }

    Player player = new(name);
    players.Add(player);
    PlayerAdded?.Invoke(player);
    AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
  }

  void RemovePlayer(Player player)
  {
    players.Remove(player);
    PlayerRemoved?.Invoke(player);
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  // Navigation validation
  public bool CanConfigureTime()
  {
    return players.Count > 1;
  }

  public bool CanStartGame()
  {
    return players.Count > 1 && timer > TimeSpan.Zero;
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

    SettingsReset?.Invoke();
  }
}
