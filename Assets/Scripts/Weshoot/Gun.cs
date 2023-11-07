using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

namespace Weshoot
{
	public class Gun : NetworkBehaviour
	{
		[SerializeField] GunData data;
		float lastShotTime = 0f;
		//NetworkedPool availableBullets;

		public override void OnNetworkSpawn()
		{
			// availableBullets = new NetworkedPool(data.maxBullets);
			// availableBullets.Fill(CreateBullet_ServerRpc);

			enabled = IsLocalPlayer;
		}

		void OnEnable()
		{
			InputHolder.instance.Action.Shoot.performed += Shoot;
		}

		void OnDisable()
		{
			InputHolder.instance.Action.Shoot.performed -= Shoot;
		}

		void Update()
		{
			ManageShootAuto();
		}

		void ManageShootAuto()
		{
			bool shootAuto = InputHolder.instance.Action.ShootAuto.IsPressed();
			if (shootAuto && Time.time - lastShotTime > data.cadenceDelay)
			{
				Shoot_ServerRpc();
			}
		}
		void Shoot(InputAction.CallbackContext context) 
		{
			if (Time.time - lastShotTime > data.cadenceDelay)
			{
				Shoot_ServerRpc();
			}
		}

		[ServerRpc]
		void Shoot_ServerRpc()
		{
			Bullet _bullet = Instantiate(data.bullet, transform.position, transform.rotation);
			_bullet.NetworkObject.Spawn();

			// bool availableBullet = availableBullets.Get(out NetworkObject _bulletNetObj, enable: false);

			// if (!availableBullet) return;

			// Bullet _bullet = _bulletNetObj.GetComponent<Bullet>();

			// _bullet.transform.position = transform.position;
			// _bullet.transform.rotation = transform.rotation;
			// _bullet.speed = data.bulletSpeed;
			// _bullet.remainigBounces = data.maxBounces;

			// _bullet.gameObject.SetActive(true);
			
			// _bullet.onDestroyed.AddListener(OnBulletDestroyed);
		
			// lastShotTime = Time.time;
		}

		// [ServerRpc]
		// void CreateBullet_ServerRpc()
		// {
		// 	NetworkObject _bullet = Instantiate<NetworkObject>(data.bullet, transform.position, transform.rotation);
		// 	_bullet.Spawn(destroyWithScene: true);
		// 	return _bullet;
		// }

		// void OnBulletDestroyed(Bullet bullet)
		// {
		// 	bullet.onDestroyed.RemoveListener(OnBulletDestroyed);
		// 	availableBullets.Store(bullet.GetComponent<NetworkObject>());
		// }
	}

	// class NetworkedPool
	// {
	// 	int size;
	// 	Stack<NetworkObject> available = new Stack<NetworkObject>();

	// 	public NetworkedPool(int size)
	// 	{
	// 		this.size = size;
	// 	}

	// 	public bool Get(out NetworkObject element, bool enable = true)
	// 	{
	// 		bool availableObject = available.Count > 0;
	// 		if (availableObject)
	// 		{
	// 			element = available.Pop();

	// 			if (enable)
	// 			{
	// 				element.Spawn();
	// 			}
	// 		}
	// 		else
	// 		{
	// 			element = null;
	// 		}
	// 		return availableObject;
	// 	}

	// 	public void Store(NetworkObject element, bool disable = true)
	// 	{
	// 		if (disable)
	// 		{
	// 			element.Despawn(destroy: false);
	// 		}
	// 		available.Push(element);
	// 	}

	// 	public void Fill(Func<NetworkObject> createElement, bool disable = true)
	// 	{
	// 		for (int i = available.Count; i < size; i++)
	// 		{
	// 			NetworkObject element = createElement.Invoke();
	// 			Store(element, disable);
	// 		}
	// 	}
	// }

	// class Pool<T> where T : MonoBehaviour
	// {
	// 	int size;
	// 	Stack<T> available = new Stack<T>();

	// 	public Pool(int size)
	// 	{
	// 		this.size = size;
	// 	}

	// 	public bool Get(out T element, bool enable = true)
	// 	{
	// 		bool availableObject = available.Count > 0;
	// 		if (availableObject)
	// 		{
	// 			element = available.Pop();
	// 			element.gameObject.SetActive(enable);
	// 		}
	// 		else
	// 		{
	// 			element = null;
	// 		}
	// 		return availableObject;
	// 	}

	// 	public void Store(T element, bool disable = true)
	// 	{
	// 		element.gameObject.SetActive(!disable);
	// 		available.Push(element);
	// 	}

	// 	public void Fill(Func<T> createElement, bool disable = true)
	// 	{
	// 		for (int i = available.Count; i < size; i++)
	// 		{
	// 			T element = createElement.Invoke();
	// 			Store(element, disable);
	// 		}
	// 	}
	// }
}