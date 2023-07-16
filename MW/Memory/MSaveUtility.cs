using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MW.Memory
{
	/// <summary>Utility to convert generics to their byte data and vice-versa.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class MSaveUtility
	{
		/// <summary>Converts T <paramref name="Object"/> to byte data.</summary>
		/// <docs>Converts T Object to byte data.</docs>
		/// <remarks>
		/// <b>Assumes <typeparamref name="T"/> is marked with a <see cref="SerializableAttribute"/>.</b>
		/// </remarks>
		/// <docremarks>&lt;b&gt;Assumes T is marked with [System.Serializable.&lt;/b&gt;</docremarks>
		/// <typeparam name="T">The type of Object.</typeparam>
		/// <decorations decor="public static byte[]"></decorations>
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

		/// <summary>Converts byte data as <typeparamref name="T"/>.</summary>
		/// <typeparam name="T">The type of Object to convert.</typeparam>
		/// <docs>Converts byte data as T.</docs>
		/// <decorations decor="public static T"></decorations>
		/// <param name="Bytes">The byte data of T.</param>
		/// <docreturns>The bytes as a T Object.</docreturns>
		/// <returns>The bytes as a <typeparamref name="T"/> Object.</returns>
		public static T ToObject<T>(byte[] Bytes)
		{
			if (Bytes == null || Bytes.Length == 0)
				throw new ArgumentNullException(nameof(Bytes), "Bytes is null or empty!");

			MemoryStream Memory = new();
			BinaryFormatter Binary = new();
			Memory.Write(Bytes, 0, Bytes.Length);
			Memory.Seek(0, SeekOrigin.Begin);

			return (T)Binary.Deserialize(Memory);
		}
	}
}
