#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

namespace Digicrafts.IAP.Pro.Editor
{	

	public class IAPEditor : UnityEditor.Editor {

		// Constant

		private static Dictionary<string,int> _IAPTypeIndex;
		public static Dictionary<string,int> IAPTypeIndex
		{
			get{
				if(_IAPTypeIndex==null)
					_IAPTypeIndex = new Dictionary<string, int>(){
					{"Currency",0},
					{"Inventory",1},
					{"Ability",2},
					{"GameLevel",3},
					{"InAppPurchase",4}
				};
				return _IAPTypeIndex;
			}
		}
			
		protected string title = "IAP Manager Pro";
	
		// Private static
		protected static IAPEditorSettings settings;
		protected static string _defaultSettingsPath=null;
		private static bool _prefsLoaded = false;
		private static IAPEditorSettingsDictionary _settingDictionary;

		// Private
		private string _editorPath;
		private string _settingKey;
		private List<string> _settingKeys;
		private List<string> _settingKeyNames;
		protected SerializedProperty _settingKeyDefault;
		protected SerializedProperty _targetType;
		protected SerializedProperty _uid;
		protected GUIContent _typeString;
		protected GUIContent _identifyString;

			
		[PreferenceItem("IAP Manager Pro")]
		public static void PreferencesGUI()
		{
			// Load the preferences
			LoadPreferences();

			EditorGUILayout.LabelField("Settings Path");
			_defaultSettingsPath = EditorGUILayout.TextField(_defaultSettingsPath);

			// Save the preferences
			if (GUI.changed){
				EditorPrefs.SetString("IAPManagerProSettingsPath", _defaultSettingsPath);
				settings=null;
			}
		}
		private static void LoadPreferences()
		{
					// Load the preferences
			if (!_prefsLoaded)
			{				
				string path = EditorPrefs.GetString("IAPManagerProSettingsPath");
				if(!string.IsNullOrEmpty(path)) _defaultSettingsPath=path;
				_prefsLoaded=true;
			}

//			Debug.Log("LoadPreferences " + _defaultSettingsPath);
		}

		/// <summary>
		/// Creates the settings.
		/// </summary>
		/// <param name="path">Path.</param>
		public static List<string> CreateSettings(string path, string settingKey, bool isShopEditor)
		{						

			List<string> keys = null;

			if(settings==null || settings.key!=settingKey)
			{										
				// Get the setting list
				_settingDictionary=(IAPEditorSettingsDictionary)EditorGUIUtility.Load(path);							

				// Check if the file exist
				if(_settingDictionary==null){

					// Create the new IAPEditorSettingsList
					_settingDictionary = ScriptableObject.CreateInstance<IAPEditorSettingsDictionary>();

					// Create new settings
					settings=new IAPEditorSettings();
					settings.key=settingKey;

				} else {		

					// if the file exist and contains the key/value
					if(_settingDictionary.dict.ContainsKey(settingKey))
					{
						settings=_settingDictionary.dict[settingKey];
					} else if(isShopEditor){
						// Create new settings
						settings=new IAPEditorSettings();
						settings.key=settingKey;
					}					
						
					keys=new List<string>(_settingDictionary.dict.Keys);
				}

			}
			return keys;
		}

		/// <summary>
		/// Saves the settings.
		/// </summary>
		/// <param name="path">Path.</param>
		public static void SaveSettings(string path, string settingKey, UnityEngine.Object target)
		{			

			if(settings!=null&&_settingDictionary!=null){

				if(_settingDictionary.dict.ContainsKey(settingKey))
				{
					
					// Remove settings
					if(target==null) {
						settings=null;
						_settingDictionary.dict.Remove(settingKey);
					} else
						_settingDictionary.dict[settingKey]=settings;

				} else {
					// Add the settings to _settingDictionary
					_settingDictionary.dict.Add(settingKey,settings);
				}

				// Save the setting dictionary
				EditorUtility.SetDirty(_settingDictionary);
				if(!AssetDatabase.Contains(_settingDictionary))
					AssetDatabase.CreateAsset(_settingDictionary, path);				
				AssetDatabase.SaveAssets();
			}
		}

		void OnEnable()
		{					
			// Get Properties
			_settingKeyDefault=serializedObject.FindProperty("settingKeyDefault");
			_targetType=serializedObject.FindProperty("targetType");
			_uid=serializedObject.FindProperty("uid");
			// Set Properties label
			_typeString=new GUIContent("Object Type");
			_identifyString=new GUIContent("Identify");
			// Editor Path
			_editorPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)))+Path.DirectorySeparatorChar;

			// Get the path to settings file
			LoadPreferences();
//			Debug.Log("path " + AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));
			string path = _editorPath+"IAPEditorSettingsDictionary.asset";
			if(!string.IsNullOrEmpty(_defaultSettingsPath)) path = _defaultSettingsPath;

			if(_settingKeyDefault!=null&&!string.IsNullOrEmpty(_settingKeyDefault.stringValue)){
				_settingKey = _settingKeyDefault.stringValue;
			} else {
				_settingKey = AssetDatabase.AssetPathToGUID(EditorSceneManager.GetActiveScene().path);
			}				

			// Get the current scene editor settings
			_settingKeys=CreateSettings(path, _settingKey, (_settingKeyDefault==null));
			if(_settingKeys!=null){
				_settingKeyNames=new List<string>();
				foreach(string n in _settingKeys){
					_settingKeyNames.Add(AssetDatabase.GUIDToAssetPath(n));
				}
			}
			InitEditor();
		}

		public override void OnInspectorGUI()
		{
			DrawLogo();
//			Debug.Log("settings.key " + settings.key + " _settingKey " + _settingKey);
			if(settings!=null && ((settings.key==_settingKey && settings.tagList!=null) || _settingKeyDefault==null))
			{				
				DrawBody();

			} else {

				if(_settingKeyDefault!=null&&_settingKeys!=null){
					// Display the settings file selection box
					EditorGUILayout.HelpBox("Please create an IAPManagerPro in this scene. Or, select the scene with the IAPManagerPro.",MessageType.Error);
					int index = _settingKeys.IndexOf(_settingKeyDefault.stringValue);
					index=EditorGUILayout.Popup("Scene",index,_settingKeyNames.ToArray());
					if(index!=-1&&index<_settingKeys.Count){
						_settingKeyDefault.stringValue=_settingKeys[index];
					}
				} else {
					//Error
					EditorGUILayout.HelpBox("You don't have any IAPManagerPro in this porject.",MessageType.Error);
				}
			}

			DrawFooter();
		}


		// Helpers

		protected virtual void InitEditor()
		{						
			IAPEditorStyles.initImages(_editorPath);
		}

		protected virtual void DrawLogo()
		{
			IAPEditorStyles.init();

			EditorGUILayout.BeginVertical();

			//Update SerialObject
			serializedObject.Update();
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Space();
			EditorGUILayout.Space ();

			/////////////////////////////////////////////////////////////////////////////////
			//// Logo
			EditorGUILayout.BeginHorizontal();//IAPEditorStyles.logoBackground
			EditorGUILayout.LabelField(title,IAPEditorStyles.logoTitle,GUILayout.Height(30));
			EditorGUILayout.LabelField("",IAPEditorStyles.logo,GUILayout.Width(80),GUILayout.Height(30));
			EditorGUILayout.EndHorizontal();
			GUILayout.Box(GUIContent.none,GUILayout.ExpandWidth(true),GUILayout.Height(1));
			EditorGUILayout.Space();
			//// Logo
			/// /////////////////////////////////////////////////////////////////////////////////

		}

		protected virtual void DrawBody()
		{

		}

		protected virtual void DrawFooter()
		{
			EditorGUILayout.Space();
			//Apply the changes to our list
			if(EditorGUI.EndChangeCheck()){				
				serializedObject.ApplyModifiedProperties();
			}

			EditorGUILayout.EndVertical();
		}

		protected void DrawSectionHeader(string title)
		{
			EditorGUILayout.LabelField(title,IAPEditorStyles.sectionTitle,GUILayout.Height(20));
//			GUILayout.Box(GUIContent.none,GUILayout.ExpandWidth(true),GUILayout.Height(1));
			DrawHorizontalLine();
			EditorGUILayout.Space ();
		}

		protected void DrawHorizontalLine()
		{
			GUILayout.Box(GUIContent.none,GUILayout.ExpandWidth(true),GUILayout.Height(1));		
		}
	}

}
#endif