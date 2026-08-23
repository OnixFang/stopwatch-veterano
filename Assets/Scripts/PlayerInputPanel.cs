using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInputPanel : MonoBehaviour
{
  [SerializeField] TournamentController tournamentController;
  [SerializeField] TMP_InputField addPlayerInput;
  [SerializeField] PlayerListPanel playerListPanel;

  public event Action<string> CreatePlayerRequested;

  void Awake()
  {
    // Add addPlayer event to playerInput
    addPlayerInput.onSubmit.AddListener(text =>
    {
      CreatePlayer(text);
      StartCoroutine(ActivatePlayerInput());
    });
  }

  void OnEnable()
  {
    tournamentController.PlayerAdded += OnPlayerAdded;
    StartCoroutine(ActivatePlayerInput());
  }

  private void OnDisable()
  {
    tournamentController.PlayerAdded -= OnPlayerAdded;
  }

  IEnumerator ActivatePlayerInput()
  {
    yield return null;
    addPlayerInput.ActivateInputField();
  }

  void CreatePlayer(string name)
  {
    string playerName = name.Trim();

    if (string.IsNullOrWhiteSpace(playerName))
    {
      Debug.Log("Cannot create player: Player input empty.");
      return;
    }

    CreatePlayerRequested?.Invoke(playerName);
    AudioManager.Instance.PlaySFX(SoundEffect.MenuAccept);
  }

  void OnPlayerAdded(Player player)
  {
    addPlayerInput.text = "";
  }
}