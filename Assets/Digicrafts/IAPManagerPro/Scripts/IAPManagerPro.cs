using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Digicrafts.IAP.Pro.UI;
using Digicrafts.IAP.Pro.Events;
using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;

/*! \mainpage IAP Manager Pro API
 *
 * \section intro_sec Introduction
 *
 * IAP Manager Pro is a total solution for in-app purchase and game data management. It provides an easy way to manage the in-app purchase, virtual currencies, inventories, game level data and player properties in the game. With a powerful UI template system for displaying managed object data. 
 * 
 * IAP Manager Pro provide a set of API to managing the data. Most operation is doing via the static class IAPInventoryManager. You can import the library by include the following code in your script header.
 * 
 * 		using Digicrafts.IAP.Pro
 * 
 * \section data_sec Data Object
 * 
 * IAP Manager Pro provides data types for common game workflow. The system contains data types included IAPCurrency, IAPInventory, IAPAbility, IAPGameLevel and IAPPackage.
 * 
 * \subsection IAPCurrency
 * 
 * IAPCurrency is a data object to store the virtual currency. It is use in local in-app purcahse to purchase other objects.
 * 
 * \subsection IAPInventory
 * 
 * IAPInventory is a data object to store the inventory. It is use for storing inventory data and procide a custom properties.
 * 
 * \subsection IAPAbility
 * 
 * IAPAbility is a data object to store the ability/powerup. It is use for storing ability/powerup data. It provide a level array to store the infromation for each ability/powerup level.
 * 
 * \subsection IAPGameLevel
 * 
 * IAPGameLevel is a data object to store the game level. It is use for storing game level data. It procide a list of sub-level item to indicate if the level it locked. Also, it can have custom properties.
 * 
 * \subsection IAPPackage
 * 
 * IAPPackage is a data object to store the package. It is use for in-app purcahse data. Each package repersent a in-app purchase product. You can define what item and amount within the package.
 * 
 * \section usage_sec Usage
 *
 * You can use static class IAPInventoryManager to manage the data within the system. IAPInventoryManager provides methods to query data object.
 * 
 * Example:
 * 
 * 	IAPCurrency currency = IAPInventoryManager.GetCurrency(uid);
 * 
 * The data class will provide methods to matain the data object.
 * 
 * Example:
 * 
 * 	// Get the amount of the currency oject within the system
 * 	int amount = currency.amount;
 * 
 * 	// Add 100 amount to the currency
 * 	currency.Fill(100);
 *
 */
namespace Digicrafts.IAP.Pro
{
	/// <summary>
	/// Core class for IAP Manager Pro inspector.
	/// </summary>
	public class IAPManagerPro : MonoBehaviour {

		/// <summary>
		/// The settings.
		/// </summary>
		[SerializeField]
		public IAPSettings settings;

		// private
		private static IAPManagerPro instance = null;  

		// Events
		public InitializedEvent OnIAPInitialized;
		public InitializeFailedEvent OnIAPInitializeFailed;
		public PurchaseStartEvent OnIAPPurchaseStart;
		public ProcessPurchaseEvent OnIAPProcessPurchase;
		public PurchaseFailedEvent OnIAPPurchaseFailed;
		public PurchaseDeferredEvent OnIAPPurchaseDeferred;
		public TransactionsRestoredEvent OnIAPTransactionsRestored;

		public CurrencyUpdatedEvent OnCurrencyUpdated;
		public InventoryUpdatedEvent OnInventoryUpdated;
		public AbilityUpdatedEvent OnAbilityUpdated;
		public PackageUpdatedEvent OnPackageUpdated;
		public GameLevelUpdatedEvent OnGameLevelUpdated;
		public CurrencyNotEnoughEvent OnCurrencyNotEnough;

		// Use this for initialization
		void Awake () 
		{
			//Check if instance already exists
			if (instance == null){
				//if not, set instance to this
				// Check the settings list
				if (settings!=null) {

					instance = this;			

					IAPInventoryManager.Log ("Initialing...");	
					/// DEBUG
//					PlayerPrefs.DeleteAll();
					/// DEBUG

					// Init IAPInventoryManager
					IAPInventoryManager.Create(settings);

					IAPInventoryManager.OnCurrencyUpdated+=handleOnCurrencyUpdated;
					IAPInventoryManager.OnAbilityUpdated+=handleOnAbilityUpdated;
					IAPInventoryManager.OnInventoryUpdated+=handleOnInventoryUpdated;
					IAPInventoryManager.OnCurrencyNotEnough+=handleOnCurrencyNotEnough;
					IAPInventoryManager.OnPackageUpdated+=handleOnPackageUpdated;
					IAPInventoryManager.OnGameLevelUpdated+=handleOnGameLevelUpdated;

					// Init the IAPManager
					if(settings.packageList != null && settings.fetchDataOnStart)
					{
						// Add events
						IAPInventoryManager.OnIAPInitialized+=handleOnIAPInitialized;
	//					IAPManager.OnIAPInitializeFailed+=handleOnIAPInitializeFailed;
						IAPInventoryManager.OnIAPProcessPurchase+=handleOnIAPProcessPurchase;
						IAPInventoryManager.OnIAPPurchaseDeferred+=handleOnIAPProcessDeferred;
						IAPInventoryManager.OnIAPPurchaseFailed+=handleOnIAPPurchaseFailed;
						IAPInventoryManager.OnIAPPurchaseStart+=handleOnIAPPurchaseStart;
						IAPInventoryManager.OnIAPTransactionsRestored+=handleOnIAPTransactionsRestored;

						IAPInventoryManager.InitIAPManager();
					}

					DontDestroyOnLoad(gameObject);

				} else {
					IAPInventoryManager.Log ("Error: Please check your config.");	
				}
			} else if (instance != this){
				
				Destroy(gameObject);
			}
		
		}

		void OnDestroy(){

			IAPInventoryManager.OnCurrencyUpdated-=handleOnCurrencyUpdated;
			IAPInventoryManager.OnAbilityUpdated-=handleOnAbilityUpdated;
			IAPInventoryManager.OnInventoryUpdated-=handleOnInventoryUpdated;
			IAPInventoryManager.OnCurrencyNotEnough-=handleOnCurrencyNotEnough;
			IAPInventoryManager.OnPackageUpdated-=handleOnPackageUpdated;
			IAPInventoryManager.OnGameLevelUpdated-=handleOnGameLevelUpdated;

			IAPInventoryManager.OnIAPInitialized-=handleOnIAPInitialized;
			//					IAPManager.OnIAPInitializeFailed-=handleOnIAPInitializeFailed;
			IAPInventoryManager.OnIAPProcessPurchase-=handleOnIAPProcessPurchase;
			IAPInventoryManager.OnIAPPurchaseDeferred-=handleOnIAPProcessDeferred;
			IAPInventoryManager.OnIAPPurchaseFailed-=handleOnIAPPurchaseFailed;
			IAPInventoryManager.OnIAPPurchaseStart-=handleOnIAPPurchaseStart;
			IAPInventoryManager.OnIAPTransactionsRestored-=handleOnIAPTransactionsRestored;

		}
			
//		void OnValidate() 
//		{
//			Debug.Log("OnValidate");
//		}

		//--- IIAPDelegate

		private void handleOnIAPInitialized (Dictionary<string, IAPProduct> products)
		{			
			OnIAPInitialized.Invoke(products);
		}

		private void handleOnIAPProcessPurchase(IAPProduct product, string transactionID, string receipt)
		{			
			OnIAPProcessPurchase.Invoke(product,transactionID,receipt);
		}			

		// Event when a purchase started
		private void handleOnIAPPurchaseStart(IAPProduct product) {
			OnIAPPurchaseStart.Invoke(product);
		}			

		// Event when a purchase failed
		private void handleOnIAPPurchaseFailed(IAPProduct product, string failureReason){
			OnIAPPurchaseFailed.Invoke(product,failureReason);
		}

		// Event for deferred purcahse
		// On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
		// On non-Apple platforms this will have no effect; OnDeferred will never be called.
		private void handleOnIAPProcessDeferred(IAPProduct product) {
			OnIAPPurchaseDeferred.Invoke(product);
		}			

		// Event for restore purchase
		// Success set to true if restore success
		private void handleOnIAPTransactionsRestored(bool success) {
			OnIAPTransactionsRestored.Invoke(success);
		}
			
		// Events of IAPInventoryManager

		private void handleOnCurrencyUpdated(IAPCurrency currency) {
			OnCurrencyUpdated.Invoke(currency);
		}

		private void handleOnInventoryUpdated(IAPInventory inventory) {
			OnInventoryUpdated.Invoke(inventory);
		}

		private void handleOnAbilityUpdated(IAPAbility ability) {
			
			OnAbilityUpdated.Invoke(ability);
		}

		private void handleOnPackageUpdated(IAPPackage package) {

			OnPackageUpdated.Invoke(package);
		}

		private void handleOnGameLevelUpdated(IAPGameLevel gamelevel) {

			OnGameLevelUpdated.Invoke(gamelevel);
		}

		private void handleOnCurrencyNotEnough(IAPCurrency currency) {
			IAPInventoryManager.ShowErrorDialog(settings.uiSettings.currencyErrorString.Replace("%currency_title%",currency.title),handleCancelButtonPressed);
			OnCurrencyNotEnough.Invoke(currency);
		}			

		private void handleCancelButtonPressed(IAPDialog dialog){
			dialog.OnCancelButtonPressed-=handleCancelButtonPressed;
		}
	}

}