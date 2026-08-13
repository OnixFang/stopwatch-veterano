using System;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
  [SerializeField] TMP_Text timerText;
  [SerializeField] TournamentMode tournamentMode;
  [SerializeField] GameObject tipPanel;

  float elapsedTime = 0f;
  TimeSpan normalizedTime = TimeSpan.Zero;
  bool isRunning = false;

  void Update()
  {
    if (isRunning)
    {
      elapsedTime += Time.deltaTime;
      normalizedTime = TimeSpan.FromSeconds(MathF.Round(elapsedTime, 2));
      UpdateTimerText(normalizedTime);
    }
  }

  void UpdateTimerText(TimeSpan time)
  {
    int seconds = (int)time.TotalSeconds;
    int centiseconds = time.Milliseconds / 10;

    timerText.text = $"{seconds:00}:{centiseconds:00}";
  }

  public void ResetTimer()
  {
    timerText.text = TimeSpan.Zero.ToString(@"ss\:ff");
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
    tipPanel.SetActive(false);
  }

  public void StopTimer()
  {
    isRunning = false;

    tournamentMode.RecordPlayerTime(normalizedTime);
  }
}
