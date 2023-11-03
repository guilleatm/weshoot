using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

namespace Weshoot
{
	public class Gun : MonoBehaviour
	{
		[SerializeField] GunData data;

		Pool<Bullet> availableBullets;

		void Start()
		{
			availableBullets = new Pool<Bullet>(data.maxBullets);
			availableBullets.Fill(CreateBullet);
		}

		void OnEnable()
		{
			Player.input.Action.Shoot.performed += Shoot;
		}

		void OnDisable()
		{
			Player.input.Action.Shoot.performed -= Shoot;
		}

		void Shoot(InputAction.CallbackContext context)
		{
			bool availableBullet = availableBullets.Get(out Bullet _bullet, enable: false);

			if (!availableBullet) return;

			Debug.Log("set data");

			_bullet.transform.position = transform.position;
			_bullet.transform.rotation = transform.rotation;
			_bullet.speed = data.speed;
			_bullet.remainigBounces = data.maxBounces;

			_bullet.gameObject.SetActive(true);
			
			_bullet.onDestroyed.AddListener(OnBulletDestroyed);

		}

		Bullet CreateBullet()
		{
			return Instantiate<Bullet>(data.bullet, transform.position, transform.rotation);
		}

		void OnBulletDestroyed(Bullet bullet)
		{
			bullet.onDestroyed.RemoveListener(OnBulletDestroyed);
			availableBullets.Store(bullet);
		}
	}

	class Pool<T> where T : MonoBehaviour
	{
		int size;
		Stack<T> available = new Stack<T>();

		public Pool(int size)
		{
			this.size = size;
		}

		public bool Get(out T element, bool enable = true)
		{
			bool availableObject = available.Count > 0;
			if (availableObject)
			{
				element = available.Pop();
				element.gameObject.SetActive(enable);
			}
			else
			{
				element = null;
			}
			return availableObject;
		}

		public void Store(T element, bool disable = true)
		{
			element.gameObject.SetActive(!disable);
			available.Push(element);
		}

		public void Fill(Func<T> createElement, bool disable = true)
		{
			for (int i = available.Count; i < size; i++)
			{
				T element = createElement.Invoke();
				Store(element, disable);
			}
		}
	}
}