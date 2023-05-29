using System;
using System.Reflection;
using UObject = UnityEngine.Object;
using UnityEditor;
using MW.Extensions;
using System.Globalization;

public static class MEditorUtility
{

	internal const BindingFlags kBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
		BindingFlags.SetField | BindingFlags.SetProperty |
		BindingFlags.GetField | BindingFlags.GetProperty;

	/// <summary>Get the corresponding object of a SerializedProperty.</summary>
	/// <typeparam name="T">The type to convert a SerializedProperty object to.</typeparam>
	/// <param name="Property">The property to convert.</param>
	/// <returns>The object with a type of T, default otherwise.</returns>
	public static T GetObject<T>(SerializedProperty Property)
	{
		UObject TargetObject = Property.serializedObject.targetObject;
		Type TargetObjectClassType = TargetObject.GetType();
		FieldInfo Field = TargetObjectClassType.GetField(Property.propertyPath, kBindingFlags);

		if (Field == null)
			return default;

		object Value = Field.GetValue(TargetObject);
		T RetVal = Value.Cast<T>();

		return RetVal;
	}

	/// <summary>Sets a SerializedProperty to an object value.</summary>
	/// <remarks>This also records to Unity's Undo/Redo system.</remarks>
	/// <param name="Property">The property to set.</param>
	/// <param name="Value">The arbitrary object to set Property to.</param>
	public static void SetProperty(SerializedProperty Property, object Value)
	{
		UObject TargetObject = Property.serializedObject.targetObject;
		Type TargetObjectClassType = TargetObject.GetType();
		FieldInfo Field = TargetObjectClassType.GetField(Property.propertyPath, kBindingFlags);

		if (Field == null)
			return;

		Undo.RecordObject(TargetObject, $"MWEditor.MEditorUtility.SetProperty(SerializedProperty, object) -> Changed property/s of {Property.name}");

		Field.SetValue(TargetObject, Value, kBindingFlags, null, CultureInfo.InvariantCulture);
		Property.serializedObject.ApplyModifiedProperties();
	}
}
