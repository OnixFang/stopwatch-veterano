using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
  [SerializeField] TMP_Text timerText;
  [SerializeField] TMP_Text startStopText;
  [SerializeField] TournamentMode tournamentMode;

  float elapsedTime = 0f;
  bool isRunning = false;

  void Update()
  {
    if (isRunning)
    {
      elapsedTime += Time.deltaTime;
      UpdateTimerText(elapsedTime);
    }
  }

  void UpdateTimerText(float seconds)
  {
    timerText.text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss\:ff");
  }

  public void ResetTimer()
  {
    timerText.text = TimeSpan.Zero.ToString(@"mm\:ss\:ff");
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
  }

  public void StartTimer()
  {
    elapsedTime = 0f;

    isRunning = true;
    startStopText.text = "Stop";
  }

  public void StopTimer()
  {
    isRunning = false;
    startStopText.text = "Start";

    tournamentMode.RecordPlayerTime(elapsedTime);
  }
}
