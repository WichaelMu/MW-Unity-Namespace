using UnityEngine;
using UnityEditor;
using MW;

[CustomPropertyDrawer(typeof(MVector))]
public class MVectorEditor : PropertyDrawer
{
	public override void OnGUI(Rect Position, SerializedProperty Property, GUIContent Label)
	{
		EditorGUI.LabelField(Position, Property.name);

		MVector M = MEditorUtility.GetObject<MVector>(Property);

		GUIContent PropertyName = new GUIContent(Property.name);
		GUIContent[] SubLabels = new GUIContent[] { new GUIContent("X: "), new GUIContent("Y: "), new GUIContent("Z: ") };
		float[] Components = new float[] { M.X, M.Y, M.Z };

		EditorGUI.MultiFloatField(Position, PropertyName, SubLabels, Components);

		MVector Modified = new MVector(Components[0], Components[1], Components[2]);
		MEditorUtility.SetProperty(Property, Modified);
	}
}
