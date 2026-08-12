using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
  [SerializeField] TMP_Text playerName;
  [SerializeField] Button buttonRemove;

  Player player;
  public event Action<Player> RemovePlayerRequest;

  void Awake()
  {
    buttonRemove.onClick.AddListener(OnRemoveClicked);
  }

  public void SetPlayer(Player newPlayer)
  {
    player = newPlayer;
    playerName.text = player.Name;
  }

  public string GetPlayerName()
  {
    return player.Name;
  }

  void OnRemoveClicked()
  {
    RemovePlayerRequest?.Invoke(player);
    Destroy(gameObject);
  }
}
