using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MW.IO;
using MW.MWPhysics;
using MW.General;
using MW.Audio;

namespace MW.Vehicle.Aircraft {
    public abstract class Aircraft : MonoBehaviour {
        /// <summary>Used to update the player's HUD with speed and altitude information.</summary>
        public event Action UpdateHUD;
        public event Action UpdateFlares;
        public event Action UpdateGForce;

        [Space()]
        [SerializeField] bool bNormalAircraft;

        [Space()]
        [Range(0f, 1f)] public float fHealth = 1f;

        [Header("Effects")]
        [SerializeField] ParticleSystem[] osExhaust;
        [SerializeField] TrailRenderer[] trVorticies;
        [SerializeField] Transform[] tLandingGear;

        [Header("HUD")]
        [SerializeField] TextMeshProUGUI tmpSpeedUI;
        [SerializeField] TextMeshProUGUI tmpAltitudeUI;
        public TextMeshProUGUI tmpVDegreesUI;

        [Header("Velocity")]
        [Tooltip("The maximum velocity in metres per second.")]
        [SerializeField] float fTerminalVelocity;
        [Range(1f, 100f)] [SerializeField] float fAccelerationDelta = 1f;
        [SerializeField] Equation EAccelerationEquation;

        [Header("Maximum Degrees Delta")]
        [SerializeField] float fTurnRadius;
        [Range(.01f, 1.5f)] [SerializeField] float fYawDelta;
        [Range(.01f, 1.5f)] [SerializeField] float fPitchDelta;
        [Range(.01f, 1.5f)] [SerializeField] float fNegativePitchDelta;
        [Range(.01f, 1.5f)] [SerializeField] float fRollDelta;
        /// <summary>Increases the rate of roll.</summary>
        const float kRollDelta = 15;

        [Header("Physics")]
        [SerializeField] AnimationCurve acAngularDragGraph;
        [SerializeField] float fWingArea;

        [HideInInspector]
        public Rigidbody rb;
        BoxCollider bc;
        LineRenderer lr;
        AudioControl aAudio = AudioControl.AAudioLogic;

        [HideInInspector] public Vector3 vPosition;

        /// <summary>The G-Force experienced by this aircraft.</summary>
        float fGForce = 1;
        float fLastSpeed = 1;
        float fCrossSection;
        bool bLandingGearDeployed;
        bool bLandInstruction = true;

        public void Start() {
            rb = GetComponent<Rigidbody>();
            bc = GetComponent<BoxCollider>();
            lr = GetComponent<LineRenderer>();

            UpdateFlares?.Invoke();

            fCrossSection = bc.size.x * transform.localScale.x * (bc.size.z * transform.localScale.z);

            Initialise();
        }

        public void Update() {
            vPosition = transform.position;
            UpdateHUD?.Invoke();

            //VortexLeft.SetActive(Speed() > 80);
            //VortexRight.SetActive(Speed() > 80);

            ShowVorticies(Speed());

            Tick();
        }

        /// <summary>Used for lerping.</summary>
        float t = 0;

        void FixedUpdate() {
            t = Time.deltaTime;

            CalculateGForce();
            UpdateGForce?.Invoke();

            DeployLandingGear(bLandInstruction);

            rb.AddForce(Physics.gravity, ForceMode.Acceleration);
            rb.AddForce(AirResistance(), ForceMode.Force);
        }

        public abstract void Initialise();
        public abstract void Tick();
        public abstract void PhysicsTick();

        public void ForwardThrust() {
            Kinematics.HomeTowards(rb, transform.forward + transform.position, Mathematics.Acceleration(EAccelerationEquation, Speed(), fAccelerationDelta, fTerminalVelocity), fTurnRadius);
		}

        public void AirBrakes() {
            Vector3 _AirBrake = -rb.velocity.normalized * Mathematics.Deceleration(Speed());
            float AirBrake = _AirBrake.magnitude;
            Vector3 _AirResist = AirResistance();
            float AirResist = _AirResist.magnitude;

            if (AirBrake < AirResist)
                rb.AddForce(_AirResist);
            else
                rb.AddForce(_AirBrake);


            StopExhaust();
		}

        public void Cruise() {
            if (!Stalling() && Speed() > 69.444444444444444444444444f)
                Kinematics.HomeTowards(rb, transform.forward + transform.position, Mathematics.Deceleration(Speed()), 0);

            StopExhaust();

		}

        #region 3 Plane Axis Movement

        /// <summary>Yaws the airplane along the Y-Axis.</summary>
        /// <param name="bNegative">True if yawing left.</param>
        public void Yaw(bool bNegative) {    //  Y
            transform.Rotate((!bNegative ? transform.up : -transform.up) * fYawDelta * ClampedSpeed() * AngularDrag(), Space.World);
        }

        /// <summary>Pitches the airplane along the X-Axis.</summary>
        /// <param name="bNegative">True if pitching upwards.</param>
        public void Pitch(bool bNegative) {  //  X
            transform.Rotate(!bNegative
                ? (transform.right * fNegativePitchDelta) * ClampedSpeed() * AngularDrag()
                : (-transform.right * fPitchDelta) * ClampedSpeed() * AngularDrag(), Space.World);
        }

        /// <summary>Rolls the airplane along the Z-Axis.</summary>
        /// <param name="bNegative">True if rolling right.</param>
        public void Roll(bool bNegative) {   //  Z
            if (bNormalAircraft) {
                float fRestrict = 1 - Mathf.Abs(transform.right.y);
                transform.RotateAround(transform.position, !bNegative ? transform.forward : -transform.forward, fRollDelta * kRollDelta * ClampedSpeed() * fRestrict);
            } else
                transform.RotateAround(transform.position, !bNegative ? transform.forward : -transform.forward, fRollDelta * kRollDelta * ClampedSpeed());
        }

        /// <summary>Smoothing for returning to level flight.</summary>
        float smooth = 0;

        /// <summary>Returns to level horizontal flight like a normal passenger plane.</summary>
        public void ReturnToLevelRoll() {
            if (bNormalAircraft) {
                float fRightAxis = transform.right.y;

                if (Mathf.Abs(fRightAxis) < .001f)
                    return;

                float SmoothRoll = Mathf.SmoothDamp(fRightAxis, 0, ref smooth, .03f, fRollDelta);
                transform.localEulerAngles += new Vector3(0, 0, -SmoothRoll);
            }
        }

        #endregion

        /// <returns>The direction where air resistance should be applied.</returns>
        Vector3 AirResistance() {
            return -(.5f * Speed() * Speed() * rb.drag * rb.velocity.normalized) * (acAngularDragGraph.Evaluate(AngularDrag()));
        }

        bool Stalling() {
            return rb.velocity.normalized.y < -.6f;
        }

        /// <returns>1 if rigidbody is moving in the direction it is facing. 0 if rigidbody is moving in a direction +-90 degrees from where it is facing. -1 if rigidbody is moving in the opposite direction of where it is facing.</returns>
        float AngularDrag() {
            Vector3 v = new Vector3 {
                x = Mathf.Abs(rb.velocity.normalized.x),
                y = Mathf.RoundToInt(Mathf.Abs(rb.velocity.normalized.y)),
                z = Mathf.Abs(rb.velocity.normalized.z)
            };

            Vector3 f = new Vector3 {
                x = Mathf.Abs(transform.forward.x),
                y = Mathf.Abs(transform.forward.y),
                z = Mathf.Abs(transform.forward.z)
            };

            return Mathf.Clamp(Vector3.Dot(v, f), .001f, .999f);
        }

        public Vector3 VLift() {
            return transform.up * FLift();
        }

        public float FLift() {
            return (Mathf.Pow(Speed(), 2) * .5f) * fCrossSection;
        }

        /// <summary>Updates the G-Force.</summary>
        public float CalculateGForce() {
            float CurrentSpeed = Speed();

            fGForce = Mathf.Abs((CurrentSpeed - fLastSpeed) / (Time.fixedDeltaTime * Physics.gravity.magnitude) + 1);
            fLastSpeed = CurrentSpeed;

            return fGForce;
        }

        /// <returns>The angle in degrees this airplane is from the horizon.</returns>
        public float HorizontalAngle() {

            Vector3 vForward = new Vector3 {
                x = 0,
                y = transform.forward.y,
                z = 0
            };

            float fXRotation = Mathf.RoundToInt(Vector3.Dot(Vector3.up, vForward) * 90);

            return fXRotation;
        }

        /// <returns>The speed of this aircraft per delta time.</returns>
        public float Speed() {
            return rb.velocity.magnitude;
        }

        /// <returns>The speed of this aircraft clamped at a minimum of zero and one by Time.deltaTime.</returns>
        float ClampedSpeed() {
            return Mathf.Clamp01(Speed() * Time.deltaTime);
        }

        void DeployLandingGear(bool bDeploy) {
            if (tLandingGear.Length != 0) {
                if (bDeploy) {
                    foreach (Transform T in tLandingGear)
                        T.localPosition = Vector3.Lerp(T.localPosition, new Vector3(T.localPosition.x, 0, T.localPosition.z), t);
                } else {
                    foreach (Transform T in tLandingGear)
                        T.localPosition = Vector3.Lerp(T.localPosition, new Vector3(T.localPosition.x, 40, T.localPosition.z), t);
                }

                bLandingGearDeployed = bDeploy;
            }
        }

        #region Effects

        /// <summary>Plays the particle system exhaust effect.</summary>
        void PlayExhaust() {
            if (osExhaust.Length == 0)
                return;
            else
                for (int i = 0; i < osExhaust.Length; i++)
                    osExhaust[i].Play();
        }

        /// <summary>Stops the particle system exhaust effect.</summary>
        void StopExhaust() {
            if (osExhaust.Length == 0)
                return;
            else
                for (int i = 0; i < osExhaust.Length; i++)
                    osExhaust[i].Stop();
        }

        /// <summary>Enables the trail renderer vorticies effect.</summary>
        void ShowVorticies(float fSpeed) {
            if (trVorticies.Length != 0) {
                if (trVorticies[0] == null)
                    return;

                float alpha = fSpeed / 300;

                for (int i = 0; i < trVorticies.Length; i++)
                    trVorticies[i].startColor = new Color(1, 1, 1, alpha);
            }
        }

        #endregion
    }

}
