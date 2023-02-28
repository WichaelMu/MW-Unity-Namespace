using MW.Math.Magic;
using UnityEngine;

namespace MW.Easing
{
	/// <summary>Interpolation equations.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Interpolate
	{

		#region EEquations

		const float kNaturalLogOf2 = 0.693147181f;

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float Linear(float Start, float End, float Duration)
		{
			return Mathf.Lerp(Start, End, Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float Spring(float Start, float End, float Duration)
		{
			Duration = Mathf.Clamp01(Duration);
			Duration = (Mathf.Sin(Duration * Mathf.PI * (0.2f + 2.5f * Duration * Duration * Duration)) * Mathf.Pow(1f - Duration, 2.2f) + Duration) * (1f + 1.2f * (1f - Duration));
			return Start + (End - Start) * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInQuad(float Start, float End, float Duration)
		{
			End -= Start;
			return End * Duration * Duration + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutQuad(float Start, float End, float Duration)
		{
			End -= Start;
			return -End * Duration * (Duration - 2) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutQuad(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;
			if (Duration < 1) return End * 0.5f * Duration * Duration + Start;
			Duration--;
			return -End * 0.5f * (Duration * (Duration - 2) - 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInCubic(float Start, float End, float Duration)
		{
			End -= Start;
			return End * Duration * Duration * Duration + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutCubic(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return End * (Duration * Duration * Duration + 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutCubic(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;
			if (Duration < 1) return End * 0.5f * Duration * Duration * Duration + Start;
			Duration -= 2;
			return End * 0.5f * (Duration * Duration * Duration + 2) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInQuart(float Start, float End, float Duration)
		{
			End -= Start;
			return End * Duration * Duration * Duration * Duration + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutQuart(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return -End * (Duration * Duration * Duration * Duration - 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutQuart(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;
			if (Duration < 1) return End * 0.5f * Duration * Duration * Duration * Duration + Start;
			Duration -= 2;
			return -End * 0.5f * (Duration * Duration * Duration * Duration - 2) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInQuint(float Start, float End, float Duration)
		{
			End -= Start;
			return End * Duration * Duration * Duration * Duration * Duration + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutQuint(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return End * (Duration * Duration * Duration * Duration * Duration + 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutQuint(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;
			if (Duration < 1) return End * 0.5f * Duration * Duration * Duration * Duration * Duration + Start;
			Duration -= 2;
			return End * 0.5f * (Duration * Duration * Duration * Duration * Duration + 2) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInSine(float Start, float End, float Duration)
		{
			End -= Start;
			return -End * Mathf.Cos(Duration * (Mathf.PI * 0.5f)) + End + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutSine(float Start, float End, float Duration)
		{
			End -= Start;
			return End * Mathf.Sin(Duration * (Mathf.PI * 0.5f)) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutSine(float Start, float End, float Duration)
		{
			End -= Start;
			return -End * 0.5f * (Mathf.Cos(Mathf.PI * Duration) - 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInExpo(float Start, float End, float Duration)
		{
			End -= Start;
			return End * Mathf.Pow(2, 10 * (Duration - 1)) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutExpo(float Start, float End, float Duration)
		{
			End -= Start;
			return End * (-Mathf.Pow(2, -10 * Duration) + 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutExpo(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;
			if (Duration < 1) return End * 0.5f * Mathf.Pow(2, 10 * (Duration - 1)) + Start;
			Duration--;
			return End * 0.5f * (-Mathf.Pow(2, -10 * Duration) + 2) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInCirc(float Start, float End, float Duration)
		{
			End -= Start;
			return -End * (Fast.FSqrt(1 - Duration * Duration) - 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutCirc(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return End * Fast.FSqrt(1 - Duration * Duration) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutCirc(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;
			if (Duration < 1) return -End * 0.5f * (Fast.FSqrt(1 - Duration * Duration) - 1) + Start;
			Duration -= 2;
			return End * 0.5f * (Fast.FSqrt(1 - Duration * Duration) + 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInBounce(float Start, float End, float Duration)
		{
			End -= Start;
			float d = 1f;
			return End - EaseOutBounce(0, End, d - Duration) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutBounce(float Start, float End, float Duration)
		{
			Duration /= 1f;
			End -= Start;
			if (Duration < 1 / 2.75f)
			{
				return End * (7.5625f * Duration * Duration) + Start;
			}
			else if (Duration < 2 / 2.75f)
			{
				Duration -= 1.5f / 2.75f;
				return End * (7.5625f * Duration * Duration + .75f) + Start;
			}
			else if (Duration < 2.5 / 2.75)
			{
				Duration -= 2.25f / 2.75f;
				return End * (7.5625f * Duration * Duration + .9375f) + Start;
			}
			else
			{
				Duration -= 2.625f / 2.75f;
				return End * (7.5625f * Duration * Duration + .984375f) + Start;
			}
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutBounce(float Start, float End, float Duration)
		{
			End -= Start;
			float d = 1f;
			if (Duration < d * 0.5f) return EaseInBounce(0, End, Duration * 2) * 0.5f + Start;
			else return EaseOutBounce(0, End, Duration * 2 - d) * 0.5f + End * 0.5f + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInBack(float Start, float End, float Duration)
		{
			End -= Start;
			Duration /= 1;
			float s = 1.70158f;
			return End * Duration * Duration * ((s + 1) * Duration - s) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutBack(float Start, float End, float Duration)
		{
			float s = 1.70158f;
			End -= Start;
			Duration = Duration - 1;
			return End * (Duration * Duration * ((s + 1) * Duration + s) + 1) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutBack(float Start, float End, float Duration)
		{
			float s = 1.70158f;
			End -= Start;
			Duration /= .5f;
			if (Duration < 1)
			{
				s *= 1.525f;
				return End * 0.5f * (Duration * Duration * ((s + 1) * Duration - s)) + Start;
			}
			Duration -= 2;
			s *= 1.525f;
			return End * 0.5f * (Duration * Duration * ((s + 1) * Duration + s) + 2) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInElastic(float Start, float End, float Duration)
		{
			End -= Start;

			float d = 1f;
			float p = d * .3f;
			float s;
			float a = 0;

			if (Duration == 0) return Start;

			if ((Duration /= d) == 1) return Start + End;

			if (a == 0f || a < Mathf.Abs(End))
			{
				a = End;
				s = p / 4;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(End / a);
			}

			return -(a * Mathf.Pow(2, 10 * (Duration -= 1)) * Mathf.Sin((Duration * d - s) * (2 * Mathf.PI) / p)) + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutElastic(float Start, float End, float Duration)
		{
			End -= Start;

			float d = 1f;
			float p = d * .3f;
			float s;
			float a = 0;

			if (Duration == 0) return Start;

			if ((Duration /= d) == 1) return Start + End;

			if (a == 0f || a < Mathf.Abs(End))
			{
				a = End;
				s = p * 0.25f;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(End / a);
			}

			return a * Mathf.Pow(2, -10 * Duration) * Mathf.Sin((Duration * d - s) * (2 * Mathf.PI) / p) + End + Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutElastic(float Start, float End, float Duration)
		{
			End -= Start;

			float d = 1f;
			float p = d * .3f;
			float s;
			float a = 0;

			if (Duration == 0) return Start;

			if ((Duration /= d * 0.5f) == 2) return Start + End;

			if (a == 0f || a < Mathf.Abs(End))
			{
				a = End;
				s = p / 4;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(End / a);
			}

			if (Duration < 1) return -0.5f * (a * Mathf.Pow(2, 10 * (Duration -= 1)) * Mathf.Sin((Duration * d - s) * (2 * Mathf.PI) / p)) + Start;
			return a * Mathf.Pow(2, -10 * (Duration -= 1)) * Mathf.Sin((Duration * d - s) * (2 * Mathf.PI) / p) * 0.5f + End + Start;
		}

		// Derivatives.

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float LinearD(float Start, float End, float Duration)
		{
			return End - Start;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInQuadD(float Start, float End, float Duration)
		{
			return 2f * (End - Start) * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutQuadD(float Start, float End, float Duration)
		{
			End -= Start;
			return -End * Duration - End * (Duration - 2);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutQuadD(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;

			if (Duration < 1)
			{
				return End * Duration;
			}

			Duration--;

			return End * (1 - Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInCubicD(float Start, float End, float Duration)
		{
			return 3f * (End - Start) * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutCubicD(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return 3f * End * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutCubicD(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;

			if (Duration < 1)
			{
				return 3f / 2f * End * Duration * Duration;
			}

			Duration -= 2;

			return 3f / 2f * End * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInQuartD(float Start, float End, float Duration)
		{
			return 4f * (End - Start) * Duration * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutQuartD(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return -4f * End * Duration * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutQuartD(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;

			if (Duration < 1)
			{
				return 2f * End * Duration * Duration * Duration;
			}

			Duration -= 2;

			return -2f * End * Duration * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInQuintD(float Start, float End, float Duration)
		{
			return 5f * (End - Start) * Duration * Duration * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutQuintD(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return 5f * End * Duration * Duration * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutQuintD(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;

			if (Duration < 1)
			{
				return 5f / 2f * End * Duration * Duration * Duration * Duration;
			}

			Duration -= 2;

			return 5f / 2f * End * Duration * Duration * Duration * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInSineD(float Start, float End, float Duration)
		{
			return (End - Start) * 0.5f * Mathf.PI * Mathf.Sin(0.5f * Mathf.PI * Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutSineD(float Start, float End, float Duration)
		{
			End -= Start;
			return Mathf.PI * 0.5f * End * Mathf.Cos(Duration * (Mathf.PI * 0.5f));
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutSineD(float Start, float End, float Duration)
		{
			End -= Start;
			return End * 0.5f * Mathf.PI * Mathf.Sin(Mathf.PI * Duration);
		}
		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInExpoD(float Start, float End, float Duration)
		{
			return 10f * kNaturalLogOf2 * (End - Start) * Mathf.Pow(2f, 10f * (Duration - 1));
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutExpoD(float Start, float End, float Duration)
		{
			End -= Start;
			return 5f * kNaturalLogOf2 * End * Mathf.Pow(2f, 1f - 10f * Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutExpoD(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;

			if (Duration < 1)
			{
				return 5f * kNaturalLogOf2 * End * Mathf.Pow(2f, 10f * (Duration - 1));
			}

			Duration--;

			return 5f * kNaturalLogOf2 * End / Mathf.Pow(2f, 10f * Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInCircD(float Start, float End, float Duration)
		{
			return (End - Start) * Duration / Fast.FSqrt(1f - Duration * Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutCircD(float Start, float End, float Duration)
		{
			Duration--;
			End -= Start;
			return -End * Duration / Fast.FSqrt(1f - Duration * Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutCircD(float Start, float End, float Duration)
		{
			Duration /= .5f;
			End -= Start;

			if (Duration < 1)
			{
				return End * Duration / (2f * Fast.FSqrt(1f - Duration * Duration));
			}

			Duration -= 2;

			return -End * Duration / (2f * Fast.FSqrt(1f - Duration * Duration));
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInBounceD(float Start, float End, float Duration)
		{
			End -= Start;
			float d = 1f;

			return EaseOutBounceD(0, End, d - Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutBounceD(float Start, float End, float Duration)
		{
			Duration /= 1f;
			End -= Start;

			if (Duration < 1 / 2.75f)
			{
				return 2f * End * 7.5625f * Duration;
			}
			else if (Duration < 2 / 2.75f)
			{
				Duration -= 1.5f / 2.75f;
				return 2f * End * 7.5625f * Duration;
			}
			else if (Duration < 2.5 / 2.75)
			{
				Duration -= 2.25f / 2.75f;
				return 2f * End * 7.5625f * Duration;
			}
			else
			{
				Duration -= 2.625f / 2.75f;
				return 2f * End * 7.5625f * Duration;
			}
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutBounceD(float Start, float End, float Duration)
		{
			End -= Start;
			float d = 1f;

			if (Duration < d * 0.5f)
			{
				return EaseInBounceD(0, End, Duration * 2) * 0.5f;
			}
			else
			{
				return EaseOutBounceD(0, End, Duration * 2 - d) * 0.5f;
			}
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInBackD(float Start, float End, float Duration)
		{
			float s = 1.70158f;

			return 3f * (s + 1f) * (End - Start) * Duration * Duration - 2f * s * (End - Start) * Duration;
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutBackD(float Start, float End, float Duration)
		{
			float s = 1.70158f;
			End -= Start;
			Duration = Duration - 1;

			return End * ((s + 1f) * Duration * Duration + 2f * Duration * ((s + 1f) * Duration + s));
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutBackD(float Start, float End, float Duration)
		{
			float s = 1.70158f;
			End -= Start;
			Duration /= .5f;

			if (Duration < 1)
			{
				s *= 1.525f;
				return 0.5f * End * (s + 1) * Duration * Duration + End * Duration * ((s + 1f) * Duration - s);
			}

			Duration -= 2;
			s *= 1.525f;
			return 0.5f * End * ((s + 1) * Duration * Duration + 2f * Duration * ((s + 1f) * Duration + s));
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInElasticD(float Start, float End, float Duration)
		{
			return EaseOutElasticD(Start, End, 1f - Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseOutElasticD(float Start, float End, float Duration)
		{
			End -= Start;

			float d = 1f;
			float p = d * .3f;
			float s;
			float a = 0;

			if (a == 0f || a < Mathf.Abs(End))
			{
				a = End;
				s = p * 0.25f;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(End / a);
			}

			return a * Mathf.PI * d * Mathf.Pow(2f, 1f - 10f * Duration) *
			    Mathf.Cos(2f * Mathf.PI * (d * Duration - s) / p) / p - 5f * kNaturalLogOf2 * a *
			    Mathf.Pow(2f, 1f - 10f * Duration) * Mathf.Sin(2f * Mathf.PI * (d * Duration - s) / p);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float EaseInOutElasticD(float Start, float End, float Duration)
		{
			End -= Start;

			float d = 1f;
			float p = d * .3f;
			float s;
			float a = 0;

			if (a == 0f || a < Mathf.Abs(End))
			{
				a = End;
				s = p / 4;
			}
			else
			{
				s = p / (2 * Mathf.PI) * Mathf.Asin(End / a);
			}

			if (Duration < 1)
			{
				Duration -= 1;

				return -5f * kNaturalLogOf2 * a * Mathf.Pow(2f, 10f * Duration) * Mathf.Sin(2 * Mathf.PI * (d * Duration - 2f) / p) -
				    a * Mathf.PI * d * Mathf.Pow(2f, 10f * Duration) * Mathf.Cos(2 * Mathf.PI * (d * Duration - s) / p) / p;
			}

			Duration -= 1;

			return a * Mathf.PI * d * Mathf.Cos(2f * Mathf.PI * (d * Duration - s) / p) / (p * Mathf.Pow(2f, 10f * Duration)) -
			    5f * kNaturalLogOf2 * a * Mathf.Sin(2f * Mathf.PI * (d * Duration - s) / p) / Mathf.Pow(2f, 10f * Duration);
		}

		/// <summary></summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="Start"></param>
		/// <param name="End"></param>
		/// <param name="Duration"></param>
		/// <returns></returns>
		public static float SpringD(float Start, float End, float Duration)
		{
			Duration = Mathf.Clamp01(Duration);
			End -= Start;

			return End * (6f * (1f - Duration) / 5f + 1f) * (-2.2f * Mathf.Pow(1f - Duration, 1.2f) *
			    Mathf.Sin(Mathf.PI * Duration * (2.5f * Duration * Duration * Duration + 0.2f)) + Mathf.Pow(1f - Duration, 2.2f) *
			    (Mathf.PI * (2.5f * Duration * Duration * Duration + 0.2f) + 7.5f * Mathf.PI * Duration * Duration * Duration) *
			    Mathf.Cos(Mathf.PI * Duration * (2.5f * Duration * Duration * Duration + 0.2f)) + 1f) -
			    6f * End * (Mathf.Pow(1 - Duration, 2.2f) * Mathf.Sin(Mathf.PI * Duration * (2.5f * Duration * Duration * Duration + 0.2f)) + Duration
			    / 5f);

		}

		/// <summary>Gets the derivative function of the appropriate easing function. If you use an easing function for position then this function can get you the speed at a given time (normalised).</summary>
		/// <decorations decor="public static Function"></decorations>
		/// <param name="Function"></param>
		/// <returns>The derivative function</returns>
		public static Function GetEasingFunctionDerivative(EEquation Function)
		{

			switch (Function)
			{
				case EEquation.EaseInQuad:
					return EaseInQuadD;
				case EEquation.EaseOutQuad:
					return EaseOutQuadD;
				case EEquation.EaseInOutQuad:
					return EaseInOutQuadD;
				case EEquation.EaseInCubic:
					return EaseInCubicD;
				case EEquation.EaseOutCubic:
					return EaseOutCubicD;
				case EEquation.EaseInOutCubic:
					return EaseInOutCubicD;
				case EEquation.EaseInQuart:
					return EaseInQuartD;
				case EEquation.EaseOutQuart:
					return EaseOutQuartD;
				case EEquation.EaseInOutQuart:
					return EaseInOutQuartD;
				case EEquation.EaseInQuint:
					return EaseInQuintD;
				case EEquation.EaseOutQuint:
					return EaseOutQuintD;
				case EEquation.EaseInOutQuint:
					return EaseInOutQuintD;
				case EEquation.EaseInSine:
					return EaseInSineD;
				case EEquation.EaseOutSine:
					return EaseOutSineD;
				case EEquation.EaseInOutSine:
					return EaseInOutSineD;
				case EEquation.EaseInExpo:
					return EaseInExpoD;
				case EEquation.EaseOutExpo:
					return EaseOutExpoD;
				case EEquation.EaseInOutExpo:
					return EaseInOutExpoD;
				case EEquation.EaseInCirc:
					return EaseInCircD;
				case EEquation.EaseOutCirc:
					return EaseOutCircD;
				case EEquation.EaseInOutCirc:
					return EaseInOutCircD;
				case EEquation.Linear:
					return LinearD;
				case EEquation.Spring:
					return SpringD;
				case EEquation.EaseInBounce:
					return EaseInBounceD;
				case EEquation.EaseOutBounce:
					return EaseOutBounceD;
				case EEquation.EaseInOutBounce:
					return EaseInOutBounceD;
				case EEquation.EaseInBack:
					return EaseInBackD;
				case EEquation.EaseOutBack:
					return EaseOutBackD;
				case EEquation.EaseInOutBack:
					return EaseInOutBackD;
				case EEquation.EaseInElastic:
					return EaseInElasticD;
				case EEquation.EaseOutElastic:
					return EaseOutElasticD;
				case EEquation.EaseInOutElastic:
					return EaseInOutElasticD;
			}

			return LinearD;
		}

		#endregion

		/// <summary>Ease with EEquation through Start to End over Duration.</summary>
		/// <decorations decor="public static float"></decorations>
		/// <param name="EEquation">EEquation to for interpolation..</param>
		/// <param name="Start">Starting value.</param>
		/// <param name="End">Ending value.</param>
		/// <param name="Alpha">Duration in seconds.</param>
		/// <returns>An interpolated value using EEquation evaluated using Alpha between Start and End.</returns>
		public static float Ease(EEquation EEquation, float Start, float End, float Alpha)
		{
			switch (EEquation)
			{
				case EEquation.EaseInQuad:
					return EaseInQuad(Start, End, Alpha);
				case EEquation.EaseOutQuad:
					return EaseOutQuad(Start, End, Alpha);
				case EEquation.EaseInOutQuad:
					return EaseInOutQuad(Start, End, Alpha);
				case EEquation.EaseInCubic:
					return EaseInCubic(Start, End, Alpha);
				case EEquation.EaseOutCubic:
					return EaseOutCubic(Start, End, Alpha);
				case EEquation.EaseInOutCubic:
					return EaseInOutCubic(Start, End, Alpha);
				case EEquation.EaseInQuart:
					return EaseInQuart(Start, End, Alpha);
				case EEquation.EaseOutQuart:
					return EaseOutQuart(Start, End, Alpha);
				case EEquation.EaseInOutQuart:
					return EaseInOutQuart(Start, End, Alpha);
				case EEquation.EaseInQuint:
					return EaseInQuint(Start, End, Alpha);
				case EEquation.EaseOutQuint:
					return EaseOutQuint(Start, End, Alpha);
				case EEquation.EaseInOutQuint:
					return EaseInOutQuint(Start, End, Alpha);
				case EEquation.EaseInSine:
					return EaseInSine(Start, End, Alpha);
				case EEquation.EaseOutSine:
					return EaseOutSine(Start, End, Alpha);
				case EEquation.EaseInOutSine:
					return EaseInOutSine(Start, End, Alpha);
				case EEquation.EaseInExpo:
					return EaseInExpo(Start, End, Alpha);
				case EEquation.EaseOutExpo:
					return EaseOutExpo(Start, End, Alpha);
				case EEquation.EaseInOutExpo:
					return EaseInOutExpo(Start, End, Alpha);
				case EEquation.EaseInCirc:
					return EaseInCirc(Start, End, Alpha);
				case EEquation.EaseOutCirc:
					return EaseOutCirc(Start, End, Alpha);
				case EEquation.EaseInOutCirc:
					return EaseInOutCirc(Start, End, Alpha);
				case EEquation.Linear:
					return Linear(Start, End, Alpha);
				case EEquation.Spring:
					return Spring(Start, End, Alpha);
				case EEquation.EaseInBounce:
					return EaseInBounce(Start, End, Alpha);
				case EEquation.EaseOutBounce:
					return EaseOutBounce(Start, End, Alpha);
				case EEquation.EaseInOutBounce:
					return EaseInOutBounce(Start, End, Alpha);
				case EEquation.EaseInBack:
					return EaseInBack(Start, End, Alpha);
				case EEquation.EaseOutBack:
					return EaseOutBack(Start, End, Alpha);
				case EEquation.EaseInOutBack:
					return EaseInOutBack(Start, End, Alpha);
				case EEquation.EaseInElastic:
					return EaseInElastic(Start, End, Alpha);
				case EEquation.EaseOutElastic:
					return EaseOutElastic(Start, End, Alpha);
				case EEquation.EaseInOutElastic:
					return EaseInOutElastic(Start, End, Alpha);
				default:
					Debug.LogWarning("Could not convert Easing.EEquation.\nReturning Easing.Linear instead.");
					return Linear(Start, End, Alpha);
			}
		}
	}

	public delegate float Function(float Start, float End, float Duration);
}