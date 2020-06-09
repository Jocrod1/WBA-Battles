using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Digicrafts.IAP.Pro.UI;

namespace Digicrafts.IAP.Pro.Editor
{	

	[CanEditMultipleObjects]
	[CustomEditor(typeof(IAPTemplate))]
	public class IAPUITemplateEditor : IAPEditor {

		override protected void InitEditor()
		{			
			base.InitEditor();
			this.title="UI Template";
		}
			
		override protected void DrawBody()
		{

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(_targetType);

			// Select list
			if(_targetType.enumValueIndex==0){
				string[] currencyListArray;
				if(IAPManagerProEditor.settings.currencyList!=null && IAPManagerProEditor.settings.currencyList.Count>0)
					currencyListArray = IAPManagerProEditor.settings.currencyList.ToArray();
				else
					currencyListArray = new string[]{"none"};				
				if(IAPManagerProEditor.settings.currencyList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,IAPManagerProEditor.settings.currencyList.IndexOf(_uid.stringValue),currencyListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.currencyList[index];					
				} else {
					EditorGUILayout.Popup(_identifyString.text,0,currencyListArray);
				}
			} else if(_targetType.enumValueIndex==1){
				string[] inventoryListArray;
				if(IAPManagerProEditor.settings.inventoryList!=null && IAPManagerProEditor.settings.inventoryList.Count>0)
					inventoryListArray = IAPManagerProEditor.settings.inventoryList.ToArray();
				else
					inventoryListArray = new string[]{"none"};				
				if(IAPManagerProEditor.settings.inventoryList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,IAPManagerProEditor.settings.inventoryList.IndexOf(_uid.stringValue),inventoryListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.inventoryList[index];					
				} else {
					EditorGUILayout.Popup(_identifyString.text,0,inventoryListArray);
				}
			} else if(_targetType.enumValueIndex==2){
				string[] abilityListArray;
				if(IAPManagerProEditor.settings.abilityList!=null && IAPManagerProEditor.settings.abilityList.Count>0)
					abilityListArray = IAPManagerProEditor.settings.abilityList.ToArray();
				else
					abilityListArray = new string[]{"none"};				
				if(IAPManagerProEditor.settings.abilityList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,IAPManagerProEditor.settings.abilityList.IndexOf(_uid.stringValue),abilityListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.abilityList[index];					
				} else {
					EditorGUILayout.Popup(_identifyString.text,0,abilityListArray);
				}
			} else if(_targetType.enumValueIndex==3){


			} else if(_targetType.enumValueIndex==4){
				string[] packageListArray;
				if(IAPManagerProEditor.settings.packageList!=null && IAPManagerProEditor.settings.packageList.Count>0)
					packageListArray = IAPManagerProEditor.settings.packageList.ToArray();
				else
					packageListArray = new string[]{"none"};							
				if(IAPManagerProEditor.settings.packageList.Count>0){
					int index = EditorGUILayout.Popup(_identifyString.text,IAPManagerProEditor.settings.packageList.IndexOf(_uid.stringValue),packageListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.packageList[index];					
				} else {
					EditorGUILayout.Popup(_identifyString.text,0,packageListArray);
				}
			}


//			EditorGUILayout.EndVertical();


			EditorGUILayout.Space();

		}
			
	}

}