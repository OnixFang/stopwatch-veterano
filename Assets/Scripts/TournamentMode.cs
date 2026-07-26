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

  TournamentData tournamentData;
  Player currentPlayer;
  int currentPlayerIndex;

  public void StartTournament(TournamentData data)
  {
    currentPlayerIndex = -1; // Set to -1 so ChangePlayer will set it to 0 on first run
    startStopButton.interactable = true;
    timerManager.ResetTimer();
    tournamentData = data;
    ChangePlayer();
    RenderRankings();
  }

  public void RecordPlayerTime(float elapsedTime)
  {
    currentPlayer.Time = elapsedTime;
    ChangePlayer();
    RenderRankings();
  }

  void ChangePlayer()
  {
    currentPlayerIndex++;
    if (currentPlayerIndex < tournamentData.Players.Count)
    {
      currentPlayer = tournamentData.Players[currentPlayerIndex];
      playerText.text = currentPlayer.Name;
    }
    else
    {
      FinishTournament();
    }
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

  void FinishTournament()
  {
    startStopButton.interactable = false;
    backToSettingsButton.gameObject.SetActive(true);
    Debug.Log("Tournament Finished!");
  }

  public void BackToSettings()
  {
    tournamentSettingsPanel.ResetSettings();
    tournamentSettingsPanel.gameObject.SetActive(true);
    backToSettingsButton.gameObject.SetActive(false);
    gameObject.SetActive(false);
  }

  void ResetTournament()
  {

  }
}
