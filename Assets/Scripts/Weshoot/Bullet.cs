using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;

namespace Weshoot
{
	public class Bullet : NetworkBehaviour
	{
		public UnityEvent<Bullet> onDestroyed { get; private set; } = new UnityEvent<Bullet>();
		public float speed { get; set; }
		public float remainigBounces { get; set; }
		public Vector3 direction { get; set; }
		
		public override void OnNetworkSpawn()
		{
			enabled = IsServer;
			gameObject.SetActive(true);
		}

		void Update()
		{
			ManageMovement();
		}

		void ManageMovement()
		{
			transform.position += direction * speed * Time.deltaTime;
		}

		void OnCollisionEnter(Collision collision)
		{
			if (remainigBounces == 0)
			{
				onDestroyed.Invoke(this);
				return;
			}
			
			direction = GetDirectionAfterCollision(collision);

			remainigBounces -= 1;
		}

		Vector3 GetDirectionAfterCollision(Collision collision)
		{
			Vector3 _normal = Vector3.zero;
			for (int i = 0; i < collision.contacts.Length; i++)
			{
				_normal += collision.contacts[i].normal;
			}

			return Vector3.Reflect(direction.normalized, _normal.normalized);
		}

		public override void OnNetworkDespawn()
		{
			gameObject.SetActive(false);
		}

	}
}