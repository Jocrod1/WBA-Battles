using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Digicrafts.IAP;

public class IAPDCExample : MonoBehaviour, IIAPDelegate {

	//--- IIAPDelegate

	// Event when IAP initialized
	public void OnIAPInitialized(Dictionary<string, IAPProduct> products){		

		// products contains a Dictionary whcih sotre product information

		foreach(KeyValuePair<string, IAPProduct> item in products){

			string key = item.Key;
			IAPProduct p = item.Value;

			Debug.Log("Id: " + key + " Product: " + p);

		}
	}

	// Event when IAP initialized Fail
	public void OnIAPInitializeFailed(string error) {}

	// Event when a purchase started
	public void OnIAPPurchaseStart(IAPProduct product) {}

	// Event when when a purchase finished and success
	public void OnIAPProcessPurchase(IAPProduct product, string transactionID, string receipt) {

		// Do somthing after purchase finished
		// You can get the product information and receipt from here.
		print(product);
	}

	// Event when a purchase failed
	public void OnIAPPurchaseFailed(IAPProduct product, string failureReason){

	}

	// Event for deferred purcahse
	// On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
	// On non-Apple platforms this will have no effect; OnDeferred will never be called.
	public void OnIAPProcessDeferred(IAPProduct product) {

	}

	// Event for restore purchase
	// Success set to true if restore success
	public void OnIAPTransactionsRestored(bool success) {

	}
}
