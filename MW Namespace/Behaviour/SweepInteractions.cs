using System;
using UnityEngine;
using UnityEngine.Events;

namespace MW.Behaviour
{
	/// <summary>A class defining how MBehaviours do Sweeps.</summary>
	[Serializable]
	public class SweepInteraction
	{
		/// <summary>Constant null reference to mark No Interaction.</summary>
		public const SweepInteraction kNoInteraction = null;

		static readonly SweepInteraction _default = new SweepInteraction();
		/// <summary>Default Sweep Interaction parameters.</summary>
		public static readonly SweepInteraction Default = _default;

		/// <summary>The radius of this Sweep's check for interactions.</summary>
		public float InteractingRadius;
		/// <summary>The layers this Sweep will consider for interactions.</summary>
		public LayerMask InteractingLayers;
		/// <summary>What happens when the Sweep makes an interaction?</summary>
		public UnityEvent<GameObject> Broadcast;

		/// <summary>Default Sweep Interaction parameters.</summary>
		public SweepInteraction()
		{
			InteractingRadius = 1f;
			InteractingLayers = ~0;
		}

		/// <summary>Initialise Sweep Interaction parameters with a Radius only.</summary>
		/// <param name="InteractingRadius">The radius of this Sweep's check for interactions.</param>
		public SweepInteraction(float InteractingRadius)
		{
			this.InteractingRadius = InteractingRadius;
		}

		/// <summary>Initialise Sweep Interaction parameters with a Radius and the interaction layer/s.</summary>
		/// <param name="InteractingRadius">The radius of this Sweep's check for interactions.</param>
		/// <param name="InteractingLayers">The layers this Sweep will consider for interactions.</param>
		public SweepInteraction(float InteractingRadius, LayerMask InteractingLayers) : this(InteractingRadius)
		{
			this.InteractingLayers = InteractingLayers;
		}

		/// <summary>Fully initialise a Sweep Interaction.</summary>
		/// <param name="InteractingRadius">The radius of this Sweep's check for interactions.</param>
		/// <param name="InteractingLayers">The layers this Sweep will consider for interactions.</param>
		/// <param name="OnObjectsInteraction">What happens when the Sweep makes an interaction?</param>
		public SweepInteraction(float InteractingRadius, LayerMask InteractingLayers, Action<GameObject> OnObjectsInteraction) : this(InteractingRadius, InteractingLayers)
		{
			Broadcast.AddListener(new UnityAction<GameObject>(OnObjectsInteraction));
		}

		internal void Execute(Collider[] Sweeped)
		{
			foreach (Collider S in Sweeped)
				Broadcast.Invoke(S.gameObject);
		}

		internal void Execute(Collider2D[] Sweeped)
		{
			foreach (Collider2D S in Sweeped)
				Broadcast.Invoke(S.gameObject);
		}

		public static implicit operator bool(SweepInteraction SI) => SI != null;
	}
}
