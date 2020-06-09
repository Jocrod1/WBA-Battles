using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

namespace Digicrafts.IAP.Pro.Editor
{	
	public class IAPEditorStyles {

//		public static GUISkin editorSkin;

		public static GUIStyle sectionTitle;

		public static GUIStyle tableBody;
		public static GUIStyle tableHeader;

		public static GUIStyle logo;
		public static GUIStyle logoTitle;

		public static GUIStyle toolbarButton;
		public static GUIStyle listFoldout;

		public static GUIStyle addButton;
		public static GUIStyle table;

		public static GUIStyle foldoutBackground;

		public static Texture2D logoImage;
		public static Texture2D deleteButtonIcon;
		public static Texture2D duplicateButtonIcon;
		public static Texture2D upButtonIcon;
		public static Texture2D downButtonIcon;

		// Tab Icon
		public static Texture2D tabIconSettings;
		public static Texture2D tabIconTags;
		public static Texture2D tabIconEvents;


		private static Texture2D MakeTex(int width, int height, Color col)
		{
			Color[] pix = new Color[width*height];

			for(int i = 0; i < pix.Length; i++)
				pix[i] = col;

			Texture2D result = new Texture2D(width, height);
			result.SetPixels(pix);
			result.Apply();

			return result;
		}

		public static void initImages(string path){

			if(logoImage==null){
				if(EditorGUIUtility.isProSkin)
					logoImage=AssetDatabase.LoadAssetAtPath<Texture2D>(path+"logo_pro.png");
				else
					logoImage=AssetDatabase.LoadAssetAtPath<Texture2D>(path+"logo.png");
				deleteButtonIcon=AssetDatabase.LoadAssetAtPath<Texture2D>(path+"delete-small.png");
				duplicateButtonIcon=AssetDatabase.LoadAssetAtPath<Texture2D>(path+"duplicate-small.png");
				upButtonIcon=AssetDatabase.LoadAssetAtPath<Texture2D>(path+"arrow-up.png");
				downButtonIcon=AssetDatabase.LoadAssetAtPath<Texture2D>(path+"arrow-down.png");

			}
		}

		public static void init(){

			if(logo==null){

				// Logo Title
				logoTitle = new GUIStyle(GUI.skin.GetStyle("Label"));
				logoTitle.fontSize=20;

				//logo
				logo = new GUIStyle(GUI.skin.GetStyle("Label"));
				logo.alignment = TextAnchor.UpperRight;
				logo.stretchWidth=false;
				logo.stretchHeight=false;
				logo.normal.background=logoImage;


				toolbarButton = new GUIStyle(EditorStyles.toolbarButton);					
				toolbarButton.imagePosition=ImagePosition.ImageAbove;

				sectionTitle =  new GUIStyle(GUI.skin.GetStyle("Label"));
				sectionTitle.fontStyle=FontStyle.Bold;
				sectionTitle.fontSize=14;

				// Foldout background
				foldoutBackground = new GUIStyle(GUI.skin.GetStyle("Label"));
				foldoutBackground.margin=new RectOffset(22,5,5,5);
				foldoutBackground.padding=new RectOffset(8,5,8,8);
				if(EditorGUIUtility.isProSkin)
					foldoutBackground.normal.background=MakeTex(10,10,new Color(0.1f,0.10f,0.15f,0.95f));
				else
					foldoutBackground.normal.background=MakeTex(10,10,new Color(0.729f, 0.808f, 0.855f,1.0f));
				
				// Table Header
				tableHeader = "RL Header";					
				tableHeader.margin=new RectOffset(5,5,5,0);
			
				// Table Body
				tableBody = new GUIStyle(GUI.skin.box);
				GUIStyle s = "RL Background";
				tableBody.normal.background=s.normal.background;
				tableBody.padding=new RectOffset(1,1,1,1);
				tableBody.margin=new RectOffset(5,5,0,5);
				tableBody.border=new RectOffset(6,3,0,6);

				// 
				GUISkin editorSkin=EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
				listFoldout = new GUIStyle(GUI.skin.toggle);
				listFoldout.normal.background=listFoldout.focused.background=listFoldout.active.background=editorSkin.GetStyle("Foldout").normal.background;
				listFoldout.onFocused.background=listFoldout.onActive.background=listFoldout.onNormal.background=listFoldout.onHover.background=editorSkin.GetStyle("PaneOptions").normal.background;//IAPEditorStyles.editorSkin.GetStyle("OL Minus").normal.backgrou

				// addButton
				addButton=new GUIStyle(GUI.skin.button);
				if(EditorGUIUtility.isProSkin)
					addButton.normal.textColor=Color.yellow;
				else
					addButton.normal.textColor=Color.black;

				//table
				table = EditorStyles.helpBox;
				table.padding=new RectOffset(4,4,4,4);


			}

		}	
	}

}