using UnityEngine;

namespace Weshoot
{

	[CreateAssetMenu(fileName = "Gun", menuName = "ScriptableObjects/Gun Data")]
	class GunData : ScriptableObject
	{
		public float speed;
		public Bullet bullet;
		public int maxBullets;
		public int maxBounces;
	}
}