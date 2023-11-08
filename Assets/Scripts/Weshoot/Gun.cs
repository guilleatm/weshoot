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

		public override void OnNetworkSpawn()
		{
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
			Bullet _bullet = Instantiate(data.bullet, transform.position, transform.rotation);
			_bullet.NetworkObject.Spawn();

			// _bullet.transform.position = transform.position;
			// _bullet.transform.rotation = transform.rotation;
			_bullet.direction = transform.forward;
			
			_bullet.speed = data.bulletSpeed;
			_bullet.remainigBounces = data.maxBounces;

			_bullet.onDestroyed.AddListener(OnBulletDestroyed_Server);
		}

		void OnBulletDestroyed_Server(Bullet bullet)
		{
			bullet.onDestroyed.RemoveListener(OnBulletDestroyed_Server);
			bullet.NetworkObject.Despawn(destroy: true);
		}
	}
}