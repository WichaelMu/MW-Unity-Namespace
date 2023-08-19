#if RELEASE
using System.Linq;
using static MW.Utils;
using MW.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MW.Console
{
	/// <summary>A list of built-in [BuiltInExec] Functions.</summary>
	public static class BuiltInExecFunctions
	{

		#region Gameplay

		/// <summary>Exit the game.</summary>
		/// <remarks>In editor mode, this will call Debug.Break() and print a quit message to the console.</remarks>
		/// <decorations decor="public static void"></decorations>
		[BuiltInExec("Exit the game.")]
		public static void QuitGame()
		{
			Application.Quit();

			if (Application.isEditor)
			{
				Debug.Break();
				Log.P($"The game has quit in editor mode with [BuiltInExec] {nameof(QuitGame)}().");
			}
		}

		/// <summary>Restarts and reloads the current scene.</summary>
		/// <decorations decor="public static void"></decorations>
		[BuiltInExec("Restarts and Reloads the current scene.")]
		public static void RestartScene()
		{
			Scene CurrentScene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(CurrentScene.buildIndex);
		}

		/// <summary>Sets Time.timeScale to InTimeScale.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="InTimeScale"></param>
		[BuiltInExec("Sets Time.timeScale to InTimeScale")]
		public static void SetTimeScale(float InTimeScale)
		{
			Time.timeScale = InTimeScale;
		}

		/// <summary>Sets the game's target frame rate to InFPS.</summary>
		/// <remarks>The minimum InFPS is 1.</remarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="InFPS"></param>
		[BuiltInExec("Sets the game's Target Frame Rate to InFPS, minimum 1.")]
		public static void SetTargetFPS(int InFPS)
		{
			ClampMin(ref InFPS, 1);
			Application.targetFrameRate = InFPS;
		}

		#endregion

		#region GameObject & Components

		/// <summary>Sends Message to G.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="G">The GameObject to send Message to.</param>
		/// <param name="Message">The Message to send to G.</param>
		[BuiltInExec("Sends Message to G.")]
		public static void SendMessage(GameObject G, string Message)
		{
			if (!string.IsNullOrEmpty(Message.Trim()))
				G.SendMessage(Message);
		}

		/// <summary>Sets G's active state to bInActive.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="G"></param>
		/// <param name="bInActive"></param>
		[BuiltInExec("Sets G's active state to bInActive.")]
		public static void SetActive(GameObject G, bool bInActive)
		{
			G.SetActive(bInActive);
		}

		/// <summary>Destroys G.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="G"></param>
		[BuiltInExec("Destroys G.")]
		public static void Destroy(GameObject G)
		{
			Object.Destroy(G);
		}

		/// <summary>Destroys component M from it's GameObject.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="M"></param>
		[BuiltInExec("Destroys component M from it's GameObject.")]
		public static void DestroyComponent(MonoBehaviour M)
		{
			Object.Destroy(M);
		}

		/// <summary>Sets M's active state to bInActive.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="M"></param>
		/// <param name="bInActive"></param>
		[BuiltInExec("Sets component M active state to bInActive.")]
		public static void SetComponentActive(MonoBehaviour M, bool bInActive)
		{
			M.enabled = bInActive;
		}

		/// <summary>Teleports T to Position in world space.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="T"></param>
		/// <param name="Position"></param>
		[BuiltInExec("Teleports T to Position in world space.")]
		public static void Teleport(Transform T, Vector3 Position)
		{
			T.position = Position;
		}

		#endregion

		#region Animations

		/// <summary>Sets a float animation parameter</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Animator"></param>
		/// <param name="Parameter"></param>
		/// <param name="F"></param>
		[BuiltInExec("Sets a float animation parameter.")]
		public static void F_SetAnimationParameter(Animator Animator, string Parameter, float F)
		{
			Animator.SetFloat(Parameter, F);
		}

		/// <summary>Sets a bool animation parameter.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Animator"></param>
		/// <param name="Parameter"></param>
		/// <param name="B"></param>
		[BuiltInExec("Sets a bool animation parameter.")]
		public static void B_SetAnimationParameter(Animator Animator, string Parameter, bool B)
		{
			Animator.SetBool(Parameter, B);
		}

		/// <summary>Sets an int animation parameter.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Animator"></param>
		/// <param name="Parameter"></param>
		/// <param name="I"></param>
		[BuiltInExec("Sets a int animation parameter.")]
		public static void I_SetAnimationParameter(Animator Animator, string Parameter, int I)
		{
			Animator.SetInteger(Parameter, I);
		}

		/// <summary>Triggers an animation trigger.</summary>
		/// <decorations decor="public static void"></decorations>
		/// <param name="Animator"></param>
		/// <param name="Trigger"></param>
		[BuiltInExec("Triggers an animation trigger.")]
		public static void TriggerAnimation(Animator Animator, string Trigger)
		{
			Animator.SetTrigger(Trigger);
		}

		#endregion

		#region Debugging

		[BuiltInExec("Reports the position of T.")]
		public static Vector3 ReportPos(Transform T) => T.position;
		[BuiltInExec("Reports the rotation of T.")]
		public static Vector3 ReportRot(Transform T) => T.localEulerAngles;

		[BuiltInExec("Reports the position of Ts.")]
		public static Vector3[] ReportPosMulti(Transform[] T) => T.Select(R => R.position).ToArray();
		[BuiltInExec("Reports the position of Ts.")]
		public static Vector3[] ReportRotMulti(Transform[] T) => T.Select(R => R.localEulerAngles).ToArray();

		[BuiltInExec("Draws a line from A to B")]
		public static void DrawLine(MVector A, MVector B, float Duration)
		{
			Arrow.DebugArrow(A, A.Distance(B) * (A > B), Duration: Duration);
		}

		[BuiltInExec("Attaches the In-Game Object Diagnostics Component to G.")]
		public static void AttachDiagnostics(GameObject G) => G.AddComponent<InGameObjectDiagnostics>();
		[BuiltInExec("Attaches the In-Game Object Diagnostics Component to all Gs.")]
		public static void AttachDiagnosticsMulti(GameObject[] G)
		{
			foreach (GameObject O in G)
				O.AddComponent<InGameObjectDiagnostics>();
		}

		#endregion
	}
}
#endif // RELEASE