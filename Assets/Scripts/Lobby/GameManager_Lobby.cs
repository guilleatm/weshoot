using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager_Lobby : NetworkBehaviour
{
	[SerializeField] LobbyPlayerSettings lobbyPlayerSettings;
	
	public override void OnNetworkSpawn()
	{
		if (IsServer)
		{
			OnClientConnected_Server(NetworkManager.Singleton.LocalClientId); // Host
			NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected_Server; // Other clients
		}
	}

	void OnClientConnected_Server(ulong clientID)
	{
		NetworkObject player = Instantiate<NetworkObject>(lobbyPlayerSettings.playerPrefab);
		player.SpawnAsPlayerObject(clientID, destroyWithScene: true);
	}
}


[System.Serializable]
class LobbyPlayerSettings
{
	public NetworkObject playerPrefab;
}