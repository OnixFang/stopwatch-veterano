using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerInputPanel : MonoBehaviour
{
  [SerializeField] TournamentController tournamentController;
  [SerializeField] TMP_Text timerText;
  [SerializeField] Button plusButton;
  [SerializeField] Button minusButton;

  public event Action<TimeSpan> UpdateTimerRequested;

  void Awake()
  {
    plusButton.onClick.AddListener(() =>
    {
      UpdateTimerRequested?.Invoke(TimeSpan.FromSeconds(1));
    });

    minusButton.onClick.AddListener(() =>
    {
      UpdateTimerRequested?.Invoke(TimeSpan.FromSeconds(-1));
    });
  }

  void OnEnable()
  {
    tournamentController.TimeChanged += RenderTimer;
    RenderTimer(tournamentController.Timer);
  }

  void OnDisable()
  {
    tournamentController.TimeChanged -= RenderTimer;
  }

  void RenderTimer(TimeSpan time)
  {
    timerText.text = TimeFormatter.Format(time);
  }
}