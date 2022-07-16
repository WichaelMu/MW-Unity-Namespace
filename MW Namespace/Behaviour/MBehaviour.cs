using UnityEngine;

namespace MW.Behaviour
{
	/// <summary></summary>
	/// <decorations decor="public class : MonoBehaviour"></decorations>
	public class MBehaviour : MonoBehaviour
	{
		/// <summary>The world position of this player.</summary>
		/// <decorations decor="public MVector"></decorations>
		public MVector Position { get => transform.position; set { transform.position = value; } }
		/// <summary>The rotation of this player.</summary>
		/// <decorations decor="public MRotator"></decorations>
		public MRotator Rotation
		{
			get
			{
				Vector3 E = transform.rotation.eulerAngles;
				return new MRotator(E.x, E.y, E.z);
			}
			set { transform.rotation = value.Quaternion(); }
		}

		protected bool bVisibleInGame;

		public virtual void Awake()
		{
			if (!bVisibleInGame && TryGetComponent(out MeshRenderer MR))
			{
				MR.enabled = false;
			}
		}

		/// <summary>
		/// Sets <see cref="Transform.position"/> to <paramref name="NewPosition"/>
		/// or the first <see cref="SweepInteraction"/> if <paramref name="bDoSweep"/>
		/// and <paramref name="bTeleportToPosition"/> are <see langword="true"/>.
		/// </summary>
		/// <docs>
		/// Sets this transform's position to NewPosition or the first SweepInteraction
		/// if bDoSweep and bTeleportToPosition are true.
		/// </docs>
		/// <decorations decor="public MArray{Collider}"></decorations>
		/// <param name="NewPosition">The target destination to go to, or go towards in a straight-line.</param>
		/// <param name="bTeleportToPosition">True to immediately teleport to the location, regardless of Sweep Interactions.</param>
		/// <param name="bDoSweep">
		/// True to teleport to the first Sweeped interaction, rather than NewPosition,
		/// if this transform intercepted another GameObject within the referenced SweepInteraction's Radius.</param>
		/// <param name="Interactions">Data containing how this transform interacts with other objects during Sweep.</param>
		/// <docreturns>An MArray&lt;Collider&gt; of GameObjects that were interacted with during sweeping, or null if bDoSweep is false.</docreturns>
		/// <returns><see cref="MArray{T}"/> of interacted <see cref="Collider"/>s during sweeping, or <see langword="null"/> if <paramref name="bDoSweep"/> is <see langword="false"/>.</returns>
		public MArray<Collider> SetObjectPosition3D(MVector NewPosition, bool bTeleportToPosition = true, bool bDoSweep = false, SweepInteraction Interactions = SweepInteraction.kNoInteraction)
		{
			MArray<Collider> Sweeped = null;

			if (bDoSweep && Interactions)
			{
				Sweeped = new MArray<Collider>();
				MVector PreMove = Position;
				MVector Direction = PreMove > NewPosition;

				const int kMaxSweepInteractionCount = 256;

				Collider[] SweepInteractions = new Collider[kMaxSweepInteractionCount];

				float Diameter = Interactions.InteractingRadius * 2f;
				float Iterations = MVector.Distance(PreMove, NewPosition) / Diameter;

				for (float F = 1; F < Iterations; F += Diameter)
				{
					MVector Current = F * Diameter * Direction;
					Physics.OverlapSphereNonAlloc(Current, Interactions.InteractingRadius, SweepInteractions);
					Sweeped.Push(SweepInteractions);
					Interactions.Execute(SweepInteractions);
				}
			}

			if (bTeleportToPosition || !Sweeped || Sweeped.IsEmpty())
			{
				Position = NewPosition;
			}
			else
			{
				Position = Sweeped[0].transform.position;
			}

			return Sweeped;
		}

		/// <summary>
		/// Sets <see cref="Transform.position"/> to <paramref name="NewPosition"/>
		/// or the first <see cref="SweepInteraction"/> if <paramref name="bDoSweep"/>
		/// and <paramref name="bTeleportToPosition"/> are <see langword="true"/>.
		/// </summary>
		/// <docs>
		/// Sets this transform's position to NewPosition or the first SweepInteraction
		/// if bDoSweep and bTeleportToPosition are true.
		/// </docs>
		/// <decorations decor="public MArray{Collider2D}"></decorations>
		/// <param name="NewPosition">The target destination to go to, or go towards in a straight-line.</param>
		/// <param name="bTeleportToPosition">True to immediately teleport to the location, regardless of Sweep Interactions.</param>
		/// <param name="bDoSweep">
		/// True to teleport to the first Sweeped interaction, rather than NewPosition,
		/// if this transform intercepted another GameObject within the referenced SweepInteraction's Radius.</param>
		/// <param name="Interactions">Data containing how this transform interacts with other objects during Sweep.</param>
		/// <docreturns>An MArray&lt;Collider2D&gt; of GameObjects that were interacted with during sweeping, or null if bDoSweep is false.</docreturns>
		/// <returns><see cref="MArray{T}"/> of interacted <see cref="Collider2D"/>s during sweeping, or <see langword="null"/> if <paramref name="bDoSweep"/> is <see langword="false"/>.</returns>
		public MArray<Collider2D> SetObjectPosition2D(MVector NewPosition, bool bTeleportToPosition = true, bool bDoSweep = false, SweepInteraction Interactions = SweepInteraction.kNoInteraction)
		{
			MArray<Collider2D> Sweeped = null;

			if (bDoSweep && Interactions)
			{
				Sweeped = new MArray<Collider2D>();
				MVector PreMove = Position;
				MVector Direction = PreMove > NewPosition;
				const int kMaxSweepInteractionCount = 256;

				Collider2D[] SweepInteractions = new Collider2D[kMaxSweepInteractionCount];

				float Diameter = Interactions.InteractingRadius * 2f;
				float Iterations = MVector.Distance(PreMove, NewPosition) / Diameter;

				for (float F = 1; F < Iterations; F += Diameter)
				{
					MVector Current = F * Diameter * Direction;
					Physics2D.OverlapCircleNonAlloc(Current, Interactions.InteractingRadius, SweepInteractions);
					Sweeped.Push(SweepInteractions);
					Interactions.Execute(SweepInteractions);
				}
			}

			if (bTeleportToPosition || !Sweeped || Sweeped.IsEmpty())
			{
				Position = NewPosition;
			}
			else
			{
				Position = Sweeped[0].transform.position;
			}

			return Sweeped;
		}
	}
}
