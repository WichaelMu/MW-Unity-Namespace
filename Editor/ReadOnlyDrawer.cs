using UnityEngine;
using UnityEditor;

namespace MW.Editor
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute)), SerializeField]
	public class ReadOnlyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			bool PreviousGUIState = GUI.enabled;
			GUI.enabled = false;
			EditorGUI.PropertyField(position, property, label);

			GUI.enabled = PreviousGUIState;
		}
	}
}
