// using System.Collections;
// using System.Collections.Generic;
// using Unity.Netcode;
// using UnityEngine;

// public class PlayerSpawner : NetworkBehaviour
// {


// 	public void Spawn()
// 	{
// 		if (IsOwner)
// 		{
// 			SpawnPlayer_ServerRpc();

// 			Instantiate(attachements, NetworkManager.Singleton.LocalClient.PlayerObject.transform);
// 		}

// 		Destroy(gameObject);
// 	}



// }
