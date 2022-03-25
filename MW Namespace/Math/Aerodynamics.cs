using UnityEngine;
using MW.Math;

namespace MW.Kinetic
{
	/// <summary>Computations for Aerodynamics.</summary>
	public static class Aerodynamics
	{
		/// <summary>The direction of natural air resistance.</summary>
		/// <param name="Self">The Rigidbody to apply air resistance to.</param>
		public static Vector3 AirResistance(Rigidbody Self)
		{
			return -(.5f * Mathematics.Speed(Self) * Mathematics.Speed(Self) * Self.drag * Self.velocity.normalized);
		}

		/// <summary>The scale of lift applied to a wing with WingArea travelling at Velocity through a fluid at Density with LiftCoefficient.</summary>
		/// <param name="LiftCoefficient">The heuristic coefficient for lift.</param>
		/// <param name="Density">The density of the fluid.</param>
		/// <param name="Velocity">The speed at which the wing is travelling.</param>
		/// <param name="WingArea">The area of the wing.</param>
		public static float Lift(float LiftCoefficient, float Density, float Velocity, float WingArea)
		    => LiftCoefficient * WingArea * .5f * Density * Velocity * Velocity;
	}
}
