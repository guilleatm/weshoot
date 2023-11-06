using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{

	void Start()
	{
		NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
	}

	public void CreateServer()
	{
		NetworkManager.Singleton.StartHost();
		UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
	}

	public void JoinServer(TextMeshProUGUI text)
	{
		if (text.text.Length > 1)
		{
			string[] @params = text.text.Split(':');
			string IP = @params[0];
			ushort port = ushort.Parse(@params[1]);

			UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

			transport.ConnectionData.Address = IP;
			transport.ConnectionData.Port = port;
		}

		NetworkManager.Singleton.StartClient();
	}


	private void OnClientConnected(ulong obj)
	{
		NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;

		const string SCENE = "Lobby";

		NetworkManager.Singleton.SceneManager.LoadScene(SCENE, LoadSceneMode.Single);

		// SceneManager.LoadScene(SCENE);
	}

}
