using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Digicrafts.IAP.Pro.UI;

namespace Digicrafts.IAP.Pro.Editor
{	

	[CustomEditor(typeof(IAPConsumeButton))]
	public class IAPUIConsumeButtonEditor : IAPEditor {

		private SerializedProperty _amount;
		private SerializedProperty _useConfirmDialog;

		private SerializedProperty _OnConsumeSuccess;
		private SerializedProperty _OnConsumeFail;

		private List<string> _targetTypes;
		private string[] _currencyListArray;
		private string[] _inventoryListArray;

		override protected void InitEditor()
		{	
			base.InitEditor();

			title="UI Consume Button";

			_amount=serializedObject.FindProperty("amount");
			_useConfirmDialog=serializedObject.FindProperty("useConfirmDialog");
			_OnConsumeSuccess=serializedObject.FindProperty("OnConsumeSuccess");
			_OnConsumeFail=serializedObject.FindProperty("OnConsumeFail");

			//
			_targetTypes = new List<string>(){"Currency","Inventory"};
			//
			if(settings.currencyList!=null && settings.currencyList.Count>0)
				_currencyListArray = settings.currencyList.ToArray();
			else
				_currencyListArray = new string[]{"none"};
			//
			if(settings.inventoryList!=null && settings.inventoryList.Count>0)
				_inventoryListArray = settings.inventoryList.ToArray();
			else
				_inventoryListArray = new string[]{"none"};
		}

		override protected void DrawBody()
		{						


			int targetTypeIndex=EditorGUILayout.Popup("Target Type",_targetTypes.IndexOf(_targetType.enumNames[_targetType.enumValueIndex]),_targetTypes.ToArray());

			if(targetTypeIndex>=0)
				_targetType.enumValueIndex=IAPEditor.IAPTypeIndex[_targetTypes[targetTypeIndex]];

			// Select list
			if(_targetType.enumValueIndex==0){
				if(settings.currencyList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,settings.currencyList.IndexOf(_uid.stringValue),_currencyListArray);
					if(index<0) index=0;
					_uid.stringValue = settings.currencyList[index];					
				} else {
					EditorGUILayout.Popup(0,_currencyListArray);
				}
			} else if(_targetType.enumValueIndex==1){
				if(settings.inventoryList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,settings.inventoryList.IndexOf(_uid.stringValue),_inventoryListArray);
					if(index<0) index=0;
					_uid.stringValue = settings.inventoryList[index];					
				} else {
					EditorGUILayout.Popup(0,_inventoryListArray);
				}
			}
				
			EditorGUILayout.PropertyField(_amount,new GUIContent("Consume Amount"));		
			EditorGUILayout.PropertyField(_useConfirmDialog);

			EditorGUILayout.Space();
			DrawSectionHeader("Events");

			EditorGUILayout.PropertyField(_OnConsumeSuccess);
			EditorGUILayout.PropertyField(_OnConsumeFail);
		}
			
	}

}