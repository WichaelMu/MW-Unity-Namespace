using UnityEngine;
using MW.Diagnostics;

namespace MW.Behaviour
{
	/// <summary>A custom utility class that assists in debugging game data.</summary>
	public class VisualDebugger : MUnique<VisualDebugger>
	{
		/// <docs>True to disable GUI draws to the game screen.</docs>
		[Tooltip("True to disable GUI draws to the game screen.")] public bool bDisableGUI;
		/// <docs>True to disable VDebug Logs to the console.</docs>
		[Tooltip("True to disable VDebug Logs to the console.")] public bool bDisableLogs;

		[SerializeField, Min(0f), Tooltip("How often should GUI be updated in seconds? 0 is every frame.")] float GUIUpdateInterval = 0f;

		/// <summary><see cref="Log.Auto(string, EVerbosity)"/> <paramref name="Content"/> with <paramref name="Verbosity"/> in <paramref name="HexColour"/>.</summary>
		/// <docs>Log.Auto() Content with Verbosity in HexColour.</docs>
		/// <param name="Content">The string to log to the console.</param>
		/// <param name="HexColour">The Colour to display Content in the console.</param>
		/// <param name="Verbosity">The level of the log.</param>
		public void Log(object Content, string HexColour = "#FFFFFF", EVerbosity Verbosity = EVerbosity.Log)
		{
			if (!bDisableLogs)
				Diagnostics.Log.Auto("<color=" + HexColour + ">" + Content.ToString() + "</color>", Verbosity);
		}

		/// <summary>Draws a line to the editor screen.</summary>
		/// <param name="Params">Draw Parameters.</param>
		public void DrawLine(DrawParams Params)
		{
			Debug.DrawLine(Params.From, Params.To, Params.Colour);
		}

		internal MArray<TTriple<string, Vector2, Color>> OnGUITextDraws;

		/// <summary>Draws text onto the game screen with OnGUI().</summary>
		/// <param name="Content">The string to display to the game screen.</param>
		/// <param name="Width">The width of the Rect that will be drawn onto the screen.</param>
		/// <param name="Height">The height of the Rect that will be drawn onto the screen.</param>
		public void DrawGUIText(string Content, int Width = 250, int Height = 150)
		{
			if (OnGUITextDraws == null)
				OnGUITextDraws = new MArray<TTriple<string, Vector2, Color>>();

			OnGUITextDraws.Push(new TTriple<string, Vector2, Color>(Content, new Vector2(Width, Height), Color.white));
		}

		/// <summary>Draws text onto the game screen with OnGUI().</summary>
		/// <param name="Content">The string to display to the game screen.</param>
		/// <param name="Colour">The colour of Content.</param>
		/// <param name="Width">The width of the Rect that will be drawn onto the screen.</param>
		/// <param name="Height">The height of the Rect that will be drawn onto the screen.</param>
		public void DrawGUIText(string Content, Color Colour, int Width = 250, int Height = 150)
		{
			if (OnGUITextDraws == null)
				OnGUITextDraws = new MArray<TTriple<string, Vector2, Color>>();

			OnGUITextDraws.Push(new TTriple<string, Vector2, Color>(Content, new Vector2(Width, Height), Colour));
		}

		float t = 0f;

		void OnGUI()
		{
			if (!bDisableGUI && MArray<TTriple<string, Vector2, Color>>.CheckNull(OnGUITextDraws))
			{
				if (t >= GUIUpdateInterval)
				{
					for (int i = 0; i < OnGUITextDraws.Num; ++i)
					{
						int IndexPosition = i == 0 ? 10 : i * 25;
						Vector2 GUIPos = OnGUITextDraws[i].Second;
						Rect Rect = new Rect(0, IndexPosition, GUIPos.x, GUIPos.y);
						GUI.color = OnGUITextDraws[i].Third;
						GUI.Label(Rect, OnGUITextDraws[i].First);
					}

					t = 0f;
				}
			}
		}
	}

	/// <summary>Parameters controlling how lines are drawn to the screen.</summary>
	public struct DrawParams
	{
		/// <summary>Where the line will originate.</summary>
		public MVector From;
		/// <summary>Where the line will terminate.</summary>
		public MVector To;
		/// <summary>The colour of the line.</summary>
		public Color Colour;
		/// <summary>How long the line will remain on screen in seconds.</summary>
		public float Duration;

		/// <summary>Define only From and To. Default Colour is white and Duration is a single frame.</summary>
		/// <param name="From"></param>
		/// <param name="To"></param>
		public DrawParams(MVector From, MVector To)
		{
			this.From = From;
			this.To = To;
			Colour = Color.white;
			Duration = 0f;
		}

		/// <summary>Define From, To and Colour. Default Duration is a single frame.</summary>
		/// <param name="From"></param>
		/// <param name="To"></param>
		/// <param name="Colour"></param>
		public DrawParams(MVector From, MVector To, Color Colour) : this(From, To)
		{
			this.Colour = Colour;
			Duration = 0f;
		}

		/// <summary>Fully construct Draw Parameters.</summary>
		/// <param name="From"></param>
		/// <param name="To"></param>
		/// <param name="Colour"></param>
		/// <param name="Duration"></param>
		public DrawParams(MVector From, MVector To, Color Colour, float Duration) : this(From, To, Colour)
		{
			this.Duration = Duration;
		}
	}
}
