using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

using Digicrafts.IAP.Pro;
using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;


namespace Digicrafts.IAP.Pro.UI
{
	/// <summary>
	/// IAP text type.
	/// </summary>
	public enum IAPTextType
	{
		uid,
		amount,
		currency,
		price,
		priceWithCurrency,
		title,
		description
	}	


	/// <summary>
	/// IAP text.
	/// </summary>
	[DisallowMultipleComponent]
	public class IAPText : IAPUI {

		// Animated number
		public bool animated = true;
		public float animationTime = 0.5f;

		public string defaultText="";
		public IAPTextType textType = IAPTextType.uid;

		// private
		private int _lastNumber=0;

		// Use this for initialization
		void Start () {			

			if(targetType==IAPType.InAppPurchase){
				IAPPackage obj = IAPInventoryManager.GetPackage(uid);
				if(obj != null){					
					handlePackageUpdated(obj);
					IAPInventoryManager.OnIAPInitialized+=handleOnIAPInitialized;
				}
			} else if(targetType==IAPType.Currency){
				IAPCurrency obj = IAPInventoryManager.GetCurrency(uid);
				if(obj != null){
					int result=0;
					if(textType==IAPTextType.price)
						result=obj.price;
					else if(textType==IAPTextType.amount)
						result=obj.amount;	
					_lastNumber=result;
					handleCurrencyUpdated(obj);
					IAPInventoryManager.OnCurrencyUpdated+=handleCurrencyUpdated;
				}
			} else if(targetType==IAPType.Inventory){
				IAPInventory obj = IAPInventoryManager.GetInventory(uid);
				if(obj != null){
					handleInventoryUpdated(obj);
					IAPInventoryManager.OnInventoryUpdated+=handleInventoryUpdated;
				}
			} else if(targetType==IAPType.Ability){
				IAPAbility obj = IAPInventoryManager.GetAbility(uid);
				if(obj != null){
					handleAbilityUpdated(obj);
					IAPInventoryManager.OnAbilityUpdated+=handleAbilityUpdated;
				}
			} else if(targetType==IAPType.GameLevel){
				IAPGameLevel obj = IAPInventoryManager.GetGameLevel(uid);
				if(obj != null){
					UpdateGameLevelTemplate(obj,0);
				}
			}


		}			

		void OnDestroy(){
			if(targetType==IAPType.InAppPurchase){				
				IAPInventoryManager.OnIAPInitialized-=handleOnIAPInitialized;
			} else if(targetType==IAPType.Currency){				
				IAPInventoryManager.OnCurrencyUpdated-=handleCurrencyUpdated;
			} else if(targetType==IAPType.Inventory){
				IAPInventoryManager.OnInventoryUpdated-=handleInventoryUpdated;
			} else if(targetType==IAPType.Ability){
				IAPInventoryManager.OnAbilityUpdated-=handleAbilityUpdated;
			}
		}			

		private void handleOnIAPInitialized (Dictionary<string, IAPProduct> products)
		{
//			IAPInventoryManager.OnIAPInitialized-=handleOnIAPInitialized;
			if (products != null) {
				if(products.ContainsKey(this.uid)){
					IAPPackage obj = IAPInventoryManager.GetPackage(uid);
					handlePackageUpdated(obj);
				}
			}
		}

		private void handlePackageUpdated(IAPPackage package)
		{						
			if(package.uid==this.uid){
				IAPUIUtility.UpdateLabelText(gameObject,GetText(package,textType));
			}
		}

		private void handleCurrencyUpdated(IAPCurrency currency)
		{
            //Debug.Log("handleCurrencyUpdated: " + currency.uid + " value: " + currency.amount);
			if(currency.uid==this.uid)
			{
				if((textType==IAPTextType.price||textType==IAPTextType.amount)){

					int result=0;
					if(textType==IAPTextType.price)
						result=currency.price;
					else if(textType==IAPTextType.amount)
						result=currency.amount;	

					if(animated&&_lastNumber!=result){						
						startCountTo(result);
						_lastNumber=result;

					} else {
						IAPUIUtility.UpdateLabelText(gameObject,GetText(currency,textType));
					}

				} else {
					IAPUIUtility.UpdateLabelText(gameObject,GetText(currency,textType));
				}
			}				
		}

		private void handleInventoryUpdated(IAPInventory inventory)
		{
			if(inventory.uid==this.uid)
				IAPUIUtility.UpdateLabelText(gameObject,GetText(inventory,textType));
		}

		private void handleAbilityUpdated(IAPAbility ability)
		{
			if(ability.uid==this.uid)
				IAPUIUtility.UpdateLabelText(gameObject,GetText(ability,textType));

		}

		// Helper for animated text

		private void startCountTo (int result) 
		{		
			try{
			if(isActiveAndEnabled){				
				StopCoroutine ("doCountTo");
				StartCoroutine ("doCountTo", result);
			} else {
				IAPUIUtility.UpdateLabelText(gameObject,result.ToString());
			}	

			} catch (Exception e){
				Debug.Log(e);
			}
		}

		protected IEnumerator doCountTo (int target) {
			int start = _lastNumber;
			for (float timer = 0; timer <= animationTime; timer += Time.deltaTime) {
				float progress = timer/animationTime;
				int score = (int)Mathf.Lerp (start, target, progress);
				IAPUIUtility.UpdateLabelText(gameObject,score.ToString());
				yield return null;
			}
			_lastNumber = target;
			IAPUIUtility.UpdateLabelText(gameObject,_lastNumber.ToString());
		}

	}
}