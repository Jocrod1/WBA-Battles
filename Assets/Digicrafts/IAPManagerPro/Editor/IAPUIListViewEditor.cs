using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using Digicrafts.IAP.Pro.UI;

namespace Digicrafts.IAP.Pro.Editor
{	

	[CanEditMultipleObjects]
	[CustomEditor(typeof(IAPListView))]
	public class IAPUIListViewEditor : IAPEditor {


		private SerializedProperty itemTemplate;
		private SerializedProperty searchTag;
		private SerializedProperty layout;
		private SerializedProperty itemType;
		// LayoutGroup
		private SerializedProperty padding;
		// VerticalLayoutGroup/HorizontalLayoutGroup
		private SerializedProperty childForceExpandHeight;
		private SerializedProperty childForceExpandWidth;
		private SerializedProperty spacingSingle;
		// Grid Layout
		private SerializedProperty spacing;
		private SerializedProperty cellSize;
		private SerializedProperty startCorner;
		private SerializedProperty startAxis;
		private SerializedProperty contraint;
		private SerializedProperty constraintCount;
		// Events
		private SerializedProperty OnGameLevelSelect;

		private string[] _tagListArray;
		private string[] _gameListArray;

		override protected void InitEditor()
		{						
			base.InitEditor();
			title="IAP List View";

			itemTemplate=serializedObject.FindProperty("itemTemplate");
			searchTag=serializedObject.FindProperty("searchTag");
			layout=serializedObject.FindProperty("layout");
			itemType=serializedObject.FindProperty("itemType");

			padding=serializedObject.FindProperty("padding");
			childForceExpandHeight=serializedObject.FindProperty("childForceExpandHeight");
			childForceExpandWidth=serializedObject.FindProperty("childForceExpandWidth");
			spacingSingle=serializedObject.FindProperty("spacingSingle");
			//
			spacing=serializedObject.FindProperty("spacing");
			cellSize=serializedObject.FindProperty("cellSize");
			startCorner=serializedObject.FindProperty("startCorner");
			startAxis=serializedObject.FindProperty("startAxis");
			contraint=serializedObject.FindProperty("contraint");
			constraintCount=serializedObject.FindProperty("constraintCount");

			OnGameLevelSelect=serializedObject.FindProperty("OnGameLevelSelect");

			if(settings!=null){

				if(settings.tagList!=null)
					_tagListArray=settings.tagList.ToArray();

				if(settings.gameList!=null&&settings.gameList.Count>0){
					_gameListArray = settings.gameList.ToArray();
				} else {
					_gameListArray = new string[]{"none"};							
				}
			}

		}

		override protected void DrawBody()
		{

			EditorGUILayout.PropertyField(itemType);
			if(itemType.enumValueIndex==3){
				if(settings.gameList.Count>0){					
					int index = EditorGUILayout.Popup(_identifyString.text,settings.gameList.IndexOf(_uid.stringValue),_gameListArray);
					if(index<0) index=0;
					_uid.stringValue = IAPManagerProEditor.settings.gameList[index];					
				} else {
					EditorGUILayout.Popup(0,_gameListArray);
				}
			} else {
				searchTag.intValue=EditorGUILayout.MaskField("Search Tag",searchTag.intValue,_tagListArray);
			}
			EditorGUILayout.PropertyField(itemTemplate);

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Layout",IAPEditorStyles.sectionTitle);
			GUILayout.Box(GUIContent.none,GUILayout.ExpandWidth(true),GUILayout.Height(1));
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(layout);

			if(layout.enumValueIndex==0){
				EditorGUILayout.PropertyField(spacing);
				EditorGUILayout.PropertyField(cellSize);
				EditorGUILayout.PropertyField(padding,true);
				EditorGUILayout.PropertyField(startCorner);
				EditorGUILayout.PropertyField(startAxis);
				EditorGUILayout.PropertyField(contraint);
				if(contraint.enumValueIndex==0){
					
				} else {
					EditorGUI.indentLevel=1;
					EditorGUILayout.PropertyField(constraintCount);
					EditorGUI.indentLevel=0;
				}
			} else {
				EditorGUILayout.PropertyField(spacingSingle);
				EditorGUILayout.PropertyField(padding,true);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Child Force Expand");
				childForceExpandWidth.boolValue=EditorGUILayout.ToggleLeft("Width",childForceExpandWidth.boolValue,GUILayout.Width(50));
				childForceExpandHeight.boolValue=EditorGUILayout.ToggleLeft("Height",childForceExpandHeight.boolValue,GUILayout.Width(50));
				EditorGUILayout.EndHorizontal();
			}			
				
			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(OnGameLevelSelect);

		}
			
	}

}