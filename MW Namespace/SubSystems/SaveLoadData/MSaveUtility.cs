using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MW.SubSystems.GameData
{
	/// <summary>Utility to convert generics to their byte data and vice-versa.</summary>
	public static class MSaveUtility
	{
		/// <summary>Converts T <paramref name="Object"/> to byte data.</summary>
		/// <docs>Converts T Object to byte data.</docs>
		/// <typeparam name="T">The type of Object.</typeparam>
		/// <param name="Object">The Object to get byte data.</param>
		/// <returns>The bytes representing Object.</returns>
		public static byte[] ToBytes<T>(T Object)
		{
			if (Object == null)
				return null;

			BinaryFormatter Binary = new();
			MemoryStream Memory = new();
			Binary.Serialize(Memory, Object);

			return Memory.ToArray();
		}

		/// <summary>Converts byte data as T.</summary>
		/// <typeparam name="T">The type of Object to convert.</typeparam>
		/// <param name="Bytes">The byte data of T.</param>
		/// <returns>The bytes as a T Object.</returns>
		public static T ToObject<T>(byte[] Bytes)
		{
			MemoryStream Memory = new();
			BinaryFormatter Binary = new();
			Memory.Write(Bytes, 0, Bytes.Length);
			Memory.Seek(0, SeekOrigin.Begin);

			return (T)Binary.Deserialize(Memory);
		}
	}
}
