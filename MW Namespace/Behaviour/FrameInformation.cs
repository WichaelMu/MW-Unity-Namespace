using MW.Diagnostics;

namespace MW.Behaviour
{
	/// <summary>Information recording utility over time.</summary>
	public struct IntervalInformation
	{
		internal LastIntervalInformation Last;
		internal ThisIntervalInformation This;

		internal Player RecordedPlayer;

		/// <summary>Records an Interval.</summary>
		/// <remarks>Call before Mark.</remarks>
		/// <param name="Player">The Player to record.</param>
		public void Record(Player Player)
		{
			Last.Position = Player.Position;
			Last.Rotation = Player.Rotation;

			RecordedPlayer = Player;
		}

		/// <summary>Marks the end of an Interval.</summary>
		/// <param name="Player">The Player to get interval information.</param>
		public void Mark(Player Player)
		{
			if (RecordedPlayer.GetHashCode() != Player.GetHashCode())
			{
				Log.E("The recorded Player is not the same as the marking Player!\nRecorded Player:", RecordedPlayer.name, "Marked Player:", Player.name);
				Stacktrace.Here(EVerbosity.Error);
			}
			else
			{
				This.Position = Player.Position;
				This.Rotation = Player.Rotation;
			}
		}

		/// <summary>Marks the end of an interval.</summary>
		/// <param name="Player">The Player to get interval information.</param>
		/// <param name="LastInterval">Outs the last interval information.</param>
		/// <param name="ThisInterval">Outs the current interval information.</param>
		public void Mark(Player Player, out LastIntervalInformation LastInterval, out ThisIntervalInformation ThisInterval)
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

	/// <summary>Information abot this Player on the previous record.</summary>
	public struct LastIntervalInformation
	{
		/// <summary>The Position of this Player on the previous record.</summary>
		public MVector Position;
		/// <summary>The Rotation of this Player on the previous record.</summary>
		public MRotator Rotation;

		internal LastIntervalInformation(MVector Position, MRotator Rotation)
		{
			this.Position = Position;
			this.Rotation = Rotation;
		}
	}

	/// <summary>Information about this Player on the current record.</summary>
	public struct ThisIntervalInformation
	{
		/// <summary>The Position of this Player on the current record.</summary>
		public MVector Position;
		/// <summary>The Position of this Player on the current record.</summary>
		public MRotator Rotation;

		internal ThisIntervalInformation(MVector Position, MRotator Rotation)
		{
			this.Position = Position;
			this.Rotation = Rotation;
		}

		public MVector DeltaPosition(LastIntervalInformation Last)
		{
			return Position - Last.Position;
		}

		public MRotator DeltaRotation(LastIntervalInformation Last)
		{
			return Rotation - Last.Rotation;
		}
	}
}
