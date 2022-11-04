using System;
using System.IO;

namespace MW.SubSystems.GameData
{
	/// <summary>A generic game saving sub-system.</summary>
	/// <decorations decor="public static class"></decorations>
	public static class MSaveSystem
	{
		/// <summary>
		/// Creates or overwrites a file to <paramref name="FullDestinationPath"/>
		/// and writes all <paramref name="Bytes"/>.
		/// </summary>
		/// <docs>
		/// Creates or overwrites a file to FullDestinationPath and writes
		/// all Bytes.
		/// </docs>
		/// <decorations decor="public static void"></decorations>
		/// <param name="FullDestinationPath">The name and directory of the file to write to.</param>
		/// <param name="Bytes">The bytes to write to the file.</param>
		/// <exception cref="ArgumentException">
		/// <paramref name="FullDestinationPath"/> is empty, is only white space, or contains illegal characters.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="FullDestinationPath"/> is null, or the <paramref name="Bytes"/> are <see langword="null"/>.
		/// </exception>
		/// <exception cref="PathTooLongException">
		/// <paramref name="FullDestinationPath"/> has a length greater than 248 characters or the file name has a length greater than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">
		/// <paramref name="FullDestinationPath"/> is invalid or does not exist.
		/// </exception>
		/// <exception cref="IOException">
		/// The system could not open <paramref name="FullDestinationPath"/>.
		/// </exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="FullDestinationPath"/> is either read-only, not supported, or the caller does not have permission, or it specifies a directory.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// <paramref name="FullDestinationPath"/> is formatted incorrectly.
		/// </exception>
		public static void Save(string FullDestinationPath, byte[] Bytes)
		{
			File.WriteAllBytes(FullDestinationPath, Bytes);
		}

		/// <summary>
		/// Creates or overwrites a file to <paramref name="FullDestinationPath"/>
		/// and writes all byte data from <paramref name="ObjectToSave"/>.
		/// </summary>
		/// <docs>
		/// Creates or overwrites a file to FullDestinationPath and writes
		/// all byte data from ObjectToSave.
		/// </docs>
		/// <remarks>
		/// <b>Assumes <typeparamref name="T"/> is marked with a <see cref="SerializableAttribute"/>.</b>
		/// </remarks>
		/// <docremarks>&lt;b&gt;Assumes T is marked with [System.Serializable].&lt;/b&gt;</docremarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="FullDestinationPath">The name and directory of the file to write to.</param>
		/// <param name="ObjectToSave">The bytes of the Object to write to the file.</param>
		/// <exception cref="ArgumentException">
		/// <paramref name="FullDestinationPath"/> is empty, is only white space, or contains illegal characters.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="FullDestinationPath"/> is null, or the <paramref name="ObjectToSave"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="PathTooLongException">
		/// <paramref name="FullDestinationPath"/> has a length greater than 248 characters or the file name has a length greater than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">
		/// <paramref name="FullDestinationPath"/> is invalid or does not exist.
		/// </exception>
		/// <exception cref="IOException">
		/// The system could not open <paramref name="FullDestinationPath"/>.
		/// </exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="FullDestinationPath"/> is either read-only, not supported, or the caller does not have permission, or it specifies a directory.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// <paramref name="FullDestinationPath"/> is formatted incorrectly.
		/// </exception>
		public static void Save<T>(string FullDestinationPath, T ObjectToSave)
		{
			byte[] Bytes = MSaveUtility.ToBytes(ObjectToSave);
			Save(FullDestinationPath, Bytes);
		}

		/// <summary>Loads data from an existing file as T.</summary>
		/// <typeparam name="T"></typeparam>
		/// <decorations decor="public static T"></decorations>
		/// <param name="FullOriginPath"></param>
		/// <exception cref="ArgumentException">
		/// <paramref name="FullOriginPath"/> is empty, is only white space, or contains illegal characters.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="FullOriginPath"/> is null.
		/// </exception>
		/// <exception cref="PathTooLongException">
		/// <paramref name="FullOriginPath"/> has a length greater than 248 characters or the file name has a length greater than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">
		/// <paramref name="FullOriginPath"/> is invalid or does not exist.
		/// </exception>
		/// <exception cref="IOException">
		/// The system could not open <paramref name="FullOriginPath"/>.
		/// </exception>
		/// <exception cref="UnauthorizedAccessException">
		/// <paramref name="FullOriginPath"/> is either read-only, not supported, or the caller does not have permission, or it specifies a directory.
		/// </exception>
		/// <exception cref="FileNotFoundException">
		/// The file at <paramref name="FullOriginPath"/> was not found.
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// <paramref name="FullOriginPath"/> is formatted incorrectly.
		/// </exception>
		/// <exception cref="System.Security.SecurityException">
		/// The caller does not have the required permission to open <paramref name="FullOriginPath"/>.
		/// </exception>
		/// <returns>The saved data as T.</returns>
		public static T Load<T>(string FullOriginPath)
		{
			byte[] Bytes = File.ReadAllBytes(FullOriginPath);
			return MSaveUtility.ToObject<T>(Bytes);
		}

		/// <summary>Deletes a save file.</summary>
		/// <remarks><b>WARNING: THIS WILL REMOVE ANY SPECIFIED FILE AND IS NOT LIMITED TO GAME SAVE FILES.</b></remarks>
		/// <docremarks>&lt;b&gt;&lt;span style="color:red"&gt;WARNING: THIS WILL REMOVE ANY SPECIFIED FILE AND IS NOT LIMITED TO GAME SAVE FILES.&lt;/span&gt;&lt;/b&gt;</docremarks>
		/// <decorations decor="public static void"></decorations>
		/// <param name="FullPath">The name and directory of the file to delete.</param>
		/// <exception cref="IOException">
		/// <paramref name="FullPath"/> is read-only, is the current working directory, contains a read-only file, or is being used by another process.
		/// </exception>
		/// <exception cref="UnauthorizedAccessException">
		/// The caller does not have the required permission to delete <paramref name="FullPath"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <paramref name="FullPath"/> is empty, is only white space, or contains illegal characters.
		/// </exception>
		/// <exception cref="PathTooLongException">
		/// <paramref name="FullPath"/> has a length greater than 248 characters or the file name has a length greater than 260 characters.
		/// </exception>
		/// <exception cref="DirectoryNotFoundException">
		/// <paramref name="FullPath"/> is invalid or does not exist.
		/// </exception>
		public static void Delete(string FullPath)
		{
			Directory.Delete(FullPath, true);
		}
	}
}
