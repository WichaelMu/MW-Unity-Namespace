using UnityEngine;

namespace MW.HUD
{
	/// <summary>Drawing <see cref="LineRenderer"/>s in the game.</summary>
	public static class Line
	{
		/// <summary>Draws a line from to to in StartColor to EndColor at LineWidth thickness with an offset at UseWorldSpace with NumberOfSegments.</summary>
		/// <param name="LRSelf">The LineRenderer of the GameObject calling this.</param>
		/// <param name="vFrom">The coordinates where the line will originate.</param>
		/// <param name="vTo">The coordinates where the line will end.</param>
		/// <param name="colStartColour">The starting colour gradient for this line.</param>
		/// <param name="colEndColour">The ending colour gradient for this line.</param>
		/// <param name="fLineWidth">The thickness of this line.</param>
		/// <param name="vOffset">The offset to place this line.</param>
		/// <param name="MMaterial">The material used to draw the line.</param>
		/// <param name="bUseWorldSpace">Should this line use world space?</param>
		public static void DrawLine(LineRenderer LRSelf, Vector3 vFrom, Vector3 vTo, Color colStartColour, Color colEndColour, float fLineWidth, Vector3 vOffset, Material MMaterial, bool bUseWorldSpace = true)
		{

			LRSelf.material = MMaterial;
			LRSelf.startColor = colStartColour;
			LRSelf.endColor = colEndColour;
			LRSelf.startWidth = fLineWidth;
			LRSelf.endWidth = fLineWidth;
			LRSelf.positionCount = 2;
			LRSelf.useWorldSpace = bUseWorldSpace;

			LRSelf.SetPosition(0, vFrom + vOffset);
			LRSelf.SetPosition(1, vTo + vOffset);
		}

		/// <summary>Draws a line from to to in StartColor to EndColor at LineWidth thickness with an offset at UseWorldSpace with NumberOfSegments.</summary>
		/// <param name="GSelf">The GameObject calling this.</param>
		/// <param name="vFrom">The coordinates where the line will originate.</param>
		/// <param name="vTo">The coordinates where the line will end.</param>
		/// <param name="colStartColour">The starting colour gradient for this line.</param>
		/// <param name="colEndColour">The ending colour gradient for this line.</param>
		/// <param name="fLineWidth">The thickness of this line.</param>
		/// <param name="vOffset">The offset to place this line.</param>
		/// <param name="bUseWorldSpace">Should this line use world space?</param>
		public static void DrawLine(GameObject GSelf, Vector3 vFrom, Vector3 vTo, Color colStartColour, Color colEndColour, float fLineWidth, Vector3 vOffset, bool bUseWorldSpace = true)
		{
			LineRenderer _LR = GSelf.GetComponent<LineRenderer>() ?? GSelf.AddComponent<LineRenderer>();
			_LR.startColor = colStartColour;
			_LR.endColor = colEndColour;
			_LR.startWidth = fLineWidth;
			_LR.endWidth = fLineWidth;
			_LR.positionCount = 2;
			_LR.useWorldSpace = bUseWorldSpace;

			_LR.SetPosition(0, vFrom + vOffset);
			_LR.SetPosition(1, vTo + vOffset);
		}

		/// <summary>Draws a circle with a centre at around at radius with a LineColour at LineWidth thickness at UseWorldSpace with NumberOfSegments.</summary>
		/// <param name="LRSelf">The LineRenderer of the GameObject calling this.</param>
		/// <param name="vAround">The centre of the circle to be drawn.</param>
		/// <param name="fRadius">The radius of this circle.</param>
		/// <param name="colLineColour">The colour of this circle.</param>
		/// <param name="fLineWidth">The thickness of this circle.</param>
		/// <param name="MMaterial">The material used to draw the circle.</param>
		/// <param name="bUseWorldSpace">Should this circle use world space?</param>
		/// <param name="nNumberOfSegments">The number of verticies of this circle.</param>
		public static void DrawCircle(LineRenderer LRSelf, Vector3 vAround, float fRadius, Color colLineColour, float fLineWidth, Material MMaterial, bool bUseWorldSpace = true, int nNumberOfSegments = 1)
		{

			LRSelf.material = MMaterial;
			LRSelf.startColor = colLineColour;
			LRSelf.endColor = colLineColour;
			LRSelf.startWidth = fLineWidth;
			LRSelf.endWidth = fLineWidth;
			LRSelf.positionCount = nNumberOfSegments + 1;
			LRSelf.useWorldSpace = bUseWorldSpace;

			float deltaTheta = (float)(2.0f * Mathf.PI) / nNumberOfSegments;
			float theta = 0f;

			for (int i = 0; i < nNumberOfSegments + 1; i++)
			{
				float x = fRadius * Mathf.Cos(theta);
				float z = fRadius * Mathf.Sin(theta);
				Vector3 pos = new Vector3(x, 0, z);
				LRSelf.SetPosition(i, pos + vAround);
				theta += deltaTheta;
			}
		}

		/// <summary>Draws a circle with a centre at around at radius with a LineColour at LineWidth thickness at UseWorldSpace with NumberOfSegments.</summary>
		/// <param name="GSelf">The GameObject calling this.</param>
		/// <param name="vAround">The centre of the circle to be drawn.</param>
		/// <param name="fRadius">The radius of this circle.</param>
		/// <param name="colLineColour">The colour of this circle.</param>
		/// <param name="fLineWidth">The thickness of this circle.</param>
		/// <param name="bUseWorldSpace">Should this circle use world space?</param>
		/// <param name="nNumberOfSegments">The number of verticies of this circle.</param>
		public static void DrawCircle(GameObject GSelf, Vector3 vAround, float fRadius, Color colLineColour, float fLineWidth, bool bUseWorldSpace = true, int nNumberOfSegments = 1)
		{
			LineRenderer _LR = GSelf.GetComponent<LineRenderer>() ?? GSelf.AddComponent<LineRenderer>();
			_LR.startColor = colLineColour;
			_LR.endColor = colLineColour;
			_LR.startWidth = fLineWidth;
			_LR.endWidth = fLineWidth;
			_LR.positionCount = nNumberOfSegments + 1;
			_LR.useWorldSpace = bUseWorldSpace;

			float deltaTheta = (float)(2.0f * Mathf.PI) / nNumberOfSegments;
			float theta = 0f;

			for (int i = 0; i < nNumberOfSegments + 1; i++)
			{
				float x = fRadius * Mathf.Cos(theta);
				float z = fRadius * Mathf.Sin(theta);
				Vector3 pos = new Vector3(x, 0, z);
				_LR.SetPosition(i, pos + vAround);
				theta += deltaTheta;
			}
		}
	}
}
