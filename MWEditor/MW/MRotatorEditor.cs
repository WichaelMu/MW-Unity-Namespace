using UnityEngine;
using UnityEditor;
using MW;

[CustomPropertyDrawer(typeof(MRotator))]
internal class MRotatorEditor : PropertyDrawer
{
	public override void OnGUI(Rect Position, SerializedProperty Property, GUIContent Label)
	{
		EditorGUI.LabelField(Position, Property.name);

		MRotator R = MEditorUtility.GetObject<MRotator>(Property);

		GUIContent PropertyName = new GUIContent(Property.name);
		GUIContent[] SubLabels = new GUIContent[] { new GUIContent("P: "), new GUIContent("Y: "), new GUIContent("R: ") };
		float[] Components = new float[] { R.Pitch, R.Yaw, R.Roll };

		EditorGUI.MultiFloatField(Position, PropertyName, SubLabels, Components);

		MRotator Modified = new MRotator(Components[0], Components[1], Components[2]);
		MEditorUtility.SetProperty(Property, Modified);
	}
}