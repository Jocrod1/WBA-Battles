using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;
using Digicrafts.IAP.Pro.Events;

namespace Digicrafts.IAP.Pro.UI
{		

	[DisallowMultipleComponent]
	public class IAPListView : MonoBehaviour 
	{
		public enum Layout
		{
			Grid, Vertical, Horizontal
		}

		// Events
		public GameLevelSelectEvent OnGameLevelSelect;

		//
		public string settingKeyDefault;
		public IAPType itemType = IAPType.Inventory;
		public string uid;

		// IAPInventoryListView
		public GameObject itemTemplate;
		public int searchTag;
		public Layout layout = Layout.Grid;

		// LayoutGroup
		public RectOffset padding;
		// VerticalLayoutGroup/HorizontalLayoutGroup
		public bool childForceExpandHeight=true;
		public bool childForceExpandWidth=true;
		public float spacingSingle = 0;
		// Grid Layout
		public Vector2 spacing = new Vector2(0,0);
		public Vector2 cellSize = new Vector2(100,100);
		public GridLayoutGroup.Corner startCorner = GridLayoutGroup.Corner.UpperLeft;
		public GridLayoutGroup.Axis startAxis = GridLayoutGroup.Axis.Horizontal;
		public GridLayoutGroup.Constraint contraint = GridLayoutGroup.Constraint.Flexible;
		public int constraintCount = 2;
		// Template
		public bool useConfirmDialog = true;

		// Private

		private GameObject _listView;

		/// <summary>
		/// Dos the layout.
		/// </summary>
		/// <param name="count">Count.</param>
		protected void DoLayout(int count)
		{
			RectTransform _thisTransform=GetComponent<RectTransform>();	
			RectTransform _contentTransform=GetComponent<ScrollRect>().content;
			RectTransform _viewportTransform=GetComponent<ScrollRect>().viewport;
			_listView = gameObject.GetComponent<ScrollRect>().content.Find("ListView").gameObject;
			RectTransform _listViewTransform = _listView.GetComponent<RectTransform>();

			float x = _thisTransform.sizeDelta.x;
			float y = _thisTransform.sizeDelta.y;

			// Change layout type
			if(layout==Layout.Horizontal){
				HorizontalLayoutGroup layoutGroup = _listView.AddComponent<HorizontalLayoutGroup>();
				layoutGroup.padding=padding;
				layoutGroup.spacing=spacingSingle;
				layoutGroup.childForceExpandWidth=childForceExpandWidth;
				layoutGroup.childForceExpandHeight=childForceExpandHeight;
			} else if(layout==Layout.Vertical) {
				VerticalLayoutGroup layoutGroup = _listView.AddComponent<VerticalLayoutGroup>();
				layoutGroup.padding=padding;
				layoutGroup.spacing=spacingSingle;
				layoutGroup.childForceExpandWidth=childForceExpandWidth;
				layoutGroup.childForceExpandHeight=childForceExpandHeight;
			} else {
				GridLayoutGroup layoutGroup = _listView.AddComponent<GridLayoutGroup>();
				layoutGroup.padding=padding;
				layoutGroup.spacing=spacing;
				layoutGroup.cellSize=cellSize;
				layoutGroup.startCorner=startCorner;
				layoutGroup.startAxis=startAxis;
				layoutGroup.constraint=contraint;
				layoutGroup.constraintCount=constraintCount;

				// calculate size
				if(contraint==GridLayoutGroup.Constraint.FixedColumnCount){
					x=(cellSize.x+spacing.x)*constraintCount - spacing.x + padding.horizontal;
					y=(cellSize.y+spacing.y)*Mathf.Ceil((float)count/constraintCount) - spacing.y + padding.vertical;
				} else if(contraint==GridLayoutGroup.Constraint.FixedRowCount){							
					x=(cellSize.x+spacing.x)*Mathf.Ceil((float)count/constraintCount) - spacing.x + padding.horizontal;
					y=(cellSize.y+spacing.y)*constraintCount - spacing.y + padding.vertical;
				}
			}

			if(_viewportTransform!=null){
				_viewportTransform.sizeDelta=new Vector2(_thisTransform.sizeDelta.x,_thisTransform.sizeDelta.y);
			}

			_contentTransform.sizeDelta=_listViewTransform.sizeDelta=new Vector2(x,y);
		}

		// Use this for initialization
		void Start () 
		{
			// hide the background
			Image background = gameObject.GetComponent<Image>();
			if(background!=null) background.enabled=false;

			// Add item to viewport content
//			if(searchTag != null && searchTag != "")
//			if(searchTag != 0)
//			{								
				if(itemType==IAPType.Inventory)
				{
					List<IAPInventory> inventoryList = IAPInventoryManager.GetInventoryListByTags(searchTag);

					if(inventoryList!=null && itemTemplate!=null){		
						DoLayout(inventoryList.Count);					
						foreach(IAPInventory inventory in inventoryList){
							GameObject obj = Instantiate(itemTemplate);
							IAPTemplate btn = obj.GetComponent<IAPTemplate>();
							if(btn==null) btn=obj.AddComponent<IAPTemplate>();

							btn.targetType=IAPType.Inventory;
							btn.uid=inventory.uid;
							btn.useConfirmDialog=useConfirmDialog;
							obj.transform.SetParent(_listView.transform);
							obj.transform.localScale=new Vector3(1,1,1);

						}
					}

				} else if(itemType==IAPType.InAppPurchase){										
					List<IAPPackage> packageList = IAPInventoryManager.GetPackageListByTags(searchTag);

					if(packageList!=null && itemTemplate!=null){		
						DoLayout(packageList.Count);					
						foreach(IAPPackage package in packageList){
//							Debug.Log("package " + package.uid + " type: ");
							GameObject obj = Instantiate(itemTemplate);
							IAPTemplate btn = obj.GetComponent<IAPTemplate>();
							if(btn==null) btn=obj.AddComponent<IAPTemplate>();

							btn.targetType=IAPType.InAppPurchase;
							btn.uid=package.uid;
							btn.useConfirmDialog=useConfirmDialog;
							obj.transform.SetParent(_listView.transform);
							obj.transform.localScale=new Vector3(1,1,1);

						}
					}	
				} else if(itemType==IAPType.Ability){					

					List<IAPAbility> abilityList = IAPInventoryManager.GetAbilityListByTags(searchTag);
//					Debug.Log("abilityList: " + abilityList + " count : " + abilityList.Count);
					if(abilityList!=null && itemTemplate!=null){		

						DoLayout(abilityList.Count);					
						foreach(IAPAbility ability in abilityList){
							GameObject obj = Instantiate(itemTemplate);
							IAPTemplate btn = obj.GetComponent<IAPTemplate>();
							if(btn==null) btn=obj.AddComponent<IAPTemplate>();

							btn.targetType=IAPType.Ability;
							btn.uid=ability.uid;
							btn.useConfirmDialog=useConfirmDialog;
							obj.transform.SetParent(_listView.transform);
							obj.transform.localScale=new Vector3(1,1,1);

						}
					}	
				} else if(itemType==IAPType.GameLevel){										
					IAPGameLevel gameLevel = IAPInventoryManager.GetGameLevel(uid);
					Debug.Log("GameLevel " +  uid);
					if(gameLevel!=null && itemTemplate!=null){	
						Debug.Log("GameLevel " + gameLevel.levels.Count);	
						DoLayout(gameLevel.levels.Count);		
						for(int i=0;i<gameLevel.levels.Count;i++){
							GameObject obj = Instantiate(itemTemplate);
							IAPTemplate btn = obj.GetComponent<IAPTemplate>();
							if(btn==null) btn=obj.AddComponent<IAPTemplate>();

							btn.targetType=IAPType.GameLevel;
							btn.uid=gameLevel.uid;
							btn.level=i;
							btn.useConfirmDialog=useConfirmDialog;
							obj.transform.SetParent(_listView.transform);
							obj.transform.localScale=new Vector3(1,1,1);

							// Add Events
							IAPUIUtility.AddButtonCallback(obj,(GameObject go)=>{
								IAPGameSubLevel lv = gameLevel.levels[btn.level];
								bool islocked = false;
								islocked=(gameLevel.GetPropertyValue("locked",btn.level)>0);
								Debug.LogFormat("Lock {0}",islocked);
								// if(gameLevel.isLocked()){
									islocked=(gameLevel.GetPropertyValue("locked",btn.level)>0);
								// }
								lv.locked=islocked;
								// if(islocked&&lv.price>0){
									
								// } else {
									if(OnGameLevelSelect!=null) OnGameLevelSelect.Invoke(gameLevel,btn.level);
								// }
							},"self,select_button");

						}
					}


				} 
//			}
		}

	}

}