using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using UnityEngine.Purchasing;

namespace Digicrafts.IAP
{

    [InitializeOnLoad]
    [CustomEditor(typeof(IAPBootstrap))]
	public class IAPManagerEditor : Editor
    {

		static private Texture2D _icon;

		static IAPManagerEditor ()
     	{
			_icon = Resources.Load("icon") as Texture2D;
			//Type testType = Type.GetType("Resources");
			//Debug.Log("testType " + testType);
			//EditorApplication.hierarchyWindowItemOnGUI += handleHierarchyWindowItemOnGUI;
			//EditorUtility.DisplayDialog("Warning", "IAP Manager request Unity Purchasing. Please install first!", "Understand");

		}

		static private void handleHierarchyWindowItemOnGUI (int instanceID, Rect selectionRect)
     	{
			// place the icoon to the right of the list:
			Rect r = new Rect (selectionRect); 
			r.x = r.width - 20;
			r.width = 18;
			
			GameObject obj = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

			if (obj != null && obj.GetComponent<IAPBootstrap>() != null) 
			{
				// Draw the texture if it's a light (e.g.)
				GUI.Label (r, _icon); 
			}
     	}

		[MenuItem("Tools/Digicrafts/IAP/Install Play Maker Plugin")]
		public static void InstallPlayMakerPlugin ()
		{
			AssetDatabase.ImportPackage(Application.dataPath+"/Digicrafts/IAPManager/Extra/IAPManagerPlayMaker.unitypackage",false);
		}


		[MenuItem("Tools/Digicrafts/IAP/Install Bolt Plugin")]
		public static void InstallBoltPlugin ()
		{
			AssetDatabase.ImportPackage(Application.dataPath+"/Digicrafts/IAPManager/Extra/IAPManagerBolt.unitypackage",false);
		}

		[MenuItem("Tools/Digicrafts/IAP/Add Receipt Validation",true)]
		public static bool CheckAddReceiptValidation ()
		{	
			return !CheckCompileDefine("RECEIPT_VALIDATION");
		}

		[MenuItem("Tools/Digicrafts/IAP/Add Receipt Validation",false,15)]
		public static void AddReceiptValidation ()
		{			
			AddCompileDefine("RECEIPT_VALIDATION");					
		}

		[MenuItem("Tools/Digicrafts/IAP/Remove Receipt Validation",true)]
		public static bool CheckRemoveReceiptValidation ()
		{			
			return CheckCompileDefine("RECEIPT_VALIDATION");
		}

		[MenuItem("Tools/Digicrafts/IAP/Remove Receipt Validation",false,16)]
		public static void RemoveReceiptValidation ()
		{			
			RemoveCompileDefine("RECEIPT_VALIDATION");		
		}
		

		private SerializedProperty _eventTarget;
		private SerializedProperty _restoreButton;
		private SerializedProperty _advancedSetting;
		private SerializedProperty _productSettings;

		private static Texture2D logo;
		private static GUIStyle logoStyle;
		private static GUIStyle logoBackgroundStyle;
		private static GUIStyle _titleStyle;
		private static GUIStyle _addButtonStyle;
		private static GUIStyle listFoldout;
		private static GUIStyle listHeader;
		private static Texture2D _deleteButtonIcon;

		private static bool _packageFoldout;
		private static bool _eventFoldout;


		private static List<bool> _packageFoldouts;
		private static List<bool> _packageFoldoutIds;
		private static List<bool> _packageFoldoutObjects;

		private Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width*height];

			for(int i = 0; i < pix.Length; i++)
				pix[i] = col;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}

		void OnEnable()
		{									

			_eventTarget=serializedObject.FindProperty("eventTarget");
			_restoreButton=serializedObject.FindProperty("restoreButton");
			_productSettings=serializedObject.FindProperty("productSettings");		
			_advancedSetting=serializedObject.FindProperty("advancedSetting");		

			if(_packageFoldoutIds==null){
				_packageFoldouts=new List<bool>();
				_packageFoldoutIds=new List<bool>();
				_packageFoldoutObjects=new List<bool>();
			}
				
		}

		public override void OnInspectorGUI(){

			//Update our list
			serializedObject.Update();

			if(logo==null) {
				if(EditorGUIUtility.isProSkin)
					logo=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Digicrafts/IAPManager/Editor/logo_pro.png");
				else
					logo=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Digicrafts/IAPManager/Editor/logo.png");
			}
			if(logoStyle==null) {
				logoStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
				logoStyle.alignment = TextAnchor.UpperCenter;
				logoStyle.normal.background=logo;
				logoBackgroundStyle = new  GUIStyle(GUI.skin.label);
				_titleStyle = new GUIStyle(GUI.skin.label);
				_titleStyle.fontSize=14;
				_titleStyle.fixedHeight=20;


				listHeader= "RL Header";

				_addButtonStyle=new GUIStyle(GUI.skin.button);

				if(EditorGUIUtility.isProSkin)
					_addButtonStyle.normal.textColor=Color.yellow;
				else
					_addButtonStyle.normal.textColor=Color.black;

				_deleteButtonIcon=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Digicrafts/IAPManager/Editor/delete.png");

				GUISkin editorSkin=EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
				listFoldout = new GUIStyle(GUI.skin.toggle);
				listFoldout.normal.background=listFoldout.focused.background=listFoldout.active.background=editorSkin.GetStyle("Foldout").normal.background;
				listFoldout.onFocused.background=listFoldout.onActive.background=listFoldout.onNormal.background=listFoldout.onHover.background=editorSkin.GetStyle("PaneOptions").normal.background;
			}	

			EditorGUILayout.Space();

			//// Logo
			EditorGUILayout.BeginHorizontal(logoBackgroundStyle);
			GUILayout.FlexibleSpace();
			EditorGUILayout.LabelField("",logoStyle,GUILayout.Width(300),GUILayout.Height(50));
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			//// Logo


			EditorGUI.BeginChangeCheck();

			EditorGUILayout.Separator();

			/////////////////////////////////////////////////////////////////////////////////
			/// Package
			EditorGUILayout.Space ();
			EditorGUILayout.LabelField("In App Products",_titleStyle);
			EditorGUILayout.Space ();

			SerializedProperty arraySizeProp = _productSettings.FindPropertyRelative("Array.size");
			EditorGUILayout.BeginVertical(GUI.skin.box);

			//Header
			EditorGUILayout.BeginHorizontal( listHeader);
			EditorGUILayout.LabelField(" ",GUILayout.Width(15));
			EditorGUILayout.LabelField("Identify",GUILayout.MinWidth(15));
			EditorGUILayout.LabelField("Type",GUILayout.Width(110));
			EditorGUILayout.EndHorizontal();
			//Header

//			EditorGUI.indentLevel=1;
//			EditorGUILayout.BeginHorizontal();
//			_packageFoldout=EditorGUILayout.Foldout(_packageFoldout,"Product List");

//			if(_packageFoldout){

//				if(GUILayout.Button("+ Add New In App Purchase",_addButtonStyle)){				
//					_productSettings.InsertArrayElementAtIndex(_productSettings.arraySize);
//					_packageFoldoutIds.Add(false);
//					_packageFoldoutObjects.Add(false);
//				}	
//				EditorGUILayout.EndHorizontal();
//				EditorGUI.indentLevel=1;
//				EditorGUILayout.BeginVertical();
				for(int i = 0; i < arraySizeProp.intValue; i++){

					if(_packageFoldouts.Count<i+1) _packageFoldouts.Add(false);
					if(_packageFoldoutIds.Count<i+1) _packageFoldoutIds.Add(false);
					if(_packageFoldoutObjects.Count<i+1) _packageFoldoutObjects.Add(false);					

					EditorGUI.indentLevel=0;
					SerializedProperty objRef = _productSettings.GetArrayElementAtIndex(i);
					SerializedProperty productId = objRef.FindPropertyRelative("productId");
					SerializedProperty productType = objRef.FindPropertyRelative("productType");
					bool isSubscription = (productType.enumNames[productType.enumValueIndex]=="Subscription");

//					if(i != 0) GUILayout.Box("",GUILayout.Height(2),GUILayout.ExpandWidth(true));
					EditorGUILayout.Space();
					EditorGUILayout.BeginHorizontal();
					_packageFoldouts[i]=EditorGUILayout.Toggle(GUIContent.none,_packageFoldouts[i],listFoldout,GUILayout.Width(15));
//					EditorGUILayout.LabelField("ID:",GUILayout.MaxWidth(20));
					productId.stringValue = EditorGUILayout.TextField(GUIContent.none,productId.stringValue,GUILayout.MinWidth(15));
					productType.enumValueIndex=(int)(IAPProductType)EditorGUILayout.EnumPopup((IAPProductType)Enum.GetValues(typeof(IAPProductType)).GetValue(productType.enumValueIndex),GUILayout.Width(90));	
					if(GUILayout.Button(_deleteButtonIcon,GUIStyle.none,GUILayout.Width(20))){
						_productSettings.DeleteArrayElementAtIndex(i);
						_packageFoldoutIds.RemoveAt(i);
						_packageFoldoutObjects.RemoveAt(i);
					}
					EditorGUILayout.EndHorizontal();

					if(_packageFoldouts[i]){
						EditorGUILayout.Space();
						EditorGUI.indentLevel=1;
						if(i<_packageFoldoutIds.Count){
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
						_packageFoldoutObjects[i] = EditorGUILayout.Foldout(_packageFoldoutObjects[i],"UI Connections");
						if(_packageFoldoutObjects[i]){
							SerializedProperty buyButton = objRef.FindPropertyRelative("buyButton");
							SerializedProperty priceLabel = objRef.FindPropertyRelative("priceLabel");
							SerializedProperty titleLabel = objRef.FindPropertyRelative("titleLabel");
							SerializedProperty descriptionLabel = objRef.FindPropertyRelative("descriptionLabel");
							SerializedProperty buyButtonText = objRef.FindPropertyRelative("buyButtonText");
							SerializedProperty disabledButtonText = objRef.FindPropertyRelative("disabledButtonText");
													
							EditorGUILayout.PropertyField(buyButtonText);
							EditorGUILayout.PropertyField(disabledButtonText);
							EditorGUILayout.PropertyField(buyButton);
							EditorGUILayout.PropertyField(priceLabel);
							EditorGUILayout.PropertyField(titleLabel);
							EditorGUILayout.PropertyField(descriptionLabel);
							if(isSubscription){
								EditorGUILayout.PropertyField(objRef.FindPropertyRelative("purchaseDateLabel"));
								EditorGUILayout.PropertyField(objRef.FindPropertyRelative("remainingTimeLabel"));
								EditorGUILayout.PropertyField(objRef.FindPropertyRelative("cancelDateLabel"));
								EditorGUILayout.PropertyField(objRef.FindPropertyRelative("expireDateLabel"));
								EditorGUILayout.PropertyField(objRef.FindPropertyRelative("freeTrialPeriodLabel"));
								EditorGUILayout.PropertyField(objRef.FindPropertyRelative("introductoryPriceLabel"));
								EditorGUILayout.PropertyField(objRef.FindPropertyRelative("introductoryPricePeriodLabel"));								
							}

							EditorGUILayout.Space();
						}						
						}
					} 

//				EditorGUILayout.EndVertical();

//			} else {
//
//				EditorGUILayout.EndHorizontal();
//
//			}
			}

			EditorGUILayout.Space ();
			if(GUILayout.Button("+ Add New In App Purchase",_addButtonStyle)){				
				_productSettings.InsertArrayElementAtIndex(_productSettings.arraySize);
				_packageFoldoutIds.Add(false);
				_packageFoldoutObjects.Add(false);
			}	

			EditorGUILayout.EndVertical();
			EditorGUILayout.Space ();
			EditorGUI.indentLevel=0;
			/// Package
			/////////////////////////////////////////////////////////////////////////////////



			/////////////////////////////////////////////////////////////////////////////////
			/// General
			EditorGUILayout.LabelField("General",_titleStyle);
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical(GUI.skin.box);
//			[Header ("The target contains script handle the events", order = 3)]
			EditorGUILayout.LabelField("Target contains script handle the events");
			EditorGUILayout.PropertyField(_eventTarget);
			EditorGUILayout.LabelField("Target object for restore button");
			EditorGUILayout.PropertyField(_restoreButton);
			EditorGUILayout.EndVertical();
			/// Events
			/////////////////////////////////////////////////////////////////////////////////

			EditorGUILayout.Separator();


			/////////////////////////////////////////////////////////////////////////////////
			/// Events
			EditorGUILayout.LabelField("Events",_titleStyle);
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical(GUI.skin.box);
			EditorGUI.indentLevel=1;
			_eventFoldout=EditorGUILayout.Foldout(_eventFoldout,"Events List");
			EditorGUI.indentLevel=0;
			if(_eventFoldout){
				SerializedProperty OnInitialized = serializedObject.FindProperty("OnInitialized");
				SerializedProperty OnInitializeFailed = serializedObject.FindProperty("OnInitializeFailed");
				SerializedProperty OnPurchaseStart = serializedObject.FindProperty("OnPurchaseStart");
				SerializedProperty OnProcessPurchase = serializedObject.FindProperty("OnProcessPurchase");
				SerializedProperty OnPurchaseFailed = serializedObject.FindProperty("OnPurchaseFailed");
				SerializedProperty OnPurchaseDeferred = serializedObject.FindProperty("OnPurchaseDeferred");
				SerializedProperty OnTransactionsRestored = serializedObject.FindProperty("OnTransactionsRestored");
				EditorGUILayout.PropertyField(OnInitialized);
				EditorGUILayout.PropertyField(OnInitializeFailed);
				EditorGUILayout.PropertyField(OnPurchaseStart);
				EditorGUILayout.PropertyField(OnProcessPurchase);
				EditorGUILayout.PropertyField(OnPurchaseFailed);
				EditorGUILayout.PropertyField(OnPurchaseDeferred);
				EditorGUILayout.PropertyField(OnTransactionsRestored);
			}
			EditorGUILayout.EndVertical();
			EditorGUI.indentLevel=0;
			/// Events
			/////////////////////////////////////////////////////////////////////////////////

			EditorGUILayout.Separator();

			/////////////////////////////////////////////////////////////////////////////////
			/// Advanced
			SerializedProperty testMode = _advancedSetting.FindPropertyRelative("testMode");
			SerializedProperty receiptValidation = _advancedSetting.FindPropertyRelative("receiptValidation");
			SerializedProperty GooglePublicKey = _advancedSetting.FindPropertyRelative("GooglePublicKey");
			SerializedProperty MoolahAppKey = _advancedSetting.FindPropertyRelative("MoolahAppKey");
			SerializedProperty MoolahHashKey = _advancedSetting.FindPropertyRelative("MoolahHashKey");
			SerializedProperty TizenGroupId = _advancedSetting.FindPropertyRelative("TizenGroupId");
			EditorGUILayout.LabelField("Advanced",_titleStyle);
			EditorGUILayout.Space ();
			EditorGUILayout.BeginVertical(GUI.skin.box);
			testMode.boolValue=EditorGUILayout.Toggle("Enable Test Mode",testMode.boolValue);
			receiptValidation.boolValue=EditorGUILayout.Toggle("Enable Receipt Validation",receiptValidation.boolValue);
			EditorGUILayout.PropertyField(GooglePublicKey);
			EditorGUILayout.PropertyField(MoolahAppKey);
			EditorGUILayout.PropertyField(MoolahHashKey);
			EditorGUILayout.PropertyField(TizenGroupId);
			EditorGUILayout.EndVertical();
			/// Advanced
			/////////////////////////////////////////////////////////////////////////////////


			if(receiptValidation.boolValue){
				AddCompileDefine("RECEIPT_VALIDATION");
			} else {
				RemoveCompileDefine("RECEIPT_VALIDATION");
			}
				
			//Apply the changes to our list
			if(EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();			

		}


		/// <summary>
		/// Attempts to add a new #define constant to the Player Settings
		/// </summary>
		/// <param name="newDefineCompileConstant">constant to attempt to define</param>
		/// <param name="targetGroups">platforms to add this for (null will add to all platforms)</param>
		public static bool CheckCompileDefine(string newDefineCompileConstant, BuildTargetGroup[] targetGroups = null)
		{
			bool result = false;

			if (targetGroups == null)
				targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));

			foreach (BuildTargetGroup grp in targetGroups)
			{
				if (CheckBuildTargetGroup(grp))  {

					string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(grp);
					if (defines.Contains(newDefineCompileConstant))
					{
						result=true;
						break;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// Attempts to add a new #define constant to the Player Settings
		/// </summary>
		/// <param name="newDefineCompileConstant">constant to attempt to define</param>
		/// <param name="targetGroups">platforms to add this for (null will add to all platforms)</param>
		public static void AddCompileDefine(string newDefineCompileConstant, BuildTargetGroup[] targetGroups = null)
		{
			if (targetGroups == null)
				targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));

			foreach (BuildTargetGroup grp in targetGroups)
			{
				if (CheckBuildTargetGroup(grp))  {

					string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(grp);
					if (!defines.Contains(newDefineCompileConstant))
					{
						if (defines.Length > 0)         //if the list is empty, we don't need to append a semicolon first
							defines += ";";

						defines += newDefineCompileConstant;
						PlayerSettings.SetScriptingDefineSymbolsForGroup(grp, defines);
					}
				}
			}
		}

		/// <summary>
		/// Attempts to remove a #define constant from the Player Settings
		/// </summary>
		/// <param name="defineCompileConstant"></param>
		/// <param name="targetGroups"></param>
		public static void RemoveCompileDefine(string defineCompileConstant, BuildTargetGroup[] targetGroups = null)
		{
			if (targetGroups == null)
				targetGroups = (BuildTargetGroup[])Enum.GetValues(typeof(BuildTargetGroup));

			foreach (BuildTargetGroup grp in targetGroups)
			{
				if (CheckBuildTargetGroup(grp))  {
					
					string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(grp);				
					
					int index = defines.IndexOf(defineCompileConstant);
					if (index < 0)
						continue;           //this target does not contain the define
					else if (index > 0)
						index -= 1;         //include the semicolon before the define
					//else we will remove the semicolon after the define

					//Remove the word and it's semicolon, or just the word (if listed last in defines)
					int lengthToRemove = Math.Min(defineCompileConstant.Length + 1, defines.Length - index);

					//remove the constant and it's associated semicolon (if necessary)
					defines = defines.Remove(index, lengthToRemove);

					PlayerSettings.SetScriptingDefineSymbolsForGroup(grp, defines);
				}
			}
		}

		private static bool CheckBuildTargetGroupObsolete(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (ObsoleteAttribute[]) fi.GetCustomAttributes(typeof(ObsoleteAttribute), false);
            return attributes.Length > 0;
        }

		private static bool CheckBuildTargetGroup(BuildTargetGroup group)
        {
            if (group == BuildTargetGroup.Unknown || CheckBuildTargetGroupObsolete(group)) return false;
#if UNITY_5_3_0
            if ((int)(object)group == 25) return false;
#endif
#if UNITY_5_4 || UNITY_5_5
            if ((int)(object)group == 15) return false;
            if ((int)(object)group == 16) return false;
#endif
            return true;
        }		
	}

}