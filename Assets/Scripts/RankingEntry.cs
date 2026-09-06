using TMPro;
using UnityEngine;

public class RankingEntry : MonoBehaviour
{
  [SerializeField] TMP_Text positionText;
  [SerializeField] TMP_Text playerName;
  [SerializeField] TMP_Text time;

  public void SetData(Player player, string ordinalPosition)
  {
    positionText.text = ordinalPosition;
    playerName.text = player.Name;
    time.text = TimeFormatter.Format(player.Time);
  }
}
