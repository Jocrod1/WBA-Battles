#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;

namespace Digicrafts.IAP.Pro.Editor
{	

	[CustomEditor(typeof(IAPManagerPro))]
	public class IAPManagerProEditor : IAPEditor {		

		private SerializedProperty _currencyListProperty;
		private SerializedProperty _inventoryListProperty;
		private SerializedProperty _abilityListProperty;
		private SerializedProperty _packageListProperty;
		private SerializedProperty _gameListProperty;
		private SerializedProperty _tagListProperty;
		private SerializedProperty _advancedSetting;
		private SerializedProperty _databaseSettings;
		private SerializedProperty _fetchDataOnStart;

		private SerializedProperty _errorDialogProperty;
		private SerializedProperty _confirmDialogProperty;
		private SerializedProperty _currencyErrorString;
		private SerializedProperty _buyConfirmString;
		private SerializedProperty _abilityConfirmString;
		private SerializedProperty _iapConfirmString;
		private SerializedProperty _consumeConfirmString;

		//
		private static int _tabIndex;
		private static GUIContent[] _tabGUIContent;

		// Foldout state
		private static List<bool> _packageFoldoutIds;
		private static bool _eventsAFoldouts;
		private static bool _eventsBFoldouts;

		private List<string> _targetTypes;
		private ReorderableList _currencyReorderableList;
		private ReorderableList _tagReorderableList;

		// Editor Internal Flow

		override protected void InitEditor()
		{			
			base.InitEditor();

			SerializedProperty settings=serializedObject.FindProperty("settings");

			_fetchDataOnStart=settings.FindPropertyRelative("fetchDataOnStart");
			_currencyListProperty=settings.FindPropertyRelative("currencyList");
			_abilityListProperty=settings.FindPropertyRelative("abilityList");
			_packageListProperty=settings.FindPropertyRelative("packageList");
			_gameListProperty=settings.FindPropertyRelative("gameList");
			_tagListProperty=settings.FindPropertyRelative("tagList");
			_inventoryListProperty=settings.FindPropertyRelative("inventoryList");
			_advancedSetting=settings.FindPropertyRelative("advancedSettings");
			_databaseSettings=settings.FindPropertyRelative("databaseSettings");

			SerializedProperty uiSettings=settings.FindPropertyRelative("uiSettings");

			_errorDialogProperty=uiSettings.FindPropertyRelative("errorDialog");
			_confirmDialogProperty=uiSettings.FindPropertyRelative("confirmDialog");
			_currencyErrorString=uiSettings.FindPropertyRelative("currencyErrorString");
			_buyConfirmString=uiSettings.FindPropertyRelative("buyConfirmString");
			_abilityConfirmString=uiSettings.FindPropertyRelative("abilityConfirmString");
			_iapConfirmString=uiSettings.FindPropertyRelative("iapConfirmString");
			_consumeConfirmString=uiSettings.FindPropertyRelative("consumeConfirmString");

			float gap = 3;
			float gap2 = gap*2;
			// Currency ReorderableList
			_currencyReorderableList=new ReorderableList(serializedObject,_currencyListProperty,true,true,true,true);
			_currencyReorderableList.drawHeaderCallback = (Rect rect) => {  
				float h = EditorGUIUtility.singleLineHeight;
				float x = rect.x + 15;
				EditorGUI.LabelField(new Rect(x, rect.y, 80, h),"Identify");
				EditorGUI.LabelField(new Rect(x + 80, rect.y, rect.width - 80-50-15, h),"Title");
				EditorGUI.LabelField(new Rect(x + rect.width - 50-80-15, rect.y, 50, h),"Amount");
				EditorGUI.LabelField(new Rect(x + rect.width - 60-15, rect.y, 60, h),"Icon");
			};
			_currencyReorderableList.drawElementCallback =  (Rect rect, int index, bool isActive, bool isFocused) => {
				SerializedProperty element = _currencyReorderableList.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;
				float h = EditorGUIUtility.singleLineHeight;
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, 80-gap, h),
					element.FindPropertyRelative("uid"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(rect.x + 80 +gap, rect.y, rect.width - 80 - 50 - 80 - gap2, h),
					element.FindPropertyRelative("title"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 80 - 50 + gap, rect.y, 50 - gap2, h),
					element.FindPropertyRelative("amount"), GUIContent.none);
				EditorGUI.PropertyField(new Rect(rect.x + rect.width - 80 + gap, rect.y, 80 - gap, h),
					element.FindPropertyRelative("icon"), GUIContent.none);
			};

			//_tagReorderableList
			int lockIndex=IAPSettings.DefaultTag.Length-1;
			_tagReorderableList=new ReorderableList(serializedObject,_tagListProperty,false,true,true,true);
			_tagReorderableList.drawHeaderCallback = (Rect rect) => {  
				float h = EditorGUIUtility.singleLineHeight;
				float x = rect.x;
				EditorGUI.LabelField(new Rect(x, rect.y, rect.width-x, h),"Tags");
			};
			_tagReorderableList.drawElementCallback =  (Rect rect, int index, bool isActive, bool isFocused) => {
				SerializedProperty element = _tagReorderableList.serializedProperty.GetArrayElementAtIndex(index);
				rect.y += 2;
				float h = EditorGUIUtility.singleLineHeight;
				GUI.enabled=(index>lockIndex);
				//, new GUIContent("Tag " + index)
				EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, h), element, new GUIContent("Tag " + index));
				GUI.enabled=true;
			};
			_tagReorderableList.onCanRemoveCallback = (ReorderableList l) => {  
				return l.index>lockIndex;
			};				

			// Foldouts
			if(_packageFoldoutIds==null){				
				_packageFoldoutIds=new List<bool>();
			}	
				
			// The target types of contents
			_targetTypes = new List<string>(){"Currency","Inventory","GameLevel"};

			// Tabs Content
			_tabGUIContent=new GUIContent[]{
				new GUIContent("Currency"),
				new GUIContent("Inventory"),
				new GUIContent("Ability"),
				new GUIContent("Game"),
				new GUIContent("IAP"),
				new GUIContent("Events"),//,IAPEditorStyles.deleteButtonIcon,"\nEvents"),
				new GUIContent("Settings")//IAPEditorStyles.deleteButtonIcon,"Settings")
			};
		}
			
		void OnDestroy()
		{
			string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)))+Path.DirectorySeparatorChar+"IAPEditorSettingsDictionary.asset";
			if(!string.IsNullOrEmpty(_defaultSettingsPath)) path = _defaultSettingsPath;
			SaveSettings(path,AssetDatabase.AssetPathToGUID(EditorSceneManager.GetActiveScene().path), target);
		}

		override protected void DrawBody()
		{
			Color backgroundColor = GUI.backgroundColor;
			Color tableBackgroundColor = Color.yellow;
			if(EditorGUIUtility.isProSkin) tableBackgroundColor = Color.yellow;

			/////////////////////////////////////////////////////////////////////////////////
			/// Tab
			_tabIndex=GUILayout.Toolbar(_tabIndex,_tabGUIContent);

			EditorGUILayout.Space ();			
			
			/// Tab
			/////////////////////////////////////////////////////////////////////////////////


			/////////////////////////////////////////////////////////////////////////////////
			/// Currency List
			if(settings.currencyList!=null)
				settings.currencyList.Clear();
			else
				settings.currencyList = new List<string>();
			settings.currencyList.Add("none");
			SerializedProperty arraySizeProp = _currencyListProperty.FindPropertyRelative("Array.size");
			for(int i = 0; i < arraySizeProp.intValue; i++) {
				SerializedProperty objRef = _currencyListProperty.GetArrayElementAtIndex(i);
				SerializedProperty uid = objRef.FindPropertyRelative("uid");
				settings.currencyList.Add(uid.stringValue);
			}
			string[] currencyListArray = settings.currencyList.ToArray();

			// Display Currency Section
			if(_tabIndex == 0){
				EditorGUILayout.Space ();
				_currencyReorderableList.DoLayoutList();
			}
			/// Currency
			/////////////////////////////////////////////////////////////////////////////////


			/////////////////////////////////////////////////////////////////////////////////
			/// inventory List
			/// 
			arraySizeProp = _tagListProperty.FindPropertyRelative("Array.size");
			if(settings.tagList!=null)
				settings.tagList.Clear();
			else
				settings.tagList = new List<string>();
			for(int i = 0; i < arraySizeProp.intValue; i++){
				SerializedProperty tag = _tagListProperty.GetArrayElementAtIndex(i);
				settings.tagList.Add(tag.stringValue);
			}
			string[] tagListArray = settings.tagList.ToArray();

			arraySizeProp = _inventoryListProperty.FindPropertyRelative("Array.size");
			if(settings.inventoryList!=null)
				settings.inventoryList.Clear();
			else
				settings.inventoryList = new List<string>();
			for(int i = 0; i < arraySizeProp.intValue; i++){
				SerializedProperty uid = _inventoryListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("uid");
				settings.inventoryList.Add(uid.stringValue);
			}

			// Display Inventory
			if(_tabIndex == 1){
				
				//Header
				EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
				GUILayout.Space(20);
				EditorGUILayout.LabelField("Identify",GUILayout.MinWidth(15));
				EditorGUILayout.LabelField("Amount",GUILayout.Width(55));
				EditorGUILayout.LabelField("Tag",GUILayout.Width(40));
				EditorGUILayout.LabelField("Icon",GUILayout.Width(108));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);

				if(arraySizeProp.intValue>0){

					EditorGUILayout.Space ();

					for(int i = 0; i < arraySizeProp.intValue; i++){

						SerializedProperty objRef = _inventoryListProperty.GetArrayElementAtIndex(i);
						SerializedProperty uid = objRef.FindPropertyRelative("uid");
						SerializedProperty icon = objRef.FindPropertyRelative("icon");
						SerializedProperty tags = objRef.FindPropertyRelative("tags");
						SerializedProperty amount = objRef.FindPropertyRelative("amount");

						EditorGUILayout.BeginHorizontal();
						objRef.isExpanded=EditorGUILayout.Toggle(objRef.isExpanded,IAPEditorStyles.listFoldout,GUILayout.Width(15));
						EditorGUILayout.PropertyField(uid,GUIContent.none);
						EditorGUILayout.PropertyField(amount,GUIContent.none,GUILayout.Width(35));
						tags.intValue=EditorGUILayout.MaskField(tags.intValue,tagListArray,GUILayout.Width(50));
						EditorGUILayout.PropertyField(icon,GUIContent.none,GUILayout.Width(60));
						DrawItemControls(_inventoryListProperty,i,arraySizeProp.intValue-1,(target as IAPManagerPro).settings.inventoryList);
						EditorGUILayout.EndHorizontal();

						if(i<arraySizeProp.intValue && objRef.isExpanded){

							SerializedProperty title = objRef.FindPropertyRelative("title");
							SerializedProperty description = objRef.FindPropertyRelative("description");	
							SerializedProperty currency = objRef.FindPropertyRelative("currency");
							SerializedProperty price = objRef.FindPropertyRelative("price");
							SerializedProperty locked = objRef.FindPropertyRelative("locked");
							SerializedProperty available = objRef.FindPropertyRelative("available");
							SerializedProperty properties = objRef.FindPropertyRelative("properties");
		
							EditorGUILayout.BeginVertical(IAPEditorStyles.foldoutBackground);
							EditorGUILayout.PropertyField(title);
							EditorGUILayout.PropertyField(description);
							EditorGUILayout.BeginHorizontal();
							int index = EditorGUILayout.Popup("Price",settings.currencyList.IndexOf(currency.stringValue),currencyListArray);
								index=(index<0||index>settings.currencyList.Count)?0:index;
								currency.stringValue = settings.currencyList[index];
								if(index==0) GUI.enabled=false;
								price.intValue = EditorGUILayout.IntField(price.intValue,GUILayout.MinWidth(30));
								GUI.enabled=true;
							EditorGUILayout.EndHorizontal();
//							DrawCurrencyPopup(objRef,currencyListArray);
							EditorGUILayout.PropertyField(locked);
							EditorGUILayout.PropertyField(available,new GUIContent("Available(-1=infinity)"));

							/// properties
							/// 
							SerializedProperty propertiesSize = properties.FindPropertyRelative("Array.size");
							EditorGUILayout.LabelField("Properties");
							//Header
							EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
							EditorGUILayout.LabelField("Name");
							EditorGUILayout.LabelField("Value",GUILayout.Width(60));
							EditorGUILayout.EndHorizontal();
							//Header
							EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
							GUI.backgroundColor=tableBackgroundColor;
							if(propertiesSize.intValue>0)
							{
								for(int k = 0; k < propertiesSize.intValue; k++){
									SerializedProperty contentRef = properties.GetArrayElementAtIndex(k);
									SerializedProperty name = contentRef.FindPropertyRelative("name");
									SerializedProperty value = contentRef.FindPropertyRelative("value");

									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.PropertyField(name,GUIContent.none);
									EditorGUILayout.PropertyField(value,GUIContent.none,GUILayout.Width(40));
									if(GUILayout.Button(IAPEditorStyles.deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
											properties.DeleteArrayElementAtIndex(k);
									}
									EditorGUILayout.EndHorizontal();
								} 
							} else {
								EditorGUILayout.LabelField("List is Empty.");
							}
							EditorGUILayout.Space ();
							GUI.backgroundColor=backgroundColor;
							EditorGUILayout.EndVertical();
							if(GUILayout.Button("+ Add Property",IAPEditorStyles.addButton)){
								Undo.RecordObject(target,"Add Inventory Property");
								(target as IAPManagerPro).settings.inventoryList[i].properties.Add(new IAPProperty());
							}
							/// properties

							EditorGUILayout.EndVertical();
						}//foldout
						EditorGUILayout.Space ();
					}//loop
				} else {
					EditorGUILayout.LabelField("List is Empty");
				}
				EditorGUILayout.Space ();
				EditorGUILayout.EndVertical();
				/// Table

				EditorGUILayout.Space();
				if(GUILayout.Button("+ Add New Inventory",IAPEditorStyles.addButton)){					
//					_inventoryListProperty.InsertArrayElementAtIndex(_inventoryListProperty.arraySize);
					Undo.RecordObject(target,"Add Inventory");
					(target as IAPManagerPro).settings.inventoryList.Add(new IAPInventorySetting());						
				}
			}				
			/// inventory List
			/////////////////////////////////////////////////////////////////////////////////



			/////////////////////////////////////////////////////////////////////////////////
			/// Ability List
			/// 

			arraySizeProp = _abilityListProperty.FindPropertyRelative("Array.size");
			if(settings.abilityList!=null)
				settings.abilityList.Clear();
			else
				settings.abilityList = new List<string>();
			for(int i = 0; i < arraySizeProp.intValue; i++){
				SerializedProperty uid = _abilityListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("uid");
				settings.abilityList.Add(uid.stringValue);
			}

			if(_tabIndex == 2){


				// Header
				EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
				GUILayout.Space(20);
				EditorGUILayout.LabelField("Identify",GUILayout.MinWidth(15));
				EditorGUILayout.LabelField("Tag",GUILayout.Width(40));
				EditorGUILayout.LabelField("Icon",GUILayout.Width(108));
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
				EditorGUILayout.Space ();

				// check if any ability
				if(arraySizeProp.intValue>0){

					for(int i = 0; i < arraySizeProp.intValue; i++){

						SerializedProperty objRef = _abilityListProperty.GetArrayElementAtIndex(i);
						SerializedProperty uid = objRef.FindPropertyRelative("uid");
						SerializedProperty icon = objRef.FindPropertyRelative("icon");
						SerializedProperty tags = objRef.FindPropertyRelative("tags");

						EditorGUILayout.BeginHorizontal();
						objRef.isExpanded=EditorGUILayout.Toggle(objRef.isExpanded,IAPEditorStyles.listFoldout,GUILayout.Width(15));
						EditorGUILayout.PropertyField(uid,GUIContent.none);
						tags.intValue=EditorGUILayout.MaskField(tags.intValue,tagListArray,GUILayout.Width(50));
						EditorGUILayout.PropertyField(icon,GUIContent.none,GUILayout.Width(60));
						DrawItemControls(_abilityListProperty,i,arraySizeProp.intValue-1,(target as IAPManagerPro).settings.abilityList);
						EditorGUILayout.EndHorizontal();

						if(i<arraySizeProp.intValue && objRef.isExpanded){

							SerializedProperty title = objRef.FindPropertyRelative("title");
							SerializedProperty description = objRef.FindPropertyRelative("description");
//							SerializedProperty currency = objRef.FindPropertyRelative("currency");
							SerializedProperty lockedString = objRef.FindPropertyRelative("lockedString");
							SerializedProperty maxString = objRef.FindPropertyRelative("maxString");

							EditorGUILayout.BeginVertical(IAPEditorStyles.foldoutBackground);

							EditorGUILayout.PropertyField(title);
							EditorGUILayout.PropertyField(description);
							EditorGUILayout.PropertyField(lockedString);
							EditorGUILayout.PropertyField(maxString);
							DrawCurrencyPopup(objRef,currencyListArray);

							/// content
							/// 
							SerializedProperty content = objRef.FindPropertyRelative("levels");
							SerializedProperty contentSize = content.FindPropertyRelative("Array.size");

							//Header
							EditorGUILayout.LabelField("Levels");
							EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
							EditorGUILayout.LabelField(" ",GUILayout.Width(15));
							EditorGUILayout.LabelField("Title",GUILayout.MinWidth(15));
							EditorGUILayout.LabelField("Price",GUILayout.Width(50));
							EditorGUILayout.LabelField("Icon",GUILayout.Width(70));
							EditorGUILayout.EndHorizontal();
							//Header

							//
							EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
							GUI.backgroundColor=tableBackgroundColor;
							if(contentSize.intValue>0)
							{
								for(int k = 0; k < contentSize.intValue; k++){
									SerializedProperty contentRef = content.GetArrayElementAtIndex(k);
									SerializedProperty contentTitle = contentRef.FindPropertyRelative("description");
									SerializedProperty contentPrice = contentRef.FindPropertyRelative("price");
									SerializedProperty contentIcon = contentRef.FindPropertyRelative("icon");

									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField(k.ToString(),GUILayout.MaxWidth(15));
									EditorGUILayout.PropertyField(contentTitle,GUIContent.none);
									if(k==contentSize.intValue-1) GUI.enabled=false;
									EditorGUILayout.PropertyField(contentPrice,GUIContent.none,GUILayout.Width(50));
									GUI.enabled=true;
//										EditorGUILayout.PropertyField(contentPower,GUIContent.none,GUILayout.Width(50));
									EditorGUILayout.PropertyField(contentIcon,GUIContent.none,GUILayout.Width(50));
									if(GUILayout.Button(IAPEditorStyles.deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
										content.DeleteArrayElementAtIndex(k);
									}
									EditorGUILayout.EndHorizontal();
								} 
							} else {
							EditorGUILayout.LabelField("List is Empty.");
							}
							EditorGUILayout.Space ();
							EditorGUILayout.EndVertical();
							GUI.backgroundColor=backgroundColor;
							if(GUILayout.Button("+ Add Level",IAPEditorStyles.addButton)){
								content.InsertArrayElementAtIndex(contentSize.intValue);
							}
							///
							/// content

							EditorGUILayout.EndVertical();
						}//foldout
					EditorGUILayout.Space ();
					} // loop

				} else {
					EditorGUILayout.LabelField("List is Empty.");
				}
				EditorGUILayout.Space ();
				EditorGUILayout.EndVertical();
				/// Table

				EditorGUILayout.Space();
				if(GUILayout.Button("+ Add New Ability",IAPEditorStyles.addButton)){				
//					_abilityListProperty.InsertArrayElementAtIndex(_abilityListProperty.arraySize);
					Undo.RecordObject(target,"Add Ability");
					(target as IAPManagerPro).settings.abilityList.Add(new IAPAbilitySetting());
				}
			}				
			/// ability List
			/////////////////////////////////////////////////////////////////////////////////


			/////////////////////////////////////////////////////////////////////////////////
			/// Game List
			/// 

			arraySizeProp = _gameListProperty.FindPropertyRelative("Array.size");
			if(settings.gameList!=null)
				settings.gameList.Clear();
			else
				settings.gameList = new List<string>();
			for(int i = 0; i < arraySizeProp.intValue; i++){
				SerializedProperty uid = _gameListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("uid");
				settings.gameList.Add(uid.stringValue);
			}

			if(_tabIndex == 3){

				// Header
				EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
				GUILayout.Space(20);
				EditorGUILayout.LabelField("Identify",GUILayout.MinWidth(15));
				EditorGUILayout.EndHorizontal();
				// Header

				EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
				EditorGUILayout.Space ();

				// check if any ability
				if(arraySizeProp.intValue>0){

					for(int i = 0; i < arraySizeProp.intValue; i++){

						SerializedProperty objRef = _gameListProperty.GetArrayElementAtIndex(i);
						SerializedProperty uid = objRef.FindPropertyRelative("uid");

						EditorGUILayout.BeginHorizontal();
						objRef.isExpanded=EditorGUILayout.Toggle(objRef.isExpanded,IAPEditorStyles.listFoldout,GUILayout.Width(15));
						EditorGUILayout.PropertyField(uid,GUIContent.none);
						DrawItemControls(_gameListProperty,i,arraySizeProp.intValue-1,(target as IAPManagerPro).settings.gameList);
						EditorGUILayout.EndHorizontal();

						if(i<arraySizeProp.intValue && objRef.isExpanded){
//
							SerializedProperty title = objRef.FindPropertyRelative("title");
							SerializedProperty description = objRef.FindPropertyRelative("description");
							SerializedProperty icon = objRef.FindPropertyRelative("icon");
							SerializedProperty lockedIcon = objRef.FindPropertyRelative("lockedIcon");
							SerializedProperty locked = objRef.FindPropertyRelative("locked");
							SerializedProperty properties = objRef.FindPropertyRelative("properties");

							EditorGUILayout.BeginVertical(IAPEditorStyles.foldoutBackground);

							EditorGUILayout.PropertyField(title);
							EditorGUILayout.PropertyField(description);
							EditorGUILayout.PropertyField(icon,new GUIContent("Normal Icon"));
							EditorGUILayout.PropertyField(lockedIcon);
							EditorGUILayout.PropertyField(locked,new GUIContent("All locked"));
							DrawCurrencyPopup(objRef,currencyListArray);

							/// properties
							/// 
							SerializedProperty propertiesSize = properties.FindPropertyRelative("Array.size");
							EditorGUILayout.LabelField("Properties");
							//Header
							EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
							EditorGUILayout.LabelField("Name");
							EditorGUILayout.EndHorizontal();
							//Header

							EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
							GUI.backgroundColor=tableBackgroundColor;
							if(propertiesSize.intValue>0)
							{
								for(int k = 0; k < propertiesSize.intValue; k++){
									SerializedProperty contentRef = properties.GetArrayElementAtIndex(k);
									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.PropertyField(contentRef,GUIContent.none);
									if(GUILayout.Button(IAPEditorStyles.deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
										properties.DeleteArrayElementAtIndex(k);
									}
									EditorGUILayout.EndHorizontal();
								} 
							} else {
								EditorGUILayout.LabelField("List is Empty.");
							}
							EditorGUILayout.Space ();
							GUI.backgroundColor=backgroundColor;
							EditorGUILayout.EndVertical();
							if(GUILayout.Button("+ Add Level Property",IAPEditorStyles.addButton)){
								Undo.RecordObject(target,"Add Level Property");
								(target as IAPManagerPro).settings.gameList[i].properties.Add("");
							}
							/// properties

							/// content
							/// 
							SerializedProperty levels = objRef.FindPropertyRelative("levels");
							SerializedProperty contentSize = levels.FindPropertyRelative("Array.size");
							EditorGUILayout.LabelField("Sub-levels");
							//Header
							EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
							EditorGUILayout.LabelField("Level",GUILayout.Width(30));
							EditorGUILayout.LabelField("Title",GUILayout.MinWidth(10));
//							EditorGUILayout.LabelField("Price",GUILayout.Width(40));
							EditorGUILayout.LabelField("Locked",GUILayout.Width(45));
							EditorGUILayout.EndHorizontal();
							//Header

							EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
							GUI.backgroundColor=tableBackgroundColor;
							if(contentSize.intValue>0)
							{
								for(int k = 0; k < contentSize.intValue; k++){
									SerializedProperty contentRef = levels.GetArrayElementAtIndex(k);
									SerializedProperty contentLocked = contentRef.FindPropertyRelative("locked");
									SerializedProperty contentTitle = contentRef.FindPropertyRelative("title");
									SerializedProperty contentPrice = contentRef.FindPropertyRelative("price");

									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField((k+1).ToString(),GUILayout.MaxWidth(30));
									EditorGUILayout.PropertyField(contentTitle,GUIContent.none);
									EditorGUILayout.PropertyField(contentPrice,GUIContent.none,GUILayout.Width(45));
									EditorGUILayout.PropertyField(contentLocked,GUIContent.none,GUILayout.Width(15));
									if(GUILayout.Button(IAPEditorStyles.deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
										levels.DeleteArrayElementAtIndex(k);
									}
									EditorGUILayout.EndHorizontal();
								} 
							} else {
								EditorGUILayout.LabelField("List is Empty.");
							}
							EditorGUILayout.Space ();
							EditorGUILayout.EndVertical();
							GUI.backgroundColor=backgroundColor;
							if(GUILayout.Button("+ Add Sub Level",IAPEditorStyles.addButton)){
								Undo.RecordObject(target,"Add Game SubLevel");
								(target as IAPManagerPro).settings.gameList[i].levels.Add(new IAPGameSubLevel());
//								levels.InsertArrayElementAtIndex(contentSize.intValue);
							}
//							///
//							/// content

							EditorGUILayout.EndVertical();
						}//foldout
						EditorGUILayout.Space ();
					} // loop

				} else {
					EditorGUILayout.LabelField("List is Empty.");
				}
				EditorGUILayout.Space ();
				EditorGUILayout.EndVertical();
				/// Table

//				EditorGUILayout.Space();
				if(GUILayout.Button("+ Add New Game Level",IAPEditorStyles.addButton)){				
					Undo.RecordObject(target,"Add Game Level");
					(target as IAPManagerPro).settings.gameList.Add(new IAPGameLevelSetting());
				}
			}				
			/// game List
			/////////////////////////////////////////////////////////////////////////////////

			/////////////////////////////////////////////////////////////////////////////////
			/// Package
			EditorGUIUtility.labelWidth = 0;
			arraySizeProp = _packageListProperty.FindPropertyRelative("Array.size");

			// Build the product id list
			if(settings.packageList!=null)
				settings.packageList.Clear();
			else
				settings.packageList=new List<string>();

			for(int i = 0; i < arraySizeProp.intValue; i++){
				SerializedProperty productId = _packageListProperty.GetArrayElementAtIndex(i).FindPropertyRelative("productId");
				settings.packageList.Add(productId.stringValue);
			}

			// Display Package Section
			if(_tabIndex == 4){

				EditorGUILayout.PropertyField(_fetchDataOnStart);
				EditorGUILayout.Space();

				//Header
				EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
				GUILayout.Space(20);
				EditorGUILayout.LabelField("Id",GUILayout.MinWidth(15));
				EditorGUILayout.LabelField("Type",GUILayout.Width(70));
				EditorGUILayout.LabelField("Tag",GUILayout.Width(50));
				EditorGUILayout.LabelField("Icon",GUILayout.Width(50));
				EditorGUILayout.LabelField("Fetch",GUILayout.Width(68));
				EditorGUILayout.EndHorizontal();
				//Header

				EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
				if(arraySizeProp.intValue>0){
					EditorGUILayout.Space ();
					string[] inventoryListArray = settings.inventoryList.ToArray();
					string[] gameListArray = settings.gameList.ToArray();
					for(int i = 0; i < arraySizeProp.intValue; i++){

						if(_packageFoldoutIds.Count<i+1) _packageFoldoutIds.Add(false);

						SerializedProperty objRef = _packageListProperty.GetArrayElementAtIndex(i);
						SerializedProperty productId = objRef.FindPropertyRelative("productId");
						SerializedProperty productType = objRef.FindPropertyRelative("productType");
						SerializedProperty fetchFromStore = objRef.FindPropertyRelative("fetchFromStore");
						SerializedProperty icon = objRef.FindPropertyRelative("icon");
						SerializedProperty tags = objRef.FindPropertyRelative("tags");

						EditorGUILayout.BeginHorizontal();

						objRef.isExpanded=EditorGUILayout.Toggle(objRef.isExpanded,IAPEditorStyles.listFoldout,GUILayout.Width(15));
						EditorGUILayout.PropertyField(productId,GUIContent.none);
						EditorGUILayout.PropertyField(productType,GUIContent.none,GUILayout.Width(80));
						tags.intValue=EditorGUILayout.MaskField(tags.intValue,tagListArray,GUILayout.Width(50));
						EditorGUILayout.PropertyField(icon,GUIContent.none,GUILayout.Width(60));
						EditorGUILayout.PropertyField(fetchFromStore,GUIContent.none,GUILayout.Width(15));
						DrawItemControls(_packageListProperty,i,arraySizeProp.intValue-1,(target as IAPManagerPro).settings.packageList,false);
						if(GUILayout.Button(IAPEditorStyles.deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
							_packageListProperty.DeleteArrayElementAtIndex(i);
							_packageFoldoutIds.RemoveAt(i);
						}
						EditorGUILayout.EndHorizontal();

						/////////
						if(i<arraySizeProp.intValue && objRef.isExpanded){

							SerializedProperty title = objRef.FindPropertyRelative("title");
							SerializedProperty description = objRef.FindPropertyRelative("description");

							EditorGUILayout.BeginVertical(IAPEditorStyles.foldoutBackground);

							EditorGUILayout.PropertyField(title);
							EditorGUILayout.PropertyField(description);

							// if not product from app store
							if(!fetchFromStore.boolValue){
								SerializedProperty currency = objRef.FindPropertyRelative("currency");
								SerializedProperty price = objRef.FindPropertyRelative("price");
								EditorGUILayout.BeginHorizontal();
								int index = EditorGUILayout.Popup("Price",settings.currencyList.IndexOf(currency.stringValue),currencyListArray);
								index=(index<0||index>settings.currencyList.Count)?0:index;
								currency.stringValue = settings.currencyList[index];
								if(index==0) GUI.enabled=false;
								EditorGUILayout.PropertyField(price,GUIContent.none,GUILayout.Width(60));
								GUI.enabled=true;
								EditorGUILayout.EndHorizontal();
								//								DrawHorizontalLine();
							} else {							
								// If product from app store
								EditorGUI.indentLevel=1;
								_packageFoldoutIds[i] = EditorGUILayout.Foldout(_packageFoldoutIds[i],"Override Product Ids");
								//
								if(_packageFoldoutIds[i]){
									SerializedProperty appleProductId = objRef.FindPropertyRelative("appleProductId");
									SerializedProperty googleProductId = objRef.FindPropertyRelative("googleProductId");
									SerializedProperty amazonProductId = objRef.FindPropertyRelative("amazonProductId");
									SerializedProperty macProductId = objRef.FindPropertyRelative("macProductId");
									SerializedProperty samsungProductId = objRef.FindPropertyRelative("samsungProductId");
									SerializedProperty tizenProductId = objRef.FindPropertyRelative("tizenProductId");
									SerializedProperty moolahProductId = objRef.FindPropertyRelative("moolahProductId");
									EditorGUILayout.PropertyField(appleProductId);
									EditorGUILayout.PropertyField(googleProductId);
									EditorGUILayout.PropertyField(amazonProductId);
									EditorGUILayout.PropertyField(macProductId);
									EditorGUILayout.PropertyField(samsungProductId);
									EditorGUILayout.PropertyField(tizenProductId);
									EditorGUILayout.PropertyField(moolahProductId);

									EditorGUILayout.Space ();
								}
								//							_packageFoldoutObjects[i] = EditorGUILayout.Foldout(_packageFoldoutObjects[i],"UI Connections");
								//							if(_packageFoldoutObjects[i]){
								//								SerializedProperty buyButton = objRef.FindPropertyRelative("buyButton");
								//								SerializedProperty priceLabel = objRef.FindPropertyRelative("priceLabel");
								//								SerializedProperty titleLabel = objRef.FindPropertyRelative("titleLabel");
								//								SerializedProperty descriptionLabel = objRef.FindPropertyRelative("descriptionLabel");
								//								EditorGUILayout.PropertyField(buyButton);
								//								EditorGUILayout.PropertyField(priceLabel);
								//								EditorGUILayout.PropertyField(titleLabel);
								//								EditorGUILayout.PropertyField(descriptionLabel);
								//								EditorGUILayout.Space();
								//							}	
								EditorGUI.indentLevel=0;
							}

							/// content
							SerializedProperty content = objRef.FindPropertyRelative("content");
							SerializedProperty contentSize = content.FindPropertyRelative("Array.size");

							EditorGUILayout.LabelField("Contents");

							//Header
							EditorGUILayout.BeginHorizontal(IAPEditorStyles.tableHeader);
							EditorGUILayout.LabelField(" ",GUILayout.Width(15));
							EditorGUILayout.LabelField("Type",GUILayout.Width(70));
							EditorGUILayout.LabelField("Identify",GUILayout.MinWidth(20));
							EditorGUILayout.LabelField("Amount",GUILayout.Width(70));
							EditorGUILayout.EndHorizontal();
							//Header

							//
							EditorGUILayout.BeginVertical(IAPEditorStyles.tableBody);
							GUI.backgroundColor=tableBackgroundColor;
							if(contentSize.intValue>0)
							{
								for(int k = 0; k < contentSize.intValue; k++){
									SerializedProperty contentRef = content.GetArrayElementAtIndex(k);
									SerializedProperty contentName = contentRef.FindPropertyRelative("uid");
									SerializedProperty contentAmount = contentRef.FindPropertyRelative("amount");
									SerializedProperty contentType = contentRef.FindPropertyRelative("type");

									EditorGUILayout.BeginHorizontal();
									EditorGUILayout.LabelField(k.ToString(),GUILayout.MaxWidth(15));
									//										contentType.enumValueIndex=(int)(IAPType)EditorGUILayout.EnumPopup((IAPType)Enum.GetValues(typeof(IAPType)).GetValue(contentType.enumValueIndex));	

									int targetTypeIndex=EditorGUILayout.Popup(_targetTypes.IndexOf(contentType.enumNames[contentType.enumValueIndex]),_targetTypes.ToArray(),GUILayout.Width(60));
									contentType.enumValueIndex=IAPTypeIndex[_targetTypes[targetTypeIndex]];
									IAPType type =(IAPType)Enum.GetValues(typeof(IAPType)).GetValue(contentType.enumValueIndex);

									if(type == IAPType.Currency){
										int index = settings.currencyList.IndexOf(contentName.stringValue);
										index=(index<0||index>currencyListArray.Length)?0:index;
										index = EditorGUILayout.Popup(index,currencyListArray);
										contentName.stringValue = currencyListArray[index];
									} else if(type == IAPType.GameLevel){
										int index = settings.gameList.IndexOf(contentName.stringValue);
										index=(index<0||index>gameListArray.Length)?0:index;
										index = EditorGUILayout.Popup(index,gameListArray);
										contentName.stringValue = gameListArray[index];
										GUI.enabled=false;
									} else {
										int index = settings.inventoryList.IndexOf(contentName.stringValue);
										index=(index<0||index>inventoryListArray.Length)?0:index;
										index = EditorGUILayout.Popup(index,inventoryListArray);
										contentName.stringValue = inventoryListArray[index];
									}

									EditorGUILayout.PropertyField(contentAmount,GUIContent.none,GUILayout.Width(50));
									GUI.enabled=true;
									if(GUILayout.Button(IAPEditorStyles.deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
										content.DeleteArrayElementAtIndex(k);
									}
									EditorGUILayout.EndHorizontal();
								} 
							} else {
								EditorGUILayout.LabelField("List is Empty.");
							}

							EditorGUILayout.Space ();
							EditorGUILayout.EndVertical();
							GUI.backgroundColor=backgroundColor;
							if(GUILayout.Button("+ Add Content",IAPEditorStyles.addButton)){
								Undo.RecordObject(target,"Add Package Content");
								(target as IAPManagerPro).settings.packageList[i].content.Add(new IAPContentSetting());
							}								
			
							EditorGUILayout.EndVertical();
						} ///////// foldout
						EditorGUILayout.Space ();
					}//loop
				} else {
					EditorGUILayout.LabelField("List is Empty.");
				}
				EditorGUILayout.Space ();
				EditorGUILayout.EndVertical();
				/// Table

				//				EditorGUILayout.Space ();
				if(GUILayout.Button("+Add In App Purchase",IAPEditorStyles.addButton)){
					//					_packageListProperty.InsertArrayElementAtIndex(_packageListProperty.arraySize);
					Undo.RecordObject(target,"Add IAP");
					(target as IAPManagerPro).settings.packageList.Add(new IAPPackageSetting("product_id"));
				}	

				EditorGUILayout.Space ();
			}
			/// Package
			/////////////////////////////////////////////////////////////////////////////////


			/////////////////////////////////////////////////////////////////////////////////
			/// Tags
			/// 

//			if(_tabIndex==4){
//				EditorGUILayout.Space ();
//				_tagReorderableList.DoLayoutList();

//			}
			/// Tags
			/////////////////////////////////////////////////////////////////////////////////


			if(_tabIndex == 5){

				/////////////////////////////////////////////////////////////////////////////////
				/// Events
				_eventsAFoldouts=EditorGUILayout.Foldout(_eventsAFoldouts,"IAP Manager Events");
				if(_eventsAFoldouts){
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIAPInitialized"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIAPInitializeFailed"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIAPPurchaseStart"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIAPProcessPurchase"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIAPPurchaseFailed"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIAPPurchaseDeferred"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnIAPTransactionsRestored"));
				}
//				DrawSectionHeader("Inventory Manager Events");
				_eventsBFoldouts=EditorGUILayout.Foldout(_eventsBFoldouts,"Inventory Manager Events");
				if(_eventsBFoldouts){
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnCurrencyUpdated"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnInventoryUpdated"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnAbilityUpdated"));
					EditorGUILayout.PropertyField(serializedObject.FindProperty("OnCurrencyNotEnough"));
				}
				/// Events
				/////////////////////////////////////////////////////////////////////////////////
			}

			if(_tabIndex == 6)
			{
				
//				Debug.Log(EditorGUIUtility.currentViewWidth);

				/////////////////////////////////////////////////////////////////////////////////
				/// Database  <

				DrawSectionHeader("Database");

				SerializedProperty datastoreTypeProperty=_databaseSettings.FindPropertyRelative("datastoreType");
				SerializedProperty datastorePathProperty=_databaseSettings.FindPropertyRelative("datastorePath");
				SerializedProperty playerPrefPrefixProperty=_databaseSettings.FindPropertyRelative("playerPrefPrefix");
				SerializedProperty cryptoHashProperty=_databaseSettings.FindPropertyRelative("cryptoHash");

				EditorGUILayout.PropertyField(datastoreTypeProperty);

//				if((IAPDatastoreType)Enum.GetValues(typeof(IAPDatastoreType)).GetValue(datastorePathProperty.enumValueIndex) == IAPDatastoreType.PlayerPref){
				if(datastoreTypeProperty.enumValueIndex == 0){//IAPDatastoreType.PlayerPref
					EditorGUILayout.PropertyField(playerPrefPrefixProperty);
				} else {					
					EditorGUILayout.PropertyField(datastorePathProperty);
					EditorGUILayout.HelpBox("The path is related to Application.persistentDataPath",MessageType.Info);
					if(datastoreTypeProperty.enumValueIndex == 3){//IAPDatastoreType.EncryptedFile
						EditorGUILayout.PropertyField(cryptoHashProperty);
					}
				}
				EditorGUILayout.Space();
				/// Database  <
				/////////////////////////////////////////////////////////////////////////////////

				/////////////////////////////////////////////////////////////////////////////////
				/// Tag  <
				/// 
				DrawSectionHeader("Tags");
				_tagReorderableList.DoLayoutList();
				EditorGUILayout.Space();
				/// Tag  <
				/////////////////////////////////////////////////////////////////////////////////

				/////////////////////////////////////////////////////////////////////////////////
				/// Template  <
				DrawSectionHeader("Templates");
				EditorGUILayout.PropertyField(_errorDialogProperty);
				EditorGUILayout.PropertyField(_confirmDialogProperty);
				EditorGUILayout.Space ();
				EditorGUILayout.PropertyField(_currencyErrorString);
				EditorGUILayout.PropertyField(_buyConfirmString);
				EditorGUILayout.PropertyField(_iapConfirmString);
				EditorGUILayout.PropertyField(_abilityConfirmString);
				EditorGUILayout.PropertyField(_consumeConfirmString);

//				EditorGUILayout.HelpBox("Templates\n\n" +
//					" %title% = title of item\n" +
//					" %description% = description of item\n" +
//					" %price% = price of item\n" +
//					" %currency_title% = title of currency used\n" +
//					" %currency_description% = description of currency used\n"
//					,MessageType.None);

				/// Template  <
				/////////////////////////////////////////////////////////////////////////////////

				EditorGUILayout.Space ();


				/////////////////////////////////////////////////////////////////////////////////
				/// Advanced
				SerializedProperty testMode = _advancedSetting.FindPropertyRelative("testMode");
				SerializedProperty receiptValidation = _advancedSetting.FindPropertyRelative("receiptValidation");
				SerializedProperty GooglePublicKey = _advancedSetting.FindPropertyRelative("GooglePublicKey");
				SerializedProperty MoolahAppKey = _advancedSetting.FindPropertyRelative("MoolahAppKey");
				SerializedProperty MoolahHashKey = _advancedSetting.FindPropertyRelative("MoolahHashKey");
				SerializedProperty TizenGroupId = _advancedSetting.FindPropertyRelative("TizenGroupId");


				DrawSectionHeader("Advanced");
				testMode.boolValue=EditorGUILayout.Toggle("Enable Test Mode (Amazon)",testMode.boolValue);
				EditorGUI.BeginChangeCheck();
				receiptValidation.boolValue=EditorGUILayout.Toggle("Enable Receipt Validation",receiptValidation.boolValue);
				if(EditorGUI.EndChangeCheck()){
					if(receiptValidation.boolValue){
						IAPManagerEditor.AddCompileDefine("RECEIPT_VALIDATION");
					} else {
						IAPManagerEditor.RemoveCompileDefine("RECEIPT_VALIDATION");
					}
						
				}
				EditorGUILayout.PropertyField(GooglePublicKey);
				EditorGUILayout.PropertyField(MoolahAppKey);
				EditorGUILayout.PropertyField(MoolahHashKey);
				EditorGUILayout.PropertyField(TizenGroupId);
				/// Advanced
				/////////////////////////////////////////////////////////////////////////////////	

			}
		}


		private void DrawCurrencyPopup(SerializedProperty objRef ,string[] currencyListArray)
		{
			SerializedProperty currency = objRef.FindPropertyRelative("currency");
			EditorGUILayout.BeginHorizontal();				
			int index = EditorGUILayout.Popup("Currency",settings.currencyList.IndexOf(currency.stringValue),currencyListArray);
			index=(index<0||index>settings.currencyList.Count)?0:index;
			currency.stringValue = settings.currencyList[index];
			if(index==0) GUI.enabled=false;
			GUI.enabled=true;
			EditorGUILayout.EndHorizontal();
		}

		private void DrawItemControls(SerializedProperty prop, int i, int size, IList list, bool drawDeleteButton = true)
		{
			///
			if(i==0)GUI.enabled=false;
			if(GUILayout.Button(IAPEditorStyles.upButtonIcon,GUIStyle.none,GUILayout.Width(10))) prop.MoveArrayElement(i,i-1);
			GUI.enabled=true;
			if(i==size)GUI.enabled=false;				
			if(GUILayout.Button(IAPEditorStyles.downButtonIcon,GUIStyle.none,GUILayout.Width(10))) prop.MoveArrayElement(i,i+1);
			GUI.enabled=true;
			// Dupicate Button
			if(GUILayout.Button(IAPEditorStyles.duplicateButtonIcon,GUIStyle.none,GUILayout.Width(14))){
				Undo.RecordObject(target,"Dupicate");
				IAPManagerPro shop = target as IAPManagerPro;
				if(list == shop.settings.inventoryList){
					IAPInventorySetting item = list[i] as IAPInventorySetting;
					list.Add(item.Clone());
				} else if(list == shop.settings.abilityList){
					IAPAbilitySetting item = list[i] as IAPAbilitySetting;
					list.Add(item.Clone());
				} else if(list == shop.settings.packageList){
					IAPPackageSetting item = list[i] as IAPPackageSetting;
					list.Add(item.Clone());
				} else if(list == shop.settings.gameList){
					IAPGameLevelSetting item = list[i] as IAPGameLevelSetting;
					list.Add(item.Clone());
				}
			}

			// Delete button
			if(drawDeleteButton){
				if(GUILayout.Button(IAPEditorStyles.deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
					prop.DeleteArrayElementAtIndex(i);
				}
			}
		}
	}		

}
#endif