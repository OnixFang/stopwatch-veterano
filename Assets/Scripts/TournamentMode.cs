using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TournamentMode : MonoBehaviour
{
  [SerializeField] TournamentSettings tournamentSettingsPanel;
  [SerializeField] TimerManager timerManager;
  [SerializeField] TMP_Text playerText;
  [SerializeField] TMP_Text leaderBoardText;
  [SerializeField] Button backToSettingsButton;
  [SerializeField] Button startStopButton;
  [SerializeField] Button nextPlayerButton;

  TournamentData tournamentData;
  Player currentPlayer;
  int currentPlayerIndex;

  public void StartTournament(TournamentData data)
  {
    currentPlayerIndex = 0;
    startStopButton.interactable = true;
    tournamentData = data;
    ChangePlayer();
    RenderRankings();
  }

  public void RecordPlayerTime(TimeSpan elapsedTime)
  {
    currentPlayer.Time = elapsedTime;
    RenderRankings();

    TournamentFinishedCheck();
  }

  public void HandleNextPlayer()
  {
    ChangePlayer();

    startStopButton.interactable = true;
    nextPlayerButton.gameObject.SetActive(false);
  }

  void ChangePlayer()
  {
    currentPlayer = tournamentData.Players[currentPlayerIndex];
    playerText.text = currentPlayer.Name;

    timerManager.ResetTimer();
  }

  void RenderRankings()
  {
    List<Player> players = GetSortedPlayersByTime();
    leaderBoardText.text = "";

    for (int i = 0; i < players.Count; i++)
    {
      int position = i + 1;
      string ordinal = GetOrdinal(position);
      leaderBoardText.text += $"{position}<sup>{ordinal}</sup> - {players[i].Name} {players[i].Time:ss\\:ff}s\n";
    }
  }

  string GetOrdinal(int position)
  {
    return position switch
    {
      1 => "st",
      2 => "nd",
      3 => "rd",
      _ => "th",
    };
  }

  List<Player> GetSortedPlayersByTime()
  {
    return tournamentData.Players.OrderBy(player => Math.Abs(tournamentData.TargetSeconds - player.Time.TotalSeconds)).ToList();
  }

  void TournamentFinishedCheck()
  {
    // Increase player list index
    currentPlayerIndex++;
    // Is there a player in queue?
    if (currentPlayerIndex < tournamentData.Players.Count)
    {
      startStopButton.interactable = false;
      nextPlayerButton.gameObject.SetActive(true);
    }
    else
    {
      FinishTournament();
    }
  }

  void FinishTournament()
  {
    startStopButton.interactable = false;
    backToSettingsButton.gameObject.SetActive(true);
    Debug.Log("Tournament Finished!");
  }

  public void BackToSettings()
  {
    tournamentSettingsPanel.ResetSettings();
    backToSettingsButton.gameObject.SetActive(false);
    gameObject.SetActive(false);
    tournamentSettingsPanel.gameObject.SetActive(true);
  }
}
