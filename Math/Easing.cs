using UnityEngine;

namespace MW.Easing {

    public static class Interpolate {

        #region EEquations

        const float kNaturalLogOf2 = 0.693147181f;

        public static float Linear(float fStart, float fEnd, float fDuration) {
            return Mathf.Lerp(fStart, fEnd, fDuration);
        }

        public static float Spring(float fStart, float fEnd, float fDuration) {
            fDuration = Mathf.Clamp01(fDuration);
            fDuration = (Mathf.Sin(fDuration * Mathf.PI * (0.2f + 2.5f * fDuration * fDuration * fDuration)) * Mathf.Pow(1f - fDuration, 2.2f) + fDuration) * (1f + (1.2f * (1f - fDuration)));
            return fStart + (fEnd - fStart) * fDuration;
        }

        public static float EaseInQuad(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * fDuration * fDuration + fStart;
        }

        public static float EaseOutQuad(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return -fEnd * fDuration * (fDuration - 2) + fStart;
        }

        public static float EaseInOutQuad(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;
            if (fDuration < 1) return fEnd * 0.5f * fDuration * fDuration + fStart;
            fDuration--;
            return -fEnd * 0.5f * (fDuration * (fDuration - 2) - 1) + fStart;
        }

        public static float EaseInCubic(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * fDuration * fDuration * fDuration + fStart;
        }

        public static float EaseOutCubic(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return fEnd * (fDuration * fDuration * fDuration + 1) + fStart;
        }

        public static float EaseInOutCubic(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;
            if (fDuration < 1) return fEnd * 0.5f * fDuration * fDuration * fDuration + fStart;
            fDuration -= 2;
            return fEnd * 0.5f * (fDuration * fDuration * fDuration + 2) + fStart;
        }

        public static float EaseInQuart(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * fDuration * fDuration * fDuration * fDuration + fStart;
        }

        public static float EaseOutQuart(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return -fEnd * (fDuration * fDuration * fDuration * fDuration - 1) + fStart;
        }

        public static float EaseInOutQuart(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;
            if (fDuration < 1) return fEnd * 0.5f * fDuration * fDuration * fDuration * fDuration + fStart;
            fDuration -= 2;
            return -fEnd * 0.5f * (fDuration * fDuration * fDuration * fDuration - 2) + fStart;
        }

        public static float EaseInQuint(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * fDuration * fDuration * fDuration * fDuration * fDuration + fStart;
        }

        public static float EaseOutQuint(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return fEnd * (fDuration * fDuration * fDuration * fDuration * fDuration + 1) + fStart;
        }

        public static float EaseInOutQuint(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;
            if (fDuration < 1) return fEnd * 0.5f * fDuration * fDuration * fDuration * fDuration * fDuration + fStart;
            fDuration -= 2;
            return fEnd * 0.5f * (fDuration * fDuration * fDuration * fDuration * fDuration + 2) + fStart;
        }

        public static float EaseInSine(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return -fEnd * Mathf.Cos(fDuration * (Mathf.PI * 0.5f)) + fEnd + fStart;
        }

        public static float EaseOutSine(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * Mathf.Sin(fDuration * (Mathf.PI * 0.5f)) + fStart;
        }

        public static float EaseInOutSine(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return -fEnd * 0.5f * (Mathf.Cos(Mathf.PI * fDuration) - 1) + fStart;
        }

        public static float EaseInExpo(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * Mathf.Pow(2, 10 * (fDuration - 1)) + fStart;
        }

        public static float EaseOutExpo(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * (-Mathf.Pow(2, -10 * fDuration) + 1) + fStart;
        }

        public static float EaseInOutExpo(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;
            if (fDuration < 1) return fEnd * 0.5f * Mathf.Pow(2, 10 * (fDuration - 1)) + fStart;
            fDuration--;
            return fEnd * 0.5f * (-Mathf.Pow(2, -10 * fDuration) + 2) + fStart;
        }

        public static float EaseInCirc(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return -fEnd * (Mathf.Sqrt(1 - fDuration * fDuration) - 1) + fStart;
        }

        public static float EaseOutCirc(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return fEnd * Mathf.Sqrt(1 - fDuration * fDuration) + fStart;
        }

        public static float EaseInOutCirc(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;
            if (fDuration < 1) return -fEnd * 0.5f * (Mathf.Sqrt(1 - fDuration * fDuration) - 1) + fStart;
            fDuration -= 2;
            return fEnd * 0.5f * (Mathf.Sqrt(1 - fDuration * fDuration) + 1) + fStart;
        }

        public static float EaseInBounce(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            float d = 1f;
            return fEnd - EaseOutBounce(0, fEnd, d - fDuration) + fStart;
        }

        public static float EaseOutBounce(float fStart, float fEnd, float fDuration) {
            fDuration /= 1f;
            fEnd -= fStart;
            if (fDuration < (1 / 2.75f)) {
                return fEnd * (7.5625f * fDuration * fDuration) + fStart;
            } else if (fDuration < (2 / 2.75f)) {
                fDuration -= (1.5f / 2.75f);
                return fEnd * (7.5625f * (fDuration) * fDuration + .75f) + fStart;
            } else if (fDuration < (2.5 / 2.75)) {
                fDuration -= (2.25f / 2.75f);
                return fEnd * (7.5625f * (fDuration) * fDuration + .9375f) + fStart;
            } else {
                fDuration -= (2.625f / 2.75f);
                return fEnd * (7.5625f * (fDuration) * fDuration + .984375f) + fStart;
            }
        }

        public static float EaseInOutBounce(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            float d = 1f;
            if (fDuration < d * 0.5f) return EaseInBounce(0, fEnd, fDuration * 2) * 0.5f + fStart;
            else return EaseOutBounce(0, fEnd, fDuration * 2 - d) * 0.5f + fEnd * 0.5f + fStart;
        }

        public static float EaseInBack(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            fDuration /= 1;
            float s = 1.70158f;
            return fEnd * (fDuration) * fDuration * ((s + 1) * fDuration - s) + fStart;
        }

        public static float EaseOutBack(float fStart, float fEnd, float fDuration) {
            float s = 1.70158f;
            fEnd -= fStart;
            fDuration = (fDuration) - 1;
            return fEnd * ((fDuration) * fDuration * ((s + 1) * fDuration + s) + 1) + fStart;
        }

        public static float EaseInOutBack(float fStart, float fEnd, float fDuration) {
            float s = 1.70158f;
            fEnd -= fStart;
            fDuration /= .5f;
            if ((fDuration) < 1) {
                s *= (1.525f);
                return fEnd * 0.5f * (fDuration * fDuration * (((s) + 1) * fDuration - s)) + fStart;
            }
            fDuration -= 2;
            s *= (1.525f);
            return fEnd * 0.5f * ((fDuration) * fDuration * (((s) + 1) * fDuration + s) + 2) + fStart;
        }

        public static float EaseInElastic(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (fDuration == 0) return fStart;

            if ((fDuration /= d) == 1) return fStart + fEnd;

            if (a == 0f || a < Mathf.Abs(fEnd)) {
                a = fEnd;
                s = p / 4;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(fEnd / a);
            }

            return -(a * Mathf.Pow(2, 10 * (fDuration -= 1)) * Mathf.Sin((fDuration * d - s) * (2 * Mathf.PI) / p)) + fStart;
        }

        public static float EaseOutElastic(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (fDuration == 0) return fStart;

            if ((fDuration /= d) == 1) return fStart + fEnd;

            if (a == 0f || a < Mathf.Abs(fEnd)) {
                a = fEnd;
                s = p * 0.25f;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(fEnd / a);
            }

            return (a * Mathf.Pow(2, -10 * fDuration) * Mathf.Sin((fDuration * d - s) * (2 * Mathf.PI) / p) + fEnd + fStart);
        }

        public static float EaseInOutElastic(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (fDuration == 0) return fStart;

            if ((fDuration /= d * 0.5f) == 2) return fStart + fEnd;

            if (a == 0f || a < Mathf.Abs(fEnd)) {
                a = fEnd;
                s = p / 4;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(fEnd / a);
            }

            if (fDuration < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (fDuration -= 1)) * Mathf.Sin((fDuration * d - s) * (2 * Mathf.PI) / p)) + fStart;
            return a * Mathf.Pow(2, -10 * (fDuration -= 1)) * Mathf.Sin((fDuration * d - s) * (2 * Mathf.PI) / p) * 0.5f + fEnd + fStart;
        }

        //
        // These are derived functions that the motor can use to get the speed at a specific time.
        //
        // The easing functions all work with a normalized time (0 to 1) and the returned fDuration here
        // reflects that. fDurations returned here should be divided by the actual time.
        //
        // TODO: These functions have not had the testing they deserve. If there is odd behavior around
        //       dash speeds then this would be the first place I'd look.

        public static float LinearD(float fStart, float fEnd, float fDuration) {
            return fEnd - fStart;
        }

        public static float EaseInQuadD(float fStart, float fEnd, float fDuration) {
            return 2f * (fEnd - fStart) * fDuration;
        }

        public static float EaseOutQuadD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return -fEnd * fDuration - fEnd * (fDuration - 2);
        }

        public static float EaseInOutQuadD(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;

            if (fDuration < 1) {
                return fEnd * fDuration;
            }

            fDuration--;

            return fEnd * (1 - fDuration);
        }

        public static float EaseInCubicD(float fStart, float fEnd, float fDuration) {
            return 3f * (fEnd - fStart) * fDuration * fDuration;
        }

        public static float EaseOutCubicD(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return 3f * fEnd * fDuration * fDuration;
        }

        public static float EaseInOutCubicD(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;

            if (fDuration < 1) {
                return (3f / 2f) * fEnd * fDuration * fDuration;
            }

            fDuration -= 2;

            return (3f / 2f) * fEnd * fDuration * fDuration;
        }

        public static float EaseInQuartD(float fStart, float fEnd, float fDuration) {
            return 4f * (fEnd - fStart) * fDuration * fDuration * fDuration;
        }

        public static float EaseOutQuartD(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return -4f * fEnd * fDuration * fDuration * fDuration;
        }

        public static float EaseInOutQuartD(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;

            if (fDuration < 1) {
                return 2f * fEnd * fDuration * fDuration * fDuration;
            }

            fDuration -= 2;

            return -2f * fEnd * fDuration * fDuration * fDuration;
        }

        public static float EaseInQuintD(float fStart, float fEnd, float fDuration) {
            return 5f * (fEnd - fStart) * fDuration * fDuration * fDuration * fDuration;
        }

        public static float EaseOutQuintD(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return 5f * fEnd * fDuration * fDuration * fDuration * fDuration;
        }

        public static float EaseInOutQuintD(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;

            if (fDuration < 1) {
                return (5f / 2f) * fEnd * fDuration * fDuration * fDuration * fDuration;
            }

            fDuration -= 2;

            return (5f / 2f) * fEnd * fDuration * fDuration * fDuration * fDuration;
        }

        public static float EaseInSineD(float fStart, float fEnd, float fDuration) {
            return (fEnd - fStart) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * fDuration);
        }

        public static float EaseOutSineD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return (Mathf.PI * 0.5f) * fEnd * Mathf.Cos(fDuration * (Mathf.PI * 0.5f));
        }

        public static float EaseInOutSineD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return fEnd * 0.5f * Mathf.PI * Mathf.Sin(Mathf.PI * fDuration);
        }
        public static float EaseInExpoD(float fStart, float fEnd, float fDuration) {
            return (10f * kNaturalLogOf2 * (fEnd - fStart) * Mathf.Pow(2f, 10f * (fDuration - 1)));
        }

        public static float EaseOutExpoD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            return 5f * kNaturalLogOf2 * fEnd * Mathf.Pow(2f, 1f - 10f * fDuration);
        }

        public static float EaseInOutExpoD(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;

            if (fDuration < 1) {
                return 5f * kNaturalLogOf2 * fEnd * Mathf.Pow(2f, 10f * (fDuration - 1));
            }

            fDuration--;

            return (5f * kNaturalLogOf2 * fEnd) / (Mathf.Pow(2f, 10f * fDuration));
        }

        public static float EaseInCircD(float fStart, float fEnd, float fDuration) {
            return ((fEnd - fStart) * fDuration) / Mathf.Sqrt(1f - fDuration * fDuration);
        }

        public static float EaseOutCircD(float fStart, float fEnd, float fDuration) {
            fDuration--;
            fEnd -= fStart;
            return (-fEnd * fDuration) / Mathf.Sqrt(1f - fDuration * fDuration);
        }

        public static float EaseInOutCircD(float fStart, float fEnd, float fDuration) {
            fDuration /= .5f;
            fEnd -= fStart;

            if (fDuration < 1) {
                return (fEnd * fDuration) / (2f * Mathf.Sqrt(1f - fDuration * fDuration));
            }

            fDuration -= 2;

            return (-fEnd * fDuration) / (2f * Mathf.Sqrt(1f - fDuration * fDuration));
        }

        public static float EaseInBounceD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            float d = 1f;

            return EaseOutBounceD(0, fEnd, d - fDuration);
        }

        public static float EaseOutBounceD(float fStart, float fEnd, float fDuration) {
            fDuration /= 1f;
            fEnd -= fStart;

            if (fDuration < (1 / 2.75f)) {
                return 2f * fEnd * 7.5625f * fDuration;
            } else if (fDuration < (2 / 2.75f)) {
                fDuration -= (1.5f / 2.75f);
                return 2f * fEnd * 7.5625f * fDuration;
            } else if (fDuration < (2.5 / 2.75)) {
                fDuration -= (2.25f / 2.75f);
                return 2f * fEnd * 7.5625f * fDuration;
            } else {
                fDuration -= (2.625f / 2.75f);
                return 2f * fEnd * 7.5625f * fDuration;
            }
        }

        public static float EaseInOutBounceD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;
            float d = 1f;

            if (fDuration < d * 0.5f) {
                return EaseInBounceD(0, fEnd, fDuration * 2) * 0.5f;
            } else {
                return EaseOutBounceD(0, fEnd, fDuration * 2 - d) * 0.5f;
            }
        }

        public static float EaseInBackD(float fStart, float fEnd, float fDuration) {
            float s = 1.70158f;

            return 3f * (s + 1f) * (fEnd - fStart) * fDuration * fDuration - 2f * s * (fEnd - fStart) * fDuration;
        }

        public static float EaseOutBackD(float fStart, float fEnd, float fDuration) {
            float s = 1.70158f;
            fEnd -= fStart;
            fDuration = (fDuration) - 1;

            return fEnd * ((s + 1f) * fDuration * fDuration + 2f * fDuration * ((s + 1f) * fDuration + s));
        }

        public static float EaseInOutBackD(float fStart, float fEnd, float fDuration) {
            float s = 1.70158f;
            fEnd -= fStart;
            fDuration /= .5f;

            if ((fDuration) < 1) {
                s *= (1.525f);
                return 0.5f * fEnd * (s + 1) * fDuration * fDuration + fEnd * fDuration * ((s + 1f) * fDuration - s);
            }

            fDuration -= 2;
            s *= (1.525f);
            return 0.5f * fEnd * ((s + 1) * fDuration * fDuration + 2f * fDuration * ((s + 1f) * fDuration + s));
        }

        public static float EaseInElasticD(float fStart, float fEnd, float fDuration) {
            return EaseOutElasticD(fStart, fEnd, 1f - fDuration);
        }

        public static float EaseOutElasticD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(fEnd)) {
                a = fEnd;
                s = p * 0.25f;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(fEnd / a);
            }

            return (a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * fDuration) *
                Mathf.Cos((2f * Mathf.PI * (d * fDuration - s)) / p)) / p - 5f * kNaturalLogOf2 * a *
                Mathf.Pow(2f, 1f - 10f * fDuration) * Mathf.Sin((2f * Mathf.PI * (d * fDuration - s)) / p);
        }

        public static float EaseInOutElasticD(float fStart, float fEnd, float fDuration) {
            fEnd -= fStart;

            float d = 1f;
            float p = d * .3f;
            float s;
            float a = 0;

            if (a == 0f || a < Mathf.Abs(fEnd)) {
                a = fEnd;
                s = p / 4;
            } else {
                s = p / (2 * Mathf.PI) * Mathf.Asin(fEnd / a);
            }

            if (fDuration < 1) {
                fDuration -= 1;

                return -5f * kNaturalLogOf2 * a * Mathf.Pow(2f, 10f * fDuration) * Mathf.Sin(2 * Mathf.PI * (d * fDuration - 2f) / p) -
                    a * Mathf.PI * d * Mathf.Pow(2f, 10f * fDuration) * Mathf.Cos(2 * Mathf.PI * (d * fDuration - s) / p) / p;
            }

            fDuration -= 1;

            return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * fDuration - s) / p) / (p * Mathf.Pow(2f, 10f * fDuration)) -
                5f * kNaturalLogOf2 * a * Mathf.Sin(2f * Mathf.PI * (d * fDuration - s) / p) / (Mathf.Pow(2f, 10f * fDuration));
        }

        public static float SpringD(float fStart, float fEnd, float fDuration) {
            fDuration = Mathf.Clamp01(fDuration);
            fEnd -= fStart;

            return fEnd * (6f * (1f - fDuration) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - fDuration, 1.2f) *
                Mathf.Sin(Mathf.PI * fDuration * (2.5f * fDuration * fDuration * fDuration + 0.2f)) + Mathf.Pow(1f - fDuration, 2.2f) *
                (Mathf.PI * (2.5f * fDuration * fDuration * fDuration + 0.2f) + 7.5f * Mathf.PI * fDuration * fDuration * fDuration) *
                Mathf.Cos(Mathf.PI * fDuration * (2.5f * fDuration * fDuration * fDuration + 0.2f)) + 1f) -
                6f * fEnd * (Mathf.Pow(1 - fDuration, 2.2f) * Mathf.Sin(Mathf.PI * fDuration * (2.5f * fDuration * fDuration * fDuration + 0.2f)) + fDuration
                / 5f);

        }

        public delegate float Function(float s, float e, float v);

        /// <summary>
        /// Returns the function associated to the easingFunction enum. This fDuration returned should be cached as it allocates memory
        /// to return.
        /// </summary>
        /// <param name="EEasingFunction">The enum associated with the easing function.</param>
        /// <returns>The easing function</returns>
        public static Function GetEasingFunction(EEquation EEasingFunction) {
            if (EEasingFunction == EEquation.EaseInQuad) {
                return EaseInQuad;
            }

            else if (EEasingFunction == EEquation.EaseOutQuad) {
                return EaseOutQuad;
            }

            else if (EEasingFunction == EEquation.EaseInOutQuad) {
                return EaseInOutQuad;
            }

            else if (EEasingFunction == EEquation.EaseInCubic) {
                return EaseInCubic;
            }

            else if (EEasingFunction == EEquation.EaseOutCubic) {
                return EaseOutCubic;
            }

            else if (EEasingFunction == EEquation.EaseInOutCubic) {
                return EaseInOutCubic;
            }

            else if (EEasingFunction == EEquation.EaseInQuart) {
                return EaseInQuart;
            }

            else if (EEasingFunction == EEquation.EaseOutQuart) {
                return EaseOutQuart;
            }

            else if (EEasingFunction == EEquation.EaseInOutQuart) {
                return EaseInOutQuart;
            }

            else if (EEasingFunction == EEquation.EaseInQuint) {
                return EaseInQuint;
            }

            else if (EEasingFunction == EEquation.EaseOutQuint) {
                return EaseOutQuint;
            }

            else if (EEasingFunction == EEquation.EaseInOutQuint) {
                return EaseInOutQuint;
            }

            else if (EEasingFunction == EEquation.EaseInSine) {
                return EaseInSine;
            }

            else if (EEasingFunction == EEquation.EaseOutSine) {
                return EaseOutSine;
            }

            else if (EEasingFunction == EEquation.EaseInOutSine) {
                return EaseInOutSine;
            }

            else if (EEasingFunction == EEquation.EaseInExpo) {
                return EaseInExpo;
            }

            else if (EEasingFunction == EEquation.EaseOutExpo) {
                return EaseOutExpo;
            }

            else if (EEasingFunction == EEquation.EaseInOutExpo) {
                return EaseInOutExpo;
            }

            else if (EEasingFunction == EEquation.EaseInCirc) {
                return EaseInCirc;
            }

            else if (EEasingFunction == EEquation.EaseOutCirc) {
                return EaseOutCirc;
            }

            else if (EEasingFunction == EEquation.EaseInOutCirc) {
                return EaseInOutCirc;
            }

            else if (EEasingFunction == EEquation.Linear) {
                return Linear;
            }

            else if (EEasingFunction == EEquation.Spring) {
                return Spring;
            }

            else if (EEasingFunction == EEquation.EaseInBounce) {
                return EaseInBounce;
            }

            else if (EEasingFunction == EEquation.EaseOutBounce) {
                return EaseOutBounce;
            }

            else if (EEasingFunction == EEquation.EaseInOutBounce) {
                return EaseInOutBounce;
            }

            else if (EEasingFunction == EEquation.EaseInBack) {
                return EaseInBack;
            }

            else if (EEasingFunction == EEquation.EaseOutBack) {
                return EaseOutBack;
            }

            else if (EEasingFunction == EEquation.EaseInOutBack) {
                return EaseInOutBack;
            }

            else if (EEasingFunction == EEquation.EaseInElastic) {
                return EaseInElastic;
            }

            else if (EEasingFunction == EEquation.EaseOutElastic) {
                return EaseOutElastic;
            }

            else if (EEasingFunction == EEquation.EaseInOutElastic) {
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
        public static Function GetEasingFunctionDerivative(EEquation easingFunction) {
            if (easingFunction == EEquation.EaseInQuad) {
                return EaseInQuadD;
            }

            else if (easingFunction == EEquation.EaseOutQuad) {
                return EaseOutQuadD;
            }

            else if (easingFunction == EEquation.EaseInOutQuad) {
                return EaseInOutQuadD;
            }

            else if (easingFunction == EEquation.EaseInCubic) {
                return EaseInCubicD;
            }

            else if (easingFunction == EEquation.EaseOutCubic) {
                return EaseOutCubicD;
            }

            else if (easingFunction == EEquation.EaseInOutCubic) {
                return EaseInOutCubicD;
            }

            else if (easingFunction == EEquation.EaseInQuart) {
                return EaseInQuartD;
            }

            else if (easingFunction == EEquation.EaseOutQuart) {
                return EaseOutQuartD;
            }

            else if (easingFunction == EEquation.EaseInOutQuart) {
                return EaseInOutQuartD;
            }

            else if (easingFunction == EEquation.EaseInQuint) {
                return EaseInQuintD;
            }

            else if (easingFunction == EEquation.EaseOutQuint) {
                return EaseOutQuintD;
            }

            else if (easingFunction == EEquation.EaseInOutQuint) {
                return EaseInOutQuintD;
            }

            else if (easingFunction == EEquation.EaseInSine) {
                return EaseInSineD;
            }

            else if (easingFunction == EEquation.EaseOutSine) {
                return EaseOutSineD;
            }

            else if (easingFunction == EEquation.EaseInOutSine) {
                return EaseInOutSineD;
            }

            else if (easingFunction == EEquation.EaseInExpo) {
                return EaseInExpoD;
            }

            else if (easingFunction == EEquation.EaseOutExpo) {
                return EaseOutExpoD;
            }

            else if (easingFunction == EEquation.EaseInOutExpo) {
                return EaseInOutExpoD;
            }

            else if (easingFunction == EEquation.EaseInCirc) {
                return EaseInCircD;
            }

            else if (easingFunction == EEquation.EaseOutCirc) {
                return EaseOutCircD;
            }

            else if (easingFunction == EEquation.EaseInOutCirc) {
                return EaseInOutCircD;
            }

            else if (easingFunction == EEquation.Linear) {
                return LinearD;
            }

            else if (easingFunction == EEquation.Spring) {
                return SpringD;
            }

            else if (easingFunction == EEquation.EaseInBounce) {
                return EaseInBounceD;
            }

            else if (easingFunction == EEquation.EaseOutBounce) {
                return EaseOutBounceD;
            }

            else if (easingFunction == EEquation.EaseInOutBounce) {
                return EaseInOutBounceD;
            }

            else if (easingFunction == EEquation.EaseInBack) {
                return EaseInBackD;
            }

            else if (easingFunction == EEquation.EaseOutBack) {
                return EaseOutBackD;
            }

            else if (easingFunction == EEquation.EaseInOutBack) {
                return EaseInOutBackD;
            }

            else if (easingFunction == EEquation.EaseInElastic) {
                return EaseInElasticD;
            }

            else if (easingFunction == EEquation.EaseOutElastic) {
                return EaseOutElasticD;
            }

            else if (easingFunction == EEquation.EaseInOutElastic) {
                return EaseInOutElasticD;
            }

            return null;
        }

#endregion

        public static float Ease(EEquation EEEquation, float fStart, float fEnd, float fDuration) {
            
            switch (EEEquation) {
                case EEquation.EaseInQuad:
                    return EaseInQuad(fStart, fEnd, fDuration);
                case EEquation.EaseOutQuad:
                    return EaseOutQuad(fStart, fEnd, fDuration);
                case EEquation.EaseInOutQuad:
                    return EaseInOutQuad(fStart, fEnd, fDuration);
                case EEquation.EaseInCubic:
                    return EaseInCubic(fStart, fEnd, fDuration);
                case EEquation.EaseOutCubic:
                    return EaseOutCubic(fStart, fEnd, fDuration);
                case EEquation.EaseInOutCubic:
                    return EaseInOutCubic(fStart, fEnd, fDuration);
                case EEquation.EaseInQuart:
                    return EaseInQuart(fStart, fEnd, fDuration);
                case EEquation.EaseOutQuart:
                    return EaseOutQuart(fStart, fEnd, fDuration);
                case EEquation.EaseInOutQuart:
                    return EaseInOutQuart(fStart, fEnd, fDuration);
                case EEquation.EaseInQuint:
                    return EaseInQuint(fStart, fEnd, fDuration);
                case EEquation.EaseOutQuint:
                    return EaseOutQuint(fStart, fEnd, fDuration);
                case EEquation.EaseInOutQuint:
                    return EaseInOutQuint(fStart, fEnd, fDuration);
                case EEquation.EaseInSine:
                    return EaseInSine(fStart, fEnd, fDuration);
                case EEquation.EaseOutSine:
                    return EaseOutSine(fStart, fEnd, fDuration);
                case EEquation.EaseInOutSine:
                    return EaseInOutSine(fStart, fEnd, fDuration);
                case EEquation.EaseInExpo:
                    return EaseInExpo(fStart, fEnd, fDuration);
                case EEquation.EaseOutExpo:
                    return EaseOutExpo(fStart, fEnd, fDuration);
                case EEquation.EaseInOutExpo:
                    return EaseInOutExpo(fStart, fEnd, fDuration);
                case EEquation.EaseInCirc:
                    return EaseInCirc(fStart, fEnd, fDuration);
                case EEquation.EaseOutCirc:
                    return EaseOutCirc(fStart, fEnd, fDuration);
                case EEquation.EaseInOutCirc:
                    return EaseInOutCirc(fStart, fEnd, fDuration);
                case EEquation.Linear:
                    return Linear(fStart, fEnd, fDuration);
                case EEquation.Spring:
                    return Spring(fStart, fEnd, fDuration);
                case EEquation.EaseInBounce:
                    return EaseInBounce(fStart, fEnd, fDuration);
                case EEquation.EaseOutBounce:
                    return EaseOutBounce(fStart, fEnd, fDuration);
                case EEquation.EaseInOutBounce:
                    return EaseInOutBounce(fStart, fEnd, fDuration);
                case EEquation.EaseInBack:
                    return EaseInBack(fStart, fEnd, fDuration);
                case EEquation.EaseOutBack:
                    return EaseOutBack(fStart, fEnd, fDuration);
                case EEquation.EaseInOutBack:
                    return EaseInOutBack(fStart, fEnd, fDuration);
                case EEquation.EaseInElastic:
                    return EaseInElastic(fStart, fEnd, fDuration);
                case EEquation.EaseOutElastic:
                    return EaseOutElastic(fStart, fEnd, fDuration);
                case EEquation.EaseInOutElastic:
                    return EaseInOutElastic(fStart, fEnd, fDuration);
                default:
                    Debug.LogWarning("Could not convert Easing.EEquation.\nReturning Easing.Linear instead.");
                    return Linear(fStart, fEnd, fDuration);
            }
        }
    }
}
