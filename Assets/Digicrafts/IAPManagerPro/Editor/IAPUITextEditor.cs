using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Digicrafts.IAP.Pro.UI;

namespace Digicrafts.IAP.Pro.Editor
{	

	[CustomEditor(typeof(IAPText))]
	public class IAPUITextEditor : IAPEditor {

		private SerializedProperty _textType;
		private SerializedProperty _defaultText;

		private SerializedProperty _animated;
		private SerializedProperty _animationTime;

		private GUIContent _targetTypeString;
		private GUIContent _textTypeString;
		private GUIContent _defaultTextString;
		private GUIContent _animatedString;
		private GUIContent _animationTimeString;

		private string[] _currencyListArray;
		private string[] _inventoryListArray;
		private string[] _abilityListArray;
		private string[] _gameListArray;
		private string[] _packageListArray;

		override protected void InitEditor()
		{	
			base.InitEditor();

			title="UI Text";


			_textType=serializedObject.FindProperty("textType");
			_defaultText=serializedObject.FindProperty("defaultText");

			_animated=serializedObject.FindProperty("animated");
			_animationTime=serializedObject.FindProperty("animationTime");

			_targetTypeString=new GUIContent("Target Type");
			_textTypeString=new GUIContent("Text Type");
			_defaultTextString=new GUIContent("Default Text");
			_animatedString=new GUIContent("Animated Number");
			_animationTimeString=new GUIContent("Animation Duration");

			if(settings!=null){
				if(settings.currencyList!=null && settings.currencyList.Count>0)
					_currencyListArray = settings.currencyList.ToArray();
				if(settings.inventoryList!=null && settings.inventoryList.Count>0)
					_inventoryListArray = settings.inventoryList.ToArray();
				if(settings.packageList!=null && settings.packageList.Count>0)
					_packageListArray = settings.packageList.ToArray();
				if(settings.gameList!=null&&settings.gameList.Count>0)
					_gameListArray = settings.gameList.ToArray();				
				if(settings.abilityList!=null&&settings.abilityList.Count>0)
					_abilityListArray = settings.abilityList.ToArray();				
			}

		}

		override protected void DrawBody()
		{			

			EditorGUILayout.PropertyField(_targetType,_targetTypeString);
			EditorGUILayout.BeginHorizontal();
			if(_targetType.enumValueIndex==0){				
				if(settings.currencyList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,settings.currencyList.IndexOf(_uid.stringValue),_currencyListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.currencyList[index];					
				} else if(_currencyListArray!=null){
					EditorGUILayout.Popup(0,_currencyListArray);
				}
			} else if(_targetType.enumValueIndex==1){				
				if(settings.inventoryList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,settings.inventoryList.IndexOf(_uid.stringValue),_inventoryListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.inventoryList[index];					
				} else if(_inventoryListArray!=null){
					EditorGUILayout.Popup(0,_inventoryListArray);
				}
			} else if(_targetType.enumValueIndex==2){
				if(settings.abilityList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,settings.abilityList.IndexOf(_uid.stringValue),_abilityListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.abilityList[index];					
				} else if(_abilityListArray!=null){
					EditorGUILayout.Popup(0,_abilityListArray);
				}
			} else if(_targetType.enumValueIndex==3){				
				if(settings.gameList.Count>0){					
					int index = EditorGUILayout.Popup(_identifyString.text,settings.gameList.IndexOf(_uid.stringValue),_gameListArray);
					if(index<0) index=0;
					_uid.stringValue = settings.gameList[index];					
				} else if(_gameListArray!=null){
					EditorGUILayout.Popup(0,_gameListArray);
				}
			} else if(_targetType.enumValueIndex==4){
				if(settings.packageList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,settings.packageList.IndexOf(_uid.stringValue),_packageListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.packageList[index];					
				} else if(_packageListArray!=null){
					EditorGUILayout.Popup(0,_packageListArray);
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.PropertyField(_textType,_textTypeString);
			EditorGUILayout.PropertyField(_defaultText,_defaultTextString);		
			EditorGUILayout.PropertyField(_animated,_animatedString);
			EditorGUILayout.PropertyField(_animationTime,_animationTimeString);		


		}
			
	}

}