using System;

public class Player
{
  public string Name { get; set; }
  public TimeSpan Time { get; set; }

  public Player(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      throw new ArgumentException("Player name cannot be empty.", nameof(name));
    }

    Name = name;
  }
}
