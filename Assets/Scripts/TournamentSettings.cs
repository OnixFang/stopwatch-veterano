using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TournamentSettings : MonoBehaviour
{
  [SerializeField] TMP_InputField addPlayerInput;
  [SerializeField] TMP_InputField targetTimeInput;
  [SerializeField] PlayerEntry playerEntryPrefab;
  [SerializeField] Transform playerList;
  [SerializeField] TournamentMode tournamentPanel;

  List<Player> players = new();

  void Awake()
  {
    addPlayerInput.onSubmit.AddListener(text =>
    {
      AddPlayer(text);
      addPlayerInput.ActivateInputField();
    });
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

  public void StartGame()
  {
    if (players.Count > 1 && !string.IsNullOrWhiteSpace(targetTimeInput.text))
    {
      TournamentData data = GetTournamentData();
      tournamentPanel.StartTournament(data);
      tournamentPanel.gameObject.SetActive(true);
      gameObject.SetActive(false);
    }
  }

  public TournamentData GetTournamentData()
  {
    return new(players, TimeSpan.FromSeconds(int.Parse(targetTimeInput.text)));
  }

  public void ResetSettings()
  {
    players.Clear();
    targetTimeInput.text = "";
    // empty player list
  }
}
