using MW.Math;
using static MW.Math.Magic.Fast;
using UnityEngine;

namespace MW
{
	/// <summary>An unsafe MVector that copies a Vector3.</summary>
	/// <decorations decor="public struct"></decorations>
	public struct UVector
	{
		/// <summary>A pointer to the X-Axis.</summary>
		/// <decorations decor="public unsafe float*"></decorations>
		public unsafe float* pX;
		/// <summary>A pointer to the Y-Axis.</summary>
		/// <decorations decor="public unsafe float*"></decorations>
		public unsafe float* pY;
		/// <summary>A pointer to the Z-Axis.</summary>
		/// <decorations decor="public unsafe float*"></decorations>
		public unsafe float* pZ;

		internal unsafe UVector(float* pX, float* pY, float* pZ)
		{
			this.pX = pX;
			this.pY = pY;
			this.pZ = pZ;
		}

		/// <summary>Copies the address of a Vector3's axes.</summary>
		/// <decorations decor="public static unsafe UVector"></decorations>
		/// <param name="V">The Vector3 to copy.</param>
		/// <returns>A new UVector initialised with XYZ pointers only.</returns>
		public static unsafe UVector Clone(ref Vector3 V)
		{
			fixed (Vector3* pV = &V)
				return new UVector(&pV->x, &pV->y, &pV->z);
		}

		/// <summary>Dereferences pointers into XYZ coordinates into an MVector.</summary>
		/// <decorations decor="public unsafe MVector"></decorations>
		/// <returns>This MVector with dereferenced components.</returns>
		public unsafe MVector Construct()
		{
			return new MVector
			{
				X = *pX,
				Y = *pY,
				Z = *pZ
			};
		}

		/// <summary>Sets all component pointers to point to null.</summary>
		/// <remarks>Calls <see cref="System.GC.Collect()"/>.</remarks>
		/// <docremarks>Also calls the C# Garbage Collector.</docremarks>
		/// <decorations decor="public void"></decorations>
		public void Dispose()
		{
			unsafe
			{
				fixed (UVector* U = &this)
				{
					U->pX = null;
					U->pY = null;
					U->pZ = null;
				}
			}

			System.GC.Collect();
		}

		public static implicit operator MVector(UVector U) => U.Construct();

		#region MVector API

		public unsafe Vector3 XY => new Vector3(*pX, *pY, 0);
		public unsafe Vector3 XZ => new Vector3(*pX, 0, *pZ);
		public unsafe Vector3 YZ => new Vector3(0, *pY, *pZ);

		unsafe float SqrDistance => *pX * *pX + *pY * *pY + *pZ * *pZ;

		public unsafe float Distance(Vector3 Target) => FSqrt(Target.sqrMagnitude - SqrDistance);

		public unsafe UVector Normalise()
		{
			float InvSqrt = FInverseSqrt(SqrDistance);

			*pX *= InvSqrt;
			*pY *= InvSqrt;
			*pZ *= InvSqrt;

			return this;
		}

		public unsafe MRotator Rotation()
		{
			MRotator R;
			R.Pitch = FArcSine(*pY);
			R.Yaw = Mathf.Atan2(*pX, *pZ);
			R.Roll = 0f;

			return R;
		}

		public unsafe UVector RotateVector(float AngleDegrees, Vector3 Axis)
		{
			Mathematics.SinCos(out float S, out float C, AngleDegrees * Mathf.Deg2Rad);

			float XX = Axis.x * Axis.x;
			float YY = Axis.y * Axis.y;
			float ZZ = Axis.z * Axis.z;

			float XY = Axis.x * Axis.y;
			float YZ = Axis.y * Axis.z;
			float ZX = Axis.z * Axis.x;

			float XS = Axis.x * S;
			float YS = Axis.y * S;
			float ZS = Axis.z * S;

			float OMC = 1f - C;

			float X = *pX;
			float Y = *pY;
			float Z = *pZ;

			*pX = (OMC * XX + C) * X + (OMC * XY - ZS) * Y + (OMC * ZX + YS) * Z;
			*pY = (OMC * XY + ZS) * X + (OMC * YY + C) * Y + (OMC * YZ - XS) * Z;
			*pZ = (OMC * ZX - YS) * X + (OMC * YZ + XS) * Y + (OMC * ZZ + C) * Z;

			return this;
		}

		#endregion
	}
}
