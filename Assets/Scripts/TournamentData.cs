using System.Collections.Generic;
using System.Linq;

public class TournamentData
{
  public TournamentData(List<Player> players, int targetSeconds)
  {
    Players = players.ToList();
    TargetSeconds = targetSeconds;
  }

  public List<Player> Players { get; set; }
  public int TargetSeconds { get; set; }
}
