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

  public void RecordPlayerTime(float elapsedTime)
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
    List<Player> sortedplayerList = GetSortedPlayersByTime();
    leaderBoardText.text = "";

    sortedplayerList.ForEach(player =>
    {
      TimeSpan time = TimeSpan.FromSeconds(player.Time);
      leaderBoardText.text += $"{player.Name} {time:ss\\:ff}s\n";
    });
  }

  List<Player> GetSortedPlayersByTime()
  {
    return tournamentData.Players.OrderBy(player => Math.Abs(tournamentData.TargetSeconds - player.Time)).ToList();
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
