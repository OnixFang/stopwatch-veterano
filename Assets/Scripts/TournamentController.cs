using System;
using System.Collections.Generic;
using UnityEngine;

public class TournamentController : MonoBehaviour
{
  [SerializeField] PlayerInputPanel playerInputPanel;
  [SerializeField] TimerInputPanel timerInputPanel;
  [SerializeField] PlayerListPanel playerListPanel;

  // Constants
  static private readonly TimeSpan INITIAL_TIMER = TimeSpan.FromSeconds(3);
  static private readonly TimeSpan MAX_TIMER = TimeSpan.FromSeconds(99);

  // Tournament data
  readonly List<Player> players = new();
  TimeSpan _timer = INITIAL_TIMER;
  public TimeSpan Timer { get => _timer; private set => _timer = value; }

  // Events
  public event Action<Player> PlayerAdded;
  public event Action<Player> PlayerRemoved;
  public event Action<TimeSpan> TimeChanged;
  public event Action SettingsReset;

  void Awake()
  {
    playerInputPanel.CreatePlayerRequested += AddPlayer;
    playerListPanel.RemovePlayerRequested += RemovePlayer;
    timerInputPanel.UpdateTimerRequested += UpdateTimer;
  }

  // Player Input
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

  // Timer Input
  void UpdateTimer(TimeSpan timeAmount)
  {
    TimeSpan newTimer = Timer + timeAmount;

    if (newTimer <= TimeSpan.Zero || newTimer > MAX_TIMER)
      return;

    Timer += timeAmount;
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
    TimeChanged?.Invoke(Timer);
  }

  // Navigation validation
  public bool CanConfigureTime()
  {
    return players.Count > 1;
  }

  public bool CanStartGame()
  {
    return players.Count > 1 && Timer > TimeSpan.Zero;
  }

  // Gameplay
  public TournamentData GetTournamentData()
  {
    return new(players, Timer);
  }

  public void ResetSettings()
  {
    players.Clear();
    Timer = INITIAL_TIMER;

    SettingsReset?.Invoke();
  }
}
