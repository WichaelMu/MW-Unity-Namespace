using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MW;
using MW.MWPhysics;
using MW.Easing;

namespace MW.Vehicle.Car {

	[RequireComponent(typeof(Rigidbody))]
	public abstract class Car : MonoBehaviour {

		/// <summary>Called before the first frame.</summary>
		public abstract void PreTick();
		/// <summary>Called just before the first frame.</summary>
		public abstract void Initialise();
		/// <summary>Called every frame.</summary>
		public abstract void Tick();
		/// <summary>Called every physics frame.</summary>
		public abstract void PhysicsTick();

		Rigidbody rb;

		/// <summary>Terminal velocity = 400 kmph (kmph * (2/3)).</summary>
		public const float kMaxSpeed = 111.111111111111111111f;

		void Awake() {
			// ...
			rb = gameObject.GetComponent<Rigidbody>();

			if (!rb) {
				Debug.LogWarning(name + " does not have an attached " + typeof(Rigidbody));
				Debug.Break();
			}

			PreTick();
		}

		void Start() {
			// ...

			Initialise();
		}

		void Update() {
			// ...

			Tick();
		}

		void FixedUpdate() {
			// ...

			PhysicsTick();
		}

		#region Transmission

		Transmission tTransmission;

		public Transmission GetTransmission() {
			if (!!tTransmission) {
				return tTransmission;
			}

			Debug.LogError("There is no transmission.");

			return null;
		}

		public void SetGearBox(int nGears, float fReverseRatio, params float[] fRatios) {
			tTransmission = new Transmission(nGears, fReverseRatio, fRatios);
		}

		public Pair<int, float> UpShift() {
			return tTransmission.UpShift();
		}
		public Pair<int, float> DownShift() {
			return tTransmission.DownShift();
		}

		#endregion

		#region Pedals

		public void Throttle(float fNormalisedInput) {
			if (!tTransmission) {
				Debug.LogError("Use Car.SetGearBox() to initialise the transmission.");
				Debug.Break();
			}

			Pair<int, float> pGear = tTransmission.CurrentGear();

			if (pGear.first == 0) {
				return;
			}

			float fGearRatio = pGear.second;
			float fForce = Mathematics.Acceleration(EEquation.EaseInSine, Speed() + fNormalisedInput, fGearRatio, kMaxSpeed * fGearRatio);

			//	Only reverse if the gear is -1 and the speed is <= 5.
			if (pGear.first == -1) {
				//	If the speed is not <= 5, then get out of reverse and back into neutral.
				if (Speed() <= 5) {
					Kinematics.HomeTowards(rb, -transform.forward + transform.position, fForce, 1);
					return;
				} else {
					UpShift();
				}
			}

			Kinematics.HomeTowards(rb, transform.forward + transform.position, fForce, 1);
		}

		public void Brake(float fNormlisedInput) {
			Kinematics.HomeTowards(rb, transform.forward + transform.position, Mathematics.Deceleration(Speed()) * fNormlisedInput, 0);
		}

		public void Cruise() {
			Kinematics.HomeTowards(rb, transform.forward + transform.position, Mathematics.Deceleration(Speed()) * .1f, 0);
		}

		#endregion

		#region Steering and Grip

		public void SteerLeft(float fInput) {

		}

		public void SteerRight(float fInput) {

		}

		#endregion

		public Vector3 Forward() {
			return transform.forward;
		}

		public Vector3 Right() {
			return transform.right;
		}

		public Collider[] ProximityCheck(float fRadius) {
			return Physics.OverlapSphere(transform.position, fRadius);
		}

		public Collider[] ProximityCheck(float fRadius, LayerMask lmCheck) {
			return Physics.OverlapSphere(transform.position, fRadius, lmCheck);
		}

		public float Speed() {
			return rb.velocity.magnitude;
		}
	}

	public class Transmission {
		public int nGears;

		public bool bNeutral, bReverse;

		Pair<int, float>[] pGearRatios;
		int nCurrentGear = 0;
		float fReverseRatio = 1;

		public Transmission(int nGears, float fReverseRatio, params float[] fRatios) {
			this.nGears = nGears;
			this.fReverseRatio = fReverseRatio;

			if (nGears > fRatios.Length) {
				Debug.LogWarning("Provide all " + nGears + " gear ratios.");
			}

			pGearRatios = new Pair<int, float>[nGears];
			for (int i = 0; i < nGears; ++i) {
				pGearRatios[i] = new Pair<int, float>(i, fRatios[i]);
			}

			if (nGears < fRatios.Length) {
				Debug.LogWarning("The transmission will only consider the first " + nGears + " gears.");
			}
		}

		public Pair<int, float> UpShift() {
			nCurrentGear++;
			nCurrentGear = Mathf.Min(nCurrentGear, nGears - 1);

			return CurrentGear();
		}

		public Pair<int, float> DownShift() {
			nCurrentGear--;
			nCurrentGear = Mathf.Max(nCurrentGear, -1);

			return CurrentGear();
		}

		public Pair<int, float> CurrentGear() {
			if (nCurrentGear == 0) {
				return new Pair<int, float>((int)EGears.Neutral, 0);
			} else if (nCurrentGear == -1) {
				return new Pair<int, float>((int)EGears.Reverse, fReverseRatio);
			}

			return pGearRatios[nCurrentGear];
		}

		public enum EGears {
			Neutral = 0,
			Reverse = -1
		}

		public static bool operator !(Transmission g) {
			return g == null;
		}
	}
}
