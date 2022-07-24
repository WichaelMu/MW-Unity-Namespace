using UnityEngine;

namespace MW
{
	/// <summary>An unsafe MVector that copies a Vector3 or normal MVector.</summary>
	/// <decorations decor="public ref struct"></decorations>
	public ref struct UVector
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

		public unsafe UVector(float* pX, float* pY, float* pZ)
		{
			this.pX = pX;
			this.pY = pY;
			this.pZ = pZ;
		}

		/// <summary>Copies the address of a Vector3's axes.</summary>
		/// <decorations decor="public static unsafe MVector"></decorations>
		/// <param name="V">The Vector3 to copy.</param>
		/// <returns>A new MVector initialised with XYZ pointers only.</returns>
		public static unsafe UVector Clone(ref Vector3 V)
		{
			fixed (Vector3* pV = &V)
			{
				return new UVector(&pV->x, &pV->y, &pV->z);
			}
		}

		/// <summary>Dereferences pointers into XYZ coordinates.</summary>
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
	}
}
