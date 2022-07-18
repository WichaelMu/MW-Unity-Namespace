using UnityEngine;

namespace MW
{
	public partial struct MVector
	{
		/// <summary>Copies the address of a Vector3's axes.</summary>
		/// <decorations decor="public static unsafe MVector"></decorations>
		/// <param name="V">The Vector3 to copy.</param>
		/// <returns>A new MVector initialised with XYZ pointers only.</returns>
		public static unsafe MVector Clone(ref Vector3 V)
		{
			MVector M = new MVector();

			fixed (Vector3* pV = &V)
			{
				MVector* pM = &M;
				pM->pX = &pV->x;
				pM->pY = &pV->y;
				pM->pZ = &pV->z;
			}

			return M;
		}

		/// <summary>Dereferences pointers into XYZ coordinates.</summary>
		/// <decorations decor="public unsafe MVector"></decorations>
		/// <returns>This MVector with dereferenced components.</returns>
		public unsafe MVector Dereference()
		{
			X = *pX;
			Y = *pY;
			Z = *pZ;

			return this;
		}
	}
}
