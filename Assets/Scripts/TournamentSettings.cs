using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TournamentSettings : MonoBehaviour
{
  [Header("Player Input")]
  [SerializeField] GameObject playerInputPanel;
  [SerializeField] TMP_InputField addPlayerInput;
  [SerializeField] PlayerEntry playerEntryPrefab;
  [SerializeField] Transform playerList;

  [Header("Time Input")]
  [SerializeField] GameObject timeInputPanel;
  [SerializeField] TMP_Text timerText;

  [Header("Tournament Mode")]
  [SerializeField] TournamentMode tournamentPanel;

  // Tournament data
  List<Player> players = new();
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
    // Add addPlayer event to playerInputq
    addPlayerInput.onSubmit.AddListener(text =>
    {
      AddPlayer(text);
      StartCoroutine(ActivatePlayerInput());
    });

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

  public void AddPlayer(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      Debug.Log("Player input empty, cannot create player.");
      return;
    }

    Player player = new(name);
    players.Add(player);

    addPlayerInput.text = "";

    PlayerEntry playerEntry = Instantiate(playerEntryPrefab, playerList);
    playerEntry.SetPlayer(player);
    playerEntry.RemovePlayerRequest += RemovePlayer;
  }

  public void RemovePlayer(Player player)
  {
    players.Remove(player);
  }

  public void AddSecond()
  {
    if (Timer < TimeSpan.FromSeconds(99))
    {
      Timer += TimeSpan.FromSeconds(1);
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

  public void ShowTimeInputPanel()
  {
    // Going from the first screen to next screen, needs confirmation on player count
    if (players.Count > 1)
    {
      playerInputPanel.SetActive(false);
      timeInputPanel.SetActive(true);
      DeactivateRemoveButtons();
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
    ActivateRemoveButtons();
  }

  public void StartGame()
  {
    if (players.Count > 1 && Timer > TimeSpan.Zero)
    {
      TournamentData data = GetTournamentData();
      tournamentPanel.StartTournament(data);
      timeInputPanel.gameObject.SetActive(false);
      tournamentPanel.gameObject.SetActive(true);
    }
    else
    {
      Debug.Log("Insuficient players or invalid timer.");
    }
  }

  public TournamentData GetTournamentData()
  {
    return new(players, Timer);
  }

  public void ResetSettings()
  {
    players.Clear();
    Timer = TimeSpan.FromSeconds(3);

    for (int i = playerList.childCount - 1; i >= 0; i--)
    {
      Destroy(playerList.GetChild(i).gameObject);
    }
  }

  void DeactivateRemoveButtons()
  {
    for (int i = playerList.childCount - 1; i >= 0; i--)
    {
      playerList.GetChild(i).GetComponent<PlayerEntry>().DeactivateRemoveButton();
    }
  }

  void ActivateRemoveButtons()
  {
    for (int i = playerList.childCount - 1; i >= 0; i--)
    {
      playerList.GetChild(i).GetComponent<PlayerEntry>().ActivateRemoveButton();
    }
  }
}
