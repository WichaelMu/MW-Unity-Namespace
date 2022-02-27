using UnityEngine;
using MW.Diagnostics;
using MW.Math;

namespace MW
{
	/// <summary>Vector representation of coordinates and points with three-dimensions.</summary>
	[System.Serializable]
	public struct MVector
	{
		public const float kEpsilon = 1E-05f;

		public float X, Y, Z;
		/// <summary>A new MVector ignoring the X component.</summary>
		public readonly MVector YZ { get => new MVector(0, Y, Z); }
		/// <summary>A new MVector ignoring the Y component.</summary>
		public readonly MVector XZ { get => new MVector(X, 0, Z); }
		/// <summary>A new MVector ignoring the Z component.</summary>
		public readonly MVector XY { get => new MVector(X, Y, 0); }

		/// <summary>Construct all components to U.</summary>
		/// <param name="U">Uniform component.</param>
		public MVector(float U)
		{
			X = U;
			Y = U;
			Z = U;
		}

		/// <summary>Construct with X and Y components only.</summary>
		/// <param name="X">X Component.</param>
		/// <param name="Y">Y Component.</param>
		public MVector(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
			Z = 0;
		}

		/// <summary>Construct an MVector with X, Y, and Z components.</summary>
		/// <param name="X">X Component.</param>
		/// <param name="Y">Y Component.</param>
		/// <param name="Z">Z Component.</param>
		public MVector(float X, float Y, float Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>Construct an MVector with respect to a Vector3.</summary>
		/// <param name="xyz">The Vector3's Components to set this MVector.</param>
		public MVector(Vector3 xyz)
		{
			X = xyz.x;
			Y = xyz.y;
			Z = xyz.z;
		}

		public float this[int i] => i switch
		{
			0 => X,
			1 => Y,
			2 => Z,
			_ => throw new System.IndexOutOfRangeException("Vector index " + i + " is out of range!")
		};

		static readonly MVector zero = new MVector(0);
		static readonly MVector right = new MVector(1, 0, 0);
		static readonly MVector up = new MVector(0, 1, 0);
		static readonly MVector forward = new MVector(0, 0, 1);
		static readonly MVector one = new MVector(1);

		/// <summary>Short for writing MVector(0).</summary>
		public static readonly MVector Zero = zero;
		/// <summary>Short for writing MVector(1, 0, 0).</summary>
		public static readonly MVector Right = right;
		/// <summary>Short for writing MVector(0, 1, 0).</summary>
		public static readonly MVector Up = up;
		/// <summary>Short for writing MVector(0, 0, 1).</summary>
		public static readonly MVector Forward = forward;
		/// <summary>Short for writing MVector(1).</summary>
		public static readonly MVector One = one;

		/// <summary>Converts an MVector to a Vector3.</summary>
		/// <param name="mVector">The MVector to convert.</param>
		public static Vector3 V3(MVector mVector) => new Vector3(mVector.X, mVector.Y, mVector.Z);
		/// <summary>Converts a Vector3 to an MVector.</summary>
		/// <param name="vVector">The Vector3 to convert.</param>
		public static MVector MV(Vector3 vVector) => new MVector(vVector.x, vVector.y, vVector.z);

		/// <summary>Normalises mVector.</summary>
		public static MVector Normalise(MVector mVector) => mVector.Normalise();
		/// <summary>The vector cross ^ product of left and right.</summary>
		public static MVector Cross(MVector left, MVector right) => left ^ right;
		/// <summary>The vector dot | product of left and right.</summary>
		/// <remarks>Does not assume left and right are normalised.</remarks>
		public static float Dot(MVector left, MVector right) => left | right;
		/// <summary>Whether left and right are Mathematics.Parallel(MVector, MVector, float) to each other.</summary>
		/// <param name="left"></param>
		/// <param name="right"></param>
		public static bool Parallel(MVector left, MVector right) => Mathematics.Parallel(left, right);
		/// <summary>A normalised MVector at fDegrees, relative to dirForward.</summary>
		/// <param name="fDegrees">The angle offset.</param>
		/// <param name="dirForward">The forward direction.</param>
		public static MVector MVectorFromAngle(float fDegrees, EDirection dirForward) => Mathematics.VectorFromAngle(fDegrees, dirForward);
		/// <summary>The distance between left and right.</summary>
		/// <param name="left">Source of the distance.</param>
		/// <param name="right">Distance from source.</param>
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
		/// <summary>The Mathf.Abs(float) of this MVector's components.</summary>
		public MVector Abs { get => new MVector(Mathf.Abs(X), Mathf.Abs(Y), Mathf.Abs(Z)); }
		/// <summary>The normalised version of this MVector.</summary>
		public MVector Normalised
		{
			get
			{
				float m = Magnitude;
				if (m > kEpsilon) return this / m;

				Log.E("MVector is zero!", "Returning MVector.Zero instead.");
				return Zero;
			}
		}

		/// <summary>Normalises this MVector.</summary>
		public MVector Normalise() => this = Normalised;

		/// <summary>Sets this MVector's components.</summary>
		public void Set(float X, float Y, float Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>Whether this MVector is a unit vector. (If this MVector is Mathematics.IsNormalised(MVector).</summary>
		/// <returns>True if this MVector has a magnitude of one.</returns>
		public bool IsNormalised() => Mathematics.IsNormalised(this);

		/// <summary>This MVector's reflection among mNormal.</summary>
		/// <param name="mNormal">The normal vector to mirror.</param>
		public MVector Mirror(MVector mNormal) => this - mNormal * 2f * (this | mNormal);

		/// <summary>Rotates this MVector at an angle of fAngleDegrees around mAxis.</summary>
		/// <param name="fAngleDegrees">The degrees at which to rotate this MVector.</param>
		/// <param name="mAxis">The axis to rotate this MVector around.</param>
		public MVector RotateAngleAxis(float fAngleDegrees, MVector mAxis)
		{
			Mathematics.SinCos(out float S, out float C, fAngleDegrees * Mathf.Deg2Rad);

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
		public void DirectionAndLength(out MVector Direction, out float Length)
		{
			Direction = Normalised;
			Length = Magnitude;
		}

		/// <summary>This MVector's projection.</summary>
		public MVector Projection()
		{
			float fZ = 1f / Z;
			return new MVector(X * fZ, Y * fZ, 1);
		}

		/// <summary>Ignores the X component of this MVector.</summary>
		public MVector IgnoreX()
		{
			X = 0;

			return this;
		}

		/// <summary>Ignores the Y component of this MVector.</summary>
		public MVector IgnoreY()
		{
			Y = 0;

			return this;
		}

		/// <summary>Ignores the Z component of this MVector.</summary>
		public MVector IgnoreZ()
		{
			Z = 0;

			return this;
		}

		/// <summary>Euclidean distance between this MVvector and another MVector.</summary>
		/// <param name="v">The MVector to find distance.</param>
		/// <returns>The Euclidean distance between this MVector and v.</returns>
		public float Distance(MVector v) => Distance(this, v);
		/// <summary>The Euclidean distance, but without the square root calculation.</summary>
		/// <param name="v">The MVector to find square distance.</param>
		/// <returns>The square distance between this MVector and v.</returns>
		public float SqrDistance(MVector v) => SqrDistance(this, v);

		/// <summary>Adds two MVectors together.</summary>
		/// <param name="l">Left-side MVector.</param>
		/// <param name="r">Right-side MVector.</param>
		/// <returns>(MVector l, MVector r) => new MVector(l.X + r.X, l.Y + r.Y, l.Z + r.Z)</returns>
		public static MVector operator +(MVector l, MVector r) => new MVector(l.X + r.X, l.Y + r.Y, l.Z + r.Z);
		/// <summary>Adds a Vector3 to an MVector.</summary>
		/// <param name="l">The MVector.</param>
		/// <param name="r">The Vector3.</param>
		/// <returns>(MVector l, Vector3 r) => l + (MVector)r</returns>
		public static MVector operator +(MVector l, Vector3 r) => l + (MVector)r;
		/// <summary>Adds an MVector to a Vector3.</summary>
		/// <param name="l">The Vector3.</param>
		/// <param name="r">The MVector.</param>
		/// <returns>(Vector3 l, MVector r) => (MVector)l + r</returns>
		public static MVector operator +(Vector3 l, MVector r) => (MVector)l + r;
		/// <summary>Subtracts two MVectors.</summary>
		/// <param name="l">Left-side MVector.</param>
		/// <param name="r">Right-side MVector.</param>
		/// <returns>(MVector l, MVector r) => new MVector(l.X - r.X, l.Y - r.Y, l.Z - r.Z)</returns>
		public static MVector operator -(MVector l, MVector r) => new MVector(l.X - r.X, l.Y - r.Y, l.Z - r.Z);
		/// <summary>Negates an MVector.</summary>
		/// <param name="v">The MVector to negate all components.</param>
		/// <returns>(MVector v) => new MVector(-v.X, -v.Y, -v.Z)</returns>
		public static MVector operator -(MVector v) => new MVector(-v.X, -v.Y, -v.Z);
		/// <summary>Multiplies an MVector by a scalar on all components.</summary>
		/// <param name="v">The MVector.</param>
		/// <param name="s">The Scalar to multiply.</param>
		/// <returns>(MVector v, float s) => new MVector(v.X * s, v.Y * s, v.Z * s)</returns>
		public static MVector operator *(MVector v, float s) => new MVector(v.X * s, v.Y * s, v.Z * s);
		/// <summary>Multiplies an MVector by a scalar on all components.</summary>
		/// <param name="s">The Scalar to multiply.</param>
		/// <param name="v">The MVector.</param>
		/// <returns>(MVector v, float s) => new MVector(v.X * s, v.Y * s, v.Z * s)</returns>
		public static MVector operator *(float s, MVector v) => new MVector(v.X * s, v.Y * s, v.Z * s);
		/// <summary>Divides an MVector by a scalar on all components.</summary>
		/// <remarks>If d == 0, this will throw a DivideByZeroException.</remarks>
		/// <param name="v">The MVector.</param>
		/// <param name="d">The denominator under all components.</param>
		/// <returns>(MVector v, float d) => new MVector(v.X / d, v.Y / d, v.Z / d)</returns>
		public static MVector operator /(MVector v, float d)
		{
			if (d == 0)
			{
				throw new System.DivideByZeroException("Attempted division by zero. Input d: " + d);
			}

			return new MVector(v.X / d, v.Y / d, v.Z / d);
		}

		/// <summary>The vector cross ^ product.</summary>
		/// <returns>(MVector l, MVector r) => new MVector(l.Y * r.Z - l.Z * r.Y, l.Z * r.X - l.X * r.Z, l.X * r.Y - l.Y * r.X)</returns>
		public static MVector operator ^(MVector l, MVector r) => new MVector(l.Y * r.Z - l.Z * r.Y, l.Z * r.X - l.X * r.Z, l.X * r.Y - l.Y * r.X);
		/// <summary>The vector dot | product.</summary>
		/// <remarks>Does not assume l and r are normalised.</remarks>
		/// <returns>(MVector l, MVector r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z</returns>
		public static float operator |(MVector l, MVector r) => l.X * r.X + l.Y * r.Y + l.Z * r.Z;

		/// <summary>Normalised direction from to.</summary>
		/// <returns>(MVector From, MVector To) => (To - From).Normalised</returns>
		public static MVector operator >(MVector From, MVector To) => (To - From).Normalised;
		/// <summary>Normalised direction from to.</summary>
		/// <returns>(MVector To, MVector From) => From > To</returns>
		public static MVector operator <(MVector To, MVector From) => From > To;

		/// <summary>Increments all components by i.</summary>
		/// <param name="v">The MVector to increment.</param>
		/// <param name="i">The number to increment.</param>
		/// <returns>v[1Σ3] + i</returns>
		public static MVector operator >>(MVector v, int i)
		{
			return new MVector
			{
				X = v.X + i,
				Y = v.Y + i,
				Z = v.Z + i
			};
		}

		/// <summary>Decrements all components by i.</summary>
		/// <param name="v">The MVector to decrement.</param>
		/// <param name="i">The number to decrement.</param>
		/// <returns>v[1Σ3] - i</returns>
		public static MVector operator <<(MVector v, int i)
		{
			return new MVector
			{
				X = v.X - i,
				Y = v.Y - i,
				Z = v.Z - i
			};
		}

		/// <summary>Compares two MVectors for equality.</summary>
		/// <param name="l">Left-side comparison.</param>
		/// <param name="r">Right-side comparison.</param>
		/// <returns>True if the square distance between l and r is less than kEpsilon * kEpsilon.</returns>
		public static bool operator ==(MVector l, MVector r)
		{
			float x = l.X - r.X;
			float y = l.Y - r.Y;
			float z = l.Z - r.Z;
			float sqr = x * x + y * y + z * z;
			return sqr < kEpsilon * kEpsilon;
		}

		/// <summary>Compares two MVectors for inequality.</summary>
		/// <param name="l">Left-side comparison.</param>
		/// <param name="r">Right-side comparison.</param>
		/// <returns>The opposite of operator ==.</returns>
		public static bool operator !=(MVector l, MVector r) => !(l == r);

		public override bool Equals(object obj) => obj is MVector && Equals(obj);
		public bool Equals(MVector mV) => mV.X == X && mV.Y == Y && mV.Z == Z;

		/// <summary>Automatic conversion from an MVector to a Vector3.</summary>
		public static implicit operator Vector3(MVector MVector) => new Vector3(MVector.X, MVector.Y, MVector.Z);
		/// <summary>Automatic conversion from an MVector to a Vector2.</summary>
		/// <remarks>Only the X and Y components are considered. The Z component is ignored.</remarks>
		public static implicit operator Vector2(MVector MVector) => new Vector2(MVector.X, MVector.Y);

		/// <summary>Automatic conversion from a Vector3 to an MVector.</summary>
		public static implicit operator MVector(Vector3 Vector) => new MVector(Vector);
		/// <summary>Automatic conversion from a Vector2 to an MVector.</summary>
		/// <remarks>The resulting MVector will have a Z equal to zero.</remarks>
		public static implicit operator MVector(Vector2 Vector) => new MVector(Vector);

		/// <summary>The Color representation of this MVector, in 0-255 XYZ/RGB.</summary>
		public static implicit operator Color(MVector mVector)
		{
			return Conversion.Colour.Colour255(mVector.X, mVector.Y, mVector.Z);
		}

		/// <summary>Hashcode for use in Maps, Sets, Dictionaries, etc.</summary>
		/// <returns>GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() &lt;&lt; 2) ^ (Z.GetHashCode() &gt;&gt; 2)</returns>
		public override int GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2);

		/// <summary>A human-readable MVector.</summary>
		/// <returns>ToString() => "X: " + X + " Y: " + Y + " Z: " + Z</returns>
		public override string ToString() => "X: " + X + " Y: " + Y + " Z: " + Z;
	}

	/// <summary>MVector axes.</summary>
	/// <remarks>Uses bytes.</remarks>
	public enum EComponentAxis : byte
	{
		/// <summary></summary>
		NoAxis = 0,
		/// <summary></summary>
		X = 1,
		/// <summary></summary>
		Y = 2,
		/// <summary></summary>
		Z = 4
	}
}
