using System;
using System.Collections.Generic;
using System.Linq;

public class TournamentData
{
  public TournamentData(List<Player> players, TimeSpan targetSeconds)
  {
    Players = players.ToList();
    TargetTime = targetSeconds;
  }

  public List<Player> Players { get; set; }
  public TimeSpan TargetTime { get; set; }
}
