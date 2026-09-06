using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopwatchController : MonoBehaviour
{
  [SerializeField] TournamentController tournamentController;
  [SerializeField] TMP_Text timerText;
  [SerializeField] Button stopwatchButton;
  [SerializeField] GameObject tipPanel;
  [SerializeField] TMP_Text objectiveText;
  [SerializeField] GameObject nextPlayerButton;
  [SerializeField] GameObject newGameButton;

  public event Action<TimeSpan> StopwatchStopped;
  public event Action NextPlayerRequested;

  float elapsedTime = 0f;
  TimeSpan normalizedTime = TimeSpan.Zero;
  bool isRunning = false;

  void OnEnable()
  {
    tournamentController.TournamentStarted += OnTournamentStarted;
    tournamentController.PlayerTurnStarted += OnPlayerTurnStarted;
    tournamentController.PlayerTurnFinished += OnPlayerTurnFinished;
    tournamentController.TournamentFinished += OnTournamentFinished;
  }

  void OnDisable()
  {
    tournamentController.TournamentStarted -= OnTournamentStarted;
    tournamentController.PlayerTurnStarted -= OnPlayerTurnStarted;
    tournamentController.PlayerTurnFinished -= OnPlayerTurnFinished;
    tournamentController.TournamentFinished -= OnTournamentFinished;
  }

  void Update()
  {
    if (isRunning)
    {
      elapsedTime += Time.deltaTime;
      normalizedTime = TimeSpan.FromSeconds(MathF.Round(elapsedTime, 2));
      timerText.text = TimeFormatter.Format(normalizedTime);
    }
  }

  public void ResetTimer()
  {
    timerText.text = TimeFormatter.Format(TimeSpan.Zero);
  }

  public void StartStopTimer()
  {
    if (!isRunning)
    {
      StartTimer();
    }
    else
    {
      StopTimer();
    }
    AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  }

  public void StartTimer()
  {
    elapsedTime = 0f;

    isRunning = true;
    tipPanel.SetActive(false);
  }

  public void StopTimer()
  {
    isRunning = false;
    stopwatchButton.interactable = false;
    StopwatchStopped?.Invoke(normalizedTime);
  }

  public void SetObjectiveText(string text)
  {
    objectiveText.text = text;
  }

  void OnTournamentStarted()
  {
    SetObjectiveText($"Objetivo\n{TimeFormatter.Format(tournamentController.Timer)}");
    newGameButton.SetActive(false);
    AudioManager.Instance.LowerMusic();
  }

  void OnPlayerTurnStarted(Player player)
  {
    ResetTimer();
    stopwatchButton.interactable = true;
    stopwatchButton.Select();
    tipPanel.SetActive(true);
  }

  void OnPlayerTurnFinished(Player player)
  {
    nextPlayerButton.SetActive(true);
  }

  void OnTournamentFinished()
  {
    newGameButton.SetActive(true);
    SetObjectiveText("Juego Terminado");
    AudioManager.Instance.RiseMusic();
  }

  public void NextPlayerClickHandler()
  {
    NextPlayerRequested?.Invoke();
    nextPlayerButton.SetActive(false);
    AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
  }
}
