using UnityEngine;
using MW.Math;

namespace MW.Kinetic
{
        /// <summary>Computations for Aerodynamics.</summary>
	public static class Aerodynamics {

        /// <summary>The direction of natural air resistance.</summary>
        /// <param name="RSelf">The rigidbody to apply air resistance to.</param>
        public static Vector3 AirResistance(Rigidbody RSelf) {
            return -(.5f * Mathematics.Speed(RSelf) * Mathematics.Speed(RSelf) * RSelf.drag * RSelf.velocity.normalized);
        }

        /// <summary>The scale of lift applied to a wing with fWingArea travelling at fVelocity through a fluid at fDensity with fLiftCoefficient.</summary>
        /// <param name="fLiftCoefficient">The heuristic coefficient for lift.</param>
        /// <param name="fDensity">The density of the fluid.</param>
        /// <param name="fVelocity">The speed at which the wing is travelling.</param>
        /// <param name="fWingArea">The area of the wing.</param>
        public static float Lift(float fLiftCoefficient, float fDensity, float fVelocity, float fWingArea)
            => fLiftCoefficient * fWingArea * .5f * fDensity * fVelocity * fVelocity;
    }
}
