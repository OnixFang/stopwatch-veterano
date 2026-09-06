using System;

public class Player
{
  public string Name { get; set; }
  public TimeSpan Time { get; set; }
  public bool HasPlayed { get; set; }

  public Player(string name)
  {
    Name = name;
    Time = TimeSpan.Zero;
    HasPlayed = false;
  }
}
