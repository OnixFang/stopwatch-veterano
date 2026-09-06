using System;

public static class TimeFormatter
{
  public static string Format(TimeSpan time)
  {
    int seconds = (int)time.TotalSeconds;
    int centiseconds = time.Milliseconds / 10;

    return $"{seconds:00}:{centiseconds:00}";
  }
}