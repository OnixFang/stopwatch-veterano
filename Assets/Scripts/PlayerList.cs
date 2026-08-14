using System;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
  [SerializeField] Transform playerList;
  [SerializeField] PlayerEntry playerEntryPrefab;
  [SerializeField] RectTransform arrowIndicator;
  [SerializeField] Vector2 arrowOffset = new(20f, 0);

  public event Action<Player> RemovedPlayer;

  public void AddPlayer(Player player)
  {
    PlayerEntry playerEntry = Instantiate(playerEntryPrefab, playerList);
    playerEntry.SetPlayer(player);
    playerEntry.RemovePlayerRequest += RemovePlayer;
  }

  void RemovePlayer(PlayerEntry entry)
  {
    Destroy(entry.gameObject);
    RemovedPlayer?.Invoke(entry.Player);
  }

  public void MarkPlayer(Player player)
  {
    for (int i = 0; i < playerList.childCount; i++)
    {
      // Extract PlayerEntry from player
      PlayerEntry entry = playerList.GetChild(i).GetComponent<PlayerEntry>();

      if (entry.IsPlayer(player))
      {
        // Get prefab position from entry
        Vector2 entryPosition = entry.GetComponent<RectTransform>().position;

        // Display arrow if disabled
        if (!arrowIndicator.gameObject.activeSelf)
          arrowIndicator.gameObject.SetActive(true);

        // Move arrow to entry
        arrowIndicator.position = entryPosition + arrowOffset;
        break;
      }
    }
  }

  public void RemoveMarker()
  {
    arrowIndicator.gameObject.SetActive(false);
  }

  public void ResetList()
  {
    for (int i = playerList.childCount - 1; i >= 0; i--)
    {
      Destroy(playerList.GetChild(i).gameObject);
    }
  }

  public void ActivateRemoveButtons()
  {
    for (int i = playerList.childCount - 1; i >= 0; i--)
    {
      playerList.GetChild(i).GetComponent<PlayerEntry>().ActivateRemoveButton();
    }
  }

  public void DeactivateRemoveButtons()
  {
    for (int i = playerList.childCount - 1; i >= 0; i--)
    {
      playerList.GetChild(i).GetComponent<PlayerEntry>().DeactivateRemoveButton();
    }
  }
}
