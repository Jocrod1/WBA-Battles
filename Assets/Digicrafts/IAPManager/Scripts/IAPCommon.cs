using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
	
namespace Digicrafts.IAP
{	

	public enum IAPStoreType {
		All,
		AppleAppStore,
		GooglePlayStore,
		AmazonAppStore
	}

	public enum IAPProductType {
		Consumable,
		NonConsumable,
		Subscription
	}

    /// <summary>
    /// Store the stucture of return json of introductoryPrice
    /// </summary>
    public class IntroductoryPrice
    {
        public string introductoryPrice;
        public string introductoryPriceLocale;
        public string introductoryPriceNumberOfPeriods;
        public string numberOfUnits;
        public string unit;

        public override string ToString()
        {
            return "introductoryPrice: " + introductoryPrice
                + "\nintroductoryPriceLocale: " + introductoryPriceLocale
                + "\nintroductoryPriceNumberOfPeriods: " + introductoryPriceNumberOfPeriods
                + "\nnumberOfUnits: " + numberOfUnits
                + "\nunit: " + unit;
        }
    }

    public class IAPSubscriptionInfo : UnityEngine.Object
	{

		public bool isSubscribed;
		public bool isExpired;
		public bool isAutoRenewing;
		public bool isCancelled;
		public bool isFreeTrial;
		public bool isIntroductoryPricePeriod;

		public DateTime purchaseDate;
		public TimeSpan remainingTime;
		public DateTime cancelDate;
		public DateTime expireDate;			
		public string freeTrialPeriodString;
		public string introductoryPrice;
		public TimeSpan introductoryPricePeriod;
		public long introductoryPricePeriodCycles;

		public static IAPSubscriptionInfo GetInstance(SubscriptionInfo info)
		{
			IAPSubscriptionInfo result = new IAPSubscriptionInfo();

			// if(info){
			result.isExpired=(info.isExpired()==Result.True);
			result.isAutoRenewing=(info.isAutoRenewing() == Result.True);
			result.isCancelled= (info.isCancelled() == Result.True);
            result.isFreeTrial= (info.isFreeTrial() == Result.True);
            result.isIntroductoryPricePeriod= (info.isIntroductoryPricePeriod() == Result.True);
            result.isSubscribed= (info.isSubscribed() == Result.True);

            result.purchaseDate=info.getPurchaseDate();
			result.remainingTime=info.getRemainingTime();			
			result.cancelDate=info.getCancelDate();
			result.expireDate=info.getExpireDate();
			// info.getFreeTrialPeriod();
			result.freeTrialPeriodString=info.getFreeTrialPeriodString();
			result.introductoryPrice=info.getIntroductoryPrice();
			result.introductoryPricePeriod=info.getIntroductoryPricePeriod();
			result.introductoryPricePeriodCycles=info.getIntroductoryPricePeriodCycles();
			// }
			
			return result;
		}

		public void Update(SubscriptionInfo info)
		{
			if(info!=null){
				this.isExpired=(info.isExpired() == Result.True);
                this.isAutoRenewing=(info.isAutoRenewing() == Result.True);
                this.isCancelled=(info.isCancelled() == Result.True);
                this.isFreeTrial=(info.isFreeTrial() == Result.True);
                this.isIntroductoryPricePeriod=(info.isIntroductoryPricePeriod() == Result.True);
                this.isSubscribed=(info.isSubscribed() == Result.True);

                this.purchaseDate=info.getPurchaseDate();
				this.remainingTime=info.getRemainingTime();			
				this.cancelDate=info.getCancelDate();
				this.expireDate=info.getExpireDate();
				// info.getFreeTrialPeriod();
				this.freeTrialPeriodString=info.getFreeTrialPeriodString();
				this.introductoryPrice=info.getIntroductoryPrice();
				this.introductoryPricePeriod=info.getIntroductoryPricePeriod();
				this.introductoryPricePeriodCycles=info.getIntroductoryPricePeriodCycles();
			}
		}
	}

	/// <summary>
	/// An example of basic Unity IAP functionality.
	/// To use with your account, configure the product ids (AddProduct)
	/// and Google Play key (SetPublicKey).
	/// </summary>
	public class IAPProduct : UnityEngine.Object
	{

		public string id; // Id of the product
		public string appleProductId;
		public string googleProductId;
		public string amazonProductId;
		public string macProductId;
		public string samsungProductId;
		public string tizenProductId;
		public string moolahProductId;
		public IAPProductType productType;
		public string isoCurrencyCode;
		public decimal price;
		public string priceString;
        public decimal introductoryPrice;
        public string introductoryPriceString;
        public string title;
		public string description;
		public bool hasReceipt;
		public bool availableToPurchase;
		public string receipt;
		public string transactionID;
		public Product rawProduct;
		public string buyButtonText;
		public string disabledButtonText;
		public IAPSubscriptionInfo subscriptionInfo;

        // Testing value

		public IAPProduct(string id, IAPProductType type)
		{			
			this.id=id;
			this.productType=type;
		
		}

		public override string ToString()
		{
			return 
				"id: " + id +
				" | productType: " + productType +
				" | priceString: " + priceString +
				" | title: " + title +
				" | description: " + description +
				" | hasReceipt: " + ((hasReceipt==true)?"True":"False") +
				" | availableToPurchase: " + ((availableToPurchase==true)?"True":"False") +
				" | transactionID: " + ((transactionID!=null)?transactionID:"Null") ;
		
		}

	}


	/// <summary>
	/// An example of basic Unity IAP functionality.
	/// To use with your account, configure the product ids (AddProduct)
	/// and Google Play key (SetPublicKey).
	/// </summary>
	public delegate void InitializedDelegate (Dictionary<string, IAPProduct> products);
	public delegate void InitializeFailedDelegate (string error);
	public delegate void PurchaseStartDelegate (IAPProduct product);
	public delegate void ProcessPurchaseDelegate (IAPProduct product, string transactionID, string receipt);
	public delegate void PurchaseFailedDelegate (IAPProduct product, string failureReason);
	public delegate void PurchaseDeferredDelegate (IAPProduct product);
	public delegate void TransactionsRestoredDelegate (bool success);

	/// <summary>
	/// IIAP delegate.
	/// </summary>
	public interface IIAPDelegate
	{
		void OnIAPInitialized(Dictionary<string, IAPProduct> products);
		void OnIAPInitializeFailed(string error);
		void OnIAPPurchaseStart(IAPProduct product);
		void OnIAPProcessPurchase(IAPProduct product, string transactionID, string receipt);
		void OnIAPPurchaseFailed(IAPProduct product, string failureReason);
		void OnIAPProcessDeferred(IAPProduct product);
		void OnIAPTransactionsRestored(bool success);
	}

	[System.Serializable]
	public class IAPAdvancedSetting
	{
		public bool testMode;
		public bool receiptValidation;
		public string GooglePublicKey;
		public string MoolahAppKey;
		public string MoolahHashKey;
		public string TizenGroupId;		
	}

	/// <summary>
	/// IAP product setting.
	/// </summary>
	[System.Serializable]
	public class IAPProductSetting
	{		
		public IAPProductType productType;
		public string productId;
		// Override Ids for stores
		public string appleProductId;
		public string googleProductId;
		public string amazonProductId;
		public string macProductId;
		public string samsungProductId;
		public string tizenProductId;
		public string moolahProductId;

		// Text override
		public string buyButtonText="";
		public string disabledButtonText="";
		// UI connection GameObject
		public GameObject buyButton;	
		public GameObject priceLabel;
		public GameObject titleLabel;
		public GameObject descriptionLabel;
		// Subscription
		public GameObject purchaseDateLabel;				
		public GameObject remainingTimeLabel;
		public GameObject cancelDateLabel;			
		public GameObject expireDateLabel;			
		public GameObject freeTrialPeriodLabel;
		public GameObject introductoryPriceLabel;
		public GameObject introductoryPricePeriodLabel;

		public IAPProductSetting (string id, IAPProductType type)
		{			
			productId = id;
			productType = type;
		}		
	}

}