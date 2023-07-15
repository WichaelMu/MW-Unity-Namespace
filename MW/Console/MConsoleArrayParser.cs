using System;
using System.Collections;
using System.Linq;
using System.Text;
using MW.Extensions;
#if RELEASE
using UnityEngine;
using UE = UnityEngine;
#endif // RELEASE

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
#if RELEASE
			else if (ElementType == typeof(Vector3))
			{
				TargetObject = Make<Vector3>(Elements);
			}
#endif // RELEASE
			else if (ElementType == typeof(MRotator))
			{
				TargetObject = Make<MRotator>(Elements);
			}
#if RELEASE
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
#endif // RELEASE
			else if (ElementType == typeof(string))
			{
				TargetObject = Make<string>(Elements);
			}
			else if (ElementType.IsPrimitive)
			{
				if (!MConsoleArrayPrimitiveTranslator.Get().Translate(Elements, ref TargetObject, ElementType))
					Diagnostics.Log.W("Unable to convert to {ElementType}[]");
			}
			else
			{
				bSuccessfulConversion = Types.Settings.Console.HandleCustomArrayType(ref TargetObject, ElementType, Elements);

				if (!bSuccessfulConversion || TargetObject == null)
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

		internal static StringBuilder ConvertArrayReturnType(object RetVal)
		{
			IEnumerable Array = RetVal.Cast<IEnumerable>();

			StringBuilder ArrayBuilder = new StringBuilder();
			foreach (object Element in Array)
			{
				ArrayBuilder.Append(Element);
				ArrayBuilder.Append(", ");
			}

			ArrayBuilder.Remove(ArrayBuilder.Length - 2, 2);

			return ArrayBuilder;
		}
	}
}
