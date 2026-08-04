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
    playerText.text = currentPlayer.Name;

    timerManager.ResetTimer();
  }

  void RenderRankings()
  {
    List<Player> players = GetSortedPlayersByTime();
    leaderBoardText.text = "";

    int position = 0;
    string ordinal = "";
    TimeSpan? previousDifference = null;
    string positionText;

    for (int i = 0; i < players.Count; i++)
    {
      if (players[i].HasPlayed)
      {
        if ((players[i].Time - tournamentData.TargetTime).Duration() != previousDifference)
        {
          position++;
          ordinal = GetOrdinal(position);
        }
        positionText = $"{position}<sup>{ordinal}</sup>";
      }
      else
      {
        positionText = "--";
      }

      leaderBoardText.text += $"{positionText} - {players[i].Name} {players[i].Time:ss\\:ff}s\n";
      previousDifference = (tournamentData.TargetTime - players[i].Time).Duration();
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
    return tournamentData.Players.OrderBy(player => (tournamentData.TargetTime - player.Time).Duration()).ToList();
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
