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

  List<Player> players = new();
  TimeSpan timer = TimeSpan.FromSeconds(3);

  void Awake()
  {
    addPlayerInput.onSubmit.AddListener(text =>
    {
      AddPlayer(text);
      StartCoroutine(ReactivatePlayerInput());
    });
  }

  IEnumerator ReactivatePlayerInput()
  {
    yield return null;
    addPlayerInput.ActivateInputField();
  }

  public void AddPlayer(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
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

  public void ShowTimeInputPanel()
  {
    // First screen, needs confirmation on player count
    if (players.Count > 1)
    {
      playerInputPanel.SetActive(false);
      timeInputPanel.SetActive(true);
    }
  }

  public void ShowPlayerEntryPanel()
  {
    // Second screen to go back, no validation needed
    timeInputPanel.SetActive(false);
    playerInputPanel.SetActive(true);
  }

  public void StartGame()
  {
    if (players.Count > 1 && timer != TimeSpan.Zero)
    {
      TournamentData data = GetTournamentData();
      tournamentPanel.StartTournament(data);
      tournamentPanel.gameObject.SetActive(true);
      gameObject.SetActive(false);
    }
  }

  public TournamentData GetTournamentData()
  {
    return new(players, timer);
  }

  public void ResetSettings()
  {
    players.Clear();
    timer = TimeSpan.FromSeconds(3);

    for (int i = playerList.childCount - 1; i >= 0; i--)
    {
      Destroy(playerList.GetChild(i).gameObject);
    }
  }
}
