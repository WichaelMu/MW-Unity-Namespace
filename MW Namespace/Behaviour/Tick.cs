using MW.Diagnostics;
using MW.Easing;
using System;
using System.Collections;
using UnityEngine;

namespace MW.Behaviour
{
	/// <summary>Generates a separate Tick function.</summary>
	/// <typeparam name="T">The data to perform OnTick.</typeparam>
	/// <decorations decor="public class {T} : MonoBehaviour"></decorations>
	public class Tick<T> : MonoBehaviour
	{
		IEnumerator ThisTick;
		EEquation Equation;
		float DurationInSeconds, StartInterpolation, EndInterpolation;
		Action<T, float> OnTick;

		MArray<T> Data;
		bool bIsRunning;
		float Time;
		float InverseEndInterpolation;

		/// <summary>Creates a new separate update loop.</summary>
		/// <param name="Equation">The Equation to use to Tick.</param>
		/// <param name="DurationInSeconds">The duration of this update loop in seconds.</param>
		/// <param name="OnTick">The method to call every tick. The executed T and interpolated Time is passed as a parameter.</param>
		public Tick(EEquation Equation, float DurationInSeconds, Action<T, float> OnTick)
		{
			this.Equation = Equation;
			this.DurationInSeconds = DurationInSeconds;
			StartInterpolation = 0;
			EndInterpolation = 1;
			InverseEndInterpolation = 1 / EndInterpolation;
			this.OnTick = OnTick;

			ThisTick = Run();
			StartCoroutine(ThisTick);
		}


		/// <inheritdoc cref="Tick(EEquation, float, Action{T, float})"/>
		/// <param name="Equation"></param> <param name="DurationInSeconds"></param> <param name="OnTick"></param>
		/// <param name="StartInterpolation">Where to begin interpolation.</param>
		/// <param name="EndInterpolation">Where to end interpolation.</param>
		public Tick(EEquation Equation, float DurationInSeconds, Action<T, float> OnTick, float StartInterpolation, float EndInterpolation)
			: this(Equation, DurationInSeconds, OnTick)
		{
			this.StartInterpolation = StartInterpolation;
			this.EndInterpolation = EndInterpolation;
		}

		/// <summary>Sets the data for the next execution of OnTick.</summary>
		/// <remarks>All entries in Data will be executed linearly.</remarks>
		/// <decorations decor="public void"></decorations>
		/// <param name="Data">The data to set for the next OnTick.</param>
		public void SetData(MArray<T> Data)
		{
			if (MArray<T>.CheckNull(Data))
			{
				Stacktrace.Here($"The MArray<{nameof(T)}> being passed into {nameof(SetData)} is null.", EVerbosity.Error);
				return;
			}

			this.Data = Data;
		}

		IEnumerator Run()
		{
			Time = 0;
			float InverseTime = 1 / DurationInSeconds;

			while (true)
			{
				if (bIsRunning)
				{
					Time += UnityEngine.Time.deltaTime * Interpolate.Ease(Equation, StartInterpolation, EndInterpolation, InverseTime);

					for (int i = 0; i < Data.Num; ++i)
						OnTick?.Invoke(Data[i], Time);
				}

				yield return null;
			}
		}

		/// <summary>Pauses this Tick from executing until this <see cref="TogglePauseTick"/> is called again.</summary>
		/// <docs>Pauses this Tick from executing until this (TogglePauseTick) is called again.</docs>
		/// <decorations decor="public void"></decorations>
		/// <remarks>This is a toggle.</remarks>
		public void TogglePauseTick()
		{
			bIsRunning = !bIsRunning;
		}

		/// <summary>Stop executing this Tick.</summary>
		/// <decorations decor="public void"></decorations>
		public void Terminate()
		{
			StopCoroutine(GetTickFunction());
		}

		public IEnumerator GetTickFunction()
		{
			return ThisTick;
		}

		/// <docreturns>Information about the time and percentage complete on this Tick function.</docreturns>
		/// <decorations decor="public TickInformation"></decorations>
		/// <returns><see cref="TickInformation"/></returns>
		public TickInformation GetTickInfo()
		{
			return new TickInformation(Time, Time * InverseEndInterpolation);
		}

		public struct TickInformation
		{
			public float Time;
			public float PercentageComplete;

			public TickInformation(float Time, float PercentageComplete)
			{
				this.Time = Time;
				this.PercentageComplete = PercentageComplete;
			}
		}
	}
}