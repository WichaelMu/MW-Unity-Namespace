using UnityEngine;
using MW.Math;

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
			return V * Mathematics.FastInverseSqrt(V.sqrMagnitude);
		}

		public static float Distance(Vector3 L, Vector3 R)
		{
			return Mathematics.FastSqrt(SqrDistance(L, R));
		}
	}
}
