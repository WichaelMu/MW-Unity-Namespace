using UnityEngine;

namespace MW.HUD
{
	/// <summary>Drawing LineRenderers in the game.</summary>
	public static class Line
	{
		/// <summary>Draws a line from to to in StartColor to EndColor at LineWidth thickness with an Offset in bUseWorldSpace.</summary>
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

		/// <summary>Draws a line from to to in StartColor to EndColor at LineWidth thickness with an Offset in bUseWorldSpace.</summary>
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
			LineRenderer _LR = Self.GetComponent<LineRenderer>() ?? Self.AddComponent<LineRenderer>();
			_LR.startColor = StartColour;
			_LR.endColor = EndColour;
			_LR.startWidth = LineWidth;
			_LR.endWidth = LineWidth;
			_LR.positionCount = 2;
			_LR.useWorldSpace = bUseWorldSpace;

			_LR.SetPosition(0, From + Offset);
			_LR.SetPosition(1, To + Offset);
		}

		/// <summary>Draws a circle with a centre at Around at Radius with a LineColour at LineWidth thickness in bUseWorldSpace with NumberOfSegments.</summary>
		/// <param name="LineRenderer">The LineRenderer of the GameObject calling this.</param>
		/// <param name="Around">The centre of the circle to be drawn.</param>
		/// <param name="Radius">The radius of this circle.</param>
		/// <param name="LineColour">The colour of this circle.</param>
		/// <param name="LineWidth">The thickness of this circle.</param>
		/// <param name="Material">The material used to draw the circle.</param>
		/// <param name="bUseWorldSpace">Should this circle use world space?</param>
		/// <param name="NumberOfSegments">The number of verticies of this circle.</param>
		public static void DrawCircle(LineRenderer LineRenderer, Vector3 Around, float Radius, Color LineColour, float LineWidth, Material Material, bool bUseWorldSpace = true, int NumberOfSegments = 1)
		{

			LineRenderer.material = Material;
			LineRenderer.startColor = LineColour;
			LineRenderer.endColor = LineColour;
			LineRenderer.startWidth = LineWidth;
			LineRenderer.endWidth = LineWidth;
			LineRenderer.positionCount = NumberOfSegments + 1;
			LineRenderer.useWorldSpace = bUseWorldSpace;

			float deltaTheta = (float)(2.0f * Mathf.PI) / NumberOfSegments;
			float theta = 0f;

			for (int i = 0; i < NumberOfSegments + 1; i++)
			{
				float x = Radius * Mathf.Cos(theta);
				float z = Radius * Mathf.Sin(theta);
				Vector3 pos = new Vector3(x, 0, z);
				LineRenderer.SetPosition(i, pos + Around);
				theta += deltaTheta;
			}
		}

		/// <summary>Draws a circle with a centre at Around at Radius with a LineColour at LineWidth thickness in bUseWorldSpace with NumberOfSegments.</summary>
		/// <param name="Self">The GameObject calling this.</param>
		/// <param name="Around">The centre of the circle to be drawn.</param>
		/// <param name="Radius">The radius of this circle.</param>
		/// <param name="LineColour">The colour of this circle.</param>
		/// <param name="LineWidth">The thickness of this circle.</param>
		/// <param name="bUseWorldSpace">Should this circle use world space?</param>
		/// <param name="NumberOfSegments">The number of verticies of this circle.</param>
		public static void DrawCircle(GameObject Self, Vector3 Around, float Radius, Color LineColour, float LineWidth, bool bUseWorldSpace = true, int NumberOfSegments = 1)
		{
			LineRenderer _LR = Self.GetComponent<LineRenderer>() ?? Self.AddComponent<LineRenderer>();
			_LR.startColor = LineColour;
			_LR.endColor = LineColour;
			_LR.startWidth = LineWidth;
			_LR.endWidth = LineWidth;
			_LR.positionCount = NumberOfSegments + 1;
			_LR.useWorldSpace = bUseWorldSpace;

			float deltaTheta = (float)(2.0f * Mathf.PI) / NumberOfSegments;
			float theta = 0f;

			for (int i = 0; i < NumberOfSegments + 1; i++)
			{
				float x = Radius * Mathf.Cos(theta);
				float z = Radius * Mathf.Sin(theta);
				Vector3 pos = new Vector3(x, 0, z);
				_LR.SetPosition(i, pos + Around);
				theta += deltaTheta;
			}
		}
	}
}
