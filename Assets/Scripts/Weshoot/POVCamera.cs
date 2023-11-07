using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Weshoot;

public class POVCamera : NetworkBehaviour
{
	public override void OnNetworkSpawn()
	{
		if (IsLocalPlayer)
		{
			
		}
		else
		{
			tag = nameof(Tag.Untagged);
			Destroy(GetComponent<Camera>());
			Destroy(GetComponent<AudioListener>());
		}

		enabled = IsLocalPlayer;
	}

	// We can move here the camera rotation logic
}
