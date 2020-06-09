using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;

using Digicrafts.Serialization;
using Digicrafts.IAP.Pro.Core;

namespace Digicrafts.IAP.Pro.Datastore
{	
	/// <summary>
	/// IAP datastore type.
	/// </summary>
	public enum IAPDatastoreType
	{
		PlayerPref,
		TextFile,
		XORBinaryFile,
		AESEncryptedFile
	}		

	/// <summary>
	/// IAP datastore.
	/// </summary>
	public class IAPDatastore
	{		
		public int version = 1;

		private static bool _batching = false;
		protected Dictionary<string,Dictionary<string,IAPObjectData>> _values;

		public IAPDatastore()
		{
			// Create dictionary to hold the object
			_values = new Dictionary<string, Dictionary<string,IAPObjectData>>();
		}

		virtual public void Build()
		{

		}

		/// <summary>
		/// Gets the group.
		/// </summary>
		/// <returns>The group.</returns>
		/// <param name="key">Key.</param>
		public Dictionary<string,IAPObjectData> GetGroup(string key)
		{			
			if(_values.ContainsKey(key)){
				return _values[key];
			} else {
				Dictionary<string,IAPObjectData> dict = new Dictionary<string, IAPObjectData>();
				_values.Add(key,dict);
				return dict;
			}
		}			

		/// <summary>
		/// Sets the value.
		/// </summary>
		/// <param name="groupId">Group identifier.</param>
		/// <param name="data">Data.</param>
		public void SetValue(string groupId, string uid, IAPObjectData data)
		{
			if(_values.ContainsKey(groupId)){
				
				Dictionary<string,IAPObjectData> dict = _values[groupId];

				if(dict.ContainsKey(uid))
					dict[uid]=data;
				else
					dict.Add(uid,data);
			}
		}

		/// <summary>
		/// Delaies the save.
		/// </summary>
		public static void DelaySave()
		{
			_batching=true;
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public bool Save()
		{
			bool result = false;
			if(_batching){
				_batching=false;
				result = true;
			} else {		
				IAPInventoryManager.Log("WriteToDatastore");
				result = WriteToDatastore();
			}
			return result;
		}

		/// <summary>
		/// Md5 the specified strToEncrypt.
		/// </summary>
		/// <param name="strToEncrypt">String to encrypt.</param>
		public static string Md5(string strToEncrypt) {
			UTF8Encoding ue = new UTF8Encoding();
			byte[] bytes = ue.GetBytes(strToEncrypt);

			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
			byte[] hashBytes = md5.ComputeHash(bytes);

			string hashString = "";

			for (int i = 0; i < hashBytes.Length; i++) {
				hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
			}

			return hashString.PadLeft(32, '0');
		}

		// Helper methods

		virtual protected void StringToDictionary(string input)
		{			
			if(!string.IsNullOrEmpty(input)){
				Dictionary<string,string> result = JsonUtility.FromJson<Serializer<string, string>>(input).ToDictionary();

				foreach (KeyValuePair<string, string> item in result)
				{
					if(item.Key=="version"){		
						IAPInventoryManager.LogFormat("Datastore Version: {0}",item.Value);
					} else {
//						Debug.LogFormat("key {0} value {1}",item.Key,item.Value);
						Dictionary<string, IAPObjectData> res = JsonUtility.FromJson<Serializer<string, IAPObjectData>>(item.Value).ToDictionary();
						_values.Add(item.Key,res);
					}
				}					
			}
		}			

		virtual protected string DictionaryToString()
		{
			Dictionary<string,string> saveData = new Dictionary<string, string>();

			// Save the version
			saveData.Add("version", this.version.ToString());

			// Save each group
			foreach (KeyValuePair<string, Dictionary<string,IAPObjectData>> item in _values)
			{								
				string json = JsonUtility.ToJson(new Serializer<string,IAPObjectData>(item.Value));
				saveData.Add(item.Key,json);
			}

			return JsonUtility.ToJson(new Serializer<string,string>(saveData));
		}			

		virtual protected bool WriteToDatastore()
		{
			// Virtual method
			return true;
		}			

	}
		

	/// <summary>
	/// IAP file datastore.
	/// </summary>
	public class IAPFileDatastore : IAPDatastore
	{
		protected string _path;

		public IAPFileDatastore(string path) : base()
		{
			_path = Application.persistentDataPath+Path.DirectorySeparatorChar+path;

			IAPInventoryManager.LogFormat("IAPDatastore File Save to: {0}", _path);

			// Check if Directory exists else create it.
			string dir = Path.GetDirectoryName(_path);
			if(!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
		}			

		override public void Build()
		{
			// If the file doesn't exist then just return the default object.
			if (!File.Exists(_path))
			{
				Debug.LogFormat("ReadFromFile({0}) -- file not found, returning new object", _path);
			}
			else
			{				
				string result = ReadFromDatastore();				
				if(!String.IsNullOrEmpty(result)) StringToDictionary(result);
			}
		}

		virtual protected string ReadFromDatastore()
		{						
			return "";
		}
	}

	/// <summary>
	/// IAP file datastore.
	/// </summary>
	public class IAPTextFileDatastore : IAPFileDatastore
	{
				
		public IAPTextFileDatastore(string path) : base(path)
		{
			
		}

		override protected string ReadFromDatastore()
		{						
			return File.ReadAllText(_path);
		}

		override protected bool WriteToDatastore()
		{									
			File.WriteAllText(_path, DictionaryToString());
			return true;
		}
	}

	/// <summary>
	/// IAP binary file datastore.
	/// </summary>
	public class IAPBinaryFileDatastore : IAPFileDatastore
	{
		public IAPBinaryFileDatastore(string path) : base(path)
		{

		}

		override protected string ReadFromDatastore()
		{					

			string result = "";

			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream(_path, FileMode.Open, FileAccess.Read, FileShare.Read);
			try {
				result = (string)formatter.Deserialize(stream);
			}
			catch (SerializationException e) {
				Debug.LogException(e);
				stream.Close();
			}
			finally {
				stream.Close();
			}

			return result;
		}

		override protected bool WriteToDatastore()
		{								

			IFormatter formatter = new BinaryFormatter();

			try {
				Stream stream = new FileStream(_path, FileMode.Create, FileAccess.Write, FileShare.None);

				try {
					formatter.Serialize(stream, DictionaryToString());
				}
				catch (SerializationException e) {
					Debug.LogException(e);
					stream.Close();
				}
				finally {
					stream.Close();
				}
			}
			catch (Exception) {
				return false;
			}				
			return true;
		}

		override protected void StringToDictionary(string input)
		{			
			base.StringToDictionary(EncryptDecrypt(input));
		}			

		override protected string DictionaryToString()
		{
			string result = base.DictionaryToString();
			return EncryptDecrypt(result);
		}			

		private string EncryptDecrypt(string textToEncrypt)
		{            
			StringBuilder inSb = new StringBuilder(textToEncrypt);
			StringBuilder outSb = new StringBuilder(textToEncrypt.Length);
			char c;
			int key = 0;
			for (int i = 0; i < textToEncrypt.Length; i++)
			{
				c = inSb[i];
				c = (char)(c ^ key);
				outSb.Append(c);
				key++;
				if(key>=256) key=0;
			}
			return outSb.ToString();
		}  
	}

	/// <summary>
	/// IAP encrypted binary file datastore.
	/// </summary>
	public class IAPEncryptedFileDatastore : IAPTextFileDatastore
	{

		private string _hash = "";
		private string _salt = "";
		private string _key  = "";

		public IAPEncryptedFileDatastore(string path, string hash, string key, string salt) : base(path)
		{
			_hash=hash;
			_key=key;
			_salt=salt;
		}

		override protected void StringToDictionary(string input)
		{			
			base.StringToDictionary(Decrypt(input));
		}			

		override protected string DictionaryToString()
		{
			string result = base.DictionaryToString();
			return Encrypt(result);
		}			

		public string Encrypt(string plainText)
		{
			byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			byte[] keyBytes = new Rfc2898DeriveBytes(_hash, Encoding.ASCII.GetBytes(_salt)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
			var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(_key));

			byte[] cipherTextBytes;

			using (var memoryStream = new MemoryStream())
			{
				using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
				{
					cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
					cryptoStream.FlushFinalBlock();
					cipherTextBytes = memoryStream.ToArray();
					cryptoStream.Close();
				}
				memoryStream.Close();
			}				
			return Convert.ToBase64String(cipherTextBytes);
		}

		public string Decrypt(string encryptedText)
		{
//			Debug.Log("_salt : " + _salt);
			byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
			byte[] keyBytes = new Rfc2898DeriveBytes(_hash, Encoding.ASCII.GetBytes(_salt)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

			var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(_key));
			var memoryStream = new MemoryStream(cipherTextBytes);
			var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
			byte[] plainTextBytes = new byte[cipherTextBytes.Length];

			int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
			memoryStream.Close();
			cryptoStream.Close();
			return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
		} 			

	}

	/// <summary>
	/// IAP player preference datastore.
	/// </summary>
	public class IAPPlayerPrefDatastore : IAPDatastore
	{
		private string _prefix;

		public IAPPlayerPrefDatastore(string prefix) : base()
		{			
			_prefix = prefix;
		}	

		override public void Build()
		{
			if(PlayerPrefs.HasKey(_prefix))
			{				
				string data = PlayerPrefs.GetString(_prefix);

				StringToDictionary(data);
			}
		}

		override protected bool WriteToDatastore()
		{									
			PlayerPrefs.SetString(_prefix, DictionaryToString());
			return true;
		}
			
	}

}