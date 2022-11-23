using MW.Diagnostics;
using UnityEngine;

namespace MW.Pathfinding
{
	/// <summary>The MonoBehavior script that manages pathfinding over frames.</summary>
	/// <typeparam name="T">Generic type that implements INode and IHeapItem for T that defines a traversable waypoint.</typeparam>
	/// <decorations decor="public class {T} : MonoBehaviour where T : INode{T}, IHeapItem{T}"></decorations>
	public class MPathManager<T> : MonoBehaviour where T : MNode, IHeapItem<T>
	{
		/// <summary>The number of paths to compute per frame.</summary>
		/// <decorations decor="[SerializeField] [Min(1)] uint"></decorations>
		[SerializeField]
		[Min(1)]
		[Tooltip("The number of paths to compute per frame.")]
		uint ComputationsPerFrame = 1;

		/// <summary>The number of frames between computing path/s.</summary>
		/// <decorations decor="[SerializeField] [Min(1)] uint"></decorations>
		[SerializeField]
		[Min(1)]
		[Tooltip("The number of frames before path/s are computed.")]
		uint FramesBetweenComputations = 1;

		bool bIsPaused = false;
		uint FrameTracker = 0;

		public virtual void Update()
		{
			#region First Compute Skip Explanation.
			/* Assume ComputationsPerFrame = 1. Assume FramesBeforeComputation = 1.
			 * First frame:
				* Will always skip the first call to compute paths.
				* 
				* Beginning of frame:
				* FrameTracker = 1.
				* No. Paths Computed = 0.
				* 
				* End of frame:
				* FrameTracker = 1.
				* No. Paths Computed = 0. // Skipped first frame computation.
				* 
			 * Second frame:
				* Beginning of frame:
				* FrameTracker = 1.
				* No. Paths Computed = 0.
				* 
				* End of frame:
				* FrameTracker = 0.
				* No.PathsComputed = 1.
				* 
			 * Third frame:
				* Beginning of frame:
				* FrameTracker = 0.
				* No. Paths Computed = 1.
				* 
				* End of frame:
				* FrameTracker = 0.
				* No. Paths Computed = 2.
				* 
			 * 
			 * ...
			 */
			#endregion

			if (!bIsPaused)
			{
				if (FrameTracker >= FramesBetweenComputations)
				{
					for (uint i = 0; i < ComputationsPerFrame; ++i)
						PathRegister<T>.ComputeNext();

					FrameTracker = 0;
				}

				FrameTracker++;
			}
		}

		/// <summary>Temporarily stop the computation of paths.</summary>
		/// <decorations decor="public void"></decorations>
		public void Pause()
		{
			bIsPaused = true;
		}

		/// <summary>Continue the computation of paths.</summary>
		/// <decorations decor="public void"></decorations>
		public void Resume()
		{
			bIsPaused = false;
		}

		/// <summary>Prints the current status of this Path Manager.</summary>
		/// <decorations decor="public EStatus"></decorations>
		/// <returns>If this Path Manager is currently Paused, or Running.</returns>
		public EStatus Status()
		{
			Log.P(nameof(MPathManager<T>), "is", bIsPaused ? "Paused." : "Running.");

			return bIsPaused ? EStatus.Paused : EStatus.Running;
		}

		public enum EStatus { Paused, Running }
	}
}
