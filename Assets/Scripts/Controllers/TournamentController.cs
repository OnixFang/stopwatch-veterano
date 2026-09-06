using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TournamentController : MonoBehaviour
{
  [SerializeField] PlayerInputPanel playerInputPanel;
  [SerializeField] TimerInputPanel timerInputPanel;
  [SerializeField] PlayerListPanel playerListPanel;
  [SerializeField] RankingPanel rankingPanel;
  [SerializeField] StopwatchController stopwatchController;

  // Constants
  static private readonly TimeSpan INITIAL_TIMER = TimeSpan.FromSeconds(3);
  static private readonly TimeSpan MAX_TIMER = TimeSpan.FromSeconds(99);

  // Tournament data
  readonly List<Player> players = new();
  TimeSpan _timer = INITIAL_TIMER;
  public TimeSpan Timer { get => _timer; private set => _timer = value; }

  // Game State
  Player currentPlayer;
  int currentPlayerIndex;

  // Settings Events
  public event Action<Player> PlayerAdded;
  public event Action<Player> PlayerRemoved;
  public event Action<TimeSpan> TimeChanged;
  public event Action SettingsReset;

  // Tournament Events
  public event Action TournamentStarted;
  public event Action<Player> PlayerTurnStarted;
  public event Action<Player> PlayerTurnFinished;
  public event Action TournamentFinished;

  void Awake()
  {
    playerInputPanel.CreatePlayerRequested += AddPlayer;
    playerListPanel.RemovePlayerRequested += RemovePlayer;
    timerInputPanel.UpdateTimerRequested += UpdateTimer;
    stopwatchController.StopwatchStopped += OnStopwatchStopped;
    stopwatchController.NextPlayerRequested += ChangePlayer;
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
  public void StartTournament()
  {
    TournamentStarted?.Invoke();
    currentPlayerIndex = 0;
    ChangePlayer();
    RenderRankings();
  }

  public void OnStopwatchStopped(TimeSpan elapsedTime)
  {
    currentPlayer.Time = elapsedTime;
    currentPlayer.HasPlayed = true;
    RenderRankings();
    TournamentFinishedCheck();
  }

  void TournamentFinishedCheck()
  {
    // Increase player list index
    currentPlayerIndex++;
    // Is there a player in queue?
    if (currentPlayerIndex < players.Count)
    {
      PlayerTurnFinished?.Invoke(currentPlayer);
    }
    else
    {
      TournamentFinished?.Invoke();
      Debug.Log("Tournament Finished!");
    }
  }

  void ChangePlayer()
  {
    currentPlayer = players[currentPlayerIndex];
    PlayerTurnStarted?.Invoke(currentPlayer);
  }

  void RenderRankings()
  {
    List<Player> sortedPlayers = players.FindAll(player => player.HasPlayed).OrderBy(player => (Timer - player.Time).Duration()).ToList();
    rankingPanel.RenderRankings(sortedPlayers, Timer);
  }

  public void ResetSettings()
  {
    players.Clear();
    Timer = INITIAL_TIMER;
    SettingsReset?.Invoke();
  }
}
