using UnityEngine;
using MW.Extensions;
using MW.Math;

namespace MW.HUD.Line
{
	/// <summary>Drawing LineRenderers in the game.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class Line
	{
		/// <summary>Draws a line from to To in StartColor to EndColor at LineWidth thickness with an Offset in bUseWorldSpace.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="LineRenderer">The LineRenderer of the GameObject calling this.</param>
		/// <param name="From">The coordinates where the line will originate.</param>
		/// <param name="To">The coordinates where the line will end.</param>
		/// <param name="StartColour">The starting colour gradient for this line.</param>
		/// <param name="EndColour">The ending colour gradient for this line.</param>
		/// <param name="LineWidth">The thickness of this line.</param>
		/// <param name="Offset">The offset to place this line.</param>
		/// <param name="Material">The material used to draw the line.</param>
		/// <param name="bUseWorldSpace">Should this line use world space?</param>
		public static void DrawLine(LineRenderer LineRenderer, Vector3 From, Vector3 To, Color StartColour, Color EndColour, float LineWidth, Vector3 Offset, Material Material, bool bUseWorldSpace = true)
		{
			LineRenderer.material = Material;
			LineRenderer.startColor = StartColour;
			LineRenderer.endColor = EndColour;
			LineRenderer.startWidth = LineWidth;
			LineRenderer.endWidth = LineWidth;
			LineRenderer.positionCount = 2;
			LineRenderer.useWorldSpace = bUseWorldSpace;

			LineRenderer.SetPosition(0, From + Offset);
			LineRenderer.SetPosition(1, To + Offset);
		}

		/// <summary>Draws a line from to To in StartColor to EndColor at LineWidth thickness with an Offset in bUseWorldSpace.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Self">The GameObject calling this.</param>
		/// <param name="From">The coordinates where the line will originate.</param>
		/// <param name="To">The coordinates where the line will end.</param>
		/// <param name="StartColour">The starting colour gradient for this line.</param>
		/// <param name="EndColour">The ending colour gradient for this line.</param>
		/// <param name="LineWidth">The thickness of this line.</param>
		/// <param name="Offset">The offset to place this line.</param>
		/// <param name="bUseWorldSpace">Should this line use world space?</param>
		public static void DrawLine(GameObject Self, Vector3 From, Vector3 To, Color StartColour, Color EndColour, float LineWidth, Vector3 Offset, bool bUseWorldSpace = true)
		{
			LineRenderer LineRenderer = Self.GetOrAddComponent<LineRenderer>();
			LineRenderer.startColor = StartColour;
			LineRenderer.endColor = EndColour;
			LineRenderer.startWidth = LineWidth;
			LineRenderer.endWidth = LineWidth;
			LineRenderer.positionCount = 2;
			LineRenderer.useWorldSpace = bUseWorldSpace;

			LineRenderer.SetPosition(0, From + Offset);
			LineRenderer.SetPosition(1, To + Offset);
		}

		/// <summary>Draws a circle with a centre at Around at Radius with a LineColour at LineWidth thickness in bUseWorldSpace with NumberOfSegments.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="LineRenderer">The LineRenderer of the GameObject calling this.</param>
		/// <param name="Around">The centre of the circle to be drawn.</param>
		/// <param name="Radius">The radius of this circle.</param>
		/// <param name="LineColour">The colour of this circle.</param>
		/// <param name="LineWidth">The thickness of this circle.</param>
		/// <param name="Material">The material used to draw the circle.</param>
		/// <param name="bUseWorldSpace">Should this circle use world space?</param>
		/// <param name="NumberOfSegments">The number of vertices of this circle.</param>
		public static void DrawCircle(LineRenderer LineRenderer, Vector3 Around, float Radius, Color LineColour, float LineWidth, Material Material, bool bUseWorldSpace = true, int NumberOfSegments = 1)
		{
			LineRenderer.material = Material;
			LineRenderer.startColor = LineColour;
			LineRenderer.endColor = LineColour;
			LineRenderer.startWidth = LineWidth;
			LineRenderer.endWidth = LineWidth;
			LineRenderer.positionCount = NumberOfSegments + 1;
			LineRenderer.useWorldSpace = bUseWorldSpace;

			float DeltaTheta = Utils.k2PI / NumberOfSegments;
			float Theta = 0f;

			for (int i = 0; i < NumberOfSegments + 1; i++)
			{
				Mathematics.SinCos(out float Sine, out float Cosine, Theta);
				float X = Radius * Cosine;
				float Z = Radius * Sine;
				Vector3 Pos = new Vector3(X, 0, Z);
				LineRenderer.SetPosition(i, Pos + Around);
				Theta += DeltaTheta;
			}
		}

		/// <summary>Draws a circle with a centre at Around at Radius with a LineColour at LineWidth thickness in bUseWorldSpace with NumberOfSegments.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Self">The GameObject calling this.</param>
		/// <param name="Around">The centre of the circle to be drawn.</param>
		/// <param name="Radius">The radius of this circle.</param>
		/// <param name="LineColour">The colour of this circle.</param>
		/// <param name="LineWidth">The thickness of this circle.</param>
		/// <param name="bUseWorldSpace">Should this circle use world space?</param>
		/// <param name="NumberOfSegments">The number of vertices of this circle.</param>
		public static void DrawCircle(GameObject Self, Vector3 Around, float Radius, Color LineColour, float LineWidth, bool bUseWorldSpace = true, int NumberOfSegments = 1)
		{
			LineRenderer LineRenderer = Self.GetOrAddComponent<LineRenderer>();
			LineRenderer.startColor = LineColour;
			LineRenderer.endColor = LineColour;
			LineRenderer.startWidth = LineWidth;
			LineRenderer.endWidth = LineWidth;
			LineRenderer.positionCount = NumberOfSegments + 1;
			LineRenderer.useWorldSpace = bUseWorldSpace;

			float DeltaTheta = Utils.k2PI / NumberOfSegments;
			float Theta = 0f;

			for (int i = 0; i < NumberOfSegments + 1; i++)
			{
				Mathematics.SinCos(out float Sine, out float Cosine, Theta);
				float X = Radius * Cosine;
				float Z = Radius * Sine;
				Vector3 Pos = new Vector3(X, 0, Z);
				LineRenderer.SetPosition(i, Pos + Around);
				Theta += DeltaTheta;
			}
		}
	}
}
