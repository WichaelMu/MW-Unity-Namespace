﻿using System.Collections;
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
			/// <summary>Append to the current <see cref="TextMeshProUGUI"/>.</summary>
			Append,
			/// <summary>Clear the current <see cref="TextMeshProUGUI"/> before typewriting.</summary>
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

		/// <summary>Animates a <see cref="TextMeshProUGUI"/> to display Content like a typewriter.</summary>
		/// <remarks>This is an extension function on <see cref="TextMeshProUGUI"/>.</remarks>
		/// <param name="TMPro">The extended <see cref="TextMeshProUGUI"/> <see cref="GameObject"/>.</param>
		/// <param name="Game">The <see cref="MonoBehaviour"/> that will be responsible for invoking the Typewriter coroutine.</param>
		/// <param name="Content">The content to typewrite.</param>
		/// <param name="Delay">The time gap between writing a new character.</param>
		/// <param name="EMode">Should the text <see cref="ETypewriterMode.Append"/>, or <see cref="ETypewriterMode.Clear"/>?</param>
		/// <returns>The instance of the Typewriter coroutine.</returns>
		public static IEnumerator Typewrite(this TextMeshProUGUI TMPro, MonoBehaviour Game, string Content, float Delay, ETypewriterMode EMode)
		{
			IEnumerator Typewriter = TypewriterText(TMPro, Content, Delay, EMode);
			Game.StartCoroutine(Typewriter);

			return Typewriter;
		}

		/// <inheritdoc cref="Typewrite(TextMeshProUGUI, MonoBehaviour, string, float, ETypewriterMode)"/>
		/// <param name="TMPro"></param> <param name="Game"></param> <param name="Content"></param> <param name="Delay"></param> <param name="EMode"></param>
		/// <param name="Sound">The <see cref="MSound"/> in <see cref="MAudio._AudioInstance"/> to play when writing a character.</param>
		/// <param name="bOverlapSound"></param>
		public static IEnumerator Typewrite(this TextMeshProUGUI TMPro, MonoBehaviour Game, string Content, float Delay, ETypewriterMode EMode, string Sound, bool bOverlapSound = false)
		{
			IEnumerator Typewriter = TypewriterText(TMPro, Content, Delay, EMode, Sound);
			Game.StartCoroutine(Typewriter);

			return Typewriter;
		}

		/// <summary>Animates a <see cref="TextMeshProUGUI"/> to display Content like a typewriter.</summary>
		/// <param name="TMPro">The text to animate.</param>
		/// <param name="Content">The content to typewrite.</param>
		/// <param name="Delay">The time gap between writing a new character.</param>
		/// <param name="EMode">Should the text <see cref="ETypewriterMode.Append"/>, or <see cref="ETypewriterMode.Clear"/>?</param>
		public static IEnumerator TypewriterText(TextMeshProUGUI TMPro, string Content, float Delay, ETypewriterMode EMode)
		{
			if (EMode == ETypewriterMode.Clear)
			{
				TMPro.text = "";
			}

			for (int i = 0; i < Content.Length; ++i)
			{
				TMPro.text += Content[i];
				yield return new WaitForSeconds(Delay);
			}
		}

		/// <summary>Animates a <see cref="TextMeshProUGUI"/> to display Content like a typewriter.</summary>
		/// <param name="TMPro">The text to animate.</param>
		/// <param name="Content">The content to typewrite.</param>
		/// <param name="Delay">The time gap between writing a new character.</param>
		/// <param name="EMode">Should the text <see cref="ETypewriterMode.Append"/>, or <see cref="ETypewriterMode.Clear"/>?</param>
		/// <param name="Sound">The <see cref="MSound"/> in <see cref="MAudio._AudioInstance"/> to play when writing a character.</param>
		/// <param name="bOverlapSound"></param>
		public static IEnumerator TypewriterText(TextMeshProUGUI TMPro, string Content, float Delay, ETypewriterMode EMode, string Sound, bool bOverlapSound = false)
		{
			if (EMode == ETypewriterMode.Clear)
			{
				TMPro.text = "";
			}

			for (int i = 0; i < Content.Length; ++i)
			{
				TMPro.text += Content[i];
				MAudio.AudioInstance.Play(Sound, bOverlapSound);
				yield return new WaitForSeconds(Delay);
			}
		}
	}
}
