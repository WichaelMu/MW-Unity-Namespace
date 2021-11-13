using System.Collections;
using UnityEngine;
using MW.Audible;
using TMPro;

namespace MW.HUD {

    public static class Draw {

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
        public static void DrawLine(LineRenderer LRSelf, Vector3 vFrom, Vector3 vTo, Color colStartColour, Color colEndColour, float fLineWidth, Vector3 vOffset, Material MMaterial, bool bUseWorldSpace = true) {

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
        public static void DrawLine(GameObject GSelf, Vector3 vFrom, Vector3 vTo, Color colStartColour, Color colEndColour, float fLineWidth, Vector3 vOffset, bool bUseWorldSpace = true) {
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
        public static void DrawCircle(LineRenderer LRSelf, Vector3 vAround, float fRadius, Color colLineColour, float fLineWidth, Material MMaterial, bool bUseWorldSpace = true, int nNumberOfSegments = 1) {

            LRSelf.material = MMaterial;
            LRSelf.startColor = colLineColour;
            LRSelf.endColor = colLineColour;
            LRSelf.startWidth = fLineWidth;
            LRSelf.endWidth = fLineWidth;
            LRSelf.positionCount = nNumberOfSegments + 1;
            LRSelf.useWorldSpace = bUseWorldSpace;

            float deltaTheta = (float)(2.0f * Mathf.PI) / nNumberOfSegments;
            float theta = 0f;

            for (int i = 0; i < nNumberOfSegments + 1; i++) {
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
        public static void DrawCircle(GameObject GSelf, Vector3 vAround, float fRadius, Color colLineColour, float fLineWidth, bool bUseWorldSpace = true, int nNumberOfSegments = 1) {
            LineRenderer _LR = GSelf.GetComponent<LineRenderer>() ?? GSelf.AddComponent<LineRenderer>();
            _LR.startColor = colLineColour;
            _LR.endColor = colLineColour;
            _LR.startWidth = fLineWidth;
            _LR.endWidth = fLineWidth;
            _LR.positionCount = nNumberOfSegments + 1;
            _LR.useWorldSpace = bUseWorldSpace;

            float deltaTheta = (float)(2.0f * Mathf.PI) / nNumberOfSegments;
            float theta = 0f;

            for (int i = 0; i < nNumberOfSegments + 1; i++) {
                float x = fRadius * Mathf.Cos(theta);
                float z = fRadius * Mathf.Sin(theta);
                Vector3 pos = new Vector3(x, 0, z);
                _LR.SetPosition(i, pos + vAround);
                theta += deltaTheta;
            }
        }
    }

    public static class UI {

        public enum Mode {
            Append,
            Clear
        }

        /// <summary>Scales the canvas element relative to self.</summary>
        /// <param name="vSelf">The position to scale from.</param>
        /// <param name="vScaleWith">The position to scale with.</param>
        /// <returns>The relative scale size in Vector2.</returns>
        public static Vector2 ScaleSize(Vector3 vSelf, Vector3 vScaleWith) {
            float scale = Mathf.Clamp(Vector3.Distance(vSelf, vScaleWith), 1, 1000);
            scale *= .01f;
            return new Vector2(scale, scale) * .03f;
        }

        /// <summary>Animates tmpTextMeshPro to display sContent like a typewriter.</summary>
        /// <param name="tmpTextMeshPro">The text to animate.</param>
        /// <param name="sContent">The content to display.</param>
        /// <param name="fDelay">The time gap between writing a new letter.</param>
        /// <param name="mMode">Should the text append, or clear?</param>
        public static IEnumerator TypewriterText(TextMeshProUGUI tmpTextMeshPro, string sContent, float fDelay, Mode mMode) {
            if (mMode == Mode.Clear) {
                tmpTextMeshPro.text = "";
			}

            for (int i = 0; i < sContent.Length; ++i) {
                tmpTextMeshPro.text += sContent[i];
                yield return new WaitForSeconds(fDelay);
			}
        }

        /// <summary>Animates tmpTextMeshPro to display sContent like a typewriter.</summary>
        /// <param name="tmpTextMeshPro">The text to animate.</param>
        /// <param name="sContent">The content to display.</param>
        /// <param name="fDelay">The time gap between writing a new letter.</param>
        /// <param name="mMode">Should the text append, or clear?</param>
        /// <param name="sSound">The sound to play for every letter added on.</param>
        public static IEnumerator TypewriterText(TextMeshProUGUI tmpTextMeshPro, string sContent, float fDelay, Mode mMode, string sSound) {
            if (mMode == Mode.Clear) {
                tmpTextMeshPro.text = "";
            }

            for (int i = 0; i < sContent.Length; ++i) {
                tmpTextMeshPro.text += sContent[i];
                Audio.AAudioInstance.Play(sSound);
                yield return new WaitForSeconds(fDelay);
            }
        }
    }
}
