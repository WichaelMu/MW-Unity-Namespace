using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace MW.Memory
{
	public sealed class Cryptography
	{
		const int kKeySize = 256;
		const int kBlockSize = 128;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] StringToByteArray(string String, Encoding Encoding)
			=> Encoding.GetBytes(String);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string ByteArrayToString(byte[] Bytes, Encoding Encoding)
			=> Encoding.GetString(Bytes, 0, Bytes.Length);

		public static byte[] Encrypt(byte[] Data, byte[] Key, byte[] IV)
		{
			using Aes AES = Aes.Create();

			AES.KeySize = kKeySize;
			AES.BlockSize = kBlockSize;

			AES.Key = Key;
			AES.IV = IV;

			using ICryptoTransform Encryptor = AES.CreateEncryptor(AES.Key, AES.IV);
			return PerformCryptography(Data, Encryptor);
		}

		public static byte[] Decrypt(byte[] Data, byte[] Key, byte[] IV)
		{
			using Aes AES = Aes.Create();

			AES.KeySize = kKeySize;
			AES.BlockSize = kBlockSize;

			AES.Key = Key;
			AES.IV = IV;

			using ICryptoTransform Decryptor = AES.CreateDecryptor(AES.Key, AES.IV);
			return PerformCryptography(Data, Decryptor);
		}

		static byte[] PerformCryptography(byte[] Data, ICryptoTransform CryptoTransform)
		{
			using MemoryStream Memory = new MemoryStream();
			using CryptoStream Stream = new CryptoStream(Memory, CryptoTransform, CryptoStreamMode.Write);

			Stream.Write(Data, 0, Data.Length);
			Stream.FlushFinalBlock();

			return Memory.ToArray();
		}

		public static byte[] EncryptString(string PlainText, byte[] Key, byte[] IV)
		{
			if (PlainText == null || PlainText.Length <= 0)
				throw new ArgumentNullException(nameof(PlainText));
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException(nameof(Key));
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException(nameof(IV));

			byte[] Encrypted;
			using (Aes AES = Aes.Create())
			{
				AES.Key = Key;
				AES.IV = IV;

				ICryptoTransform Encryptor = AES.CreateEncryptor(AES.Key, AES.IV);

				using (MemoryStream Stream = new MemoryStream())
				{
					using (CryptoStream Encrypt = new CryptoStream(Stream, Encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter Writer = new StreamWriter(Encrypt))
							Writer.Write(PlainText);

						Encrypted = Stream.ToArray();
					}
				}
			}

			return Encrypted;
		}

		public static string DecryptString(byte[] CipherText, byte[] Key, byte[] IV)
		{
			if (CipherText == null || CipherText.Length <= 0)
				throw new ArgumentNullException(nameof(CipherText));
			if (Key == null || Key.Length <= 0)
				throw new ArgumentNullException(nameof(Key));
			if (IV == null || IV.Length <= 0)
				throw new ArgumentNullException(nameof(IV));

			string PlainText = null;
			using (Aes AES = Aes.Create())
			{
				AES.Key = Key;
				AES.IV = IV;

				ICryptoTransform Decryptor = AES.CreateDecryptor(AES.Key, AES.IV);

				// Create the streams used for decryption.
				using (MemoryStream Stream = new MemoryStream(CipherText))
				{
					using (CryptoStream Decrypt = new CryptoStream(Stream, Decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader Reader = new StreamReader(Decrypt))
							PlainText = Reader.ReadToEnd();
					}
				}
			}

			return PlainText;
		}
	}
}
