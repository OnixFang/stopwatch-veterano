using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntry : MonoBehaviour
{
  [SerializeField] TMP_Text playerName;
  [SerializeField] Button buttonRemove;

  Player _player;

  public Player Player => _player;

  public event Action<PlayerEntry> RemovePlayerRequest;

  void Awake()
  {
    buttonRemove.onClick.AddListener(OnRemoveClicked);
  }

  public void SetPlayer(Player newPlayer)
  {
    _player = newPlayer;
    playerName.text = _player.Name;
  }

  void OnRemoveClicked()
  {
    RemovePlayerRequest?.Invoke(this);
  }

  public void DeactivateRemoveButton()
  {
    buttonRemove.gameObject.SetActive(false);
  }

  public void ActivateRemoveButton()
  {
    buttonRemove.gameObject.SetActive(true);
  }

  public bool IsPlayer(Player player)
  {
    return player == _player;
  }
}
