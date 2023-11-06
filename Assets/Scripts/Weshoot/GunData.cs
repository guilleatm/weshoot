using UnityEngine;

namespace Weshoot
{

	[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun Data")]
	class GunData : ScriptableObject
	{
		public float cadenceDelay;
		public float bulletSpeed;
		public Bullet bullet;
		public int maxBullets;
		public int maxBounces;
	}
}