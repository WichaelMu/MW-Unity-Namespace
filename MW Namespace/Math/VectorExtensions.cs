using UnityEngine;
using static MW.Math.Magic.Fast;

namespace MW.VectorExtensions
{
	public static class Fast
	{
		public static float SqrDistance(Vector3 L, Vector3 R)
		{
			float x = L.x - R.x;
			float y = L.y - R.y;
			float z = L.z - R.z;

			return x * x + y * y + z * z;
		}

		public static Vector3 Normalise(this Vector3 V)
		{
			return V * InverseSqrt(V.sqrMagnitude);
		}

		public static float Distance(Vector3 L, Vector3 R)
		{
			return Sqrt(SqrDistance(L, R));
		}
	}
}
