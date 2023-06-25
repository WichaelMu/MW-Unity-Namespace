#if RELEASE
using System.Collections;
using MW.Audio;
using TMPro;
using UnityEngine;

namespace MW.HUD.UI
{
	/// <summary>World space <see cref="Canvas"/> scaling and <see cref="TextMeshProUGUI"/> utilities.</summary>
	/// <docs>World space Canvas scaling and TextMeshProUGUI utilities.</docs>
	/// <decorations decor="public static class"></decorations>
	public static class UI
	{
		/// <summary>Scales the canvas element relative to Self.</summary>
		/// <decorations decor="public static Vector3"></decorations>
		/// <param name="Self">The position to scale from.</param>
		/// <param name="ScaleWith">The position to scale with.</param>
		/// <returns>The relative scale size in Vector2.</returns>
		public static Vector2 ScaleSize(Vector3 Self, Vector3 ScaleWith)
		{
			float Scale = Mathf.Clamp(Vector3.Distance(Self, ScaleWith), 1, 1000);
			Scale *= .01f;
			return new Vector2(Scale, Scale) * .03f;
		}

		/// <summary>Animates TMPro to display Content like a typewriter.</summary>
		/// <remarks>This is an extension function on TextMeshProUGUI.</remarks>
		/// <decorations decor="|Extension| IEnumerator"></decorations>
		/// <param name="TMPro">The extended TextMeshProUGUI GameObject.</param>
		/// <param name="Game">The MonoBehaviour that will be responsible for invoking the Typewriter coroutine.</param>
		/// <param name="Content">The content to typewrite.</param>
		/// <param name="Delay">The time gap between writing a new character.</param>
		/// <param name="Mode">Should the text ETypewriterMode.Append, or ETypewriterMode.Clear?</param>
		/// <returns>The instance of the Typewriter coroutine.</returns>
		public static IEnumerator Typewrite(this TextMeshProUGUI TMPro, MonoBehaviour Game, string Content, float Delay, ETypewriterMode Mode)
		{
			IEnumerator Typewriter = TypewriterText(TMPro, Content, Delay, Mode);
			Game.StartCoroutine(Typewriter);

			return Typewriter;
		}

		/// <summary>Animates TMPro to display Content like a typewriter.</summary>
		/// <remarks>This is an extension function on TextMeshProUGUI.</remarks>
		/// <decorations decor="|Extension| IEnumerator"></decorations>
		/// <param name="TMPro">The extended TextMeshProUGUI GameObject.</param>
		/// <param name="Game">The MonoBehaviour that will be responsible for invoking the Typewriter coroutine.</param>
		/// <param name="Content">The content to typewrite.</param>
		/// <param name="Delay">The time gap between writing a new character.</param>
		/// <param name="Mode">Should the text ETypewriterMode.Append, or ETypewriterMode.Clear?</param>
		/// <param name="Sound">The MSound in MAudio.AudioInstance to play when writing a character.</param>
		/// <param name="bOverlapSound"></param>
		/// <returns>The instance of the Typewriter coroutine.</returns>
		public static IEnumerator Typewrite(this TextMeshProUGUI TMPro, MonoBehaviour Game, string Content, float Delay, ETypewriterMode Mode, string Sound, bool bOverlapSound = false)
		{
			IEnumerator Typewriter = TypewriterTextWithSound(TMPro, Game.gameObject, Content, Delay, Mode, Sound, bOverlapSound);
			Game.StartCoroutine(Typewriter);

			return Typewriter;
		}

		/// <summary>Animates TMPro to display Content like a typewriter.</summary>
		/// <decorations decor="public static IEnumerator"></decorations>
		/// <param name="TMPro">The text to animate.</param>
		/// <param name="Content">The content to typewrite.</param>
		/// <param name="Delay">The time gap between writing a new character.</param>
		/// <param name="Mode">Should the text ETypewriterMode.Append, or ETypewriterMode.Clear?</param>
		public static IEnumerator TypewriterText(TextMeshProUGUI TMPro, string Content, float Delay, ETypewriterMode Mode)
		{
			if (Mode == ETypewriterMode.Clear)
			{
				TMPro.text = "";
			}

			for (int i = 0; i < Content.Length; ++i)
			{
				TMPro.text += Content[i];
				yield return new WaitForSeconds(Delay);
			}
		}

		/// <summary>Animates TMPro to display Content like a typewriter.</summary>
		/// <decorations decor="public static IEnumerator"></decorations>
		/// <param name="TMPro">The text to animate.</param>
		/// <param name="Caller">The GameObject requesting to play the sound. Only required when bOverlapSound is true.</param>
		/// <param name="Content">The content to typewrite.</param>
		/// <param name="Delay">The time gap between writing a new character.</param>
		/// <param name="Mode">Should the text ETypewriterMode.Append, or ETypewriterMode.Clear?</param>
		/// <param name="Sound">The MSound in MAudio.AudioInstance to play when writing a character.</param>
		/// <param name="bOverlapSound"></param>
		public static IEnumerator TypewriterTextWithSound(TextMeshProUGUI TMPro, GameObject Caller, string Content, float Delay, ETypewriterMode Mode, string Sound, bool bOverlapSound = false)
		{
			if (Mode == ETypewriterMode.Clear)
			{
				TMPro.text = "";
			}

			for (int i = 0; i < Content.Length; ++i)
			{
				TMPro.text += Content[i];

				if (!bOverlapSound)
				{
					MAudio.AudioInstance.Play(Sound);
				}
				else
				{
					if (!Caller)
					{
						throw new System.ArgumentNullException(nameof(Caller), "A request was made to play a typewriter sound with bOverlapSound = true. " +
							"This requires a GameObject 'Caller', but null was provided.");
					}

					MAudio.AudioInstance.PlayWithOverlap(Sound, Caller);
				}

				yield return new WaitForSeconds(Delay);
			}
		}
	}
}
#endif // RELEASE
