using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Purchasing;

using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System;

using Digicrafts.IAP;

public class IAPBootstrap : MonoBehaviour
{	
//	[Header ("IAP Manager", order = 0)]
//	[Header ("Copyright 2019(R) Digicrafts", order = 1)]
//	[Header ("*********************************", order = 2)]
//	[Header ("The target contains script handle the events", order = 3)]
//	[Space (10, order = 4)]
	public GameObject eventTarget;
	public GameObject restoreButton;

//	[Header ("Setting for each product", order = 5)]
	public List<IAPProductSetting> productSettings;
	public IAPAdvancedSetting advancedSetting;

	public InitializedEvent OnInitialized;
	public InitializeFailedEvent OnInitializeFailed;
	public PurchaseStartEvent OnPurchaseStart;
	public ProcessPurchaseEvent OnProcessPurchase;
	public PurchaseFailedEvent OnPurchaseFailed;
	public PurchaseDeferredEvent OnPurchaseDeferred;
	public TransactionsRestoredEvent OnTransactionsRestored;

	// private
	private IIAPDelegate _eventTarget;
	private bool _hasEvents = false;

	void Start ()
	{	
		
		// Check the settings list
		if (productSettings != null && productSettings.Count > 0) {

			Debug.Log ("[IAPManager] Initialing IAP Manager.");	

			// get the script with IIAPDelegate interface
			if(eventTarget !=null){
				_eventTarget = eventTarget.GetComponent<IIAPDelegate> ();
				if (_eventTarget != null) {
					// Add events to target,
					_addEvents();
				}
			}

			// Disable Buttons
			if(restoreButton != null){
				if(restoreButton.GetComponent<Button>() != null){
					restoreButton.GetComponent<Button>().interactable=false;
					restoreButton.GetComponent<Button>().onClick.AddListener(()=>{
						RestorePurchases();
					});
				}
				#if ENABLE_NGUI
				else if(restoreButton.GetComponent<UIButton>() != null){
					EventDelegate del = new EventDelegate(this,"handleRestoreButton");
					EventDelegate.Add(restoreButton.GetComponent<UIButton>().onClick,del);									
					restoreButton.GetComponent<UIButton>().isEnabled=false;
				} 
				#endif
			}
			foreach(IAPProductSetting setting in productSettings){
				GameObject btn = setting.buyButton;
				if(btn !=null){
					if(btn.GetComponent<Button>() != null){
						btn.GetComponent<Button>().interactable=false;
					}
					#if ENABLE_NGUI
					else if(btn.GetComponent<UIButton>() != null){
						btn.GetComponent<UIButton>().isEnabled=false;
					}
					#endif
				}
			}

			// add event for internal
			IAPManager.OnIAPInitialized+=handleOnIAPInitialized;
			IAPManager.OnIAPInitializeFailed+=handleOnIAPInitializeFailed;
			IAPManager.OnIAPProcessPurchase+=handleOnIAPProcessPurchase;
			IAPManager.OnIAPPurchaseDeferred+=handleOnIAPProcessDeferred;
			IAPManager.OnIAPPurchaseFailed+=handleOnIAPPurchaseFailed;
			IAPManager.OnIAPPurchaseStart+=handleOnIAPPurchaseStart;
			IAPManager.OnIAPTransactionsRestored+=handleOnIAPTransactionsRestored;

			// init the IAPManager
			IAPManager.Create (productSettings, advancedSetting);
//			} else {
//				Debug.Log ("[IAPManager] Warning: Please specify the event target which inmplements the IIAPDelegate.");	
//			}				

		} else {
			Debug.Log ("[IAPManager] Error: Please check your config.");	
		}
	}

	void OnEnable ()
	{		
		_addEvents();
	}

	void OnDisable ()
	{
		_removeEvents();
	}

	void Destroy ()
	{
		_removeEvents();
	}

	//--- Private Methods

	private void _addEvents(){		
		if (_eventTarget != null && !_hasEvents) {		
			Debug.Log ("[IAPManager] Add events to target.");	
			IAPManager.OnIAPInitialized += _eventTarget.OnIAPInitialized;
			IAPManager.OnIAPInitializeFailed += _eventTarget.OnIAPInitializeFailed;
			IAPManager.OnIAPPurchaseStart += _eventTarget.OnIAPPurchaseStart;
			IAPManager.OnIAPProcessPurchase += _eventTarget.OnIAPProcessPurchase;
			IAPManager.OnIAPPurchaseFailed += _eventTarget.OnIAPPurchaseFailed;
			IAPManager.OnIAPPurchaseDeferred += _eventTarget.OnIAPProcessDeferred;
			IAPManager.OnIAPTransactionsRestored += _eventTarget.OnIAPTransactionsRestored;
			_hasEvents=true;
		}		
	}

	private void _removeEvents(){
		if (_eventTarget != null && _hasEvents) {
			Debug.Log ("[IAPManager] Remove events to target.");	
			IAPManager.OnIAPInitialized -= _eventTarget.OnIAPInitialized;
			IAPManager.OnIAPInitializeFailed -= _eventTarget.OnIAPInitializeFailed;
			IAPManager.OnIAPPurchaseStart -= _eventTarget.OnIAPPurchaseStart;
			IAPManager.OnIAPProcessPurchase -= _eventTarget.OnIAPProcessPurchase;
			IAPManager.OnIAPPurchaseFailed -= _eventTarget.OnIAPPurchaseFailed;		
			IAPManager.OnIAPPurchaseDeferred -= _eventTarget.OnIAPProcessDeferred;
			IAPManager.OnIAPTransactionsRestored -= _eventTarget.OnIAPTransactionsRestored;
			_hasEvents=false;
		}		
	}

	private bool _setLabel(GameObject label, string text){
		bool success = false;
		if(label != null){
			if(label.GetComponent<Text>()){				
				label.GetComponent<Text>().text = text;
				success = true;
			} 
			#if ENABLE_NGUI
			else if(label.GetComponent<UILabel>() != null){
				label.GetComponent<UILabel>().text = text;
				success = true;
			}
			#endif
		}
		return success;
	}

	private bool _setButtonLabel(IAPProduct product, GameObject btn, string text = null){
		bool success = false;
		if(btn != null){
//			Debug.Log("SetButton: " + product.productType + " hasReceipt: " + product.hasReceipt);
			bool isEnabled = true;
			if(product.productType == IAPProductType.NonConsumable){
				if(product.hasReceipt)
					isEnabled=false;
			} else if(product.productType == IAPProductType.Subscription){
				// if(product.subscriptionInfo!=null&&product.subscriptionInfo.isSubscribed==Result.True&&product.subscriptionInfo.isExpired!=Result.True)
				// if(product.subscriptionInfo==null){

				// } else {
				if(product.hasReceipt)
					isEnabled=false;	
				// }
			}

			// Change the text for button state
			if(!string.IsNullOrEmpty(product.disabledButtonText)&&!isEnabled){
				text=product.disabledButtonText;
			} else if(!string.IsNullOrEmpty(product.buyButtonText)){
				text=product.buyButtonText;
			}

			if(btn.GetComponent<Button>() != null){
				// Fill in the label in button if no dedicated text object
				if(text !=null)
					btn.GetComponentInChildren<Text>().text = text;

				// Add event to the button
				btn.GetComponent<Button>().interactable = isEnabled;
				btn.GetComponent<Button>().onClick.AddListener(()=>{
					PurchaseProduct(product.id);
				});

			}
			#if ENABLE_NGUI
			else if(btn.GetComponent<UIButton>() != null){

				// Fill in the label in button if no dedicated text object
				if(text !=null && btn.GetComponentInChildren<UILabel>() != null) 
					btn.GetComponentInChildren<UILabel>().text = text;

				btn.GetComponent<UIButton>().isEnabled = isEnabled;

				// Add event to the button
//				string str = new string(productId);
				EventDelegate del = new EventDelegate(this,"handleBuyButton");
				del.parameters[0] = new EventDelegate.Parameter(product,"id");
				del.parameters[0].value = product.id;
				EventDelegate.Set(btn.GetComponent<UIButton>().onClick,del);									

			}
			#endif

		}
		return success;
	}		

	private void handleRestoreButton(){
		IAPManager.RestorePurchases();
	}

	private void handleBuyButton(string id){
//		Debug.Log(id);
		PurchaseProduct(id);
	}

	//--- Public Methods

	/// <summary>
	/// Purchases the product.
	/// </summary>
	/// <param name="productId">Product identifier.</param>
	public void PurchaseProduct (string productId)
	{
		IAPManager.PurchaseProduct (productId);		
	}	

	/// <summary>
	/// Restores the purchases.
	/// </summary>
	public void RestorePurchases()
	{
		IAPManager.RestorePurchases();
	}

	/// <summary>
	/// Updates the button.
	/// </summary>
	/// <param name="productId">Product identifier.</param>
	/// <param name="btn">Button.</param>	 
	public void UpdateButton(string productId, GameObject btn)
	{

		// Get the product
		if(IAPManager.IsProductReady(productId)){

			IAPProduct p = IAPManager.products[productId];

			_setButtonLabel(p, btn, p.priceString);
		}

	}

	private void UpdateUI(IAPProduct product)
	{		

		// Debug.Log("UpdateUI: " + product.id);
		// if(product==null) return;

		IAPProductSetting setting=null;

		// Get setting
		foreach(IAPProductSetting item in productSettings){
			if(item.productId==product.id){
				setting=item;
				break;
			}
		}

		if(setting!=null)
		{
		// Loop Settings
		// foreach(IAPProductSetting setting in productSettings){
			// string k = setting.productId;
			// if(products.ContainsKey(k)){
				IAPProduct p = product;
//					Debug.Log("productSettings: " + p);
				// Title Label
				_setLabel(setting.titleLabel,p.title);
				// Description Label
				_setLabel(setting.descriptionLabel,p.description);
				// Pirce Label && Button
				if(_setLabel(setting.priceLabel,p.priceString))	{
					_setButtonLabel(product, setting.buyButton);
				} else {
					_setButtonLabel(product, setting.buyButton,p.priceString);
				}
				// Debug.Log("p.subscriptionInfo: " + p.subscriptionInfo);					
				// Subscription
				if(p.productType==IAPProductType.Subscription){												
					 if(p.subscriptionInfo==null){
					 	_setLabel(setting.expireDateLabel,"-");
					 	_setLabel(setting.freeTrialPeriodLabel,"-");
					 	_setLabel(setting.introductoryPriceLabel,p.introductoryPriceString);
					 	_setLabel(setting.introductoryPricePeriodLabel,"-");
					 	_setLabel(setting.purchaseDateLabel,"-");
					 	_setLabel(setting.remainingTimeLabel,"-");
					 } else {

                    //Debug.Log("[SubscriptionInfo]: upate" + p.subscriptionInfo.expireDate);                    
                    // Update new info							
                    if (p.subscriptionInfo.expireDate !=null)
                    	_setLabel(setting.expireDateLabel,p.subscriptionInfo.expireDate.ToLocalTime().ToString());
                    	_setLabel(setting.freeTrialPeriodLabel,p.subscriptionInfo.freeTrialPeriodString);
                    	_setLabel(setting.introductoryPriceLabel,p.subscriptionInfo.introductoryPrice);
                    if (p.subscriptionInfo.introductoryPricePeriod != null)
                        _setLabel(setting.introductoryPricePeriodLabel,p.subscriptionInfo.introductoryPricePeriod.ToString());
                    if (p.subscriptionInfo.purchaseDate != null)
                        _setLabel(setting.purchaseDateLabel,p.subscriptionInfo.purchaseDate.ToLocalTime().ToString());
                    if (p.subscriptionInfo.remainingTime != null)
                        _setLabel(setting.remainingTimeLabel,p.subscriptionInfo.remainingTime.ToString());												
					 }
				} else {
					_setLabel(setting.expireDateLabel,"-");
					_setLabel(setting.freeTrialPeriodLabel,"-");
					_setLabel(setting.introductoryPriceLabel,"-");
					_setLabel(setting.introductoryPricePeriodLabel,"-");
					_setLabel(setting.purchaseDateLabel,"-");
					_setLabel(setting.remainingTimeLabel,"-");
				}
			// }
		}
	}

	//--- IIAPDelegate

	private void handleOnIAPInitialized (Dictionary<string, IAPProduct> products)
	{
		OnInitialized.Invoke(products);

//		Debug.Log("handleOnIAPInitialized: " + products);
		if (products != null) {	

			// Enabled restore button
			if(restoreButton != null){
				if(restoreButton.GetComponent<Button>() != null){
					restoreButton.GetComponent<Button>().interactable=true;
					restoreButton.GetComponent<Button>().enabled=true;
				}
				#if ENABLE_NGUI
				else if(restoreButton.GetComponent<UIButton>() != null){					
					restoreButton.GetComponent<UIButton>().isEnabled=true;
	//					restoreButton.GetComponent<UIButton>().enabled=true;
				} 
				#endif
			}

			foreach(IAPProduct product in products.Values){				
				UpdateUI(product);
			}
		}

		IAPManager.OnIAPInitialized-=handleOnIAPInitialized;
	}		
		
	//--- Events

	// Event when IAP initialized Fail
	public void handleOnIAPInitializeFailed(string error) {
		OnInitializeFailed.Invoke(error);
	}

	// Event when a purchase started
	public void handleOnIAPPurchaseStart(IAPProduct product) {
		OnPurchaseStart.Invoke(product);
	}

	// Event when when a purchase finished and success
	public void handleOnIAPProcessPurchase(IAPProduct product, string transactionID, string receipt) 
	{
		UpdateUI(product);
		OnProcessPurchase.Invoke(product,transactionID,receipt);
	}

	// Event when a purchase failed
	public void handleOnIAPPurchaseFailed(IAPProduct product, string failureReason){
		OnPurchaseFailed.Invoke(product,failureReason);
	}

	// Event for deferred purcahse
	// On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
	// On non-Apple platforms this will have no effect; OnDeferred will never be called.
	public void handleOnIAPProcessDeferred(IAPProduct product) {
		OnPurchaseDeferred.Invoke(product);
	}

	// Event for restore purchase
	// Success set to true if restore success
	public void handleOnIAPTransactionsRestored(bool success) {
		OnTransactionsRestored.Invoke(success);
	}
}

