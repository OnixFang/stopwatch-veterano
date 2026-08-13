using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TournamentMode : MonoBehaviour
{
  [SerializeField] TournamentSettings tournamentSettings;
  [SerializeField] GameObject playerInputPanel;
  [SerializeField] RankingPanel rankingPanel;
  [SerializeField] TimerManager timerManager;
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
    currentPlayer.HasPlayed = true;
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
    // playerText.text = currentPlayer.Name;

    timerManager.ResetTimer();
  }

  void RenderRankings()
  {
    List<Player> players = GetSortedPlayersByTime();

    rankingPanel.RenderRankings(players, tournamentData.TargetTime);
  }

  List<Player> GetSortedPlayersByTime()
  {
    return tournamentData.Players.FindAll(player => player.HasPlayed).OrderBy(player => (tournamentData.TargetTime - player.Time).Duration()).ToList();
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
    tournamentSettings.ResetSettings();
    backToSettingsButton.gameObject.SetActive(false);
    gameObject.SetActive(false);
    playerInputPanel.SetActive(true);
  }
}
