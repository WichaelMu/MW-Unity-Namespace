using MW.Diagnostics;

namespace MW.Behaviour
{
	/// <summary>Inter-frame information recording utility.</summary>
	public struct IntervalInformation
	{
		internal LastIntervalInformation Last;
		internal ThisIntervalInformation This;

		internal PlayerBase RecordedPlayer;

		/// <summary>Records an Interval.</summary>
		/// <remarks>Call before Mark.</remarks>
		/// <param name="Player">The Player to record.</param>
		public void Record(PlayerBase Player)
		{
			Last.Position = Player.Position;
			Last.Rotation = Player.Rotation;
			Last.Time = UnityEngine.Time.time;

			RecordedPlayer = Player;
		}

		/// <summary>Marks the end of an Interval.</summary>
		/// <param name="Player">The Player to get interval information.</param>
		public void Mark(PlayerBase Player)
		{
			if (RecordedPlayer.GetHashCode() != Player.GetHashCode())
			{
				Log.E("The recorded Player is not the same as the marking Player!\nRecorded Player:", RecordedPlayer.name, "Marking Player:", Player.name);
				Stacktrace.Here(EVerbosity.Error);
			}
			else
			{
				This.Position = Player.Position;
				This.Rotation = Player.Rotation;
				This.Time = UnityEngine.Time.time;
			}
		}

		/// <summary>Marks the end of an interval.</summary>
		/// <param name="Player">The Player to get interval information.</param>
		/// <param name="LastInterval">Outs the last interval information.</param>
		/// <param name="ThisInterval">Outs the current interval information.</param>
		public void Mark(PlayerBase Player, out LastIntervalInformation LastInterval, out ThisIntervalInformation ThisInterval)
		{
			Mark(Player);

			LastInterval = Last;
			ThisInterval = This;
		}

		/// <summary>Get the information at the last recorded interval.</summary>
		/// <returns>The Last Interval Information from the last recorded interval.</returns>
		public LastIntervalInformation GetLast()
		{
			return Last;
		}
	}

	/// <summary>Information about this Player on the previous record.</summary>
	public struct LastIntervalInformation
	{
		/// <summary>The Position of this Player on the previous record.</summary>
		public MVector Position;
		/// <summary>The Rotation of this Player on the previous record.</summary>
		public MRotator Rotation;

		public float Time;

		internal LastIntervalInformation(MVector Position, MRotator Rotation, float Time)
		{
			this.Position = Position;
			this.Rotation = Rotation;

			this.Time = Time;
		}
	}

	/// <summary>Information about this Player on the current record.</summary>
	public struct ThisIntervalInformation
	{
		/// <summary>The Position of this Player on the current record.</summary>
		public MVector Position;
		/// <summary>The Position of this Player on the current record.</summary>
		public MRotator Rotation;

		public float Time;

		internal ThisIntervalInformation(MVector Position, MRotator Rotation, float Time)
		{
			this.Position = Position;
			this.Rotation = Rotation;

			this.Time = Time;
		}

		/// <summary>The difference in positions of This and Last.</summary>
		/// <param name="Last">The Last position.</param>
		/// <returns>This.Position - Last.Position.</returns>
		public MVector DeltaPosition(LastIntervalInformation Last)
		{
			return Position - Last.Position;
		}

		/// <summary>The difference in rotations of This and Last.</summary>
		/// <param name="Last">The Last rotation.</param>
		/// <returns>This.Rotation - Last.Rotation.</returns>
		public MRotator DeltaRotation(LastIntervalInformation Last)
		{
			return Rotation - Last.Rotation;
		}

		/// <summary>The time between the Last Interval and This Interval.</summary>
		/// <param name="Last">The Last recorded time.</param>
		/// <returns>This.Time - Last.Time.</returns>
		public float TimeSinceLast(LastIntervalInformation Last)
		{
			return Time - Last.Time;
		}

		/// <summary>Get Deltas in all respects.</summary>
		/// <param name="Last">The Last recorded interval.</param>
		/// <param name="DeltaPosition"></param>
		/// <param name="DeltaRotation"></param>
		/// <param name="DeltaTime"></param>
		public void GetDeltas(LastIntervalInformation Last, out MVector DeltaPosition, out MRotator DeltaRotation, out float DeltaTime)
		{
			DeltaPosition = this.DeltaPosition(Last);
			DeltaRotation = this.DeltaRotation(Last);
			DeltaTime = TimeSinceLast(Last);
		}
	}
}
