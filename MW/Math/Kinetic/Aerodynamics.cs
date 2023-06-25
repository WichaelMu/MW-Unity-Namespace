#if RELEASE
using MW.Math;
using UnityEngine;
#endif // RELEASE

namespace MW.Kinetic
{
	/// <summary>Computations for Aerodynamics.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Aerodynamics
	{
#if RELEASE
		/// <summary>The direction of natural air resistance.</summary>
		/// <decorations decor="public static Vector3"></decorations>
		/// <param name="Self">The Rigidbody to apply air resistance to.</param>
		public static Vector3 AirResistance(Rigidbody Self)
		{
			float Speed = Mathematics.Speed(Self);
			return -(.5f * Speed * Speed * Self.drag * Self.velocity.normalized);
		}
#endif // RELEASE

		/// <summary>The scale of lift applied to a wing with WingArea travelling at Velocity through a fluid at Density with LiftCoefficient.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="LiftCoefficient">The heuristic coefficient for lift.</param>
		/// <param name="Density">The density of the fluid.</param>
		/// <param name="Velocity">The speed at which the wing is travelling.</param>
		/// <param name="WingArea">The area of the wing.</param>
		public static float Lift(float LiftCoefficient, float Density, float Velocity, float WingArea)
		    => LiftCoefficient * WingArea * .5f * Density * Velocity * Velocity;
	}
}
