using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

using Digicrafts.IAP.Pro;
using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;

namespace Digicrafts.IAP.Pro.UI
{	

	[DisallowMultipleComponent]
	public class IAPUI : MonoBehaviour {

		public string settingKeyDefault;
		public IAPType targetType = IAPType.InAppPurchase;
		public string uid;

		protected void UpdateTemplate(IAPObject obj)
		{

			if(obj != null){				

				// Update images
				Image[] imgs = gameObject.GetComponentsInChildren<Image>();
				foreach(Image img in imgs){
					if(img.name=="icon" && obj.icon!=null){
						img.sprite=obj.icon;
					} else if(obj is IAPInventory && img.name=="lock_icon"){
						IAPInventory obji = (IAPInventory)obj;
						img.gameObject.SetActive(obji.isLocked());
					} else if(obj is IAPInventory && img.name=="unlock_icon"){
						IAPInventory obji = (IAPInventory)obj;
						img.gameObject.SetActive(!obji.isLocked());
					} else if(img.name=="currency_icon"){												
						IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
						if(currency!=null&&currency.icon!=null){
							img.sprite=currency.icon;
						}
					} else if(img.name=="level"){
						if(targetType==IAPType.Ability){
							IAPAbilityLevel lv = (obj as IAPAbility).GetCurrentLevel();
							if(lv!=null&&lv.icon!=null){
								img.sprite=lv.icon;
							}
						}
					}
				}

				// For inventory
				if(obj is IAPInventory){				

					IAPInventory inventory = obj as IAPInventory;
					bool locked = inventory.isLocked();
					
					Text[] inventoryText = gameObject.GetComponentsInChildren<Text>();
					Debug.Log("UPDATE Tempate: " + uid + " obj: " + inventoryText.Length);
					foreach(Text txt in inventoryText){
						string name = txt.name.ToLower();
						Debug.Log("UPDATE Text: " + name + " locked: " + locked);
						if(name.StartsWith("locked")){
							txt.enabled=locked;
						} else if(name.StartsWith("unlocked")){
							txt.enabled=!locked;
						}
					}

					Button[] inventoryBtns = gameObject.GetComponentsInChildren<Button>();
					foreach(Button btn in inventoryBtns){
						string name = btn.name.ToLower();
						Debug.Log("UPDATE button: " + name + " locked: " + locked);
						if(name.StartsWith("locked")){
							btn.enabled=locked;
						} else if(name.StartsWith("unlocked")){
							btn.enabled=!locked;
						}
					}

					if(inventory.isLocked())	
					{
						IAPUIUtility.SetButtonActive(true,gameObject,"locked_button");
						IAPUIUtility.SetButtonActive(false,gameObject,"unlocked_button");
					} else {
						IAPUIUtility.SetButtonActive(false,gameObject,"locked_button");
						IAPUIUtility.SetButtonActive(true,gameObject,"unlocked_button");
					}
				}

				// Update text
				Text[] txts = gameObject.GetComponentsInChildren<Text>();
				// InAppPurchase
				if(targetType==IAPType.InAppPurchase){					
					IAPPackage package = (obj as IAPPackage);
					foreach(Text txt in txts){
						string name = txt.name.ToLower();
						if(name.StartsWith("content_")){								
							string[] ss = name.Split('_');
							if(ss.Length==3){
								IAPPackageContent cc = package.GetContent(ss[1]);
								if(cc!=null){									
									if(ss[2]=="amount")
										txt.text=cc.amount.ToString();
									else
										UpdateTextWithObject(txt,cc.obj,ss[2]);
								}
							}													
						} else if(package.fetchFromStore) {
							UpdateTextWithPackage(txt,package);
						} else {
							UpdateTextWithObject(txt,obj,name);
						}
					}
				// Ability
				} else if(targetType==IAPType.Ability){				
					IAPAbility ability = (obj as IAPAbility);
					foreach(Text txt in txts){
						string name = txt.name.ToLower();
						if(name=="price"){		
							if(ability.isLocked()&&ability.lockedString!=null&&ability.lockedString!=""){
								txt.text=ability.lockedString;
							} else {
								IAPAbilityLevel lv = ability.GetCurrentLevel();
								if(lv!=null){

									if(ability.level==ability.levels.Count-1&&ability.maxString!=null&&ability.maxString!="")
										txt.text=ability.maxString;
									else
										txt.text=lv.price.ToString();
								}

							}										
						} else {
//							Debug.Log("text " + txt + " name: " + name);
							UpdateTextWithObject(txt,obj,name);
						}
					}
				} else {
					foreach(Text txt in txts)
						UpdateTextWithObject(txt,obj,txt.name);
				}
			}
		}

		protected void UpdateGameLevelTemplate(IAPGameLevel obj, int subLevelIndex)
		{
			
			if(obj != null && obj.levels !=null && subLevelIndex<obj.levels.Count){				

				IAPGameSubLevel lv = obj.levels[subLevelIndex];
				// Update images
				Image[] imgs = gameObject.GetComponentsInChildren<Image>();
				foreach(Image img in imgs){					
					if(img.gameObject==gameObject || img.name=="icon"){
						// check if level locked
						bool islocked = false;
						if(obj.isLocked()){
							islocked=(obj.GetPropertyValue("locked",subLevelIndex)>0);
						}
						if(islocked){
							if(obj.lockedIcon!=null){
								img.sprite=obj.lockedIcon;
							}
						} else {
							if(obj.icon!=null){
								img.sprite=obj.icon;
							}
						}
					} else if(img.name=="currency_icon"){
						IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
						if(currency!=null&&currency.icon!=null){
							img.sprite=currency.icon;
						}
					} else if(img.name=="level"){												
						if(lv!=null&&lv.icon!=null){
							img.sprite=lv.icon;
						}
					}
				}

				// Update Property Image
				IAPPropertyImage[] propImgs = gameObject.GetComponentsInChildren<IAPPropertyImage>();
				foreach(IAPPropertyImage propImg in propImgs){					
					propImg.level=subLevelIndex;
					propImg.obj=obj;
					propImg.UpdateImage();
				}

				// Update text
				Text[] txts = gameObject.GetComponentsInChildren<Text>();
				foreach(Text txt in txts){
					string name = txt.name.ToLower();
					if(name=="level"){	
						txt.text=(subLevelIndex+1).ToString();
					} else {
						UpdateTextWithObject(txt,obj,name);
					}
				}

			}
		}

		// Helpers

		private void UpdateTextWithObject(Text txt, IAPObject obj, string name)
		{			
			switch(name.ToLower()){
			case "uid":
				txt.text=obj.uid;
				break;
			case "title":
				txt.text=obj.title;
				break;
			case "description":
				txt.text=obj.description;
				break;
			case "price":
				txt.text=obj.price.ToString();					
				break;
			case "currency":
				txt.text=obj.currency;
				break;
			case "amount":
				txt.text=obj.amount.ToString();
				break;
			case "locked_text":
				if(obj is IAPInventory){
					txt.gameObject.SetActive((obj as IAPInventory).isLocked());
				}
				break;
			case "unlock_text":
				if(obj is IAPInventory){
					txt.gameObject.SetActive(!(obj as IAPInventory).isLocked());
				}
				break;
			}
		}

		private void UpdateTextWithPackage(Text txt, IAPPackage package)
		{
			Debug.Log("package: " + package.uid + " name: " + txt.name + " package: " + package);
			// if(string.IsNullOrEmpty(txt)) return;
//			try{
				switch(txt.name.ToLower()){
				case "price":		
					txt.text=package.priceString;				
					break;
				case "currency":				
					txt.text=package.isoCurrencyCode;
					break;
				case "uid":
					txt.text=package.uid;
					break;
				case "title":				
					txt.text=package.title;
					break;
				case "description":						
					txt.text=package.description;
					break;			
				case "amount":				
					break;
				}
//			} catch(Exception e){
//				Debug.LogWarning(e);
//			}
		}			

		protected string GetText(IAPObject obj, IAPTextType type)
		{
//			Debug.Log("name " + obj.uid + " type " + type );
			if((obj is IAPPackage) && (obj as IAPPackage).fetchFromStore){
				IAPPackage p = obj as IAPPackage;
				if(p.product!=null){
					switch(type){
					case IAPTextType.uid:
						return p.product.id;
					case IAPTextType.currency:					
						return p.product.isoCurrencyCode;
					case IAPTextType.price:									
						return p.product.priceString;
					case IAPTextType.title:
						return string.IsNullOrEmpty(p.title)?p.product.title:p.title;						
					case IAPTextType.description:				
						return string.IsNullOrEmpty(p.description)?p.product.description:p.description;
					}
				}
			} else {
				switch(type){
				case IAPTextType.uid:
					return obj.uid;
				case IAPTextType.amount:
					return obj.amount.ToString();
				case IAPTextType.currency:										
					return obj.currency;
				case IAPTextType.price:									
					return obj.price.ToString();
				case IAPTextType.title:
					return obj.title;
				case IAPTextType.description:				
					return "";
				}
			}
			return "error";
		}
			

	}

	/// <summary>
	/// IAPUI utility.
	/// </summary>
	public class IAPUIUtility {

		/// <summary>
		/// Sets the label text.
		/// </summary>
		/// <param name="label">Label.</param>
		/// <param name="text">Text.</param>
		public static void UpdateLabelText(GameObject label, string text){
			if(label != null){
				Text txt = label.GetComponentInChildren<Text>();
				if(txt!=null){				
					txt.text = text;
				} 
				#if ENABLE_NGUI
				else if(label.GetComponent<UILabel>() != null){
					label.GetComponent<UILabel>().text = text;
				}
				#endif
			}
		}					

		/// <summary>
		/// Sets the button enabled.
		/// </summary>
		/// <param name="btn">Button.</param>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		/// <param name="targetName">Target name.</param>
		public static void SetButtonEnabled(GameObject btn, bool isEnabled, string targetName = null){
			
			if(btn != null){				
				Button[] btns = btn.GetComponentsInChildren<Button>();
				foreach(Button button in btns){			
					if(targetName==null || targetName.Contains(button.name) || (button.gameObject==btn&&targetName.Contains("self"))){
						button.interactable=isEnabled;
					}				
				}
				#if ENABLE_NGUI
				UIButton[] nbtns = btn.GetComponentsInChildren<UIButton>();
				foreach(UIButton button in nbtns){			
					if(targetName==null || targetName.Contains(button.name) || (button.gameObject==btn&&targetName.Contains("self"))){
					button.isEnabled=isEnabled;
					}
				}
				#endif
			}
		}

		/// <summary>
		/// Adds the button callback.
		/// </summary>
		/// <param name="btn">Button.</param>
		/// <param name="callback">Callback.</param>
		/// <param name="targetName">Target name.</param>
		public static void AddButtonCallback(GameObject btn, Action<GameObject> callback, string targetName = null){
			if(btn != null){
				Button[] btns = btn.GetComponentsInChildren<Button>();
				foreach(Button button in btns){
					if(targetName==null || targetName.Contains(button.name) || (button.gameObject==btn&&targetName.Contains("self"))){						
						GameObject target = button.gameObject;
						button.onClick.AddListener(()=>callback(target));
					}
				}
				#if ENABLE_NGUI
//				UIButton[] nbtns = btn.GetComponentsInChildren<UIButton>();
//				foreach(UIButton button in nbtns){			
//					// Add event to the button
//					EventDelegate del = new EventDelegate(this,"handleBuyButton");
//					del.parameters[0] = new EventDelegate.Parameter(product,"id");
//					del.parameters[0].value = product.id;
//					EventDelegate.Set(button.onClick,del);									
//				}
				#endif

			}
		}

		
		public static void SetButtonActive(bool active, GameObject btn, string targetName = null){
			if(btn != null){
				Button[] btns = btn.GetComponentsInChildren<Button>();
				foreach(Button button in btns){
					if(targetName==null || targetName.Contains(button.name) || (button.gameObject==btn&&targetName.Contains("self"))){						
						GameObject target = button.gameObject;
						target.SetActive(active);
					}
				}
				#if ENABLE_NGUI
//				UIButton[] nbtns = btn.GetComponentsInChildren<UIButton>();
//				foreach(UIButton button in nbtns){			
//					// Add event to the button
//					EventDelegate del = new EventDelegate(this,"handleBuyButton");
//					del.parameters[0] = new EventDelegate.Parameter(product,"id");
//					del.parameters[0].value = product.id;
//					EventDelegate.Set(button.onClick,del);									
//				}
				#endif

			}
		}

	}		

}