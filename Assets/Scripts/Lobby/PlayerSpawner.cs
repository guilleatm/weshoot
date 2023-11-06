using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
	[SerializeField] NetworkObject playerPrefab;
	[SerializeField] GameObject attachements;

	public override void OnNetworkSpawn()
	{
		if (IsOwner)
		{
			SpawnPlayer_ServerRpc();

			Instantiate(attachements, NetworkManager.Singleton.LocalClient.PlayerObject.transform);
		}
	}


	[ServerRpc(RequireOwnership = true)]
	void SpawnPlayer_ServerRpc(ServerRpcParams rpcParams = default)
	{
		NetworkObject player = Instantiate<NetworkObject>(playerPrefab);
		player.SpawnAsPlayerObject(rpcParams.Receive.SenderClientId, destroyWithScene: true);
	}
}
