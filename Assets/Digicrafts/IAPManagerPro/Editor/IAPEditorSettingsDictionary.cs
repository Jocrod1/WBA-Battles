using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Digicrafts.IAP.Pro.Editor
{	
	[Serializable]
	public class IAPEditorSettingsDictionary : ScriptableObject, ISerializationCallbackReceiver {				

		public string version = "1";

		[SerializeField]
		private List<string> _keys = new List<string>();
		[SerializeField]
		private List<IAPEditorSettings> _values = new List<IAPEditorSettings>();

		// Not Serializable
		public Dictionary<string, IAPEditorSettings> dict;

		public IAPEditorSettingsDictionary()
		{
			if(dict==null){
				dict=new Dictionary<string, IAPEditorSettings>();
			}
		}

		public void OnBeforeSerialize()
		{
			_keys = new List<string>(dict.Keys);
			_values = new List<IAPEditorSettings>(dict.Values);
		}

		public void OnAfterDeserialize()
		{
			var count = Math.Min(_keys.Count, _values.Count);
			dict = new Dictionary<string, IAPEditorSettings>(count);
			for (var i = 0; i < count; ++i)
				dict.Add(_keys[i], _values[i]);			
		}
	}

}
