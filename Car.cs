using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using MW;
using MW.MWPhysics;

namespace MW.Vehicle.Car {

	[RequireComponent(typeof(Rigidbody))]
	public abstract class Car : MonoBehaviour {

		public abstract void PreTick();
		public abstract void Initialise();
		public abstract void Tick();
		public abstract void PhysicsTick();

		Rigidbody rb;

		public const float kMaxSpeed = 400 * 3.6f;

		void Awake() {
			// ...
			rb = GetComponent<Rigidbody>();

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

		Gear gGearBox;

		public void SetGearBox(int nGears, float fReverseRatio, params float[] fRatios) {
			gGearBox = new Gear(nGears, fReverseRatio, fRatios);
		}

		public Pair<int, float> UpShift() {
			return gGearBox.UpShift();
		}
		public Pair<int, float> DownShift() {
			return gGearBox.DownShift();
		}

		#endregion

		#region Pedals

		public void Throttle() {
			float fGearRatio = 1 / gGearBox.CurrentGear().second;
			rb.AddForce(Forward() * Mathematics.Acceleration(Equation.EaseInOutSine, Speed(), fGearRatio, kMaxSpeed), ForceMode.Acceleration);
		}

		public void Brake() {
			Vector3 vAirBrake = -rb.velocity.normalized * Mathematics.Deceleration(Speed());
			Vector3 _AirResist = Aerodynamics.AirResistance(rb);
			float fAirResist = _AirResist.magnitude;

			rb.AddForce(vAirBrake * fAirResist);
		}

		public void Cruise() {
			Kinematics.HomeTowards(rb, transform.forward + transform.position, Mathematics.Deceleration(Speed()), 0);
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

	class Gear {
		public int nGears;

		public bool bNeutral, bReverse;

		Pair<int, float>[] pGearRatios;
		int nCurrentGear = -1;
		float fReverseRatio = 1;

		public Gear(int nGears, float fReverseRatio, params float[] fRatios) {
			this.nGears = nGears;
			this.fReverseRatio = fReverseRatio;

			pGearRatios = new Pair<int, float>[nGears];
			for (int i = 0; i < nGears; ++i) {
				pGearRatios[i] = new Pair<int, float>(i, fRatios[i]);
			}
		}

		public Pair<int, float> UpShift() {
			nCurrentGear++;
			nCurrentGear = Mathf.Min(nCurrentGear, nGears);

			return CurrentGear();
		}

		public Pair<int, float> DownShift() {
			nCurrentGear--;
			nCurrentGear = Mathf.Max(nCurrentGear, -2);

			return CurrentGear();
		}

		public Pair<int, float> CurrentGear() {
			if (nCurrentGear == -1) {
				return new Pair<int, float>((int)EGears.Neutral, 0);
			} else if (nCurrentGear == -2) {
				return new Pair<int, float>((int)EGears.Reverse, fReverseRatio);
			}

			return pGearRatios[nCurrentGear];
		}

		public enum EGears {
			Neutral = -1,
			Reverse = -2
		}
	}
}
