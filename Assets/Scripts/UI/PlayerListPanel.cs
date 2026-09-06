using System;
using UnityEngine;

public class PlayerListPanel : MonoBehaviour
{
  [SerializeField] TournamentController tournamentController;
  [SerializeField] Transform playerList;
  [SerializeField] PlayerEntry playerEntryPrefab;
  [SerializeField] RectTransform arrowIndicator;
  [SerializeField] Vector2 arrowOffset = new(20f, 0);

  public event Action<Player> RemovePlayerRequested;

  void OnEnable()
  {
    tournamentController.PlayerAdded += OnPlayerAdded;
    tournamentController.PlayerRemoved += OnPlayerRemoved;
    tournamentController.PlayerTurnStarted += MarkPlayer;
    tournamentController.SettingsReset += OnSettingsReset;
  }

  void OnDisable()
  {
    tournamentController.PlayerAdded -= OnPlayerAdded;
    tournamentController.PlayerRemoved -= OnPlayerRemoved;
    tournamentController.PlayerTurnStarted -= MarkPlayer;
    tournamentController.SettingsReset -= OnSettingsReset;
  }

  void OnPlayerAdded(Player player)
  {
    PlayerEntry playerEntry = Instantiate(playerEntryPrefab, playerList);
    playerEntry.SetPlayer(player);
    playerEntry.RemovePlayerRequested += OnRemovePlayerRequested;
  }

  void OnRemovePlayerRequested(PlayerEntry entry)
  {
    Destroy(entry.gameObject);
    RemovePlayerRequested?.Invoke(entry.Player);
  }

  void OnPlayerRemoved(Player player)
  {
    for (int i = 0; i < playerList.childCount; i++)
    {
      PlayerEntry entry = playerList.GetChild(i).GetComponent<PlayerEntry>();

      if (entry.IsPlayer(player))
      {
        Destroy(entry.gameObject);
      }
    }
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

  void OnSettingsReset()
  {
    ResetList();
    RemoveMarker();
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
