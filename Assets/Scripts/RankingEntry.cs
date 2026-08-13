using TMPro;
using UnityEngine;

public class RankingEntry : MonoBehaviour
{
  [SerializeField] TMP_Text positionText;
  [SerializeField] TMP_Text playerName;
  [SerializeField] TMP_Text time;

  public void SetData(Player player, string ordinalPosition)
  {
    int seconds = (int)player.Time.TotalSeconds;
    int centiseconds = player.Time.Milliseconds / 10;

    positionText.text = ordinalPosition;
    playerName.text = player.Name;
    time.text = $"{seconds:00}:{centiseconds:00}";
  }
}
