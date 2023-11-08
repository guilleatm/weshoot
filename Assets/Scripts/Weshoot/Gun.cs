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

		ObjectPool<Bullet> bulletPool_Server;

		public override void OnNetworkSpawn()
		{
			if (IsServer)
			{
				CreateBulletPool_Server();
			}

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
				lastShotTime = Time.time;
			}
		}
		void Shoot(InputAction.CallbackContext context) 
		{
			if (Time.time - lastShotTime > data.cadenceDelay)
			{
				Shoot_ServerRpc();
				lastShotTime = Time.time;
			}
		}

		[ServerRpc]
		void Shoot_ServerRpc()
		{
			Bullet _bullet = bulletPool_Server.Get();

			_bullet.transform.position = transform.position;
			_bullet.transform.rotation = transform.rotation;
			_bullet.direction = transform.forward;
			
			_bullet.speed = data.bulletSpeed;
			_bullet.remainigBounces = data.maxBounces;

			_bullet.onDestroyed.AddListener(OnBulletDestroyed_Server);
		}

		void OnBulletDestroyed_Server(Bullet bullet)
		{
			bullet.onDestroyed.RemoveListener(OnBulletDestroyed_Server);
			bulletPool_Server.Release(bullet);
		}

		void CreateBulletPool_Server()
		{
			Bullet CreateBullet()
			{
				return Instantiate(data.bullet);
			}

			void OnGet(Bullet bullet)
			{
				bullet.gameObject.SetActive(true);
				bullet.NetworkObject.Spawn();
			}

			void OnRelease(Bullet bullet)
			{
				bullet.NetworkObject.Despawn(destroy: false);
			}

			void OnDestroy(Bullet bullet)
			{
				bullet.NetworkObject.Despawn(destroy: true);
			}

			bulletPool_Server = new ObjectPool<Bullet>(
				createFunc: CreateBullet,
				actionOnGet: OnGet,
				actionOnRelease: OnRelease,
				actionOnDestroy: OnDestroy,
				defaultCapacity: 20,
				maxSize: 10000
			);
		}
	}
}
