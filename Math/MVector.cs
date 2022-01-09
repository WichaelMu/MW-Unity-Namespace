using UnityEngine;
using MW.Diagnostics;
using MW.Math;

namespace MW.Vector {
	[System.Serializable]
	public struct MVector {
		public const float kEpsilon = 1E-05f;

		public float X, Y, Z;
		/// <summary>A new MVector ignoring the X component.</summary>
		public readonly MVector YZ { get => new MVector(0, Y, Z); }
		/// <summary>A new MVector ignoring the Y component.</summary>
		public readonly MVector XZ { get => new MVector(X, 0, Z); }
		/// <summary>A new MVector ignoring the Z component.</summary>
		public readonly MVector XY { get => new MVector(X, Y, 0); }

		/// <summary>Construct all components to U.</summary>
		public MVector(float U)
		{
			X = U;
			Y = U;
			Z = U;
		}

		/// <summary>Construct with X and Y components only.</summary>
		public MVector(float X, float Y) {
			this.X = X;
			this.Y = Y;
			Z = 0;
		}

		/// <summary>Construct an MVector with X, Y, and Z components.</summary>
		public MVector(float X, float Y, float Z) {
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>Construct an MVector with respect to a Vector3.</summary>
		/// <param name="xyz"></param>
		public MVector(Vector3 xyz)
		{
			X = xyz.x;
			Y = xyz.y;
			Z = xyz.z;
		}

		public float this[int i] => i switch {
			0 => X,
			1 => Y,
			2 => Z,
			_ => throw new System.IndexOutOfRangeException("Vector index " + i + " is out of range!")
		};

		static readonly MVector zero = new MVector(0, 0, 0);
		static readonly MVector right = new MVector(1, 0, 0);
		static readonly MVector up = new MVector(0, 1, 0);
		static readonly MVector forward = new MVector(0, 0, 1);

		/// <summary>Short for writing MVector(0, 0, 0).</summary>
		public static readonly MVector Zero = zero;
		/// <summary>Short for writing MVector(1, 0, 0).</summary>
		public static readonly MVector Right = right;
		/// <summary>Short for writing MVector(0, 1, 0).</summary>
		public static readonly MVector Up = up;
		/// <summary>Short for writing MVector(0, 0, 1).</summary>
		public static readonly MVector Forward = forward;

		/// <summary>Converts an <see cref="MVector"/> to a <see cref="Vector3"/>.</summary>
		/// <param name="mVector">The MVector to convert.</param>
		public static Vector3 ToVector3(MVector mVector) => new Vector3(mVector.X, mVector.Y, mVector.Z);
		/// <summary>Converts a <see cref="Vector3"/> to an <see cref="MVector"/>.</summary>
		/// <param name="vVector">The Vector3 to convert.</param>
		public static MVector ToMVector(Vector3 vVector) => new MVector(vVector.x, vVector.y, vVector.z);

		/// <summary>Normalises mVector.</summary>
		public static MVector Normalise(MVector mVector) => mVector.Normalise();
		/// <summary>The vector cross ^ product of left and right.</summary>
		public static MVector Cross(MVector left, MVector right) => left ^ right;
		/// <summary>The vector dot | product of left and right.</summary>
		public static float Dot(MVector left, MVector right) => left | right;
		/// <summary>Whether left and right are <see cref="Mathematics.Parallel(MVector, MVector, float)"/> to each other.</summary>
		public static bool Parallel(MVector left, MVector right) => Mathematics.Parallel(left, right);
		/// <summary>A normalised MVector at fDegrees, relative to dirForward.</summary>
		/// <param name="fDegrees">The angle offset.</param>
		/// <param name="dirForward">The forward direction.</param>
		public static MVector MVectorFromAngle(float fDegrees, EDirection dirForward) => Mathematics.VectorFromAngle(fDegrees, dirForward);
		/// <summary>The distance between left and right.</summary>
		public static float Distance(MVector left, MVector right) => Mathf.Sqrt(SqrDistance(left, right));

		public static float SqrDistance(MVector left, MVector right)
		{
			float x = left.X - right.X;
			float y = left.Y - right.Y;
			float z = left.Z - right.Z;

			return x * x + y * y + z * z;
		}

		/// <summary>The square magnitude of this MVector.</summary>
		public float SqrMagnitude { get => X * X + Y * Y + Z * Z; }
		/// <summary>The magnitude of this MVector.</summary>
		public float Magnitude { get => Mathf.Sqrt(SqrMagnitude); }
		/// <summary>The <see cref="Mathf.Abs(float)"/> of this MVector's components.</summary>
		public MVector Abs { get => new MVector(Mathf.Abs(X), Mathf.Abs(Y), Mathf.Abs(Z)); }
		/// <summary>The normalised version of this MVector.</summary>
		public MVector Normalised { get {
				float m = Magnitude;
				if (m > kEpsilon) return this / m;

				Log.PrintError("MVector is zero!", "Returning MVector.Zero instead.");
				return Zero;
			}
		}

		/// <summary>Normalises this MVector.</summary>
		public MVector Normalise() => Normalised;

		/// <summary>Sets this MVector's components.</summary>
		public void Set(float X, float Y, float Z) {
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>Whether this MVector is a unit vector. (If this MVector is <see cref="Mathematics.IsNormalised(MVector)"/>)</summary>
		/// <returns></returns>
		public bool IsNormalised() => Mathematics.IsNormalised(this);

		/// <summary>This MVector's reflection among mNormal.</summary>
		/// <param name="mNormal">The normal vector to mirror.</param>
		public MVector Mirror(MVector mNormal) => this - mNormal * 2f * (this | mNormal);

		/// <summary>Rotates this MVector at an angle of fAngleDegrees around mAxis.</summary>
		/// <param name="fAngleDegrees">The degrees at which to rotate this MVector.</param>
		/// <param name="mAxis">The axis to rotate this MVector around.</param>
		public MVector RotateAngleAxis(float fAngleDegrees, MVector mAxis) {
			float S = 0, C = 0;
			Mathematics.SinCos(ref S, ref C, fAngleDegrees * Mathf.Deg2Rad);

			float XX = mAxis.X * mAxis.X;
			float YY = mAxis.Y * mAxis.Y;
			float ZZ = mAxis.Z * mAxis.Z;

			float XY = mAxis.X * mAxis.Y;
			float YZ = mAxis.Y * mAxis.Z;
			float ZX = mAxis.Z * mAxis.X;

			float XS = mAxis.X * S;
			float YS = mAxis.Y * S;
			float ZS = mAxis.Z * S;

			float OMC = 1f - C;

			return new MVector(
				(OMC * XX + C) * X + (OMC * XY - ZS) * Y + (OMC * ZX + YS) * Z,
				(OMC * XY + ZS) * X + (OMC * YY + C) * Y + (OMC * YZ - XS) * Z,
				(OMC * ZX - YS) * X + (OMC * YZ + XS) * Y + (OMC * ZZ + C) * Z
			);
		}

		/// <summary>The direction and length of this MVector.</summary>
		/// <returns><see cref="TPair{TFirst, TSecond}.First"/> The direction of this MVector. <see cref="TPair{TFirst, TSecond}.Second"/> The length of this MVector.</returns>
		public TPair<MVector, float> DirectionAndLength() => new TPair<MVector, float>(Normalised, Magnitude);

		/// <summary>This MVector's projection.</summary>
		public MVector Projection() {
			float fZ = 1f / Z;
			return new MVector(X * fZ, Y * fZ, 1);
		}

		/// <summary>The <see cref="Quaternion"/> this MVector represents.</summary>
		public Quaternion Rotation() {
			float fYAxis = Mathf.Atan2(Y, X);
			float fXAxis = Mathf.Atan2(Z, Mathf.Sqrt(X * X + Y * Y));

			float sq = 0, sw = 0;
			float cq = 0, cw = 0;
			Mathematics.SinCos(ref sq, ref sw, fXAxis * Utils.kHalf);
			Mathematics.SinCos(ref cq, ref cw, fYAxis * Utils.kHalf);

			return new Quaternion {
				x = sq * sw,
				y = -sq * cw,
				z = cq * sw,
				w = cq * cw
			};
		}

		/// <summary>Ignores the X component of this MVector.</summary>
		public MVector IgnoreX() {
			X = 0;

			return this;
		}

		/// <summary>Ignores the Y component of this MVector.</summary>
		public MVector IgnoreY() {
			Y = 0;

			return this;
		}

		/// <summary>Ignores the Z component of this MVector.</summary>
		public MVector IgnoreZ() {
			Z = 0;

			return this;
		}

		public float Distance(MVector v) => Distance(this, v);
		public float SqrDistance(MVector v) => SqrDistance(this, v);

		public static MVector operator +(MVector l, MVector r) => new MVector(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
		public static MVector operator +(MVector l, Vector3 r) => l + (MVector)r;
		public static MVector operator +(Vector3 l, MVector r) => (MVector)l + r;
		public static MVector operator -(MVector l, MVector r) => new MVector(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
		public static MVector operator -(MVector v) => new MVector(-v.X, -v.Y, -v.Z);
		public static MVector operator *(MVector v, float m) => new MVector(v.X * m, v.Y * m, v.Z * m);
		public static MVector operator *(float m, MVector v) => new MVector(v.X * m, v.Y * m, v.Z * m);
		public static MVector operator /(MVector v, float d) => new MVector(v.X / d, v.Y / d, v.Z / d);

		/// <summary>The vector cross ^ product.</summary>
		public static MVector operator ^(MVector l, MVector r) => new MVector(l.Y * r.Z - l.Z * r.Y, l.Z * r.X - l.X * r.Z, l.X * r.Y - l.Y * r.X);
		/// <summary>The vector dot | product.</summary>
		public static float operator |(MVector l, MVector r) => l.X * r.X + l.Y + r.Y + l.Z + r.Z;

		/// <summary><see cref="Normalised"/> direction from to.</summary>
		public static MVector operator >(MVector From, MVector To) => (To - From).Normalised;
		/// <summary><see cref="Normalised"/> direction from to.</summary>
		public static MVector operator <(MVector To, MVector From) => From > To;

		public static MVector operator >>(MVector v, int i)
		{
			return new MVector
			{
				X = v.X + i,
				Y = v.Y + i,
				Z = v.Z + i
			};
		}

		public static MVector operator <<(MVector v, int i)
		{
			return new MVector
			{
				X = v.X - i,
				Y = v.Y - i,
				Z = v.Z - i
			};
		}

		public static bool operator ==(MVector l, MVector r) {
			float x = l.X - r.X;
			float y = l.Y - r.Y;
			float z = l.Z - r.Z;
			float sqr = x * x + y * y + z * z;
			return sqr < kEpsilon * kEpsilon;
		}

		public static bool operator !=(MVector l, MVector r) => !(l == r);

		public override bool Equals(object obj) => obj is MVector && Equals(obj);
		public bool Equals(MVector mV) => mV.X == X && mV.Y == Y && mV.Z == Z;

		public static implicit operator Vector3(MVector mVector) => new Vector3(mVector.X, mVector.Y, mVector.Z);
		public static implicit operator Vector2(MVector mVector) => new Vector2(mVector.X, mVector.Y);
		
		public static implicit operator MVector(Vector3 vVector) => new MVector(vVector);
		public static implicit operator MVector(Vector2 vVector) => new MVector(vVector);
		
		/// <summary>The <see cref="Color"/> representation of this MVector, in 0-255 XYZ/RGB.</summary>
		public static implicit operator Color(MVector mVector) {
			return Conversion.Colour.Colour255(mVector.X, mVector.Y, mVector.Z);
		}

		public override int GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2);

		public override string ToString() => "X: " + X + " Y: " + Y + " Z: " + Z;
	}
}
