using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MW {

    public enum Direction {
        forward,
        right,
        back,
        left,
        up,
        down
    }

    /// <seealso cref="https://easings.net/"/>
    public enum Equation {
        Linear,
        SineEaseOut,
        SineEaseIn,
        SineEaseInOut,
        SineEaseOutIn,
        CubicEaseOut,
        CubicEaseIn,
        CubicEaseInOut,
        CubicEaseOutIn,
        QuadEaseOut,
        QuadEaseIn,
        QuadEaseInOut,
        QuadEaseOutIn,
        QuartEaseIn,
        QuartEaseOut,
        QuartEaseInOut,
        QuartEaseOutIn,
        QuintEaseIn,
        QuintEaseOut,
        QuintEaseInOut,
        QuintEaseOutIn,
        ExpoEaseInOut,
        ExpoEaseOut,
        ExpoEaseIn,
        ExpoEaseOutIn,
        CircEaseOut,
        CircEaseIn,
        CircEaseInOut,
        CircEaseOutIn,
        ElasticEaseIn,
        ElasticEaseOut,
        ElasticEaseInOut,
        ElasticEaseOutIn,
        BounceEaseIn,
        BounceEaseOut,
        BounceEaseInOut,
        BounceEaseOutIn,
        BackEaseIn,
        BackEaseOut,
        BackEaseInOut,
        BackEaseOutIn,
    };

    public enum Units {
        MetresPerSecond,
        KilometresPerSecond,
        MetresPerHour,
        KilometrsePerHour,
        FeetPerSecond,
        MilesPerSecond,
        FeetPerHour,
        MilesPerHour
    }

    public static class Generic {

        public const float k10Percent = .1f;
        public const float kQuarter = .25f;
        public const float kHalf = .5f;
        public const float kThreeQuarters = .75f;
        public const float kOneThird = .3333333333333333333333333333333333f;
        public const float kTwoThirds = kOneThird * 2;

        public const float k1To255RGB = 0.0039215686274509803921568627451F;

        /// <summary>If self can see target within SearchAngle degrees while facing face.</summary>
        /// <param name="face">The direction self is facing.</param>
        /// <param name="self">The transform searching for target.</param>
        /// <param name="target">The transform to look out for.</param>
        /// <param name="SearchAngle">The maximum degrees to search for target.</param>
        public static bool InFOV(Direction face, Transform self, Transform target, float SearchAngle) {

            switch (face) {
                case Direction.forward:
                    return Vector3.Angle(self.forward, target.position - self.position) < SearchAngle;
                case Direction.right:
                    return Vector3.Angle(self.right, target.position - self.position) < SearchAngle;
                case Direction.back:
                    return Vector3.Angle(-self.forward, target.position - self.position) < SearchAngle;
                case Direction.left:
                    return Vector3.Angle(-self.right, target.position - self.position) < SearchAngle;
                case Direction.up:
                    return Vector3.Angle(self.up, target.position - self.position) < SearchAngle;
                case Direction.down:
                    return Vector3.Angle(-self.up, target.position - self.position) < SearchAngle;
                default:
                    Debug.LogWarning("There was a problem in determining a face direction");
                    return false;
            }
        }

        /// <summary>If self can see target within SearchAngle degrees while facing face.</summary>
        /// <param name="face">The direction self is facing.</param>
        /// <param name="self">The transform searching for target.</param>
        /// <param name="target">The position to look out for.</param>
        /// <param name="SearchAngle">The maximum degrees to search for target.</param>
        public static bool InFOV(Direction face, Transform self, Vector3 target, float SearchAngle) {

            switch (face) {
                case Direction.forward:
                    return Vector3.Angle(self.forward, target - self.position) < SearchAngle;
                case Direction.right:
                    return Vector3.Angle(self.right, target - self.position) < SearchAngle;
                case Direction.back:
                    return Vector3.Angle(-self.forward, target - self.position) < SearchAngle;
                case Direction.left:
                    return Vector3.Angle(-self.right, target - self.position) < SearchAngle;
                case Direction.up:
                    return Vector3.Angle(self.up, target - self.position) < SearchAngle;
                case Direction.down:
                    return Vector3.Angle(-self.up, target - self.position) < SearchAngle;
                default:
                    Debug.LogWarning("There was a problem in determining a face direction");
                    return false;
            }
        }

        /// <summary>If self has a line of sight to to.</summary>
        /// <param name="self">The position to look from.</param>
        /// <param name="to">The position to look to.</param>
        /// <param name="obstacles">The obstacles to consider obtrusive.</param>
        public static bool LineOfSight(Vector3 self, Vector3 to, LayerMask obstacles) {
            return !Physics.Linecast(self, to, obstacles);
        }

        /// <summary>If self has a line of sight to to.</summary>
        public static bool LineOfSight(Vector3 self, Vector3 to) {
            return !Physics.Linecast(self, to);
        }

        ///<summary>The fValue rounded to dp decimal places.</summary>
        /// <param name="fValue">The value to be rounded.</param>
        /// <param name="dp">The decimal places to be included.</param>
        public static float RoundToDP(float fValue, int dp = 2) {
            if (dp == 0) {
                Debug.LogWarning("Use Mathf.RoundToInt(" + nameof(fValue) + ") instead.");
                return Mathf.RoundToInt(fValue);
            }

            if (dp < 0) {
                Debug.LogWarning("Please use a number greater than 0. Rounding to 2 instead.");
                dp = 2;
            }

            int fFactor = (int)Mathf.Pow(10, dp);
            return Mathf.Round(fValue * fFactor) / fFactor;
        }

        /// <summary>Flip-Flops Bool.</summary>
        /// <param name="Bool"></param>
        public static void FlipFlop(ref bool Bool) {
            Bool = !Bool;
        }

        /// <summary>If value is within the +- limit of from.</summary>
        /// <param name="value">The value to check.</param>
        /// <param name="from">The value to compare.</param>
        /// <param name="limit">The limits to consider.</param>
        public static bool IsWithin(float value, float from, float limit) {
            if (limit == 0) {
                Debug.LogWarning("Use the '== 0' comparison operator instead.");
                return value == from;
            }
            if (limit < 0) {
                Debug.LogWarning("Please use a positive number");
                limit = Mathf.Abs(limit);
            }

            return (from + limit > value) && (value > from - limit);
        }

        /// <summary>The largest vector between L and R, according to magnitude.</summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        public static Vector3 Max(Vector3 L, Vector3 R) {
            return (L.magnitude < R.magnitude) ? R : L;
        }

        /// <summary>The smallest vector vector between L and R, according to magnitude.</summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        public static Vector3 Min(Vector3 L, Vector3 R) {
            return (L.magnitude > R.magnitude) ? R : L;
        }

        /// <summary>Returns the n'th Fibonacci number.</summary>
        /// <param name="n"></param>
        public static int Fibonacci(int n) {
            if (n <= 2)
                return 1;
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

        /// <summary>Generates spherical points with an equal distribution.</summary>
        /// <param name="resolution">The number of points to generate.</param>
        /// <param name="goldenRationModifier">Adjusts the golden ratio.</param>
        /// <returns>The Vector3[] points for the sphere.</returns>
        public static Vector3[] GenerateEqualSphere(int resolution, float goldenRationModifier) {
            Vector3[] directions = new Vector3[resolution];

            float phi = (1 + Mathf.Sqrt(goldenRationModifier) * .5f);
            float inc = Mathf.PI * 2 * phi;

            for (int i = 0; i < resolution; i++) {
                float t = (float)i / resolution;
                float incline = Mathf.Acos(1 - 2 * t);
                float azimuth = inc * i;

                float x = Mathf.Sin(incline) * Mathf.Cos(azimuth);
                float y = Mathf.Sin(incline) * Mathf.Sin(azimuth);
                float z = Mathf.Cos(incline);

                directions[i] = new Vector3(x, y, z);
            }

            return directions;
        }

        /// <summary>Generates the points to 'bridge' origin and target together at a height as an arc.</summary>
        /// <param name="origin">The starting point of the bridge.</param>
        /// <param name="target">The ending point of the bridge.</param>
        /// <param name="resolution">The number of points for the bridge.</param>
        /// <param name="height">The maximum height of the bridge.</param>
        /// <returns>The Vector3[] points for the bridge.</returns>
        public static Vector3[] Bridge(Vector3 origin, Vector3 target, int resolution, float height) {
            Vector3[] points = new Vector3[resolution];

            Vector3 dirToTarget = (target - origin).normalized;

            float theta = 0f;
            float horizontalIncrement = 2 * Mathf.PI / resolution;
            float resolutionToDistance = resolution * .5f - 1;
            float distanceIncrement = Vector3.Distance(target, origin) / resolutionToDistance;

            int k = 0;

            for (float i = 0; i < resolution; i += distanceIncrement) {
                float y = Mathf.Sin(theta);

                if (y < 0)
                    break;

                Vector3 point = new Vector3 {
                    x = dirToTarget.x * i,
                    y = y * height,
                    z = dirToTarget.z * i
                };

                points[k] = point;
                theta += horizontalIncrement;

                k++;
            }

            return points;
        }
    }

    public static class Conversion {

        /// <summary>The corresponding colour in RGA using Vector3.</summary>
        /// <param name="colour">The RGB/XYZ channel values, respectively.</param>
        public static Color Colour255(Vector3 colour) {
            colour *= Generic.k1To255RGB;

            for (int i = 0; i < 3; i++)
                colour[i] = Mathf.Clamp(colour[i], 0, 255);

            return new Color(colour.x, colour.y, colour.z);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public static Color Colour255(int r, int g, int b) {

            float _r = r * Generic.k1To255RGB;
            float _g = g * Generic.k1To255RGB;
            float _b = b * Generic.k1To255RGB;

            return new Color(_r, _g, _b);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGB.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public static Color Colour255(float r, float g, float b) {

            r *= Generic.k1To255RGB;
            g *= Generic.k1To255RGB;
            b *= Generic.k1To255RGB;

            return new Color(r, g, b);
        }

        /// <summary>The corresponding colour in RGBA using Vector4.</summary>
        /// <param name="colour">The RGBA/XYZW channel values, respectivaly.</param>
        public static Color Colour255(Vector4 colour) {
            colour *= Generic.k1To255RGB;

            for (int i = 0; i < 4; i++)
                colour[i] = Mathf.Clamp(colour[i], 0, 255);

            return new Color(colour.x, colour.y, colour.z, colour.w);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        /// <param name="a">The alpha value.</param>
        public static Color Colour255(int r, int g, int b, int a) {

            float _r = r * Generic.k1To255RGB;
            float _g = g * Generic.k1To255RGB;
            float _b = b * Generic.k1To255RGB;
            float _a = a * Generic.k1To255RGB;

            return new Color(_r, _g, _b, _a);
        }

        /// <summary>The corresponding colour from 0 - 255 in RGBA.</summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        /// <param name="a">The alpha value.</param>
        public static Color Colour255(float r, float g, float b, float a) {

            r *= Generic.k1To255RGB;
            g *= Generic.k1To255RGB;
            b *= Generic.k1To255RGB;
            a *= Generic.k1To255RGB;

            return new Color(r, g, b, a);
        }

        ///<summary>Converts a hexadecimal to its corresponding colour.</summary>
        /// <param name="hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
        public static Color ColourHex(string hex) {

            if (hex[0] != '#') {
                Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
                return Color.white;
            }

            if (hex.Length > 7) {
                Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
                return Color.white;
            }

            int nH1 = int.Parse(hex[1] + "" + hex[2] + "", System.Globalization.NumberStyles.HexNumber);
            int nH2 = int.Parse(hex[3] + "" + hex[4] + "", System.Globalization.NumberStyles.HexNumber);
            int nH3 = int.Parse(hex[5] + "" + hex[6] + "", System.Globalization.NumberStyles.HexNumber);

            Vector3 V = new Vector3 {
                x = nH1,
                y = nH2,
                z = nH3
            };

            return Colour255(V);
        }

        /// <summary>The corresponding hexadecimal and alpha colour.</summary>
        /// <param name="hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
        /// <param name="alpha">The float alpha.</param>
        public static Color ColourHex(string hex, float alpha) {

            if (hex[0] != '#') {
                Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
                return Color.white;
            }

            if (hex.Length > 7) {
                Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
                return Color.white;
            }

            int nH1 = int.Parse(hex[1] + "" + hex[2] + "", System.Globalization.NumberStyles.HexNumber);
            int nH2 = int.Parse(hex[3] + "" + hex[4] + "", System.Globalization.NumberStyles.HexNumber);
            int nH3 = int.Parse(hex[5] + "" + hex[6] + "", System.Globalization.NumberStyles.HexNumber);

            Vector4 V = new Vector4 {
                x = nH1,
                y = nH2,
                z = nH3,
                w = alpha
            };

            return Colour255(V);

        }

        /// <summary>The corresponding hexadecimal colour and hexadecimal alpha.</summary>
        /// <param name="hex">The hexadecimal in the format: "#RRGGBB"; where '#' denotes a hexadecimal, 'RR' denotes the Red colour channel, 'GG' denotes the Green colour channel and 'BB' denotes the Blue colour channel.</param>
        /// <param name="alpha">The hexadecimal in the format: "#AA"; where '#' denotes a hexadecimal and 'AA' denotes the Alpha channel.</param>
        public static Color ColourHex(string hex, string alpha) {

            if (hex[0] != '#' || alpha[0] != '#') {
                Debug.LogWarning("Please use '#' to denote Hex.\nReturning White by default.");
                return Color.white;
            }

            if (hex.Length > 7) {
                Debug.LogWarning("Please use the format: '#RRGGBB' for hex to colour conversion.\nReturning White by default");
                return Color.white;
            }

            if (alpha.Length > 3) {
                Debug.LogWarning("Please use the format: '#AA' for hex to alpha conversion.\nReturning White by default");
                return Color.white;
            }

            int nH1 = int.Parse(hex[1] + "" + hex[2] + "", System.Globalization.NumberStyles.HexNumber);
            int nH2 = int.Parse(hex[3] + "" + hex[4] + "", System.Globalization.NumberStyles.HexNumber);
            int nH3 = int.Parse(hex[5] + "" + hex[6] + "", System.Globalization.NumberStyles.HexNumber);
            int nH4 = int.Parse(alpha[1] + "" + alpha[2] + "", System.Globalization.NumberStyles.HexNumber);

            Vector4 V = new Vector4 {
                x = nH1,
                y = nH2,
                z = nH3,
                w = nH4
            };

            return Colour255(V);

        }

        /// <summary>The normalised direction to to, relative to from.</summary>
        /// <param name="from">The Vector3 seeking a direction to to.</param>
        /// <param name="to">The direction to look at.</param>
        public static Vector3 Direction(Vector3 from, Vector3 to) {
            return (to - from).normalized;
        }

        /// <summary>The normalised direction to to, relative to from.</summary>
        /// <param name="from">The Vector3 seeking a direction to to.</param>
        /// <param name="to">The direction to look at.</param>
        public static Vector2 Direction(Vector2 from, Vector2 to) {
            return (to - from).normalized;
        }
    }

    public static class Moveable {
        /// <summary>Convert inspector speed to m/s.</summary>
        public const int VelocityRatio = 50;

        /// <summary>
        /// If the distance between from and to is less than or EqualTo detection.
        /// </summary>
        /// <param name="from">The reference coordinate to compare.</param>
        /// <param name="to">The target coordinate to compare.</param>
        /// <param name="distance">The range that is considered if from has 'reached' to.</param>
        /// <param name="EqualTo">If this calculation needs to check if the distance between from and to are equal.</param>
        /// <returns></returns>
        public static bool HasReached(Vector3 from, Vector3 to, float distance = .1f, bool EqualTo = false) {
            if (EqualTo)
                return Vector3.Distance(from, to) < distance;
            return Vector3.Distance(from, to) <= distance;
        }

        /// <summary>
        /// If the distance between from and to is less than or EqualTo detection.
        /// </summary>
        /// <param name="from">The reference coordinate to compare.</param>
        /// <param name="to">The target coordinate to compare.</param>
        /// <param name="EqualTo">If this calculation needs to check if the distance between from and to are equal.</param>
        /// <param name="distance">The range that is considered if from has 'reached' to.</param>
        /// <returns></returns>
        public static bool HasReached(Vector3 from, Vector3 to, bool EqualTo = false, float distance = .1f) {
            if (EqualTo)
                return Vector3.Distance(from, to) < distance;
            return Vector3.Distance(from, to) <= distance;
        }

        /// <summary>
        /// Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.
        /// </summary>
        /// <param name="self">The Rigidbody to move.</param>
        /// <param name="target">The Transform destination.</param>
        /// <param name="velocity">The rate at which self moves towards target.</param>
        /// <param name="TurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody self, Transform target, float velocity, float TurnRadius) {

            Transform _self = self.transform;
            self.velocity = _self.forward * velocity * Time.deltaTime;
            self.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(target.position - _self.position), TurnRadius));
        }

        /// <summary>
        /// Moves self towards target while moving at velocity with a maximum turn angle of TurnRadius degrees.
        /// </summary>
        /// <param name="self">The Rigidbody to move.</param>
        /// <param name="target">The destination coordinates.</param>
        /// <param name="velocity">The rate at which self moves towards target.</param>
        /// <param name="TurnRadius">The maximum degrees self can turn towards target.</param>
        public static void HomeTowards(Rigidbody self, Vector3 target, float velocity, float TurnRadius) {

            Transform _self = self.transform;
            self.velocity = _self.forward * velocity * Time.deltaTime;
            self.MoveRotation(Quaternion.RotateTowards(_self.rotation, Quaternion.LookRotation(target - _self.position, _self.up), TurnRadius));
        }

        /// <param name="equation">The equation to use to accelerate.</param>
        /// <param name="CurrentSpeed">The current speed of the acceleration.</param>
        /// <param name="RateOfAcceleration">The rate to accelerate towards to terminal from current speed.</param>
        /// <param name="terminal">The maximum speed.</param>
        /// <param name="time">The elapsed time.</param>
        /// <returns>The acceleration value using Easing equation, using the current speed and rate of acceleration towards terminal by over time.</returns>
        public static float Acceleration(Equation equation, float CurrentSpeed, float RateOfAcceleration, float terminal, float time) {
            terminal *= VelocityRatio;

            if (RateOfAcceleration == 0)
                Debug.LogError(nameof(RateOfAcceleration) + " cannot be zero");

            return Mathf.Clamp(Mathf.Lerp(CurrentSpeed / Time.deltaTime, terminal, Easing.Ease(equation, time, 0, 1, RateOfAcceleration)), 0, terminal);
        }

        public static float Deceleration(float CurrentSpeed, float TargetVelocity = 0) {
            return -((TargetVelocity - CurrentSpeed) / Time.deltaTime);
        }

        static float fAR = 0;

        /// <param name="self">The rigidbody to calculate an acceleration rate.</param>
        /// <param name="Speed">The current speed of the rigidbody.</param>
        /// <returns>The float rate of movement in metres per second.</returns>
        public static float AccelerationRate(Rigidbody self, float Speed) {
            float a = self.velocity.magnitude - Speed / Time.deltaTime;
            float fAccelerationRate = (a - fAR) * -1;
            fAR = a;
            return fAccelerationRate;
        }

        /// <param name="self">The Rigidbody to read a speed from.</param>
        /// <param name="unit">The desired unit of measurement.</param>
        /// <returns>A speed reading from self in unit of measurement.</returns>
        public static float Speed(Rigidbody self, Units unit = Units.MetresPerSecond) {
            float speed = self.velocity.magnitude;

            switch (unit) {
                case Units.MetresPerSecond:
                    return speed;
                case Units.KilometrsePerHour:
                    return speed * 3.6f;
                case Units.MilesPerHour:
                    return speed * 2.23694f;
                case Units.KilometresPerSecond:
                    return speed * .001f;
                case Units.MetresPerHour:
                    return speed * 3600;
                case Units.FeetPerSecond:
                    return speed * 3.28084f;
                case Units.FeetPerHour:
                    return speed * 11811.02362f;
                case Units.MilesPerSecond:
                    return speed * 1609.34f;
                default:
                    Debug.LogWarning("Failed to convert speed to: " + nameof(unit) + "\nReturning metres per second.");
                    return speed;
            }
        }

        /// <summary>The direction of natural air resistance.</summary>
        /// <param name="self">The rigidbody to apply air resistance to.</param>
        public static Vector3 AirResitance(Rigidbody self) {
            return -(.5f * Speed(self) * Speed(self) * self.drag * self.velocity.normalized);
        }

        /// <summary>Interpolates between origin and destination using equation in duration.</summary>
        /// <param name="equation">The equation to use to interpolate.</param>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        /// <param name="duration">The duration of the interpolation.</param>
        /// <param name="start">The starting value of equation.</param>
        /// <param name="final">The final value of equation.</param>
        public static void V3Interpolate(Equation equation, Vector3 origin, Vector3 destination, float duration, float start = 0, float final = 1) {
            Vector3.Lerp(origin, destination, Easing.Ease(equation, Time.deltaTime, start, final, duration));
        }

        /// <summary>Interpolates between origin and destination using equation.</summary>
        /// <param name="equation">The equation to use to interpolate.</param>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        /// <param name="start">The starting value of equation.</param>
        /// <param name="final">The final value of equation.</param>
        public static void V3Interpolate(Equation equation, Vector3 origin, Vector3 destination, float start = 0, float final = 1) {
            Vector3.Lerp(origin, destination, Easing.Ease(equation, Time.deltaTime, start, final, 1));
        }

        /// <summary>Interpolates between origin and destination.</summary>
        /// <param name="equation">The equation to use for interpolation.</param>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        public static void V3Interpolate(Equation equation, Vector3 origin, Vector3 destination) {
            Vector3.Lerp(origin, destination, Easing.Ease(equation, Time.deltaTime, 0, 1, .3f));
        }

        /// <summary>Interpolates between origin and destination at a constant rate.</summary>
        /// <param name="origin">The origin of the interpolation.</param>
        /// <param name="destination">The destination of this interpolation.</param>
        public static void V3Interpolate(Vector3 origin, Vector3 destination) {
            Vector3.Lerp(origin, destination, Time.deltaTime);
        }


        /// <summary>The direction in which to avoid colliding with obstacles.</summary>
        /// <param name="self">The transform wanting to avoid collisions.</param>
        /// <param name="angle">The angle to search for potential collisions.</param>
        /// <param name="searchDistance">The distance to search for collisions.</param>
        /// <param name="obstacles">The layer to avoid colliding with.</param>
        /// <param name="debug">[EDITOR ONLY] Draw lines of the collision avoidance search. Red is the closest collision. Blue is the moving forward direction.</param>
        public static Vector3 CollisionAvoidance(Transform self, float angle, float searchDistance, LayerMask obstacles, bool debug) {
            Collider[] colliders = Physics.OverlapSphere(self.position, searchDistance, obstacles);

            float min = float.MaxValue;
            Vector3 dir = self.forward;

            for (int i = 0; i < colliders.Length; i++) {
                Vector3 closestPoint = colliders[i].ClosestPoint(self.position);
                if (Generic.InFOV(Direction.forward, self, closestPoint, angle)) {
                    float distance = Vector3.Distance(self.position, closestPoint);
                    if (distance < min) {
                        min = distance;
                        dir = closestPoint;
                    }
                }
            }


            Vector3 flipped = Vector3.Reflect(self.forward, (dir - self.position).normalized);

            Physics.Raycast(self.position, flipped, out RaycastHit hit, 500000f, obstacles);

            if (debug) {
                Debug.DrawRay(self.position, (dir - self.position).normalized * searchDistance, Color.red);
                Debug.DrawRay(self.position, (flipped - self.position).normalized, Color.blue);
            }

            return hit.point;
        }
    }

        public static class MWCamera {
        /// <summary>Have the camera follow target's transform.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's transform component.</param>
        public static void CameraFollow(Camera camera, Transform target) {
            camera.transform.position += target.position;
        }

        /// <summary>Have the camera to follow target's transform at an offset.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's transform component.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Camera camera, Transform target, Vector3 offset) {
            camera.transform.position += target.position + offset;
        }

        /// <summary>Have the camera follow target's position.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's position to follow.</param>
        public static void CameraFollow(Camera camera, Vector3 target) {
            camera.transform.position += target;
        }

        /// <summary>Have the camera follow target's position at an offset.</summary>
        /// <param name="camera">The camera to move.</param>
        /// <param name="target">The target's position to follow.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Camera camera, Vector3 target, Vector3 offset) {
            camera.transform.position += target + offset;
        }

        /// <summary>Have the main camera follow target's transform.</summary>
        public static void CameraFollow(Transform target) {
            Camera.main.transform.position = target.position;
        }

        /// <summary>Have the main camera follow target's transform at an offset.</summary>
        /// <param name="target">The target's transform component.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Transform target, Vector3 offset) {
            Camera.main.transform.position = target.position + offset;
        }

        /// <summary>Have the main camera follow target's position.</summary>
        public static void CameraFollow(Vector3 target) {
            Camera.main.transform.position = target;
        }

        /// <summary>Have the main camera follow target's position at an offset.</summary>
        /// <param name="target">The target's position.</param>
        /// <param name="offset">The target's position at an offset.</param>
        public static void CameraFollow(Vector3 target, Vector3 offset) {
            Camera.main.transform.position = target + offset;
        }

        /// <summary>Ensures the transform always faces the main camera.</summary>
        /// <param name="self">The transform to look towards the main camera.</param>
        public static void Billboard(Transform self) {
            self.LookAt(self.position + Camera.main.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces camera.</summary>
        /// <param name="self">The transform to look towards the camera.</param>
        /// <param name="camera">The camera to look at.</param>
        public static void Billboard(Transform self, Camera camera) {
            self.LookAt(self.position + camera.transform.rotation * Vector3.forward, Vector3.up);
        }

        /// <summary>Ensures the transform always faces point.</summary>
        /// <param name="self">The transform to look towards the point.</param>
        /// <param name="point">The transform of where self needs to look towards</param>
        public static void Billboard(Transform self, Transform point) {
            self.LookAt(point);
        }
    }

    /// <summary>Input/Output.</summary>
    public static class IO {

        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If the Left Mouse Button was clicked or Held.</returns>
        public static bool LeftClick(bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetMouseButtonUp(1);

            if (Hold)
                return Input.GetMouseButton(0);
            return Input.GetMouseButtonDown(0);
        }

        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If the Right Mouse Button was clicked or Held.</returns>
        public static bool RightClick(bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetMouseButtonUp(1);

            if (Hold)
                return Input.GetMouseButton(1);
            return Input.GetMouseButtonDown(1);
        }

        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If the Middle Mouse Button was clicked or Held.</returns>
        public static bool MiddleClick(bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetMouseButtonUp(2);

            if (Hold)
                return Input.GetMouseButton(2);
            return Input.GetMouseButtonDown(2);
        }

        /// <param name="Stroke">The key that was pressed on the keyboard.</param>
        /// <param name="Hold">Whether or not to check if this button is held down.</param>
        /// <param name="Up">Whether or not to check if this button is released.</param>
        /// <returns>If Stroke was pressed or Held.</returns>
        public static bool Key(KeyCode Stroke, bool Hold = false, bool Up = false) {
            if (Up)
                return Input.GetKeyUp(Stroke);

            if (Hold)
                return Input.GetKey(Stroke);
            return Input.GetKeyDown(Stroke);
        }
    }

    public static class HUD {

        /// <summary>
        /// Draws a line from to to in StartColor to EndColor at LineWidth thickness with an offset at UseWorldSpace with NumberOfSegments.
        /// </summary>
        /// <param name="self">The LineRenderer of the GameObject calling this.</param>
        /// <param name="from">The coordinates where the line will originate.</param>
        /// <param name="to">The coordinates where the line will end.</param>
        /// <param name="StartColour">The starting colour gradient for this line.</param>
        /// <param name="EndColour">The ending colour gradient for this line.</param>
        /// <param name="LineWidth">The thickness of this line.</param>
        /// <param name="offset">The offset to place this line.</param>
        /// <param name="material">The material used to draw the line.</param>
        /// <param name="UseWorldSpace">Should this line use world space?</param>
        /// <param name="NumberOfSegments">The number of verticies of this line.</param>
        public static void DrawLine(LineRenderer self, Vector3 from, Vector3 to, Color StartColour, Color EndColour, float LineWidth, Vector3 offset, Material material, bool UseWorldSpace = true, int NumberOfSegments = 1) {

            self.material = material;
            self.startColor = StartColour;
            self.endColor = EndColour;
            self.startWidth = LineWidth;
            self.endWidth = LineWidth;
            self.positionCount = NumberOfSegments + 1;
            self.useWorldSpace = UseWorldSpace;
            
            self.SetPosition(0, from + offset);
            self.SetPosition(1, to + offset);
        }

        /// <summary>
        /// Draws a circle with a centre at around at radius with a LineColour at LineWidth thickness at UseWorldSpace with NumberOfSegments.
        /// </summary>
        /// <param name="self">The LineRenderer of the GameObject calling this.</param>
        /// <param name="around">The centre of the circle to be drawn.</param>
        /// <param name="radius">The radius of this circle.</param>
        /// <param name="LineColour">The colour of this circle.</param>
        /// <param name="LineWidth">The thickness of this circle.</param>
        /// <param name="material">The material used to draw the circle.</param>
        /// <param name="UseWorldSpace">Should this circle use world space?</param>
        /// <param name="NumberOfSegments">The number of verticies of this circle.</param>
        public static void DrawCircle(LineRenderer self, Vector3 around, float radius, Color LineColour, float LineWidth, Material material, bool UseWorldSpace = true, int NumberOfSegments = 1) {
            
            self.material = material;
            self.startColor = LineColour;
            self.endColor = LineColour;
            self.startWidth = LineWidth;
            self.endWidth = LineWidth;
            self.positionCount = NumberOfSegments + 1;
            self.useWorldSpace = UseWorldSpace;

            float deltaTheta = (float)(2.0 * Mathf.PI) / NumberOfSegments;
            float theta = 0f;

            for (int i = 0; i < NumberOfSegments + 1; i++) {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, 0, z);
                self.SetPosition(i, pos + around);
                theta += deltaTheta;
            }
        }

        /// <summary>Scales the canvas element relative to self.</summary>
        /// <param name="self">The position to scale from.</param>
        /// <param name="ScaleWith">The position to scale with.</param>
        /// <returns>The relative scale size in Vector2.</returns>
        public static Vector2 ScaleSize(Vector3 self, Vector3 ScaleWith) {
            float scale = Mathf.Clamp(Vector3.Distance(self, ScaleWith), 1, 1000);
            scale *= .01f;
            return new Vector2(scale, scale) * .03f;
        }
    }

    public static class Easing {

#region Equations

#region Linear

        /// <summary>
        /// Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float Linear(float t, float b, float c, float d) {
            return c * t / d + b;
        }

#endregion
    
#region Expo

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseOut(float t, float b, float c, float d) {
            return (t == d) ? b + c : c * (-Mathf.Pow(2, -10 * t / d) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseIn(float t, float b, float c, float d) {
            return (t == 0) ? b : c * Mathf.Pow(2, 10 * (t / d - 1)) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseInOut(float t, float b, float c, float d) {
            if (t == 0)
                return b;

            if (t == d)
                return b + c;

            if ((t /= d / 2) < 1)
                return c / 2 * Mathf.Pow(2, 10 * (t - 1)) + b;

            return c / 2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return ExpoEaseOut(t * 2, b, c / 2, d);

            return ExpoEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Circular

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseOut(float t, float b, float c, float d) {
            return c * Mathf.Sqrt(1 - (t = t / d - 1) * t) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseIn(float t, float b, float c, float d) {
            return -c * (Mathf.Sqrt(1 - (t /= d) * t) - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1)
                return -c / 2 * (Mathf.Sqrt(1 - t * t) - 1) + b;

            return c / 2 * (Mathf.Sqrt(1 - (t -= 2) * t) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return CircEaseOut(t * 2, b, c / 2, d);

            return CircEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Quad

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseOut(float t, float b, float c, float d) {
            return -c * (t /= d) * (t - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t + b;

            return -c / 2 * ((--t) * (t - 2) - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return QuadEaseOut(t * 2, b, c / 2, d);

            return QuadEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Sine

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseOut(float t, float b, float c, float d) {
            return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseIn(float t, float b, float c, float d) {
            return -c * Mathf.Cos(t / d * (Mathf.PI / 2)) + c + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1)
                return c / 2 * (Mathf.Sin(Mathf.PI * t / 2)) + b;

            return -c / 2 * (Mathf.Cos(Mathf.PI * --t / 2) - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return SineEaseOut(t * 2, b, c / 2, d);

            return SineEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Cubic

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseOut(float t, float b, float c, float d) {
            return c * ((t = t / d - 1) * t * t + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t + b;

            return c / 2 * ((t -= 2) * t * t + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return CubicEaseOut(t * 2, b, c / 2, d);

            return CubicEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Quartic

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseOut(float t, float b, float c, float d) {
            return -c * ((t = t / d - 1) * t * t * t - 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t + b;

            return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return QuartEaseOut(t * 2, b, c / 2, d);

            return QuartEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Quintic

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseOut(float t, float b, float c, float d) {
            return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) < 1)
                return c / 2 * t * t * t * t * t + b;
            return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return QuintEaseOut(t * 2, b, c / 2, d);
            return QuintEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Elastic

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseOut(float t, float b, float c, float d) {
            if ((t /= d) == 1)
                return b + c;

            float p = d * .3f;
            float s = p / 4;

            return (c * Mathf.Pow(2, -10 * t) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) + c + b);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseIn(float t, float b, float c, float d) {
            if ((t /= d) == 1)
                return b + c;

            float p = d * .3f;
            float s = p / 4;

            return -(c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseInOut(float t, float b, float c, float d) {
            if ((t /= d / 2) == 2)
                return b + c;

            float p = d * (.3f * 1.5f);
            float s = p / 4;

            if (t < 1)
                return -.5f * (c * Mathf.Pow(2, 10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p)) + b;
            return c * Mathf.Pow(2, -10 * (t -= 1)) * Mathf.Sin((t * d - s) * (2 * Mathf.PI) / p) * .5f + c + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return ElasticEaseOut(t * 2, b, c / 2, d);
            return ElasticEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Bounce

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseOut(float t, float b, float c, float d) {
            if ((t /= d) < (1f / 2.75f))
                return c * (7.5625f * t * t) + b;
            else if (t < (2f / 2.75f))
                return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
            else if (t < (2.5f / 2.75f))
                return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
            else
                return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseIn(float t, float b, float c, float d) {
            return c - BounceEaseOut(d - t, 0, c, d) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseInOut(float t, float b, float c, float d) {
            if (t < d / 2)
                return BounceEaseIn(t * 2, 0, c, d) * .5f + b;
            else
                return BounceEaseOut(t * 2 - d, 0, c, d) * .5f + c * .5f + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return BounceEaseOut(t * 2, b, c / 2, d);
            return BounceEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#region Back

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseOut(float t, float b, float c, float d) {
            return c * ((t = t / d - 1) * t * ((1.70158f + 1) * t + 1.70158f) + 1) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseIn(float t, float b, float c, float d) {
            return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseInOut(float t, float b, float c, float d) {
            float s = 1.70158f;
            if ((t /= d / 2) < 1)
                return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseOutIn(float t, float b, float c, float d) {
            if (t < d / 2)
                return BackEaseOut(t * 2, b, c / 2, d);
            return BackEaseIn((t * 2) - d, b + c / 2, c / 2, d);
        }

#endregion
    
#endregion

        public static float Ease(Equation equation, float t, float b, float c, float d) {
            switch (equation) {
                case Equation.ExpoEaseInOut:
                    return ExpoEaseInOut(t, b, c, d);
                case Equation.ExpoEaseOut:
                    return ExpoEaseOut(t, b, c, d);
                case Equation.ExpoEaseIn:
                    return ExpoEaseIn(t, b, c, d);
                case Equation.ExpoEaseOutIn:
                    return ExpoEaseOutIn(t, b, c, d);
                case Equation.CircEaseOut:
                    return CircEaseOut(t, b, c, d);
                case Equation.CircEaseIn:
                    return CircEaseIn(t, b, c, d);
                case Equation.CircEaseInOut:
                    return CircEaseInOut(t, b, c, d);
                case Equation.CircEaseOutIn:
                    return CircEaseOutIn(t, b, c, d);
                case Equation.QuadEaseOut:
                    return QuadEaseOut(t, b, c, d);
                case Equation.QuadEaseIn:
                    return QuadEaseIn(t, b, c, d);
                case Equation.QuadEaseInOut:
                    return QuadEaseInOut(t, b, c, d);
                case Equation.QuadEaseOutIn:
                    return QuadEaseOutIn(t, b, c, d);
                case Equation.SineEaseOut:
                    return SineEaseOut(t, b, c, d);
                case Equation.SineEaseIn:
                    return SineEaseIn(t, b, c, d);
                case Equation.SineEaseInOut:
                    return SineEaseInOut(t, b, c, d);
                case Equation.SineEaseOutIn:
                    return SineEaseOutIn(t, b, c, d);
                case Equation.CubicEaseOut:
                    return CubicEaseOut(t, b, c, d);
                case Equation.CubicEaseIn:
                    return CubicEaseIn(t, b, c, d);
                case Equation.CubicEaseInOut:
                    return CubicEaseInOut(t, b, c, d);
                case Equation.CubicEaseOutIn:
                    return CubicEaseOutIn(t, b, c, d);
                case Equation.QuartEaseIn:
                    return QuartEaseIn(t, b, c, d);
                case Equation.QuartEaseOut:
                    return QuartEaseOut(t, b, c, d);
                case Equation.QuartEaseInOut:
                    return QuartEaseInOut(t, b, c, d);
                case Equation.QuartEaseOutIn:
                    return QuartEaseOutIn(t, b, c, d);
                case Equation.QuintEaseIn:
                    return QuintEaseIn(t, b, c, d);
                case Equation.QuintEaseOut:
                    return QuintEaseOut(t, b, c, d);
                case Equation.QuintEaseInOut:
                    return QuintEaseInOut(t, b, c, d);
                case Equation.QuintEaseOutIn:
                    return QuintEaseOutIn(t, b, c, d);
                case Equation.ElasticEaseIn:
                    return ElasticEaseIn(t, b, c, d);
                case Equation.ElasticEaseOut:
                    return ElasticEaseOut(t, b, c, d);
                case Equation.ElasticEaseInOut:
                    return ElasticEaseInOut(t, b, c, d);
                case Equation.ElasticEaseOutIn:
                    return ElasticEaseOutIn(t, b, c, d);
                case Equation.BounceEaseIn:
                    return BounceEaseIn(t, b, c, d);
                case Equation.BounceEaseOut:
                    return BounceEaseOut(t, b, c, d);
                case Equation.BounceEaseInOut:
                    return BounceEaseInOut(t, b, c, d);
                case Equation.BounceEaseOutIn:
                    return BounceEaseOutIn(t, b, c, d);
                case Equation.BackEaseIn:
                    return BackEaseIn(t, b, c, d);
                case Equation.BackEaseOut:
                    return BackEaseOut(t, b, c, d);
                case Equation.BackEaseInOut:
                    return BackEaseInOut(t, b, c, d);
                case Equation.BackEaseOutIn:
                    return BackEaseOutIn(t, b, c, d);
                case Equation.Linear:
                    return Linear(t, b, c, d);
                default:
                    Debug.LogError(equation + " cannot be calculated.");
                    return Linear(t, b, c, d);
            }
        }
    }

    [Serializable]
    public class Sound {
        public AudioClip Sounds;
        public string Name;

        [Range(0f, 1f)]
        public float Volume = 1, Pitch = 1;

        public bool Loop;

        [HideInInspector]
        public AudioSource sound;
    }

    /// <summary>The Audio controller for in-game sounds.</summary>
    public class Audio : MonoBehaviour {
        public static Audio AudioLogic;

        public bool MuteAll;
        public Sound[] Sounds;

        const string kErr1 = "Sound of name: ";
        const string kErr2 = " could not be ";

        /// <summary>Populates the Sounds array to match the settings.</summary>
        public void Initialise(Sound[] Sounds) {
            if (AudioLogic == null) {
                AudioLogic = this;
                gameObject.name = "Audio";
            } else {
                Debug.LogWarning("Ensure there is only one Audio object in the scene and that only one is being initialised");
                Destroy(gameObject);
            }

            this.Sounds = Sounds;

            if (!MuteAll)
                foreach (Sound s in Sounds) {
                    s.sound = gameObject.AddComponent<AudioSource>();
                    s.sound.clip = s.Sounds;
                    s.sound.volume = s.Volume;
                    s.sound.pitch = s.Pitch;
                    s.sound.loop = s.Loop;
                }
        }

        /// <summary>Plays sound of name n.</summary>
        /// <param name="n">The name of the requested sound to play in capital casing.</param>

        public void Play(string n) {
            if (MuteAll)
                return;
            Sound s = Find(n);
            if (s != null && !IsPlaying(s))
                s.sound.Play();
            if (s == null)
                Debug.LogWarning(kErr1 + n + kErr2 + "played!");
        }

        /// <summary>Stops sound of name n.</summary>
        /// <param name="n">The name of the requested sound to stop playing in capital casing.</param>

        public void Stop(string n) {
            if (MuteAll)
                return;
            Sound s = Find(n);
            if (s != null && IsPlaying(s))
                s.sound.Stop();
            if (s == null)
                Debug.LogWarning(kErr1 + n + kErr2 + "stopped!");
        }

        /// <summary>Stop every sound in the game.</summary>

        public void StopAll() {
            if (MuteAll)
                return;
            foreach (Sound s in Sounds)
                s.sound.Stop();
        }

        /// <summary>Returns a sound in the Sounds array.</summary>
        /// <param name="n">The name of the requested sound.</param>
        /// <returns>The sound clip of the requested sound.</returns>

        Sound Find(string n) {
            return Array.Find(Sounds, sound => sound.Name == n);
        }

        /// <summary>Whether or not sound of name n is playing.</summary>
        /// <param name="n">The name of the sound to query in capital casing.</param>
        public bool IsPlaying(string n) {
            if (MuteAll)
                return false;
            return Find(n).sound.isPlaying;
        }

        bool IsPlaying(Sound s) {
            return s.sound.isPlaying;
        }
    }

    /// <summary>Generates a new pair of two types of values.</summary>
    /// <typeparam name="T">The type of the first variable to store.</typeparam>
    /// <typeparam name="Y">The type of the second variable to store.</typeparam>
    public struct Pair <T, Y>{
        public T first { get; set; }
        public Y second { get; set; }

        public Pair(T first, Y second) {
            this.first = first;
            this.second = second;
        }
    }

    /// <summary>Generates a new variable of three types of values.</summary>
    /// <typeparam name="T">The type of the first variable to store.</typeparam>
    /// <typeparam name="Y">The type of the second variable to store.</typeparam>
    /// <typeparam name="U">The type of the third variable to store.</typeparam>
    public struct Triple<T, Y, U> {
        public T first { get; set; }
        public Y second { get; set; }
        public U third { get; set; }

        public Triple(T first, Y second, U third) {
            this.first = first;
            this.second = second;
            this.third = third;
        }
    }
}