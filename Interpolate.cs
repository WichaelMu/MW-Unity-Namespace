using UnityEngine;

namespace MW.Easing {

    public static class Interpolate {

        #region Equations

        const float kNATURAL_LOG_OF_2 = 0.693147181f;

        public static float Linear(float start, float end, float value) {
            return Mathf.Lerp(start, end, value);
        }

        public static float Spring(float start, float end, float value) {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        public static float EaseInQuad(float start, float end, float value) {
            end -= start;
            return end * value * value + start;
        }

        public static float EaseOutQuad(float start, float end, float value) {
            end -= start;
            return -end * value * (value - 2) + start;
        }

        public static float EaseInOutQuad(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value + start;
            value--;
            return -end * 0.5f * (value * (value - 2) - 1) + start;
        }

        public static float EaseInCubic(float start, float end, float value) {
            end -= start;
            return end * value * value * value + start;
        }

        public static float EaseOutCubic(float start, float end, float value) {
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }

        public static float EaseInOutCubic(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value + 2) + start;
        }

        public static float EaseInQuart(float start, float end, float value) {
            end -= start;
            return end * value * value * value * value + start;
        }

        public static float EaseOutQuart(float start, float end, float value) {
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }

        public static float EaseInOutQuart(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value + start;
            value -= 2;
            return -end * 0.5f * (value * value * value * value - 2) + start;
        }

        public static float EaseInQuint(float start, float end, float value) {
            end -= start;
            return end * value * value * value * value * value + start;
        }

        public static float EaseOutQuint(float start, float end, float value) {
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }

        public static float EaseInOutQuint(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value * value * value + 2) + start;
        }

        public static float EaseInSine(float start, float end, float value) {
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }

        public static float EaseOutSine(float start, float end, float value) {
            end -= start;
            return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
        }

        public static float EaseInOutSine(float start, float end, float value) {
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
        }

        public static float EaseInExpo(float start, float end, float value) {
            end -= start;
            return end * Mathf.Pow(2, 10 * (value - 1)) + start;
        }

        public static float EaseOutExpo(float start, float end, float value) {
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
        }

        public static float EaseInOutExpo(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }

        public static float EaseInCirc(float start, float end, float value) {
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }

        public static float EaseOutCirc(float start, float end, float value) {
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;
        }

        public static float EaseInOutCirc(float start, float end, float value) {
            value /= .5f;
            end -= start;
            if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
            value -= 2;
            return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
        }

        public static float EaseInBounce(float start, float end, float value) {
            end -= start;
            float d = 1f;
            return end - EaseOutBounce(0, end, d - value) + start;
        }

        public static float EaseOutBounce(float start, float end, float value) {
            value /= 1f;
            end -= start;
            if (value < (1 / 2.75f)) {
                return end * (7.5625f * value * value) + start;
            } else if (value < (2 / 2.75f)) {
                value -= (1.5f / 2.75f);
                return end * (7.5625f * (value) * value + .75f) + start;
            } else if (value < (2.5 / 2.75)) {
                value -= (2.25f / 2.75f);
                return end * (7.5625f * (value) * value + .9375f) + start;
            } else {
                value -= (2.625f / 2.75f);
                return end * (7.5625f * (value) * value + .984375f) + start;
            }
        }

        public static float EaseInOutBounce(float start, float end, float value) {
            end -= start;
            float d = 1f;
            if (value < d * 0.5f) return EaseInBounce(0, end, value * 2) * 0.5f + start;
            else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
        }

        public static float EaseInBack(float start, float end, float value) {
            end -= start;
            value /= 1;
            float s = 1.70158f;
            return end * (value) * value * ((s + 1) * value - s) + start;
        }

        public static float EaseOutBack(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;
            return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
        }

        public static float EaseInOutBack(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value /= .5f;
            if ((value) < 1) {
                s *= (1.525f);
                return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
            }
            value -= 2;
            s *= (1.525f);
            return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
        }

        public static float EaseInElastic(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
        }

        public static float EaseOutElastic(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d) == 1) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p * 0.25f;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
        }

        public static float EaseInOutElastic(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (value == 0) return start;

            if ((value /= d * 0.5f) == 2) return start + end;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
            return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end + start;
        }

        //
        // These are derived functions that the motor can use to get the speed at a specific time.
        //
        // The easing functions all work with a normalized time (0 to 1) and the returned value here
        // reflects that. Values returned here should be divided by the actual time.
        //
        // TODO: These functions have not had the testing they deserve. If there is odd behavior around
        //       dash speeds then this would be the first place I'd look.

        public static float LinearD(float start, float end, float value) {
            return end - start;
        }

        public static float EaseInQuadD(float start, float end, float value) {
            return 2f * (end - start) * value;
        }

        public static float EaseOutQuadD(float start, float end, float value) {
            end -= start;
            return -end * value - end * (value - 2);
        }

        public static float EaseInOutQuadD(float start, float end, float value) {
            value /= .5f;
            end -= start;

            if (value < 1) {
                return end * value;
            }

            value--;

            return end * (1 - value);
        }

        public static float EaseInCubicD(float start, float end, float value) {
            return 3f * (end - start) * value * value;
        }

        public static float EaseOutCubicD(float start, float end, float value) {
            value--;
            end -= start;
            return 3f * end * value * value;
        }

        public static float EaseInOutCubicD(float start, float end, float value) {
            value /= .5f;
            end -= start;

            if (value < 1) {
                return (3f / 2f) * end * value * value;
            }

            value -= 2;

            return (3f / 2f) * end * value * value;
        }

        public static float EaseInQuartD(float start, float end, float value) {
            return 4f * (end - start) * value * value * value;
        }

        public static float EaseOutQuartD(float start, float end, float value) {
            value--;
            end -= start;
            return -4f * end * value * value * value;
        }

        public static float EaseInOutQuartD(float start, float end, float value) {
            value /= .5f;
            end -= start;

            if (value < 1) {
                return 2f * end * value * value * value;
            }

            value -= 2;

            return -2f * end * value * value * value;
        }

        public static float EaseInQuintD(float start, float end, float value) {
            return 5f * (end - start) * value * value * value * value;
        }

        public static float EaseOutQuintD(float start, float end, float value) {
            value--;
            end -= start;
            return 5f * end * value * value * value * value;
        }

        public static float EaseInOutQuintD(float start, float end, float value) {
            value /= .5f;
            end -= start;

            if (value < 1) {
                return (5f / 2f) * end * value * value * value * value;
            }

            value -= 2;

            return (5f / 2f) * end * value * value * value * value;
        }

        public static float EaseInSineD(float start, float end, float value) {
            return (end - start) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * value);
        }

        public static float EaseOutSineD(float start, float end, float value) {
            end -= start;
            return (Mathf.PI * 0.5f) * end * Mathf.Cos(value * (Mathf.PI * 0.5f));
        }

        public static float EaseInOutSineD(float start, float end, float value) {
            end -= start;
            return end * 0.5f * Mathf.PI * Mathf.Sin(Mathf.PI * value);
        }
        public static float EaseInExpoD(float start, float end, float value) {
            return (10f * kNATURAL_LOG_OF_2 * (end - start) * Mathf.Pow(2f, 10f * (value - 1)));
        }

        public static float EaseOutExpoD(float start, float end, float value) {
            end -= start;
            return 5f * kNATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 1f - 10f * value);
        }

        public static float EaseInOutExpoD(float start, float end, float value) {
            value /= .5f;
            end -= start;

            if (value < 1) {
                return 5f * kNATURAL_LOG_OF_2 * end * Mathf.Pow(2f, 10f * (value - 1));
            }

            value--;

            return (5f * kNATURAL_LOG_OF_2 * end) / (Mathf.Pow(2f, 10f * value));
        }

        public static float EaseInCircD(float start, float end, float value) {
            return ((end - start) * value) / Mathf.Sqrt(1f - value * value);
        }

        public static float EaseOutCircD(float start, float end, float value) {
            value--;
            end -= start;
            return (-end * value) / Mathf.Sqrt(1f - value * value);
        }

        public static float EaseInOutCircD(float start, float end, float value) {
            value /= .5f;
            end -= start;

            if (value < 1) {
                return (end * value) / (2f * Mathf.Sqrt(1f - value * value));
            }

            value -= 2;

            return (-end * value) / (2f * Mathf.Sqrt(1f - value * value));
        }

        public static float EaseInBounceD(float start, float end, float value) {
            end -= start;
            float d = 1f;

            return EaseOutBounceD(0, end, d - value);
        }

        public static float EaseOutBounceD(float start, float end, float value) {
            value /= 1f;
            end -= start;

            if (value < (1 / 2.75f)) {
                return 2f * end * 7.5625f * value;
            } else if (value < (2 / 2.75f)) {
                value -= (1.5f / 2.75f);
                return 2f * end * 7.5625f * value;
            } else if (value < (2.5 / 2.75)) {
                value -= (2.25f / 2.75f);
                return 2f * end * 7.5625f * value;
            } else {
                value -= (2.625f / 2.75f);
                return 2f * end * 7.5625f * value;
            }
        }

        public static float EaseInOutBounceD(float start, float end, float value) {
            end -= start;
            float d = 1f;

            if (value < d * 0.5f) {
                return EaseInBounceD(0, end, value * 2) * 0.5f;
            } else {
                return EaseOutBounceD(0, end, value * 2 - d) * 0.5f;
            }
        }

        public static float EaseInBackD(float start, float end, float value) {
            float s = 1.70158f;

            return 3f * (s + 1f) * (end - start) * value * value - 2f * s * (end - start) * value;
        }

        public static float EaseOutBackD(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value = (value) - 1;

            return end * ((s + 1f) * value * value + 2f * value * ((s + 1f) * value + s));
        }

        public static float EaseInOutBackD(float start, float end, float value) {
            float s = 1.70158f;
            end -= start;
            value /= .5f;

            if ((value) < 1) {
                s *= (1.525f);
                return 0.5f * end * (s + 1) * value * value + end * value * ((s + 1f) * value - s);
            }

            value -= 2;
            s *= (1.525f);
            return 0.5f * end * ((s + 1) * value * value + 2f * value * ((s + 1f) * value + s));
        }

        public static float EaseInElasticD(float start, float end, float value) {
            return EaseOutElasticD(start, end, 1f - value);
        }

        public static float EaseOutElasticD(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p * 0.25f;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            return (a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * value) *
                Mathf.Cos((2f * Mathf.PI * (d * value - s)) / p)) / p - 5f * kNATURAL_LOG_OF_2 * a *
                Mathf.Pow(2f, 1f - 10f * value) * Mathf.Sin((2f * Mathf.PI * (d * value - s)) / p);
        }

        public static float EaseInOutElasticD(float start, float end, float value) {
            end -= start;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(end)) {
                a = end;
                s = p / 4;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
            }

            if (value < 1) {
                value -= 1;

                return -5f * kNATURAL_LOG_OF_2 * a * Mathf.Pow(2f, 10f * value) * Mathf.Sin(2 * Mathf.PI * (d * value - 2f) / p) -
                    a * Mathf.PI * d * Mathf.Pow(2f, 10f * value) * Mathf.Cos(2 * Mathf.PI * (d * value - s) / p) / p;
            }

            value -= 1;

            return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * value - s) / p) / (p * Mathf.Pow(2f, 10f * value)) -
                5f * kNATURAL_LOG_OF_2 * a * Mathf.Sin(2f * Mathf.PI * (d * value - s) / p) / (Mathf.Pow(2f, 10f * value));
        }

        public static float SpringD(float start, float end, float value) {
            value = Mathf.Clamp01(value);
            end -= start;

            return end * (6f * (1f - value) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - value, 1.2f) *
                Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + Mathf.Pow(1f - value, 2.2f) *
                (Mathf.PI * (2.5f * value * value * value + 0.2f) + 7.5f * Mathf.PI * value * value * value) *
                Mathf.Cos(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + 1f) -
                6f * end * (Mathf.Pow(1 - value, 2.2f) * Mathf.Sin(Mathf.PI * value * (2.5f * value * value * value + 0.2f)) + value
                / 5f);

        }

        public delegate float Function(float s, float e, float v);

        /// <summary>
        /// Returns the function associated to the easingFunction enum. This value returned should be cached as it allocates memory
        /// to return.
        /// </summary>
        /// <param name="easingFunction">The enum associated with the easing function.</param>
        /// <returns>The easing function</returns>
        public static Function GetEasingFunction(Equation easingFunction) {
            if (easingFunction == Equation.EaseInQuad) {
                return EaseInQuad;
            }

            if (easingFunction == Equation.EaseOutQuad) {
                return EaseOutQuad;
            }

            if (easingFunction == Equation.EaseInOutQuad) {
                return EaseInOutQuad;
            }

            if (easingFunction == Equation.EaseInCubic) {
                return EaseInCubic;
            }

            if (easingFunction == Equation.EaseOutCubic) {
                return EaseOutCubic;
            }

            if (easingFunction == Equation.EaseInOutCubic) {
                return EaseInOutCubic;
            }

            if (easingFunction == Equation.EaseInQuart) {
                return EaseInQuart;
            }

            if (easingFunction == Equation.EaseOutQuart) {
                return EaseOutQuart;
            }

            if (easingFunction == Equation.EaseInOutQuart) {
                return EaseInOutQuart;
            }

            if (easingFunction == Equation.EaseInQuint) {
                return EaseInQuint;
            }

            if (easingFunction == Equation.EaseOutQuint) {
                return EaseOutQuint;
            }

            if (easingFunction == Equation.EaseInOutQuint) {
                return EaseInOutQuint;
            }

            if (easingFunction == Equation.EaseInSine) {
                return EaseInSine;
            }

            if (easingFunction == Equation.EaseOutSine) {
                return EaseOutSine;
            }

            if (easingFunction == Equation.EaseInOutSine) {
                return EaseInOutSine;
            }

            if (easingFunction == Equation.EaseInExpo) {
                return EaseInExpo;
            }

            if (easingFunction == Equation.EaseOutExpo) {
                return EaseOutExpo;
            }

            if (easingFunction == Equation.EaseInOutExpo) {
                return EaseInOutExpo;
            }

            if (easingFunction == Equation.EaseInCirc) {
                return EaseInCirc;
            }

            if (easingFunction == Equation.EaseOutCirc) {
                return EaseOutCirc;
            }

            if (easingFunction == Equation.EaseInOutCirc) {
                return EaseInOutCirc;
            }

            if (easingFunction == Equation.Linear) {
                return Linear;
            }

            if (easingFunction == Equation.Spring) {
                return Spring;
            }

            if (easingFunction == Equation.EaseInBounce) {
                return EaseInBounce;
            }

            if (easingFunction == Equation.EaseOutBounce) {
                return EaseOutBounce;
            }

            if (easingFunction == Equation.EaseInOutBounce) {
                return EaseInOutBounce;
            }

            if (easingFunction == Equation.EaseInBack) {
                return EaseInBack;
            }

            if (easingFunction == Equation.EaseOutBack) {
                return EaseOutBack;
            }

            if (easingFunction == Equation.EaseInOutBack) {
                return EaseInOutBack;
            }

            if (easingFunction == Equation.EaseInElastic) {
                return EaseInElastic;
            }

            if (easingFunction == Equation.EaseOutElastic) {
                return EaseOutElastic;
            }

            if (easingFunction == Equation.EaseInOutElastic) {
                return EaseInOutElastic;
            }

            return null;
        }

        /// <summary>
        /// Gets the derivative function of the appropriate easing function. If you use an easing function for position then this
        /// function can get you the speed at a given time (normalized).
        /// </summary>
        /// <param name="easingFunction"></param>
        /// <returns>The derivative function</returns>
        public static Function GetEasingFunctionDerivative(Equation easingFunction) {
            if (easingFunction == Equation.EaseInQuad) {
                return EaseInQuadD;
            }

            if (easingFunction == Equation.EaseOutQuad) {
                return EaseOutQuadD;
            }

            if (easingFunction == Equation.EaseInOutQuad) {
                return EaseInOutQuadD;
            }

            if (easingFunction == Equation.EaseInCubic) {
                return EaseInCubicD;
            }

            if (easingFunction == Equation.EaseOutCubic) {
                return EaseOutCubicD;
            }

            if (easingFunction == Equation.EaseInOutCubic) {
                return EaseInOutCubicD;
            }

            if (easingFunction == Equation.EaseInQuart) {
                return EaseInQuartD;
            }

            if (easingFunction == Equation.EaseOutQuart) {
                return EaseOutQuartD;
            }

            if (easingFunction == Equation.EaseInOutQuart) {
                return EaseInOutQuartD;
            }

            if (easingFunction == Equation.EaseInQuint) {
                return EaseInQuintD;
            }

            if (easingFunction == Equation.EaseOutQuint) {
                return EaseOutQuintD;
            }

            if (easingFunction == Equation.EaseInOutQuint) {
                return EaseInOutQuintD;
            }

            if (easingFunction == Equation.EaseInSine) {
                return EaseInSineD;
            }

            if (easingFunction == Equation.EaseOutSine) {
                return EaseOutSineD;
            }

            if (easingFunction == Equation.EaseInOutSine) {
                return EaseInOutSineD;
            }

            if (easingFunction == Equation.EaseInExpo) {
                return EaseInExpoD;
            }

            if (easingFunction == Equation.EaseOutExpo) {
                return EaseOutExpoD;
            }

            if (easingFunction == Equation.EaseInOutExpo) {
                return EaseInOutExpoD;
            }

            if (easingFunction == Equation.EaseInCirc) {
                return EaseInCircD;
            }

            if (easingFunction == Equation.EaseOutCirc) {
                return EaseOutCircD;
            }

            if (easingFunction == Equation.EaseInOutCirc) {
                return EaseInOutCircD;
            }

            if (easingFunction == Equation.Linear) {
                return LinearD;
            }

            if (easingFunction == Equation.Spring) {
                return SpringD;
            }

            if (easingFunction == Equation.EaseInBounce) {
                return EaseInBounceD;
            }

            if (easingFunction == Equation.EaseOutBounce) {
                return EaseOutBounceD;
            }

            if (easingFunction == Equation.EaseInOutBounce) {
                return EaseInOutBounceD;
            }

            if (easingFunction == Equation.EaseInBack) {
                return EaseInBackD;
            }

            if (easingFunction == Equation.EaseOutBack) {
                return EaseOutBackD;
            }

            if (easingFunction == Equation.EaseInOutBack) {
                return EaseInOutBackD;
            }

            if (easingFunction == Equation.EaseInElastic) {
                return EaseInElasticD;
            }

            if (easingFunction == Equation.EaseOutElastic) {
                return EaseOutElasticD;
            }

            if (easingFunction == Equation.EaseInOutElastic) {
                return EaseInOutElasticD;
            }

            return null;
        }

        #endregion

        public static float Ease(Equation equation, float start, float end, float value) {

            switch (equation) {
                case Equation.EaseInQuad:
                    return EaseInQuad(start, end, value);
                case Equation.EaseOutQuad:
                    return EaseOutQuad(start, end, value);
                case Equation.EaseInOutQuad:
                    return EaseInOutQuad(start, end, value);
                case Equation.EaseInCubic:
                    return EaseInCubic(start, end, value);
                case Equation.EaseOutCubic:
                    return EaseOutCubic(start, end, value);
                case Equation.EaseInOutCubic:
                    return EaseInOutCubic(start, end, value);
                case Equation.EaseInQuart:
                    return EaseInQuart(start, end, value);
                case Equation.EaseOutQuart:
                    return EaseOutQuart(start, end, value);
                case Equation.EaseInOutQuart:
                    return EaseInOutQuart(start, end, value);
                case Equation.EaseInQuint:
                    return EaseInQuint(start, end, value);
                case Equation.EaseOutQuint:
                    return EaseOutQuint(start, end, value);
                case Equation.EaseInOutQuint:
                    return EaseInOutQuint(start, end, value);
                case Equation.EaseInSine:
                    return EaseInSine(start, end, value);
                case Equation.EaseOutSine:
                    return EaseOutSine(start, end, value);
                case Equation.EaseInOutSine:
                    return EaseInOutSine(start, end, value);
                case Equation.EaseInExpo:
                    return EaseInExpo(start, end, value);
                case Equation.EaseOutExpo:
                    return EaseOutExpo(start, end, value);
                case Equation.EaseInOutExpo:
                    return EaseInOutExpo(start, end, value);
                case Equation.EaseInCirc:
                    return EaseInCirc(start, end, value);
                case Equation.EaseOutCirc:
                    return EaseOutCirc(start, end, value);
                case Equation.EaseInOutCirc:
                    return EaseInOutCirc(start, end, value);
                case Equation.Linear:
                    return Linear(start, end, value);
                case Equation.Spring:
                    return Spring(start, end, value);
                case Equation.EaseInBounce:
                    return EaseInBounce(start, end, value);
                case Equation.EaseOutBounce:
                    return EaseOutBounce(start, end, value);
                case Equation.EaseInOutBounce:
                    return EaseInOutBounce(start, end, value);
                case Equation.EaseInBack:
                    return EaseInBack(start, end, value);
                case Equation.EaseOutBack:
                    return EaseOutBack(start, end, value);
                case Equation.EaseInOutBack:
                    return EaseInOutBack(start, end, value);
                case Equation.EaseInElastic:
                    return EaseInElastic(start, end, value);
                case Equation.EaseOutElastic:
                    return EaseOutElastic(start, end, value);
                case Equation.EaseInOutElastic:
                    return EaseInOutElastic(start, end, value);
                default:
                    Debug.LogWarning("Could not convert Easing.Equation.\nReturning Easing.Linear instead.");
                    return Linear(start, end, value);
            }
        }
    }
}
