﻿using static MW.Utils;
using MW.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MW.Console
{
	/// <summary>A list of built-in [Exec] Functions.</summary>
	public static class BuiltInExecFunctions
	{

		#region Gameplay

		/// <summary>Exit the game.</summary>
		/// <remarks>In editor mode, this will call Debug.Break() and print a quit message to the console.</remarks>
		[Exec("Exit the game.")]
		public static void QuitGame()
		{
			Application.Quit();

			if (Application.isEditor)
			{
				Debug.Break();
				Log.P($"The game has quit in editor mode with [Exec] {nameof(QuitGame)}().");
			}
		}

		/// <summary>Restarts and reloads the current scene.</summary>
		[Exec("Restarts and Reloads the current scene.")]
		public static void RestartScene()
		{
			Scene CurrentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(CurrentScene.buildIndex);
		}

		/// <summary>Sets Time.timeScale to InTimeScale.</summary>
		/// <param name="InTimeScale"></param>
		[Exec("Sets Time.timeScale to InTimeScale")]
		public static void SetTimeScale(float InTimeScale)
		{
			Time.timeScale = InTimeScale;
		}

		/// <summary>Sets the game's target frame rate to InFPS.</summary>
		/// <remarks>The minimum InFPS is 1.</remarks>
		/// <param name="InFPS"></param>
		[Exec("Sets the game's Target Frame Rate to InFPS, minimum 1.")]
		public static void SetTargetFPS(int InFPS)
		{
			ClampMin(ref InFPS, 1);
			Application.targetFrameRate = InFPS;
		}

		#endregion

		#region GameObject & Components

		/// <summary>Sends Message to G.</summary>
		/// <param name="G">The GameObject to send Message to.</param>
		/// <param name="Message">The Message to send to G.</param>
		[Exec("Sends Message to G.")]
		public static void SendMessage(GameObject G, string Message)
		{
			if (!string.IsNullOrEmpty(Message.Trim()))
				G.SendMessage(Message);
		}

		/// <summary>Sets G's active state to bInActive.</summary>
		/// <param name="G"></param>
		/// <param name="bInActive"></param>
		[Exec("Sets G's active state to bInActive.")]
		public static void SetActive(GameObject G, bool bInActive)
		{
			G.SetActive(bInActive);
		}

		/// <summary>Destroys G.</summary>
		/// <param name="G"></param>
		[Exec("Destroys G.")]
		public static void Destroy(GameObject G)
		{
			Object.Destroy(G);
		}

		/// <summary>Destroys component M from it's GameObject.</summary>
		/// <param name="M"></param>
		[Exec("Destroys component M from it's GameObject.")]
		public static void DestroyComponent(MonoBehaviour M)
		{
			Object.Destroy(M);
		}

		/// <summary>Sets M's active state to bInActive.</summary>
		/// <param name="M"></param>
		/// <param name="bInActive"></param>
		public static void SetComponentActive(MonoBehaviour M, bool bInActive)
		{
			M.enabled = bInActive;
		}

		/// <summary>Teleports T to Position in world space.</summary>
		/// <param name="T"></param>
		/// <param name="Position"></param>
		[Exec("Teleports T to Position in world space.")]
		public static void Teleport(Transform T, Vector3 Position)
		{
			T.position = Position;
		}

		#endregion

		#region Animations

		/// <summary>Sets a float animation parameter</summary>
		/// <param name="Animator"></param>
		/// <param name="Parameter"></param>
		/// <param name="F"></param>
		[Exec("Sets a float animation parameter.")]
		public static void F_SetAnimationParameter(Animator Animator, string Parameter, float F)
		{
			Animator.SetFloat(Parameter, F);
		}

		/// <summary>Sets a bool animation parameter.</summary>
		/// <param name="Animator"></param>
		/// <param name="Parameter"></param>
		/// <param name="B"></param>
		[Exec("Sets a bool animation parameter.")]
		public static void B_SetAnimationParameter(Animator Animator, string Parameter, bool B)
		{
			Animator.SetBool(Parameter, B);
		}

		/// <summary>Sets an int animation parameter.</summary>
		/// <param name="Animator"></param>
		/// <param name="Parameter"></param>
		/// <param name="I"></param>
		[Exec("Sets a int animation parameter.")]
		public static void I_SetAnimationParameter(Animator Animator, string Parameter, int I)
		{
			Animator.SetInteger(Parameter, I);
		}

		/// <summary>Triggers an animation trigger.</summary>
		/// <param name="Animator"></param>
		/// <param name="Trigger"></param>
		[Exec("Triggers an animation trigger.")]
		public static void TriggerAnimation(Animator Animator, string Trigger)
		{
			Animator.SetTrigger(Trigger);
		}

		#endregion
	}
}
