using System;
using UnityEngine;

public class PlayerList : MonoBehaviour
{
  [SerializeField] Transform playerList;
  [SerializeField] PlayerEntry playerEntryPrefab;

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
