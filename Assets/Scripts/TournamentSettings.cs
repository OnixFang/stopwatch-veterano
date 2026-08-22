using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TournamentSettings : MonoBehaviour
{
  [SerializeField] GameObject titleScreenPanel;

  [Header("Player Input")]
  [SerializeField] GameObject playerInputPanel;
  [SerializeField] TMP_InputField addPlayerInput;
  [SerializeField] PlayerList playerList;

  [Header("Time Input")]
  [SerializeField] GameObject timeInputPanel;
  [SerializeField] TMP_Text timerText;

  [Header("Tournament Mode")]
  [SerializeField] TournamentMode tournamentPanel;

  // Tournament data
  readonly List<Player> players = new();
  TimeSpan _timer = TimeSpan.FromSeconds(3);
  TimeSpan Timer
  {
    get => _timer;
    set
    {
      _timer = value;
      RenderTimer();
    }
  }

  void Awake()
  {
    // Add addPlayer event to playerInput
    addPlayerInput.onSubmit.AddListener(text =>
    {
      AddPlayer(text);
      StartCoroutine(ActivatePlayerInput());
    });

    // Subscribe to player removal from player list
    playerList.RemovedPlayer += RemovePlayer;

    // Render initial timer
    RenderTimer();
  }

  void OnEnable()
  {
    StartCoroutine(ActivatePlayerInput());
  }

  IEnumerator ActivatePlayerInput()
  {
    yield return null;
    addPlayerInput.ActivateInputField();
  }

  // Player Input Panel
  void AddPlayer(string name)
  {
    string playerName = name.Trim();
    if (string.IsNullOrWhiteSpace(playerName) || players.Count >= 15)
    {
      Debug.Log("Player input empty, or max players reached.");
      return;
    }

    foreach (var savedPlayer in players)
    {
      if (savedPlayer.Name.ToLower() == playerName.ToLower())
      {
        Debug.Log("Player input empty, or max players reached.");
        return;
      }
    }

    Player player = new(playerName);
    players.Add(player);

    addPlayerInput.text = "";

    playerList.AddPlayer(player);
    AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
  }

  void RemovePlayer(Player player)
  {
    players.Remove(player);
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  // Timer Input Panel
  public void AddSecond()
  {
    if (Timer < TimeSpan.FromSeconds(99))
    {
      Timer += TimeSpan.FromSeconds(1);
      AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
    }
    else
    {
      Debug.Log("Cannot increase timer");
    }
  }

  public void SubtractSecond()
  {
    if (Timer > TimeSpan.FromSeconds(1))
    {
      Timer -= TimeSpan.FromSeconds(1);
      AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
    }
    else
    {
      Debug.Log("Cannot reduce timer");
    }
  }

  void RenderTimer()
  {
    int seconds = (int)Timer.TotalSeconds;
    int centiseconds = Timer.Milliseconds / 10;

    timerText.text = $"{seconds:00}:{centiseconds:00}";
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
      playerInputPanel.SetActive(false);
      timeInputPanel.SetActive(true);
      playerList.DeactivateRemoveButtons();
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
    timeInputPanel.SetActive(false);
    playerInputPanel.SetActive(true);
    playerList.ActivateRemoveButtons();
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  public void StartGame()
  {
    if (players.Count > 1 && Timer > TimeSpan.Zero)
    {
      TournamentData data = GetTournamentData();
      tournamentPanel.StartTournament(data);
      timeInputPanel.SetActive(false);
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
    return new(players, Timer);
  }

  public void ResetSettings()
  {
    players.Clear();
    Timer = TimeSpan.FromSeconds(3);

    playerList.ResetList();
  }
}
