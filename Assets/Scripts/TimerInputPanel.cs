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
    int seconds = (int)time.TotalSeconds;
    int centiseconds = time.Milliseconds / 10;

    timerText.text = $"{seconds:00}:{centiseconds:00}";
  }

  // public void AddSecond()
  // {
  //   if (Timer < TimeSpan.FromSeconds(99))
  //   {
  //     Timer += TimeSpan.FromSeconds(1);
  //     AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  //   }
  //   else
  //   {
  //     Debug.Log("Cannot increase timer");
  //   }
  // }

  // public void SubtractSecond()
  // {
  //   if (Timer > TimeSpan.FromSeconds(1))
  //   {
  //     Timer -= TimeSpan.FromSeconds(1);
  //     AudioManager.Instance.PlaySFX(SoundEffect.TimerClick);
  //   }
  //   else
  //   {
  //     Debug.Log("Cannot reduce timer");
  //   }
  // }
}