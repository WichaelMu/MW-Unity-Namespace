using UnityEngine;

namespace MW.HUD {

    public static class HUD {

        /// <summary>
        /// Draws a line from to to in StartColor to EndColor at LineWidth thickness with an offset at UseWorldSpace with NumberOfSegments.
        /// </summary>
        /// <param name="self">The LineRenderer of the GameObject calling this.</param>
        /// <param name="from">The coordinates where the line will originate.</param>
        /// <param name="to">The coordinates where the line will end.</param>
        /// <param name="StartColour">The starting colour gradient for this line.</param>
        /// <param name="EndColour">The ending colour gradient for this line.</param>
        /// <param name="LineWidth">The thickness of this line.</param>
        /// <param name="offset">The offset to place this line.</param>
        /// <param name="material">The material used to draw the line.</param>
        /// <param name="UseWorldSpace">Should this line use world space?</param>
        public static void DrawLine(LineRenderer self, Vector3 from, Vector3 to, Color StartColour, Color EndColour, float LineWidth, Vector3 offset, Material material, bool UseWorldSpace = true) {

            self.material = material;
            self.startColor = StartColour;
            self.endColor = EndColour;
            self.startWidth = LineWidth;
            self.endWidth = LineWidth;
            self.positionCount = 2;
            self.useWorldSpace = UseWorldSpace;

            self.SetPosition(0, from + offset);
            self.SetPosition(1, to + offset);
        }
        /// <summary>
        /// Draws a line from to to in StartColor to EndColor at LineWidth thickness with an offset at UseWorldSpace with NumberOfSegments.
        /// </summary>
        /// <param name="self">The GameObject calling this.</param>
        /// <param name="from">The coordinates where the line will originate.</param>
        /// <param name="to">The coordinates where the line will end.</param>
        /// <param name="StartColour">The starting colour gradient for this line.</param>
        /// <param name="EndColour">The ending colour gradient for this line.</param>
        /// <param name="LineWidth">The thickness of this line.</param>
        /// <param name="offset">The offset to place this line.</param>
        /// <param name="UseWorldSpace">Should this line use world space?</param>
        public static void DrawLine(GameObject self, Vector3 from, Vector3 to, Color StartColour, Color EndColour, float LineWidth, Vector3 offset, bool UseWorldSpace = true) {
            LineRenderer _LR = self.GetComponent<LineRenderer>() ?? self.AddComponent<LineRenderer>();
            _LR.startColor = StartColour;
            _LR.endColor = EndColour;
            _LR.startWidth = LineWidth;
            _LR.endWidth = LineWidth;
            _LR.positionCount = 2;
            _LR.useWorldSpace = UseWorldSpace;

            _LR.SetPosition(0, from + offset);
            _LR.SetPosition(1, to + offset);
        }

        /// <summary>
        /// Draws a circle with a centre at around at radius with a LineColour at LineWidth thickness at UseWorldSpace with NumberOfSegments.
        /// </summary>
        /// <param name="self">The LineRenderer of the GameObject calling this.</param>
        /// <param name="around">The centre of the circle to be drawn.</param>
        /// <param name="radius">The radius of this circle.</param>
        /// <param name="LineColour">The colour of this circle.</param>
        /// <param name="LineWidth">The thickness of this circle.</param>
        /// <param name="material">The material used to draw the circle.</param>
        /// <param name="UseWorldSpace">Should this circle use world space?</param>
        /// <param name="NumberOfSegments">The number of verticies of this circle.</param>
        public static void DrawCircle(LineRenderer self, Vector3 around, float radius, Color LineColour, float LineWidth, Material material, bool UseWorldSpace = true, int NumberOfSegments = 1) {

            self.material = material;
            self.startColor = LineColour;
            self.endColor = LineColour;
            self.startWidth = LineWidth;
            self.endWidth = LineWidth;
            self.positionCount = NumberOfSegments + 1;
            self.useWorldSpace = UseWorldSpace;

            float deltaTheta = (float)(2.0f * Mathf.PI) / NumberOfSegments;
            float theta = 0f;

            for (int i = 0; i < NumberOfSegments + 1; i++) {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, 0, z);
                self.SetPosition(i, pos + around);
                theta += deltaTheta;
            }
        }

        /// <summary>
        /// Draws a circle with a centre at around at radius with a LineColour at LineWidth thickness at UseWorldSpace with NumberOfSegments.
        /// </summary>
        /// <param name="self">The GameObject calling this.</param>
        /// <param name="around">The centre of the circle to be drawn.</param>
        /// <param name="radius">The radius of this circle.</param>
        /// <param name="LineColour">The colour of this circle.</param>
        /// <param name="LineWidth">The thickness of this circle.</param>
        /// <param name="UseWorldSpace">Should this circle use world space?</param>
        /// <param name="NumberOfSegments">The number of verticies of this circle.</param>
        public static void DrawCircle(GameObject self, Vector3 around, float radius, Color LineColour, float LineWidth, bool UseWorldSpace = true, int NumberOfSegments = 1) {
            LineRenderer _LR = self.GetComponent<LineRenderer>() ?? self.AddComponent<LineRenderer>();
            _LR.startColor = LineColour;
            _LR.endColor = LineColour;
            _LR.startWidth = LineWidth;
            _LR.endWidth = LineWidth;
            _LR.positionCount = NumberOfSegments + 1;
            _LR.useWorldSpace = UseWorldSpace;

            float deltaTheta = (float)(2.0f * Mathf.PI) / NumberOfSegments;
            float theta = 0f;

            for (int i = 0; i < NumberOfSegments + 1; i++) {
                float x = radius * Mathf.Cos(theta);
                float z = radius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, 0, z);
                _LR.SetPosition(i, pos + around);
                theta += deltaTheta;
            }
        }

        /// <summary>Scales the canvas element relative to self.</summary>
        /// <param name="self">The position to scale from.</param>
        /// <param name="ScaleWith">The position to scale with.</param>
        /// <returns>The relative scale size in Vector2.</returns>
        public static Vector2 ScaleSize(Vector3 self, Vector3 ScaleWith) {
            float scale = Mathf.Clamp(Vector3.Distance(self, ScaleWith), 1, 1000);
            scale *= .01f;
            return new Vector2(scale, scale) * .03f;
        }
    }
}
