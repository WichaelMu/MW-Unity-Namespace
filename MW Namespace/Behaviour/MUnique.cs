using MW.Diagnostics;
using UnityEngine;

namespace MW.Behaviour
{
	/// <summary>Base class for any unique MonoBehaviour in a game.</summary>
	/// <typeparam name="T">The class to make unique.</typeparam>
	/// <decorations decor="public class T where T : MonoBehaviour"></decorations>
	public class MUnique<T> : MonoBehaviour where T : MonoBehaviour
	{
		/// <summary>A unique reference to T.</summary>
		/// <decorations decor="public static T"></decorations>
		public static T Instance;

		/// <summary>Sets this gameObject to DontDestroyOnLoad.</summary>
		/// <decorations decor="bool"></decorations>
		[SerializeField] bool bMakePersistent;
		/// <summary>True to spawn a new unique instance when this is destroyed.</summary>
		/// <decorations decor="bool"></decorations>
		[SerializeField] bool bSpawnNewOnDestroy;

		/// <summary>Automatically sets the instance to this.</summary>
		/// <decorations decor="public virtual void"></decorations>
		public virtual void Awake()
		{
			SetInstance(this as T);
		}

		/// <summary>Sets the unique instance.</summary>
		/// <param name="UniqueInstance">The component to mark as the unique instance.</param>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void SetInstance(T UniqueInstance)
		{
			if (!Instance)
			{
				Instance = UniqueInstance;

				if (bMakePersistent)
				{
					DontDestroyOnLoad(gameObject);
				}
			}
			else
			{
				Log.W("Ensure there is only one " + typeof(T).Name + " in the scene. Instance is: " + Instance.name + ". Duplicate is: " + name);
				Destroy(gameObject);
			}
		}

		/// <summary>Will spawn a new GameObject if <see cref="bSpawnNewOnDestroy"/> is <see langword="true"/>.</summary>
		/// <docs>Will spawn a new GameObject if bSpawnNewOnDestroy is true.</docs>
		/// <decorations decor="protected virtual void"></decorations>
		protected virtual void OnDestroy()
		{
			if (bSpawnNewOnDestroy)
			{
				Instance = Instantiate(new GameObject(name, typeof(T).GetType())) as T;
			}
		}
	}
}
