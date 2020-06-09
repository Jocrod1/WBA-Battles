using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Digicrafts.IAP.Pro.UI;

namespace Digicrafts.IAP.Pro.Editor
{	

	[CanEditMultipleObjects]
	[CustomEditor(typeof(IAPPropertyImage))]
	public class IAPUIPropertyImageEditor : IAPEditor {

		private SerializedProperty _property;
		private SerializedProperty _icons;

		override protected void InitEditor()
		{			
			base.InitEditor();
			this.title="UI Property Image";

			_property=serializedObject.FindProperty("property");
			_icons=serializedObject.FindProperty("icons");
		}
			
		override protected void DrawBody()
		{

			EditorGUILayout.Space();
			EditorGUILayout.PropertyField(_property);
			EditorGUILayout.PropertyField(_icons,true);

			EditorGUILayout.Space();

		}
			
	}

}