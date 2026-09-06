using System;
using System.Collections.Generic;
using UnityEngine;

public class RankingPanel : MonoBehaviour
{
  [SerializeField] RankingEntry rankingEntryPrefab;
  [SerializeField] Transform rankingList;

  string GetOrdinal(int position)
  {
    return position switch
    {
      1 => "st",
      2 => "nd",
      3 => "rd",
      _ => "th",
    };
  }

  public void RenderRankings(List<Player> players, TimeSpan targetTime)
  {
    for (int i = rankingList.childCount - 1; i >= 0; i--)
    {
      Destroy(rankingList.GetChild(i).gameObject);
    }

    int position = 0;
    string ordinal = "";
    TimeSpan? previousDifference = null;
    string ordinalPosition;

    for (int i = 0; i < players.Count; i++)
    {
      if ((players[i].Time - targetTime).Duration() != previousDifference)
      {
        position++;
        ordinal = GetOrdinal(position);
      }
      ordinalPosition = $"{position}<sup>{ordinal}</sup>";

      RankingEntry rankingEntry = Instantiate(rankingEntryPrefab, rankingList);
      rankingEntry.SetData(players[i], ordinalPosition);
      previousDifference = (targetTime - players[i].Time).Duration();
    }
  }
}
