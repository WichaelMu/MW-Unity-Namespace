using static MW.Utils;
using MW.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MW.Console
{
	public class BuiltInExecFunctions
	{
		[Exec("Exit the game.")]
		public static void QuitGame()
		{
			Application.Quit();

			if (Application.isEditor)
			{
				Debug.Break();
				Log.P("The game has quit in editor mode with [Exec] QuitGame().");
			}
		}

		[Exec("Restarts and Reloads the current scene.")]
		public static void RestartScene()
		{
			Scene CurrentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(CurrentScene.buildIndex);
		}

		[Exec("Sets Time.timeScale to InTimeScale")]
		public static void SetTimeScale(float InTimeScale)
		{
			Time.timeScale = InTimeScale;
		}

		[Exec("Sets the game's Target Frame Rate to InFPS, minimum 1.")]
		public static void SetTargetFPS(int InFPS)
		{
			ClampMin(ref InFPS, 1);
			Application.targetFrameRate = InFPS;
		}

		[Exec("Sends Message to G.")]
		public static void SendMessage(GameObject G, string Message)
		{
			if (!string.IsNullOrEmpty(Message.Trim()))
				G.SendMessage(Message);
		}
	}
}
