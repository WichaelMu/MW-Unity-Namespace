﻿using UnityEngine;
using MW.Math;
using MW.Conversion;

namespace MW
{
	/// <summary>Vector representation of coordinates and points with three-dimensions.</summary>
	[System.Serializable]
	public struct MVector
	{
		/// <summary>Vector floating-point precision.</summary>
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

		/// <summary>Construct an MVector with respect to a <see cref="Vector3"/>.</summary>
		/// <docs>Construct an MVector with respect to a Vector3.</docs>
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
			_ => throw new System.IndexOutOfRangeException("Vector index " + i + " is out of range! Expected i >= 0 && i <= 2.\nInput i: " + i)
		};

		static readonly MVector zero = new MVector(0);
		static readonly MVector right = new MVector(1, 0, 0);
		static readonly MVector up = new MVector(0, 1, 0);
		static readonly MVector forward = new MVector(0, 0, 1);
		static readonly MVector one = new MVector(1);
		static readonly MVector twod = new MVector(1, 1);
		static readonly MVector threed = new MVector(1, 0, 1);
		static readonly MVector nan = new MVector(float.NaN);

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
		/// <summary>1 on XY components. 2D MVector Flag.</summary>
		public static readonly MVector _2D = twod;
		/// <summary>1 on XZ components. 3D MVector Flag.</summary>
		public static readonly MVector _3D = threed;
		/// <summary><see cref="float.NaN"/> on all components.</summary>
		/// <docs>NaN on all components.</docs>
		public static readonly MVector NaN = nan;

		/// <summary>Converts an MVector to a Vector3.</summary>
		/// <param name="mVector">The MVector to convert.</param>
		public static Vector3 V3(MVector mVector) => new Vector3(mVector.X, mVector.Y, mVector.Z);
		/// <summary>Converts a Vector3 to an MVector.</summary>
		/// <param name="vVector">The Vector3 to convert.</param>
		public static MVector MV(Vector3 vVector) => new MVector(vVector.x, vVector.y, vVector.z);

		/// <summary>Normalises V.</summary>
		public static MVector Normalise(MVector V) => V.Normalise();
		/// <summary>The vector cross ^ product of Left and Right.</summary>
		public static MVector Cross(MVector Left, MVector Right) => Left ^ Right;
		/// <summary>The vector dot | product of Left and Right.</summary>
		/// <remarks>Does not assume Left and Right are normalised.</remarks>
		public static float Dot(MVector Left, MVector Right) => Left | Right;
		/// <summary>Whether Left and Right are <see cref="Mathematics.Parallel(MVector, MVector, float)"/> to each other.</summary>
		/// <docs>Whether left and right are Mathematics.Parallel(MVector, MVector, float) to each other.</docs>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		public static bool Parallel(MVector Left, MVector Right) => Mathematics.Parallel(Left, Right);
		/// <summary>A normalised MVector at Degrees, relative to Forward.</summary>
		/// <param name="Degrees">The angle offset.</param>
		/// <param name="Forward">The forward direction.</param>
		public static MVector MVectorFromAngle(float Degrees, EDirection Forward) => Mathematics.VectorFromAngle(Degrees, Forward);
		/// <summary>The distance between Left and Right.</summary>
		/// <param name="Left">Source of the distance.</param>
		/// <param name="Right">Distance from source.</param>
		public static float Distance(MVector Left, MVector Right) => Mathf.Sqrt(SqrDistance(Left, Right));

		/// <summary>Square Euclidean distance between Left and Right.</summary>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		public static float SqrDistance(MVector Left, MVector Right)
		{
			float x = Left.X - Right.X;
			float y = Left.Y - Right.Y;
			float z = Left.Z - Right.Z;

			return x * x + y * y + z * z;
		}

		/// <summary>The square magnitude of this MVector.</summary>
		public float SqrMagnitude { get => X * X + Y * Y + Z * Z; }
		/// <summary>The magnitude of this MVector.</summary>
		public float Magnitude { get => Mathf.Sqrt(SqrMagnitude); }
		/// <summary>The <see cref="Mathf.Abs(float)"/> of this MVector's components.</summary>
		/// <docs>The Mathf.Abs(float) of this MVector's components.</docs>
		public MVector Abs { get => new MVector(Mathf.Abs(X), Mathf.Abs(Y), Mathf.Abs(Z)); }
		/// <summary>The normalised version of this MVector.</summary>
		public MVector Normalised
		{
			get
			{
				float m = Magnitude;
				if (m > kEpsilon) return this / m;

				// Log.E("MVector is zero!", "Returning MVector.Zero instead.");
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

		/// <summary>This MVector's reflection among Normal.</summary>
		/// <param name="Normal">The normal vector to mirror.</param>
		public MVector Mirror(MVector Normal) => 2f * (this | Normal) * this - Normal;

		/// <summary>Rotates this MVector at an angle of AngleDegrees around Axis.</summary>
		/// <param name="AngleDegrees">The degrees at which to rotate this MVector.</param>
		/// <param name="Axis">The axis to rotate this MVector around.</param>
		public MVector RotateAngleAxis(float AngleDegrees, MVector Axis)
		{
			Mathematics.SinCos(out float S, out float C, AngleDegrees * Mathf.Deg2Rad);

			float XX = Axis.X * Axis.X;
			float YY = Axis.Y * Axis.Y;
			float ZZ = Axis.Z * Axis.Z;

			float XY = Axis.X * Axis.Y;
			float YZ = Axis.Y * Axis.Z;
			float ZX = Axis.Z * Axis.X;

			float XS = Axis.X * S;
			float YS = Axis.Y * S;
			float ZS = Axis.Z * S;

			float OMC = 1f - C;

			return new MVector(
				(OMC * XX + C) * X + (OMC * XY - ZS) * Y + (OMC * ZX + YS) * Z,
				(OMC * XY + ZS) * X + (OMC * YY + C) * Y + (OMC * YZ - XS) * Z,
				(OMC * ZX - YS) * X + (OMC * YZ + XS) * Y + (OMC * ZZ + C) * Z
			);
		}

		/// <summary>Converts a normalised vector to a Pitch, Yaw rotation.</summary>
		/// <remarks>Roll is zero.</remarks>
		/// <ret>An MRotator defining the pitch and yaw of this direction vector.</ret>
		/// <returns>An <see cref="MRotator"/> defining the <see cref="MRotator.Pitch"/> and <see cref="MRotator.Yaw"/> of this direction vector.</returns>
		public MRotator Rotation()
		{
			MRotator R = new MRotator();

			R.Pitch = Mathf.Asin(Y);
			R.Yaw = Mathf.Atan2(X, Z);
			R.Roll = 0;

			return R * Mathf.Rad2Deg;
		}

		/// <summary>The direction and length of this MVector.</summary>
		/// <param name="Direction"></param>
		/// <param name="Length"></param>
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
		/// <remarks>Modifies this MVector.</remarks>
		/// <returns>This MVector with a zeroed X component.</returns>
		public MVector IgnoreX()
		{
			X = 0;

			return this;
		}

		/// <summary>Ignores the Y component of this MVector.</summary>
		/// <remarks>Modifies this MVector.</remarks>
		/// <returns>This MVector with a zeroed Y component.</returns>
		public MVector IgnoreY()
		{
			Y = 0;

			return this;
		}

		/// <summary>Ignores the Z component of this MVector.</summary>
		/// <remarks>Modifies this MVector.</remarks>
		/// <returns>This MVector with a zeroed Z component.</returns>
		public MVector IgnoreZ()
		{
			Z = 0;

			return this;
		}

		/// <summary>Ignores a set of <see cref="EComponentAxis"/> from this MVector.</summary>
		/// <docs>Ignores a set of Components on this MVector.</docs>
		/// <param name="Components">The vector components to ignore.</param>
		/// <returns>This MVector zeroed over Components.</returns>
		public MVector Ignore(EComponentAxis Components)
		{
			byte Mask = (byte)((byte)Components & 7);

			if (Mask == 0)
			{
				return this;
			}
			else if (Mask == 7)
			{
				return Zero;
			}

			switch (Mask)
			{
				case 1:
					IgnoreX();
					break;
				case 2:
					IgnoreY();
					break;
				case 3:
					IgnoreX();
					IgnoreY();
					break;
				case 4:
					IgnoreZ();
					break;
				case 5:
					IgnoreX();
					IgnoreZ();
					break;
				case 6:
					IgnoreY();
					IgnoreZ();
					break;
			}

			return this;
		}

		/// <summary>Euclidean distance between this MVector and another V.</summary>
		/// <param name="V">The MVector to find distance.</param>
		/// <returns>The Euclidean distance between this MVector and V.</returns>
		public float Distance(MVector V) => Distance(this, V);
		/// <summary>The Euclidean distance, but without the square root calculation.</summary>
		/// <param name="V">The MVector to find square distance.</param>
		/// <returns>The square distance between this MVector and V.</returns>
		public float SqrDistance(MVector V) => SqrDistance(this, V);

		/// <summary>Adds two MVectors together.</summary>
		/// <param name="Left">Left-side MVector.</param>
		/// <param name="Right">Right-side MVector.</param>
		/// <returns>(MVector Left, MVector Right) => new MVector(Left.X + Right.X, Left.Y + Right.Y, Left.Z + Right.Z)</returns>
		public static MVector operator +(MVector Left, MVector Right) => new MVector(Left.X + Right.X, Left.Y + Right.Y, Left.Z + Right.Z);
		/// <summary>Adds a Vector3 to an MVector.</summary>
		/// <param name="Left">The MVector.</param>
		/// <param name="Right">The Vector3.</param>
		/// <returns>(MVector Left, Vector3 Right) => Left + (MVector)Right</returns>
		public static MVector operator +(MVector Left, Vector3 Right) => Left + (MVector)Right;
		/// <summary>Adds an MVector to a Vector3.</summary>
		/// <param name="Left">The Vector3.</param>
		/// <param name="Right">The MVector.</param>
		/// <returns>(Vector3 Left, MVector Right) => (MVector)Left + Right</returns>
		public static MVector operator +(Vector3 Left, MVector Right) => (MVector)Left + Right;
		/// <summary>Subtracts two MVectors.</summary>
		/// <param name="Left">Left-side MVector.</param>
		/// <param name="Right">Right-side MVector.</param>
		/// <returns>(MVector Left, MVector Right) => new MVector(Left.X - Right.X, Left.Y - Right.Y, Left.Z - Right.Z)</returns>
		public static MVector operator -(MVector Left, MVector Right) => new MVector(Left.X - Right.X, Left.Y - Right.Y, Left.Z - Right.Z);
		/// <summary>Negates an MVector.</summary>
		/// <param name="V">The MVector to negate all components.</param>
		/// <returns>(MVector V) => V *= -1f</returns>
		public static MVector operator -(MVector V) => -1f * V;
		/// <summary>Multiplies an MVector by a scalar on all components.</summary>
		/// <param name="S">The Scalar to multiply.</param>
		/// <param name="V">The MVector.</param>
		/// <returns>(MVector V, float S) => new MVector(V.X * S, V.Y * S, V.Z * S)</returns>
		public static MVector operator *(float S, MVector V) => new MVector(V.X * S, V.Y * S, V.Z * S);
		/// <summary>Divides an MVector by a scalar on all components.</summary>
		/// <remarks>If d == 0, this will throw a DivideByZeroException.</remarks>
		/// <param name="V">The MVector.</param>
		/// <param name="D">The denominator under all components.</param>
		/// <returns>(MVector V, float D) => float S = 1 / D; return S * V</returns>
		public static MVector operator /(MVector V, float D)
		{
			if (D == 0)
			{
				throw new System.DivideByZeroException("Attempted division by zero.");
			}

			float S = 1 / D;

			return S * V;
		}

		/// <summary>The vector cross ^ product.</summary>
		/// <returns>(MVector Left, MVector Right) => new MVector(Left.Y * Right.Z - Left.Z * Right.Y, Left.Z * Right.X - Left.X * Right.Z, Left.X * Right.Y - Left.Y * Right.X)</returns>
		public static MVector operator ^(MVector Left, MVector Right) => new MVector(Left.Y * Right.Z - Left.Z * Right.Y, Left.Z * Right.X - Left.X * Right.Z, Left.X * Right.Y - Left.Y * Right.X);
		/// <summary>The vector dot | product.</summary>
		/// <remarks>Does not assume Left and Right are normalised.</remarks>
		/// <returns>(MVector Left, MVector Right) => Left.X * Right.X + Left.Y * Right.Y + Left.Z * Right.Z</returns>
		public static float operator |(MVector Left, MVector Right) => Left.X * Right.X + Left.Y * Right.Y + Left.Z * Right.Z;

		/// <summary>Normalised direction from to.</summary>
		/// <returns>(MVector From, MVector To) => (To - From).Normalised</returns>
		public static MVector operator >(MVector From, MVector To) => (To - From).Normalised;
		/// <summary>Normalised direction from to.</summary>
		/// <returns>(MVector To, MVector From) => From > To</returns>
		public static MVector operator <(MVector To, MVector From) => From > To;

		/// <summary>Increments all components by I.</summary>
		/// <param name="V">The MVector to increment.</param>
		/// <param name="I">The number to increment.</param>
		/// <returns>v[1Σ3] + i</returns>
		public static MVector operator >>(MVector V, int I)
		{
			return new MVector
			{
				X = V.X + I,
				Y = V.Y + I,
				Z = V.Z + I
			};
		}

		/// <summary>Decrements all components by I.</summary>
		/// <param name="V">The MVector to decrement.</param>
		/// <param name="I">The number to decrement.</param>
		/// <returns>v[1Σ3] - i</returns>
		public static MVector operator <<(MVector V, int I)
		{
			return new MVector
			{
				X = V.X - I,
				Y = V.Y - I,
				Z = V.Z - I
			};
		}

		/// <summary>Compares two MVectors for equality.</summary>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <ret>True if the square distance between Left and Right is less than kEpsilon * kEpsilon.</ret>
		/// <returns>True if the square distance between Left and Right is less than <see cref="kEpsilon"/>^2.</returns>
		public static bool operator ==(MVector Left, MVector Right)
		{
			float x = Left.X - Right.X;
			float y = Left.Y - Right.Y;
			float z = Left.Z - Right.Z;
			float sqr = x * x + y * y + z * z;
			return sqr < kEpsilon * kEpsilon;
		}

		/// <summary>Compares two MVectors for inequality.</summary>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <returns>The opposite of operator ==.</returns>
		public static bool operator !=(MVector Left, MVector Right) => !(Left == Right);

		public override bool Equals(object O) => O is MVector V && Equals(V);
		public bool Equals(MVector V) => Mathf.Abs(V.X - X + V.Y - Y + V.Z - Z) < 4f * kEpsilon;

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
		public static implicit operator Color(MVector V) => Colour.Colour255(V.X, V.Y, V.Z);

		/// <summary>Hashcode for use in Maps, Sets, MArrays, etc.</summary>
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
