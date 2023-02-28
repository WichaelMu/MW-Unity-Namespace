using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using MW.Conversion;
using MW.Math;
using UnityEngine;
using static MW.Math.Magic.Fast;

namespace MW
{
	/// <summary>Vector representation of coordinates and points with three-dimensions.</summary>
	/// <decorations decors="[Serializable] public struct"></decorations>
	[Serializable]
	public struct MVector
	{
		/// <summary>Vector floating-point precision.</summary>
		/// <decorations decors="public const float"></decorations>
		public const float kEpsilon = 1E-05f;

		public float X, Y, Z;

		/// <summary>A new MVector ignoring the X component.</summary>
		/// <decorations decor="public readonly MVector"></decorations>
		public readonly MVector YZ { get => new MVector(0, Y, Z); }
		/// <summary>A new MVector ignoring the Y component.</summary>
		/// <decorations decor="public readonly MVector"></decorations>
		public readonly MVector XZ { get => new MVector(X, 0, Z); }
		/// <summary>A new MVector ignoring the Z component.</summary>
		/// <decorations decor="public readonly MVector"></decorations>
		public readonly MVector XY { get => new MVector(X, Y, 0); }

		/// <summary>Construct all components to U.</summary>
		/// <param name="U">Uniform component.</param>
		public unsafe MVector(float U) : this()
		{
			X = Y = Z = U;
		}

		/// <summary>Construct with X and Y components only.</summary>
		/// <param name="X">X Component.</param>
		/// <param name="Y">Y Component.</param>
		public MVector(float X, float Y) : this()
		{
			this.X = X;
			this.Y = Y;
			Z = 0;
		}

		/// <summary>Construct an MVector with X, Y, and Z components.</summary>
		/// <param name="X">X Component.</param>
		/// <param name="Y">Y Component.</param>
		/// <param name="Z">Z Component.</param>
		public MVector(float X, float Y, float Z) : this(X, Y)
		{
			this.Z = Z;
		}

		/// <summary>Construct an MVector with respect to a <see cref="Vector3"/>.</summary>
		/// <docs>Construct an MVector with respect to a Vector3.</docs>
		/// <param name="xyz">The Vector3's Components to set this MVector.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MVector(Vector3 xyz) : this(xyz.x, xyz.y, xyz.z) { }

		/// <summary>Construct an MVector with respect to a <see cref="Vector2"/>.</summary>
		/// <summary>Construct an MVector with respect to a Vector2.</summary>
		/// <param name="xy">The Vector2's Components to set this MVector.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MVector(Vector2 xy) : this(xy.x, xy.y) { }

		public float this[int i] => i switch
		{
			0 => X,
			1 => Y,
			2 => Z,
			_ => throw new IndexOutOfRangeException("Vector index " + i + " is out of range! Expected i >= 0 && i <= 2.\nInput i: " + i)
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
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector Zero = zero;
		/// <summary>Short for writing MVector(1, 0, 0).</summary>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector Right = right;
		/// <summary>Short for writing MVector(0, 1, 0).</summary>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector Up = up;
		/// <summary>Short for writing MVector(0, 0, 1).</summary>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector Forward = forward;
		/// <summary>Short for writing MVector(1).</summary>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector One = one;
		/// <summary>1 on XY components. 2D MVector Flag.</summary>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector _2D = twod;
		/// <summary>1 on XZ components. 3D MVector Flag.</summary>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector _3D = threed;
		/// <summary><see cref="float.NaN"/> on all components.</summary>
		/// <docs>NaN on all components.</docs>
		/// <decorations decor="public static readonly MVector"></decorations>
		public static readonly MVector NaN = nan;

		/// <summary>Converts an MVector to a Vector3.</summary>
		/// <decorations decors="public static Vector3"></decorations>
		/// <param name="M">The MVector to convert.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 V3(MVector M) => new Vector3(M.X, M.Y, M.Z);
		/// <summary>Converts a Vector3 to an MVector.</summary>
		/// <decorations decors="public static MVector"></decorations>
		/// <param name="V">The Vector3 to convert.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector MV(Vector3 V) => new MVector(V.x, V.y, V.z);

		/// <summary>Normalises V.</summary>
		/// <decorations decors="public static MVector"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector Normalise(MVector V) => V.Normalise();
		/// <summary>The vector cross ^ product of Left and Right.</summary>
		/// <decorations decors="public static MVector"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector Cross(MVector Left, MVector Right) => Left ^ Right;
		/// <summary>The vector dot | product of Left and Right.</summary>
		/// <remarks>Does not assume Left and Right are normalised.</remarks>
		/// <decorations decors="public static float"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Dot(MVector Left, MVector Right) => Left | Right;
		/// <summary>Whether Left and Right are <see cref="Mathematics.Parallel(MVector, MVector, float)"/> to each other.</summary>
		/// <docs>Whether left and right are Mathematics.Parallel(MVector, MVector, float) to each other.</docs>
		/// <decorations decors="public static bool"></decorations>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Parallel(MVector Left, MVector Right) => Mathematics.Parallel(Left, Right);
		/// <summary>A normalised MVector at Degrees, relative to Forward.</summary>
		/// <decorations decors="public static MVector"></decorations>
		/// <param name="Degrees">The angle offset.</param>
		/// <param name="Forward">The forward direction.</param>
		[Obsolete($"Use {nameof(RotateVector)} instead!")]
		public static MVector MVectorFromAngle(float Degrees, EDirection Forward) => Mathematics.VectorFromAngle(Degrees, Forward);
		/// <summary>The angle between two vectors in degrees.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns>An approximation of the angle between L and R in degrees, accurate to +-.1 degrees.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Angle(MVector L, MVector R) => FAngle(L, R);
		/// <summary>The distance between Left and Right.</summary>
		/// <decorations decors="public static float"></decorations>
		/// <param name="Left">Source of the distance.</param>
		/// <param name="Right">Distance from source.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Distance(MVector Left, MVector Right) => FSqrt(SqrDistance(Left, Right));

		/// <summary>Square Euclidean distance between Left and Right.</summary>
		/// <decorations decors="public static float"></decorations>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SqrDistance(MVector Left, MVector Right)
		{
			float x = Left.X - Right.X;
			float y = Left.Y - Right.Y;
			float z = Left.Z - Right.Z;

			return x * x + y * y + z * z;
		}

		/// <summary>The square magnitude of this MVector.</summary>
		/// <decorations decors="public float"></decorations>
		public float SqrMagnitude { get => X * X + Y * Y + Z * Z; }
		/// <summary>The magnitude of this MVector.</summary>
		/// <decorations decors="public float"></decorations>
		public float Magnitude { get => FSqrt(SqrMagnitude); }
		/// <summary>The <see cref="Mathf.Abs(float)"/> of this MVector's components.</summary>
		/// <docs>The Mathf.Abs(float) of this MVector's components.</docs>
		/// <decorations decors="public MVector"></decorations>
		public MVector Abs { get => new MVector(FAbs(X), FAbs(Y), FAbs(Z)); }
		/// <summary>The normalised version of this MVector.</summary>
		/// <decorations decors="public MVector"></decorations>
		public MVector Normalised { get => FInverseSqrt(SqrMagnitude) * this; }

		/// <summary>Normalises this MVector.</summary>
		/// <decorations decors="public MVector"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MVector Normalise() => this = Normalised;

		/// <summary>Sets this MVector's components.</summary>
		/// <decorations decors="public void"></decorations>
		public void Set(float X, float Y, float Z)
		{
			this.X = X;
			this.Y = Y;
			this.Z = Z;
		}

		/// <summary>Whether this MVector is a unit vector. (If this MVector is Mathematics.IsNormalised(MVector).</summary>
		/// <decorations decors="public bool"></decorations>
		/// <returns>True if this MVector has a magnitude of one.</returns>
		public bool IsNormalised() => Mathematics.IsNormalised(this);

		/// <summary>This MVector's reflection among Normal.</summary>
		/// <decorations decors="public MVector"></decorations>
		/// <param name="Normal">The normal vector to mirror.</param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public MVector Mirror(MVector Normal) => 2f * (this | Normal) * this - Normal;

		/// <summary>Rotates this MVector at an angle of AngleDegrees around Axis.</summary>
		/// <remarks>
		/// <b>While looking towards Axis:</b><br></br>
		/// + Angle is CW. - Angle is CCW.
		/// </remarks>
		/// <docremarks>
		/// While looking towards Axis:&lt;br&gt;
		/// + Angle is CW. - Angle is CCW.
		/// </docremarks>
		/// <decorations decors="public MVector"></decorations>
		/// <param name="AngleDegrees">The degrees at which to rotate this MVector.</param>
		/// <param name="Axis">The axis to rotate this MVector around.</param>
		public MVector RotateVector(float AngleDegrees, MVector Axis)
		{
			if (AngleDegrees < 0f || AngleDegrees > 360f)
				AngleDegrees = Mathematics.Wrap(AngleDegrees, 0f, 360f);

			if (AngleDegrees == 0f || AngleDegrees == 360f)
				return this;

			Axis = -Axis;

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
		/// <decorations decors="public MRotator"></decorations>
		/// <docreturns>An MRotator defining the pitch and yaw of this direction vector.</docreturns>
		/// <returns>An <see cref="MRotator"/> defining the <see cref="MRotator.Pitch"/> and <see cref="MRotator.Yaw"/> of this direction vector.</returns>
		public MRotator Rotation()
		{
			MRotator R = new MRotator();

			R.Pitch = FArcSine(Y);
			R.Yaw = Mathf.Atan2(X, Z);
			R.Roll = 0;

			return R * Mathf.Rad2Deg;
		}

		/// <summary>The direction and length of this MVector.</summary>
		/// <decorations decors="public void"></decorations>
		/// <param name="Direction"></param>
		/// <param name="Length"></param>
		public void DirectionAndLength(out MVector Direction, out float Length)
		{
			Direction = Normalised;
			Length = Magnitude;
		}

		/// <summary>This MVector's projection.</summary>
		/// <decorations decors="public MVector"></decorations>
		public MVector Projection()
		{
			float fZ = 1f / Z;
			return new MVector(X * fZ, Y * fZ, 1);
		}

		/// <summary>Ignores the X component of this MVector.</summary>
		/// <remarks>Modifies this MVector.</remarks>
		/// <decorations decors="public MVector"></decorations>
		/// <returns>This MVector with a zeroed X component.</returns>
		public MVector IgnoreX()
		{
			X = 0;

			return this;
		}

		/// <summary>Ignores the Y component of this MVector.</summary>
		/// <remarks>Modifies this MVector.</remarks>
		/// <decorations decors="public MVector"></decorations>
		/// <returns>This MVector with a zeroed Y component.</returns>
		public MVector IgnoreY()
		{
			Y = 0;

			return this;
		}

		/// <summary>Ignores the Z component of this MVector.</summary>
		/// <remarks>Modifies this MVector.</remarks>
		/// <decorations decors="public MVector"></decorations>
		/// <returns>This MVector with a zeroed Z component.</returns>
		public MVector IgnoreZ()
		{
			Z = 0;

			return this;
		}

		/// <summary>Ignores a set of <see cref="EComponentAxis"/> from this MVector.</summary>
		/// <docs>Ignores a set of Components on this MVector.</docs>
		/// <decorations decors="public MVector"></decorations>
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
		/// <decorations decors="public float"></decorations>
		/// <returns>The Euclidean distance between this MVector and V.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float Distance(MVector V) => Distance(this, V);
		/// <summary>The Euclidean distance, but without the square root calculation.</summary>
		/// <param name="V">The MVector to find square distance.</param>
		/// <decorations decors="public float"></decorations>
		/// <returns>The square distance between this MVector and V.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public float SqrDistance(MVector V) => SqrDistance(this, V);

		/// <summary>Get the <see cref="SqrMagnitude"/> of <paramref name="M"/>.</summary>
		/// <docs>Get the SqrMagnitude of M.</docs>
		/// <param name="M">The MVector.</param>
		/// <docreturns>The SquMagnitude of M.</docreturns>
		/// <returns><see cref="SqrMagnitude"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator ~(MVector M) => M.SqrMagnitude;

		/// <summary>Adds two MVectors together.</summary>
		/// <decorations decors="public static MVector operator+"></decorations>
		/// <param name="Left">Left-side MVector.</param>
		/// <param name="Right">Right-side MVector.</param>
		/// <returns>(MVector Left, MVector Right) => new MVector(Left.X + Right.X, Left.Y + Right.Y, Left.Z + Right.Z)</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator +(MVector Left, MVector Right) => new MVector(Left.X + Right.X, Left.Y + Right.Y, Left.Z + Right.Z);
		/// <summary>Adds a Vector3 to an MVector.</summary>
		/// <decorations decors="public static MVector operator+"></decorations>
		/// <param name="Left">The MVector.</param>
		/// <param name="Right">The Vector3.</param>
		/// <returns>(MVector Left, Vector3 Right) => Left + (MVector)Right</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator +(MVector Left, Vector3 Right) => Left + (MVector)Right;
		/// <summary>Adds an MVector to a Vector3.</summary>
		/// <decorations decors="public static MVector operator+"></decorations>
		/// <param name="Left">The Vector3.</param>
		/// <param name="Right">The MVector.</param>
		/// <returns>(Vector3 Left, MVector Right) => (MVector)Left + Right</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator +(Vector3 Left, MVector Right) => (MVector)Left + Right;
		/// <summary>Subtracts two MVectors.</summary>
		/// <decorations decors="public static MVector operator-"></decorations>
		/// <param name="Left">Left-side MVector.</param>
		/// <param name="Right">Right-side MVector.</param>
		/// <returns>(MVector Left, MVector Right) => new MVector(Left.X - Right.X, Left.Y - Right.Y, Left.Z - Right.Z)</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator -(MVector Left, MVector Right) => new MVector(Left.X - Right.X, Left.Y - Right.Y, Left.Z - Right.Z);
		/// <summary>Negates an MVector.</summary>
		/// <decorations decors="public static MVector operator-"></decorations>
		/// <param name="V">The MVector to negate all components.</param>
		/// <returns>(MVector V) => V *= -1f</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator -(MVector V) => -1f * V;
		/// <summary>Multiplies an MVector by a scalar on all components.</summary>
		/// <decorations decors="public static MVector operator*"></decorations>
		/// <param name="S">The Scalar to multiply.</param>
		/// <param name="V">The MVector.</param>
		/// <returns>(MVector V, float S) => new MVector(V.X * S, V.Y * S, V.Z * S)</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator *(float S, MVector V) => new MVector(V.X * S, V.Y * S, V.Z * S);
		/// <summary>Divides an MVector by a scalar on all components.</summary>
		/// <remarks>If d == 0, this will throw a DivideByZeroException.</remarks>
		/// <decorations decors="public static MVector operator/"></decorations>
		/// <param name="V">The MVector.</param>
		/// <param name="D">The denominator under all components.</param>
		/// <returns>(MVector V, float D) => float S = 1 / D; return S * V</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator /(MVector V, float D)
		{
			if (D == 0)
			{
				throw new DivideByZeroException("Attempted division by zero.");
			}

			return FInverse(D) * V;
		}

		/// <summary>The vector cross ^ product.</summary>
		/// <decorations decors="public static MVector operator^"></decorations>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns>(MVector Left, MVector Right) => new MVector(Left.Y * Right.Z - Left.Z * Right.Y, Left.Z * Right.X - Left.X * Right.Z, Left.X * Right.Y - Left.Y * Right.X)</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator ^(MVector Left, MVector Right) => new MVector(Left.Y * Right.Z - Left.Z * Right.Y, Left.Z * Right.X - Left.X * Right.Z, Left.X * Right.Y - Left.Y * Right.X);
		/// <summary>The vector dot | product.</summary>
		/// <remarks>Does not assume Left and Right are normalised.</remarks>
		/// <decorations decors="public static float operator|"></decorations>
		/// <param name="Left"></param>
		/// <param name="Right"></param>
		/// <returns>(MVector Left, MVector Right) => Left.X * Right.X + Left.Y * Right.Y + Left.Z * Right.Z</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float operator |(MVector Left, MVector Right) => Left.X * Right.X + Left.Y * Right.Y + Left.Z * Right.Z;

		/// <summary>Normalised direction from to.</summary>
		/// <decorations decors="public static MVector operator>"></decorations>
		/// <param name="From"></param>
		/// <param name="To"></param>
		/// <returns>(MVector From, MVector To) => (To - From).Normalised</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator >(MVector From, MVector To) => (To - From).Normalised;
		/// <summary>Normalised direction from to.</summary>
		/// <decorations decors="public static MVector operator&lt;"></decorations>
		/// <param name="From"></param>
		/// <param name="To"></param>
		/// <returns>(MVector To, MVector From) => From > To</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator <(MVector To, MVector From) => From > To;

		/// <summary>Shifts all components to the right.</summary>
		/// <decorations decors="public static MVector operator>>"></decorations>
		/// <param name="V">The MVector to shift.</param>
		/// <param name="I">The number of times to shift right.</param>
		/// <returns>X = Y, Y = Z, Z = X.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator >>(MVector V, int I)
		{
			if (I == 0)
				return V;

			if (I < 0)
				return V << I;

			I %= 3;

			for (int Shift = 0; Shift < I; ++Shift)
			{
				float X = V.X;
				Utils.Swap(ref V.X, ref V.Z);
				Utils.Swap(ref V.Z, ref V.Y);
				Utils.Swap(ref V.Y, ref X);
			}

			return V;
		}

		/// <summary>Shifts all components to the left.</summary>
		/// <decorations decors="public static MVector operator&lt;&lt;"></decorations>
		/// <param name="V">The MVector to shift.</param>
		/// <param name="I">The number of times to shift left.</param>
		/// <returns>X = Z, Y = X, Z = Y.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static MVector operator <<(MVector V, int I)
		{
			if (I == 0)
				return V;

			if (I < 0)
				return V >> I;

			I %= 3;

			for (int Shift = 0; Shift < I; ++Shift)
			{
				float X = V.X;
				Utils.Swap(ref V.X, ref V.Y);
				Utils.Swap(ref V.Y, ref V.Z);
				Utils.Swap(ref V.Z, ref X);
			}

			return V;
		}

		/// <summary>Compares two MVectors for equality.</summary>
		/// <decorations decors="public static bool operator=="></decorations>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <docreturns>True if the square distance between Left and Right is less than kEpsilon ^ 2.</docreturns>
		/// <returns>True if the square distance between Left and Right is less than <see cref="kEpsilon"/>^2.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(MVector Left, MVector Right)
		{
			float x = Left.X - Right.X;
			float y = Left.Y - Right.Y;
			float z = Left.Z - Right.Z;
			float Sqr = x * x + y * y + z * z;
			return Sqr < kEpsilon * kEpsilon;
		}

		/// <summary>Compares two MVectors for inequality.</summary>
		/// <decorations decors="public static bool operator!="></decorations>
		/// <param name="Left">Left-side comparison.</param>
		/// <param name="Right">Right-side comparison.</param>
		/// <returns>The opposite of operator ==.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(MVector Left, MVector Right) => !(Left == Right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool Equals(object O) => O is MVector V && Equals(V);
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Equals(MVector V) => this == V || FAbs(V.X - X + V.Y - Y + V.Z - Z) < 4f * kEpsilon;

		/// <summary><see langword="true"/>if the MVector is non-zero.</summary>
		/// <decorations decor="public static bool operator true"></decorations>
		/// <param name="M">The Vector to check.</param>
		/// <docreturns>M != MVector(NaN) &amp;&amp; M != MVector.Zero &amp;&amp; M.SqrMagnitude &gt; MVector.kEpsilon.</docreturns>
		/// <returns><paramref name="M"/> != <see cref="NaN"/> &amp;&amp; M != <see cref="Zero"/> &amp;&amp; <paramref name="M"/>'s <see cref="SqrMagnitude"/> &gt; <see cref="kEpsilon"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator true(MVector M) => M != NaN && M != Zero && M.SqrMagnitude > kEpsilon;

		/// <summary><see langword="true"/>if the MVector is zero or considered zero.</summary>
		/// <decorations decor="public static bool operator false"></decorations>
		/// <param name="M">The Vector to check.</param>
		/// <docreturns>M == MVector(NaN) &amp;&amp; M == MVector.Zero &amp;&amp; M.SqrMagnitude &lt; MVector.kEpsilon.</docreturns>
		/// <returns><paramref name="M"/> == <see cref="NaN"/> &amp;&amp; M == <see cref="Zero"/> &amp;&amp; <paramref name="M"/>'s <see cref="SqrMagnitude"/> &lt; <see cref="kEpsilon"/>.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator false(MVector M) => M == NaN || M == Zero || M.SqrMagnitude < kEpsilon;

		/// <summary>Automatic conversion from an MVector to a bool. <see cref="operator true(MVector)"/> and <see cref="operator false(MVector)"/>.</summary>
		/// <docs>Automatic conversion from an MVector to a bool. See operator true and operator false.</docs>
		/// <decorations decor="public static implicit operator bool"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator bool(MVector M) => M;

		/// <summary>Automatic conversion from an MVector to a Vector3.</summary>
		/// <decorations decors="public static implicit operator Vector3"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector3(MVector MVector) => new Vector3(MVector.X, MVector.Y, MVector.Z);
		/// <summary>Automatic conversion from an MVector to a Vector2.</summary>
		/// <remarks>Only the X and Y components are considered. The Z component is ignored.</remarks>
		/// <decorations decors="public static implicit operator Vector2"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Vector2(MVector MVector) => new Vector2(MVector.X, MVector.Y);

		/// <summary>Automatic conversion from a Vector3 to an MVector.</summary>
		/// <decorations decors="public static implicit operator MVector"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator MVector(Vector3 Vector) => new MVector(Vector);
		/// <summary>Automatic conversion from a Vector2 to an MVector.</summary>
		/// <remarks>The resulting MVector will have a Z equal to zero.</remarks>
		/// <decorations decors="public static implicit operator MVector"></decorations>
		public static implicit operator MVector(Vector2 Vector) => new MVector(Vector);

		/// <summary>The Colour representation of this MVector, in 0-255 XYZ/RGB.</summary>
		/// <decorations decors="public static implicit operator Color"></decorations>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator Color(MVector V) => Colour.Colour255(V.X, V.Y, V.Z);

		/// <summary>Hash code for use in Maps, Sets, MArrays, etc.</summary>
		/// <decorations decors="public override int"></decorations>
		/// <returns>GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() &lt;&lt; 2) ^ (Z.GetHashCode() &gt;&gt; 2)</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override int GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() << 2) ^ (Z.GetHashCode() >> 2);

		/// <summary>A human-readable MVector.</summary>
		/// <decorations decors="public override string"></decorations>
		/// <returns>ToString() => "X: " + X + " Y: " + Y + " Z: " + Z</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override string ToString() => "X: " + X + " Y: " + Y + " Z: " + Z;

		/// <summary>A human-readable MVector with formatting.</summary>
		/// <decorations decors="public string"></decorations>
		/// <param name="Format">The format to present float values.</param>
		/// <returns>A string with X, Y, and Z formatted according to Format.</returns>
		public string ToString(string Format)
		{
			return ToString(Format, null);
		}

		/// <summary>A human-readable MVector with formatting and a format provider.</summary>
		/// <decorations decors="public string"></decorations>
		/// <param name="Format">The format to present the float values.</param>
		/// <param name="Provider">The format provider to present the float values.</param>
		/// <returns>A string with X, Y, and Z formatting according to Format and Provider.</returns>
		public string ToString(string Format, IFormatProvider Provider)
		{
			if (string.IsNullOrEmpty(Format))
			{
				Format = "F1";
			}

			if (Provider == null)
			{
				Provider = CultureInfo.InvariantCulture.NumberFormat;
			}

			return $"X: {X.ToString(Format, Provider)} Y: {Y.ToString(Format, Provider)} Z: {Z.ToString(Format, Provider)}";
		}
	}

	/// <summary>MVector axes.</summary>
	/// <decorations decor="public enum : byte"></decorations>
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
