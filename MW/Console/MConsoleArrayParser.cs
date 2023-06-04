using System;
using System.Linq;
using MW.Extensions;
using UnityEngine;
using UE = UnityEngine;

namespace MW.Console
{
	class MConsoleArrayParser
	{
		internal static bool Convert(MConsoleSupportedTypes Types, ref object TargetObject, Type ElementType, MArray<object> Elements)
		{
			bool bSuccessfulConversion = true;

			if (ElementType == typeof(MVector))
			{
				TargetObject = Make<MVector>(Elements);
			}
			else if (ElementType == typeof(Vector3))
			{
				TargetObject = Make<Vector3>(Elements);
			}
			else if (ElementType == typeof(MRotator))
			{
				TargetObject = Make<MRotator>(Elements);
			}
			else if (ElementType == typeof(GameObject))
			{
				TargetObject = Make<GameObject>(Elements);
			}
			else if (ElementType == typeof(Transform))
			{
				TargetObject = Make<Transform>(Elements);
			}
			else if (ElementType == typeof(MonoBehaviour))
			{
				TargetObject = Make<MonoBehaviour>(Elements);
			}
			else if (ElementType == typeof(UE.Behaviour))
			{
				TargetObject = Make<UE.Behaviour>(Elements);
			}
			else if (ElementType == typeof(string))
			{
				TargetObject = Make<string>(Elements);
			}
			else if (ElementType.IsPrimitive)
			{
				TargetObject = Elements.TArray();
			}
			else
			{
				bSuccessfulConversion = Types.Settings.Console.HandleCustomArrayType(ref TargetObject, ElementType, Elements);

				if (TargetObject == null)
					Types.ThrowError($"The type: '{ElementType.Name}' cannot be parsed into an Array.");
			}

			return bSuccessfulConversion;
		}

		static T[] Make<T>(MArray<object> Elements)
		{
			MArray<T> Array = new MArray<T>();
			foreach (object Element in Elements)
				Array.Push(Element.Cast<T>());
			return Array.TArray();
		}
	}
}
