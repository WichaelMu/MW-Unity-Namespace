using System.Collections;
using UnityEngine;
using MW.Audio;
using TMPro;

namespace MW.HUD
{
	/// <summary>World space <see cref="Canvas"/> scaling and <see cref="TextMeshProUGUI"/> utilities.</summary>
	public static class UI
	{

		public enum ETypewriterMode
		{
			Append,
			Clear
		}

		/// <summary>Scales the canvas element relative to self.</summary>
		/// <param name="vSelf">The position to scale from.</param>
		/// <param name="vScaleWith">The position to scale with.</param>
		/// <returns>The relative scale size in Vector2.</returns>
		public static Vector2 ScaleSize(Vector3 vSelf, Vector3 vScaleWith)
		{
			float scale = Mathf.Clamp(Vector3.Distance(vSelf, vScaleWith), 1, 1000);
			scale *= .01f;
			return new Vector2(scale, scale) * .03f;
		}

		/// <summary>Animates tmpTextMeshPro to display sContent like a typewriter.</summary>
		/// <param name="tmpTextMeshPro">The text to animate.</param>
		/// <param name="sContent">The content to display.</param>
		/// <param name="fDelay">The time gap between writing a new letter.</param>
		/// <param name="mMode">Should the text append, or clear?</param>
		public static IEnumerator TypewriterText(TextMeshProUGUI tmpTextMeshPro, string sContent, float fDelay, ETypewriterMode mMode)
		{
			if (mMode == ETypewriterMode.Clear)
			{
				tmpTextMeshPro.text = "";
			}

			for (int i = 0; i < sContent.Length; ++i)
			{
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
		public static IEnumerator TypewriterText(TextMeshProUGUI tmpTextMeshPro, string sContent, float fDelay, ETypewriterMode mMode, string sSound)
		{
			if (mMode == ETypewriterMode.Clear)
			{
				tmpTextMeshPro.text = "";
			}

			for (int i = 0; i < sContent.Length; ++i)
			{
				tmpTextMeshPro.text += sContent[i];
				MAudio.AAudioInstance.Play(sSound);
				yield return new WaitForSeconds(fDelay);
			}
		}
	}
}
