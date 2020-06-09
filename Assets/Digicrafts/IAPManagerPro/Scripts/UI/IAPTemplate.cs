using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;

namespace Digicrafts.IAP.Pro.UI
{
	[DisallowMultipleComponent]
	public class IAPTemplate : IAPUI 
	{
		public bool useConfirmDialog = false;
		public int level = -1;

		// Use this for initialization
		void Start () {
			
			if(uid!=null){				

				// Purchase button action
				System.Action<GameObject> purchaseButtonCallback = null;
		
				//
				if(targetType==IAPType.Currency){
					
					IAPCurrency obj = IAPInventoryManager.GetCurrency(uid);

					if(obj !=null){
						UpdateTemplate(obj);						
						IAPInventoryManager.OnCurrencyUpdated+=handleCurrencyUpdated;
						purchaseButtonCallback=(GameObject go)=>{							
							handleButtonCallback(obj);
						};

					}						

				} else if(targetType==IAPType.Inventory){
					
					IAPInventory obj = IAPInventoryManager.GetInventory(uid);	

					if(obj !=null){

Debug.Log("UpdateTemplate " + obj.uid);
						UpdateTemplate(obj);

						IAPInventoryManager.OnInventoryUpdated+=handleInventoryUpdated;		
						// Debug.Log("obj.available: " + obj.available);			
						// Check if inventory available
						// if(obj.available==0){
						// 	IAPUIUtility.SetButtonActive(false,gameObject,"purchase_button");
						// } else {
							purchaseButtonCallback=(GameObject go)=>{
								// Debug.Log("213123");
								handleButtonCallback(obj);
							};
						// }					

					}							

				} else if(targetType==IAPType.Ability){
					
					IAPAbility obj = IAPInventoryManager.GetAbility(uid);			

					if(obj != null){

						UpdateTemplate(obj);

						IAPInventoryManager.OnAbilityUpdated+=handleAbilityUpdated;
						IAPAbility ability = (obj as IAPAbility);
						if(ability.level<ability.levels.Count-1){

							// Get the currency
							IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
							purchaseButtonCallback=(GameObject go)=>{									
								if(useConfirmDialog){
									if(currency!=null){
										// Construct confirm msg
										IAPAbilityLevel lv = ability.GetCurrentLevel();
										string msg = IAPInventoryManager.uiSettings.abilityConfirmString.Replace("%title%",obj.title);
										msg = msg.Replace("%description%",lv.description.ToString());
										msg = msg.Replace("%price%",lv.price.ToString());
										msg = msg.Replace("%currency_title%",currency.title);
										msg = msg.Replace("%currency_description%",currency.description);
										// Show confirm diaglog
										IAPInventoryManager.ShowConfirmDialog(msg,
											delegate(IAPDialog diag){
												ability.Upgrade();
											}
										);
									}
								} else {
									// Purchase the package
									ability.Upgrade();
								}
							};

						} else {
							// Disable the button
							IAPUIUtility.SetButtonEnabled(gameObject,false);
						}

					}
				} else if(targetType==IAPType.InAppPurchase){

					IAPPackage obj = IAPInventoryManager.GetPackage(uid);

					if(obj != null){
						
						// UpdateTemplate(obj);						

						// if((obj.productType==IAPProductType.NonConsumable || obj.productType==IAPProductType.Subscription) && obj.amount>0)
						// {
						// 	IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");

						// } else {

							if(obj.fetchFromStore)
							{
							// For Real Money IAP	

								IAPInventoryManager.OnIAPInitialized+=handleOnIAPInitialized;

								// Check if IAP initialized
								if(IAPManager.IsInitialized())
								{
									Debug.LogFormat("uid: {0} type: {1}  amount: {2}",uid,obj.productType,obj.amount);

									if((obj.productType==IAPProductType.NonConsumable || obj.productType==IAPProductType.Subscription) && obj.amount>0)
									{
										IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
									} else {
										purchaseButtonCallback=(GameObject go)=>{	
											IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
											obj.Purchase(handlePackageUpdated);
										};
									}

								} else {			
									IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
									IAPInventoryManager.InitIAPManager();
								}


							} else {
								// For Virtual Currency IAP

								purchaseButtonCallback=(GameObject go)=>{													

									if(useConfirmDialog){
										// Construct confirm msg
										IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
										if(currency!=null){
											string msg = IAPInventoryManager.uiSettings.iapConfirmString.Replace("%title%",obj.title);
											msg = msg.Replace("%description%",obj.description);
											msg = msg.Replace("%price%",obj.price.ToString());
											msg = msg.Replace("%currency_title%",currency.title);
											msg = msg.Replace("%currency_description%",currency.description);
											// Show confirm diaglog
											IAPInventoryManager.ShowConfirmDialog(msg,
												delegate(IAPDialog diag){											
													// Purchase the package
													obj.Purchase();											
												}
											);		
										}
									} else {
										// Purchase the package
										obj.Purchase();
									}

								};

							}
						// }

						UpdateTemplate(obj);
					}
						
				} else if(targetType==IAPType.GameLevel && level != -1){
					
					IAPGameLevel obj = IAPInventoryManager.GetGameLevel(uid);
					IAPInventoryManager.OnGameLevelUpdated+=handleGameLevelUpdated;

					if(obj != null){						
						UpdateGameLevelTemplate(obj,level);

						// IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
// Debug.LogFormat("obj {0}",level);												

						// purchaseButtonCallback=(GameObject go)=>{
						IAPUIUtility.AddButtonCallback(gameObject,(GameObject go)=>{

							IAPGameSubLevel subLevel = obj.levels[level];
							bool islocked =(obj.GetPropertyValue("locked",level)>0);
							// Debug.LogFormat("obj {0} {1} {2}", level, useConfirmDialog, obj.currency);
							
							// Check if price valid and
							if(subLevel.price>0&&islocked){
								if(useConfirmDialog){
									// Construct confirm msg
									IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
									if(currency!=null){
										string msg = IAPInventoryManager.uiSettings.iapConfirmString.Replace("%title%",obj.title);
										msg = msg.Replace("%description%",obj.description + " Level " + level.ToString());
										msg = msg.Replace("%price%",subLevel.price.ToString());
										msg = msg.Replace("%currency_title%",currency.title);
										msg = msg.Replace("%currency_description%",currency.description);
										// Show confirm diaglog
										IAPInventoryManager.ShowConfirmDialog(msg,
											delegate(IAPDialog diag){											
												// Check if enough currency
												if(currency != null && currency.Consume(subLevel.price)){									
													// obj.SetPropertyValue("locked",level,1);
													obj.UnlockLevel(level);

												}
											}
										);
									}
								} else {
									// Get the currency
									IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
									// Check if enough currency
									if(currency != null && currency.Consume(subLevel.price)){									
										obj.UnlockLevel(level);
									}
								}
							}

						},"self,select_button");

					}
				}								

				// Add the button callback to purchase_button				
				if(purchaseButtonCallback!=null)
					IAPUIUtility.AddButtonCallback(gameObject,purchaseButtonCallback,"purchase_button");				
					

			}				
		}

		void OnDestroy(){
			if(targetType==IAPType.Currency){				
				IAPInventoryManager.OnCurrencyUpdated-=handleCurrencyUpdated;
			} else if(targetType==IAPType.Inventory){				
				IAPInventoryManager.OnInventoryUpdated-=handleInventoryUpdated;
			} else if(targetType==IAPType.Ability){				
				IAPInventoryManager.OnAbilityUpdated-=handleAbilityUpdated;
			} else if(targetType==IAPType.GameLevel){				
				IAPInventoryManager.OnGameLevelUpdated-=handleGameLevelUpdated;
			} else if(targetType==IAPType.InAppPurchase){
				//
//				IAPInventoryManager.OnIAPProcessPurchase-=handleOnIAPProcessPurchase;
//				IAPInventoryManager.OnIAPPurchaseFailed-=handleOnIAPPurchaseFailed;					
			}
		}

		// Button callback

		private void handleButtonCallback(IAPObject obj)
		{								
			if(useConfirmDialog){
				// Construct confirm msg
				IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
				if(currency!=null){
					string msg = IAPInventoryManager.uiSettings.iapConfirmString.Replace("%title%",obj.title);
					msg = msg.Replace("%description%",obj.description);
					msg = msg.Replace("%price%",obj.price.ToString());
					msg = msg.Replace("%currency_title%",currency.title);
					msg = msg.Replace("%currency_description%",currency.description);
					// Show confirm diaglog
					IAPInventoryManager.ShowConfirmDialog(msg,
						delegate(IAPDialog diag){	
							// Debug.Log("available: " + obj.available);										
							// Check if enough currency
							if((obj.available != 0 )&& currency != null && currency.Consume(obj.price)){									
								obj.Refill(1);								
								if(obj is IAPInventory) obj.Unlock();
								UpdateTemplate(obj);
							}
						}
					);
				}
			} else {
				// Get the currency
				IAPCurrency currency = IAPInventoryManager.GetCurrency(obj.currency);
				// Check if enough currency
				if(currency != null && currency.Consume(obj.price)){									
					obj.Refill(1);
					if(obj is IAPInventory) obj.Unlock();
					UpdateTemplate(obj);
				}
			}
		}


		// IAPObject callback

		private void handleCurrencyUpdated(IAPCurrency currency)
		{
			if(currency.uid==this.uid)
			{
				UpdateTemplate(currency);
			}				
		}

		private void handleAbilityUpdated(IAPAbility ability)
		{
			if(ability.uid==this.uid)
			{
				UpdateTemplate(ability);
			}				
		}

		private void handleInventoryUpdated(IAPInventory inventory)
		{
			if(inventory.uid==this.uid)
			{
				Debug.Log("handleInventoryUpdated: " + inventory.uid + " lock: " + inventory.isLocked());
				UpdateTemplate(inventory);
			}				
		}

		private void handleGameLevelUpdated(IAPGameLevel obj)
		{
			if(obj.uid==this.uid)
			{
				UpdateGameLevelTemplate(obj,level);
			}				
		}

		private void handleOnIAPInitialized (Dictionary<string, IAPProduct> products)
		{			
			IAPInventoryManager.OnIAPInitialized-=handleOnIAPInitialized;
			if (products != null) {								
				if(products.ContainsKey(uid)){
					IAPPackage package = IAPInventoryManager.GetPackage(uid);
					// IAPUIUtility.AddButtonCallback(gameObject,(GameObject go)=>{
					// 	IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
					// 	obj.Purchase(handlePackageUpdated);
					// },"purchase_button");
					handlePackageUpdated(package);
				}
			}
		}				

		private void handlePackageUpdated(IAPPackage package)
		{			
			if(package.product==null && package.product.id==this.uid){

				// Update template				
//				Debug.LogFormat("handlePackageUpdated {0} type {1} amount {2} hasReceipt {3} {4}",uid,package.productType,package.amount,package.product.hasReceipt,package.product.rawProduct.hasReceipt);
				// To enable/disable button
				if(package.productType==IAPProductType.NonConsumable && package.product.hasReceipt) {
					if(package.amount<=0)package.Refill(1);// Add 1 if no
					IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
				} else if(package.productType==IAPProductType.Subscription && package.product.hasReceipt) {
					if(package.amount<=0)package.Refill(1);// Add 1 if no
					package.Unlock();
					IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
				} else if((package.productType==IAPProductType.NonConsumable && package.amount>0)) {
					IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
				} else {					
					IAPUIUtility.SetButtonEnabled(gameObject,true,"purchase_button");
					IAPUIUtility.AddButtonCallback(gameObject,(GameObject go)=>{
						IAPUIUtility.SetButtonEnabled(gameObject,false,"purchase_button");
						package.Purchase(handlePackageUpdated);
					},"purchase_button");
				}

				UpdateTemplate(package);
			}
		}	

		private void handleConfirmButtonPressed(IAPDialog dialog)
		{
			dialog.OnConfirmButtonPressed-=handleConfirmButtonPressed;
		}

		private void handleCancelButtonPressed(IAPDialog dialog)
		{
			dialog.OnCancelButtonPressed-=handleCancelButtonPressed;
		}

	}

}