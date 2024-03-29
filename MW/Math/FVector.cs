﻿#if RELEASE
using static MW.Utils;
using static MW.Math.Magic.Fast;
using MW.Extensions;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace MW
{
	/// <summary>An unsafe MVector that copies a Vector3 for Faster calculations.</summary>
	/// <remarks>Any modification made to either the FVector -or- the Vector3 will alter the values of the other.</remarks>
	/// <decorations decor="public struct"></decorations>
	public struct FVector
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

		internal unsafe FVector(float* pX, float* pY, float* pZ)
		{
			this.pX = pX;
			this.pY = pY;
			this.pZ = pZ;
		}

		/// <summary>Copies the address of a Vector3's axes.</summary>
		/// <decorations decor="[MethodImpl(MethodImplOptions.AggressiveInlining)] public static unsafe FVector"></decorations>
		/// <param name="V">The Vector3 to copy.</param>
		/// <returns>A new FVector initialised with XYZ pointers only.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe FVector Clone(ref Vector3 V)
		{
			fixed (Vector3* pV = &V)
				return new FVector(&pV->x, &pV->y, &pV->z);
		}

		/// <summary>Dereferences pointers into XYZ coordinates into an MVector.</summary>
		/// <decorations decor="public unsafe MVector"></decorations>
		/// <returns>This MVector with dereferenced components.</returns>
		public unsafe MVector MV()
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
				fixed (FVector* U = &this)
				{
					U->pX = null;
					U->pY = null;
					U->pZ = null;
				}
			}

			System.GC.Collect();
		}

		/// <summary>Implicit conversion from a FVector to an MVector.</summary>
		/// <param name="U"></param>
		/// <decorations decor="public static implicit operator MVector"></decorations>
		public static implicit operator MVector(FVector U) => U.MV();

		#region MVector API

		/// <summary>A Vector3 ignoring the Z component.</summary>
		/// <decorations decor="public unsafe Vector3"></decorations>
		public unsafe Vector3 XY => new Vector3(*pX, *pY, 0);
		/// <summary>A Vector3 ignoring the Y component.</summary>
		/// <decorations decor="public unsafe Vector3"></decorations>
		public unsafe Vector3 XZ => new Vector3(*pX, 0, *pZ);
		/// <summary>A Vector3 ignoring the X component.</summary>
		/// <decorations decor="public unsafe Vector3"></decorations>
		public unsafe Vector3 YZ => new Vector3(0, *pY, *pZ);

		/// <summary>The square magnitude of this Vector.</summary>
		/// <decorations decor="public unsafe float"></decorations>
		public unsafe float SqrMagnitude => *pX * *pX + *pY * *pY + *pZ * *pZ;

		/// <summary>Fast distance calculation.</summary>
		/// <decorations decor="[MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe float"></decorations>
		/// <param name="Target"></param>
		/// <returns>The distance between this FVector and Vector3 Target.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe float FDistance(Vector3 Target) => FSqrt(Target.sqrMagnitude - SqrMagnitude);

		/// <summary>Fast vector normalisation.</summary>
		/// <decorations decor="[MethodImpl(MethodImplOptions.AggressiveInlining)] public unsafe FVector"></decorations>
		/// <returns>The normalised FVector.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public unsafe FVector FNormalise()
		{
			float InvSqrt = FInverseSqrt(SqrMagnitude);

			*pX *= InvSqrt;
			*pY *= InvSqrt;
			*pZ *= InvSqrt;

			return this;
		}

		/// <summary>Fast Pitch and Yaw calculation.</summary>
		/// <remarks>Roll is zero.</remarks>
		/// <decorations decor="public unsafe MRotator"></decorations>
		/// <returns>An MRotator rotated to face this FVector's direction.</returns>
		public unsafe MRotator Rotation()
		{
			MRotator R;
			R.Pitch = FArcSine(*pY);
			R.Yaw = FArcTangent2(*pX, *pZ);
			R.Roll = 0f;

			return R;
		}

		/// <summary>Rotates this FVector at an angle of AngleDegrees around Axis.</summary>
		/// <remarks>
		/// <b>While looking towards Axis:</b><br></br>
		/// + Angle is CW. - Angle is CCW.
		/// </remarks>
		/// <docremarks>
		/// While looking towards Axis:&lt;br&gt;
		/// + Angle is CW.
		/// </docremarks>
		/// <decorations decors="public unsafe FVector"></decorations>
		/// <param name="AngleDegrees">The degrees at which to rotate this FVector.</param>
		/// <param name="Axis">The axis to rotate this FVector around.</param>
		public unsafe FVector RotateVector(float AngleDegrees, Vector3 Axis)
		{
			MVector This = MV().RotateVector(AngleDegrees, Axis.MV());

			*pX = This.X;
			*pY = This.Y;
			*pZ = This.Z;

			return this;
		}

		/// <summary>The angle between two vectors in degrees.</summary>
		/// <decorations decor="public static unsafe float"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns>An approximation of the angle between L and R in degrees, accurate to +-.1 degrees.</returns>
		public static unsafe float Angle(FVector L, FVector R)
		{
			float ZeroOrEpsilon = FSqrt(L.SqrMagnitude * R.SqrMagnitude);
			if (ZeroOrEpsilon < 1E-15f)
				return 0f;

			float Radians = Dot(L, R) * FInverse(ZeroOrEpsilon);
			Clamp(ref Radians, -1f, 1f);
			return FArcCosine(Radians) * Mathf.Rad2Deg;

		}

		/// <summary>The vector dot | product of Left and Right.</summary>
		/// <remarks>Does not assume Left and Right are normalised.</remarks>
		/// <decorations decors="public static unsafe float"></decorations>
		public static unsafe float Dot(FVector L, FVector R) => *L.pX * *R.pX + *L.pY * *R.pY + *L.pZ * *R.pZ;

		#endregion
	}
}
#endif // RELEASE
