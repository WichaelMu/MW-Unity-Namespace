using System.Runtime.CompilerServices;
using MW.Kinetic;
using UnityEngine;

namespace MW.Extensions
{
	/// <summary>Utility extension methods for Unity Rigidbody and Rigidbody 2D Physics.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class UnityPhysicsExtensions
	{
		/// <summary>Fast magnitude of a Rigidbody's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody to get the magnitude of.</param>
		/// <returns>The magnitude of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FMagnitude(this Rigidbody R) => R.velocity.FMagnitude();

		/// <summary>Fast magnitude of a Rigidbody2D's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody2D to get the magnitude of.</param>
		/// <returns>The magnitude of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FMagnitude(this Rigidbody2D R) => R.velocity.FMagnitude();

		/// <summary>Fast direction of a Rigidbody's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody to get the direction of.</param>
		/// <returns>The direction of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FDirection(this Rigidbody R) => R.velocity.FNormalise();

		/// <summary>Fast direction of a Rigidbody2D's velocity.</summary>
		/// <decorations decor="|Extension| float"></decorations>
		/// <param name="R">The Rigidbody2D to get the direction of.</param>
		/// <returns>The direction of R's velocity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 FDirection(this Rigidbody2D R) => R.velocity.FNormalise();

		/// <summary>Launches this Rigidbody towards Target at LaunchSpeed in an arc.</summary>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="R">The Rigidbody to launch.</param>
		/// <param name="Target">The position to hit.</param>
		/// <param name="LaunchSpeed">The speed to launch at.</param>
		/// <param name="bUseHighArc">True to use a high arc trajectory, false otherwise.</param>
		/// <param name="bDebug">True to debug the trajectory of the launch.</param>
		/// <returns>True if R can be launched at LaunchSpeed to Target.</returns>
		public static bool Launch(this Rigidbody R, MVector Target, float LaunchSpeed, bool bUseHighArc, bool bDebug = false)
		{
			bool bCanLaunch = Kinematics.LaunchTowards(out MVector LaunchVelocity, R.position, Target, LaunchSpeed, bUseHighArc, true, bDebug);
			if (bCanLaunch)
				R.velocity = LaunchVelocity;
			return bCanLaunch;
		}

		/// <summary>Launches this Rigidbody towards Target by reaching a Peak Height, then arcing downwards.</summary>
		/// <decorations decor="|Extension| void"></decorations>
		/// <param name="R">The Rigidbody to launch.</param>
		/// <param name="Target">The position to hit.</param>
		/// <param name="PeakHeight">The maximum altitude the Rigidbody should reach before arcing downwards towards Target.</param>
		/// <param name="Time">The time for R to reach Target.</param>
		/// <param name="bLaunchRegardless">True if R should launch and hit Target, regardless of Peak Height.</param>
		public static void Launch(this Rigidbody R, MVector Target, float PeakHeight, out float Time, bool bLaunchRegardless)
		{
			MVector LaunchVelocity = Kinematics.LaunchTowards(R.position, Target, PeakHeight, out Time, true, bLaunchRegardless);
			if (LaunchVelocity != MVector.NaN)
				R.velocity = LaunchVelocity;
		}

		/// <summary>Launches this Rigidbody towards Target at LaunchSpeed in an arc.</summary>
		/// <decorations decor="|Extension| bool"></decorations>
		/// <param name="R">The Rigidbody to launch.</param>
		/// <param name="Target">The position to hit.</param>
		/// <param name="LaunchSpeed">The speed to launch at.</param>
		/// <param name="bUseHighArc">True to use a high arc trajectory, false otherwise.</param>
		/// <param name="bDebug">True to debug the trajectory of the launch.</param>
		/// <returns>True if R can be launched at LaunchSpeed to Target.</returns>
		public static bool Launch(this Rigidbody2D R, MVector Target, float LaunchSpeed, bool bUseHighArc, bool bDebug = false)
		{
			bool bCanLaunch = Kinematics.LaunchTowards(out MVector LaunchVelocity, R.position, Target, LaunchSpeed, bUseHighArc, false, bDebug);
			if (bCanLaunch)
				R.velocity = LaunchVelocity;
			return bCanLaunch;
		}

		/// <summary>Launches this Rigidbody towards Target by reaching a Peak Height, then arcing downwards.</summary>
		/// <decorations decor="|Extension| void"></decorations>
		/// <param name="R">The Rigidbody to launch.</param>
		/// <param name="Target">The position to hit.</param>
		/// <param name="PeakHeight">The maximum altitude the Rigidbody should reach before arcing downwards towards Target.</param>
		/// <param name="Time">The time for R to reach Target.</param>
		/// <param name="bLaunchRegardless">True if R should launch and hit Target, regardless of Peak Height.</param>
		public static void Launch(this Rigidbody2D R, MVector Target, float PeakHeight, out float Time, bool bLaunchRegardless)
		{
			MVector LaunchVelocity = Kinematics.LaunchTowards(R.position, Target, PeakHeight, out Time, false, bLaunchRegardless);
			if (LaunchVelocity)
				R.velocity = LaunchVelocity;
		}
	}
}
