#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// You must obfuscate your secrets using Window > Unity IAP > Receipt Validation Obfuscator
// before receipt validation will compile.
// #define RECEIPT_VALIDATION
#endif

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
#if RECEIPT_VALIDATION
using UnityEngine.Purchasing.Security;
#endif


namespace Digicrafts.IAP
{
    [Serializable]
    public class InitializedEvent : UnityEvent<Dictionary<string, IAPProduct>> { }
    [Serializable]
    public class InitializeFailedEvent : UnityEvent<string> { }
    [Serializable]
    public class PurchaseStartEvent : UnityEvent<IAPProduct> { }
    [Serializable]
    public class ProcessPurchaseEvent : UnityEvent<IAPProduct, string, string> { }
    [Serializable]
    public class PurchaseFailedEvent : UnityEvent<IAPProduct, string> { }
    [Serializable]
    public class PurchaseDeferredEvent : UnityEvent<IAPProduct> { }
    [Serializable]
    public class TransactionsRestoredEvent : UnityEvent<bool> { }

    /// <summary>
    /// IAP manager.
    /// </summary>
    public class IAPManager : IStoreListener
    {    

        // Events
        public static event InitializedDelegate OnIAPInitialized;
        public static event InitializeFailedDelegate OnIAPInitializeFailed;
        public static event PurchaseStartDelegate OnIAPPurchaseStart;
        public static event ProcessPurchaseDelegate OnIAPProcessPurchase;
        public static event PurchaseFailedDelegate OnIAPPurchaseFailed;
        public static event PurchaseDeferredDelegate OnIAPPurchaseDeferred;
        public static event TransactionsRestoredDelegate OnIAPTransactionsRestored;

        // Public
        public static Dictionary<string, IAPProduct> products;

        // Private
        private static IAPManager _instance;
        private static IAPAdvancedSetting _setting;
        private static IStoreController _StoreController;
        private static IExtensionProvider _StoreExtensionProvider;
        private static IAppleExtensions _AppleExtensions;
        private static ISamsungAppsExtensions _SamsungExtensions;
        private static IMoolahExtension _MoolahExtensions;
        private static IGooglePlayStoreExtensions _GooglePlayStoreExtensions;

        private static bool _isGooglePlayStore = false;
        private static bool _isSamsungAppsStore = false;
        private static bool _isCloudMoolahStore = false;
        private static bool _isLogined = false;

#if RECEIPT_VALIDATION
		private static CrossPlatformValidator _ReceiptValidator;
#endif

        public static IAPManager Create(List<IAPProductSetting> productsList, IAPAdvancedSetting advancedSetting = null)
        {
            if (productsList == null)
            {
                Debug.Log("[IAPManger] Product list should not be null.");
                if (OnIAPInitializeFailed != null) OnIAPInitializeFailed("Product list should not be null");
                return _instance;
            }

            // Create instance of the IAPManager
            if (_instance == null)
            {
                _instance = new IAPManager();
            }

            // Get the advanced setting
            _setting = advancedSetting;

            // If we have already connected to Purchasing ...
            if (IsInitialized())
            {

                if (OnIAPInitialized != null) OnIAPInitialized(products);

                return _instance;

            }
            else
            {

                // initializing
                _init(productsList);
            }

            return _instance;
        }

        public static bool IsProductReady(string productId)
        {
            if (products.ContainsKey(productId))
            {
                return true;
            }
            return false;
        }

        public static bool IsInitialized()
        {
            // Only say we are initialized if both the Purchasing references are set.
            return _StoreController != null && _StoreExtensionProvider != null;
        }


        public static void PurchaseProduct(string productId)
        {
            // If the stores throw an unexpected exception, use try..catch to protect my logic here.
            try
            {
                // If Purchasing has been initialized ...
                if (IsInitialized())
                {

                    if (productId != null && productId != "" && IsProductReady(productId))
                    {

                        // event
                        if (OnIAPPurchaseStart != null) OnIAPPurchaseStart(null);

                        // ... look up the Product reference with the general product identifier and the Purchasing system's products collection.
                        Product product = _StoreController.products.WithID(productId);

                        // If the look up found a product for this device's store and that product is ready to be sold ... 
                        if (product != null && product.availableToPurchase)
                        {
                            Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed asynchronously.

                            if (OnIAPPurchaseStart != null) OnIAPPurchaseStart(ConvertFromProduct(product));

                            _StoreController.InitiatePurchase(product);

                        }
                        // Otherwise ...
                        else
                        {
                            // ... report the product look-up failure situation  
                            Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");

                            if (OnIAPPurchaseFailed != null) OnIAPPurchaseFailed(null, "BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                        }

                    }//IsProductReady
                }
                // Otherwise ...
                else
                {
                    if (OnIAPPurchaseFailed != null) OnIAPPurchaseFailed(null, "BuyProductID FAIL. Not initialized.");
                }
            }
            // Complete the unexpected exception handling ...
            catch (Exception e)
            {
                if (OnIAPPurchaseFailed != null) OnIAPPurchaseFailed(null, "BuyProductID: FAIL. Exception during purchase. " + e);
            }
        }

        public static void RestorePurchases()
        {
            // If Purchasing has not yet been set up ...
            if (!IsInitialized())
            {
                // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
                Debug.Log("RestorePurchases FAIL. Not initialized.");
                return;
            }

            // If we are running on an Apple device ... 

            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            if (_isCloudMoolahStore)
            {
                if (_isLogined == false)
                {
                    Debug.LogError("CloudMoolah purchase restoration aborted. Login incomplete.");
                }
                else
                {
                    // Restore abnornal transaction identifer, if Client don't receive transaction identifer.
                    _MoolahExtensions.RestoreTransactionID((RestoreTransactionIDState restoreTransactionIDState) =>
                    {
                        Debug.Log("restoreTransactionIDState = " + restoreTransactionIDState.ToString());
                        bool success =
                            restoreTransactionIDState != RestoreTransactionIDState.RestoreFailed &&
                            restoreTransactionIDState != RestoreTransactionIDState.NotKnown;
                        OnTransactionsRestored(success);
                    });
                }
            }
            else if (_isSamsungAppsStore)
            {
                _SamsungExtensions.RestoreTransactions(OnTransactionsRestored);
            }
            else if (_isGooglePlayStore)
            {

            }
            else
            {
                _AppleExtensions.RestoreTransactions(OnTransactionsRestored);
            }

        }

        /// <summary>
        /// Logins the cloud moolah.
        /// </summary>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public void LoginCloudMoolah(string username, string password)
        {
            if (_isCloudMoolahStore)
            {
                // _MoolahExtensions.Login(username, password, handleLoginResult);
            }
        }

        /// <summary>
        /// Registers the cloud moolah.
        /// </summary>
        /// <param name="password">Password.</param>
        public void RegisterCloudMoolah(string password)
        {
            if (_isCloudMoolahStore)
            {
                // _MoolahExtensions.FastRegister(password, handleRegisterSucceeded, handleRegisterFailed);
            }
        }

        // Handle Moolah

        public void handleLoginResult(LoginResultState state, string errorMsg)
        {
            if (state == LoginResultState.LoginSucceed)
            {
                _isLogined = true;
            }
            else
            {
                _isLogined = false;
            }
            Debug.Log("LoginResult: state: " + state.ToString() + " errorMsg: " + errorMsg);
        }

        public void handleRegisterSucceeded(string cmUserName)
        {
            Debug.Log("CloudMoolah RegisterSucceeded: cmUserName = " + cmUserName);
            //			_CloudMoolahUserName = cmUserName;
        }

        public void handleRegisterFailed(FastRegisterError error, string errorMessage)
        {
            Debug.Log("CloudMoolah RegisterFailed: error = " + error.ToString() + ", errorMessage = " + errorMessage);
        }

        // Private Methods

        private static void _init(List<IAPProductSetting> productSettings)
        {

            // build the product list
            products = new Dictionary<string, IAPProduct>();
            foreach (IAPProductSetting setting in productSettings)
            {
                string id = setting.productId;
                IAPProductType type = setting.productType;
                IAPProduct p = new IAPProduct(id, type);
                p.amazonProductId = (setting.amazonProductId != null && setting.amazonProductId != "") ? setting.amazonProductId : setting.productId;
                p.googleProductId = (setting.googleProductId != null && setting.googleProductId != "") ? setting.googleProductId : setting.productId;
                p.appleProductId = (setting.appleProductId != null && setting.appleProductId != "") ? setting.appleProductId : setting.productId;
                p.samsungProductId = (setting.samsungProductId != null && setting.samsungProductId != "") ? setting.samsungProductId : setting.productId;
                p.macProductId = (setting.macProductId != null && setting.macProductId != "") ? setting.macProductId : setting.productId;
                p.tizenProductId = (setting.tizenProductId != null && setting.tizenProductId != "") ? setting.tizenProductId : setting.productId;
                p.moolahProductId = (setting.moolahProductId != null && setting.moolahProductId != "") ? setting.moolahProductId : setting.productId;

                p.buyButtonText = setting.buyButtonText;
                p.disabledButtonText = setting.disabledButtonText;

                if (products.ContainsKey(id))
                {
                    Debug.Log("[IAPManager] Product id \"" + id + "\" already exist.");
                }
                else
                {
                    Debug.Log("[IAPManager] Add product with id \"" + id + "\"");
                    products.Add(id, p);
                }
                //				Debug.Log("product id: " + p.id);
            }

            var module = StandardPurchasingModule.Instance();

            // Create a builder, first passing in a suite of Unity provided stores.
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // CloudMoolah Configuration setings 
            // All games must set the configuration. the configuration need to apply on the CloudMoolah Portal.
            // CloudMoolah APP Key
            if (_setting.MoolahAppKey != null && _setting.MoolahAppKey != "")
                builder.Configure<IMoolahConfiguration>().appKey = _setting.MoolahAppKey;
            // CloudMoolah Hash Key
            if (_setting.MoolahHashKey != null && _setting.MoolahHashKey != "")
                builder.Configure<IMoolahConfiguration>().hashKey = _setting.MoolahHashKey;

            // Test Mode Enbled
            if (_setting.testMode)
            {
                // This enables the Microsoft IAP simulator for local testing.
                builder.Configure<IMicrosoftConfiguration>().useMockBillingSystem = true;
                if (_setting.GooglePublicKey != null && _setting.GooglePublicKey != "")
                    builder.Configure<IGooglePlayConfiguration>().SetPublicKey(_setting.GooglePublicKey);
                builder.Configure<IMoolahConfiguration>().SetMode(CloudMoolahMode.AlwaysSucceed);
                // This records whether we are using Cloud Moolah IAP. 
                // Cloud Moolah requires logging in to access your Digital Wallet, so: 
                // A) IAPDemo (this) displays the Cloud Moolah GUI button for Cloud Moolah
            }

            // Add Product to builder
            foreach (KeyValuePair<string, IAPProduct> item in products)
            {
                IAPProduct p = item.Value;
                IDs ids = new IDs();

                if (p.appleProductId != null) ids.Add(p.appleProductId, AppleAppStore.Name);
                if (p.googleProductId != null) ids.Add(p.googleProductId, GooglePlay.Name);
                if (p.amazonProductId != null) ids.Add(p.amazonProductId, AmazonApps.Name);
                if (p.macProductId != null) ids.Add(p.macProductId, MacAppStore.Name);
                if (p.samsungProductId != null) ids.Add(p.samsungProductId, SamsungApps.Name);
                if (p.tizenProductId != null) ids.Add(p.tizenProductId, TizenStore.Name);
                if (p.moolahProductId != null) ids.Add(p.moolahProductId, MoolahAppStore.Name);
                //				Debug.Log("builder id: " + p.id);
                builder.AddProduct(p.id, ConvertFromIAPProductType(p.productType), ids);

            }

            // Test Mode Enabled
            if (_setting.testMode)
            {
                // Write Amazon's JSON description of our products to storage when using Amazon's local sandbox.
                // This should be removed from a production build.
                builder.Configure<IAmazonConfiguration>().WriteSandboxJSON(builder.products);

                // This enables simulated purchase success for Samsung IAP.
                // You would remove this, or set to SamsungAppsMode.Production, before building your release package.
                builder.Configure<ISamsungAppsConfiguration>().SetMode(SamsungAppsMode.AlwaysSucceed);
                // This records whether we are using Samsung IAP. Currently ISamsungAppsExtensions.RestoreTransactions
                // displays a blocking Android Activity, so: 
                // A) Unity IAP does not automatically restore purchases on Samsung Galaxy Apps
                // B) IAPDemo (this) displays the "Restore" GUI button for Samsung Galaxy Apps
            }


            // This selects the GroupId that was created in the Tizen Store for this set of products
            // An empty or non-matching GroupId here will result in no products available for purchase
            builder.Configure<ITizenStoreConfiguration>().SetGroupId(_setting.TizenGroupId);


            // Check Platform
            _isGooglePlayStore = Application.platform == RuntimePlatform.Android && module.appStore == AppStore.GooglePlay;
            _isCloudMoolahStore = Application.platform == RuntimePlatform.Android && module.appStore == AppStore.CloudMoolah;
            _isSamsungAppsStore = module.appStore == AppStore.SamsungApps;

            // build validator
#if RECEIPT_VALIDATION
				string appIdentifier;
#if UNITY_5_6_OR_NEWER
				appIdentifier = Application.identifier;
#else
				appIdentifier = Application.bundleIdentifier;
#endif
				_ReceiptValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(),
					UnityChannelTangle.Data(), appIdentifier);							
#endif

            UnityPurchasing.Initialize(_instance, builder);

        }

        private static IAPProductType ConvertFromProductTypeString(string productType)
        {

            switch (productType)
            {
                case "consumable":
                    return IAPProductType.Consumable;
                case "NonConsumable":
                    return IAPProductType.NonConsumable;
                case "Subscription":
                    return IAPProductType.Subscription;
            }
            return IAPProductType.Consumable;
        }

        // Private Helper Methods

        /// <summary>
        /// Converts the product to IAP product.
        /// </summary>
        /// <returns>The product to IAP product.</returns>
        /// <param name="product">Product.</param>

        private static IAPProduct ConvertFromProduct(Product product)
        {
            IAPProduct p = null;
            if (products.ContainsKey(product.definition.id))
            {
                p = products[product.definition.id];
                p.priceString = product.metadata.localizedPriceString;
                p.price = product.metadata.localizedPrice;
                p.title = product.metadata.localizedTitle;
                p.description = product.metadata.localizedDescription;
                p.isoCurrencyCode = product.metadata.isoCurrencyCode;
                p.availableToPurchase = product.availableToPurchase;
                p.hasReceipt = product.hasReceipt;
                p.rawProduct = product;
            }
            return p;
        }

        private static ProductType ConvertFromIAPProductType(IAPProductType productType)
        {
            switch (productType)
            {
                case IAPProductType.Consumable:
                    return ProductType.Consumable;
                case IAPProductType.NonConsumable:
                    return ProductType.NonConsumable;
                case IAPProductType.Subscription:
                    return ProductType.Subscription;
            }
            return ProductType.Consumable;
        }

        private static IAPProductType ConvertFromProductType(ProductType productType)
        {

            switch (productType)
            {
                case ProductType.Consumable:
                    return IAPProductType.Consumable;
                case ProductType.NonConsumable:
                    return IAPProductType.NonConsumable;
                case ProductType.Subscription:
                    return IAPProductType.Subscription;
            }
            return IAPProductType.Consumable;
        }

        //  
        // --- IStoreListener
        //


        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            // Purchasing has succeeded initializing. Collect our Purchasing references.
            Debug.Log("[IAPManager] OnInitialized.");

            // Overall Purchasing system, configured with products for this application.
            _StoreController = controller;
            // Store specific subsystem, for accessing device-specific store features.
            _StoreExtensionProvider = extensions;

            _AppleExtensions = extensions.GetExtension<IAppleExtensions>();
            _SamsungExtensions = extensions.GetExtension<ISamsungAppsExtensions>();
            _MoolahExtensions = extensions.GetExtension<IMoolahExtension>();

            // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
            // On non-Apple platforms this will have no effect; OnDeferred will never be called.
#if UNITY_IOS
			_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);			
#endif
#if UNITY_ANDROID
			_GooglePlayStoreExtensions = extensions.GetExtension<IGooglePlayStoreExtensions>();
#endif

#if RECEIPT_VALIDATION
    #if UNITY_EDITOR
    #else
        #if UNITY_IOS
			Dictionary<string, string> introductory_info_dict = _AppleExtensions.GetIntroductoryPriceDictionary();
        #endif
        #if UNITY_ANDROID
			Dictionary<string, string> introductory_info_dict = _GooglePlayStoreExtensions.GetProductJSONDictionary();
        #endif
    #endif
#endif
            // Store products
            for (int i = 0; i < controller.products.all.Length; i++)
            {
                Product pp = controller.products.all[i];
                IAPProduct p = null;
                if (products.ContainsKey(pp.definition.id))
                {
                    p = products[pp.definition.id];
                    p.priceString = pp.metadata.localizedPriceString;
                    p.price = pp.metadata.localizedPrice;
                    p.title = pp.metadata.localizedTitle;
                    p.description = pp.metadata.localizedDescription;
                    p.isoCurrencyCode = pp.metadata.isoCurrencyCode;
                    p.hasReceipt = pp.hasReceipt;
                    p.availableToPurchase = pp.availableToPurchase;
                    p.receipt = pp.receipt;
                    p.transactionID = pp.transactionID;
                    p.rawProduct = pp;
                    p.hasReceipt = pp.hasReceipt;

#if RECEIPT_VALIDATION
#if UNITY_EDITOR
#else
					_UpdateReceipt(pp,ref p,introductory_info_dict);
#endif
#endif
                }
            }
            // callback
            if (OnIAPInitialized != null) OnIAPInitialized(products);

        }


        public void OnInitializeFailed(InitializationFailureReason error)
        {
            // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
            Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);

            switch (error)
            {
                case InitializationFailureReason.AppNotKnown:
                    Debug.LogError("AppNotKnown: Is your App correctly uploaded on the relevant publisher console?");
                    break;
                case InitializationFailureReason.PurchasingUnavailable:
                    // Ask the user if billing is disabled in device settings.
                    Debug.Log("PurchasingUnavailable: Billing disabled!");
                    break;
                case InitializationFailureReason.NoProductsAvailable:
                    // Developer configuration error; check product metadata.
                    Debug.Log("NoProductsAvailable: No products available for purchase!");
                    break;
            }

            if (OnIAPInitializeFailed != null) OnIAPInitializeFailed(error.ToString());
        }

        private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
        {
            var receipt_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
            if (!receipt_wrapper.ContainsKey("Store") || !receipt_wrapper.ContainsKey("Payload"))
            {
                Debug.Log("The product receipt does not contain enough information");
                return false;
            }
            var store = (string)receipt_wrapper["Store"];
            var payload = (string)receipt_wrapper["Payload"];

            if (payload != null)
            {
                switch (store)
                {
                    case GooglePlay.Name:
                        {
                            var payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(payload);
                            if (!payload_wrapper.ContainsKey("json"))
                            {
                                Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
                                return false;
                            }
                            var original_json_payload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode((string)payload_wrapper["json"]);
                            if (original_json_payload_wrapper == null || !original_json_payload_wrapper.ContainsKey("developerPayload"))
                            {
                                Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
                                return false;
                            }
                            var developerPayloadJSON = (string)original_json_payload_wrapper["developerPayload"];
                            var developerPayload_wrapper = (Dictionary<string, object>)MiniJson.JsonDecode(developerPayloadJSON);
                            if (developerPayload_wrapper == null || !developerPayload_wrapper.ContainsKey("is_free_trial") || !developerPayload_wrapper.ContainsKey("has_introductory_price_trial"))
                            {
                                Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
                                return false;
                            }
                            return true;
                        }
                    case AppleAppStore.Name:
                    case AmazonApps.Name:
                    case MacAppStore.Name:
                        {
                            return true;
                        }
                    default:
                        {
                            return false;
                        }
                }
            }
            return false;
        }

        /// <summary>
        /// iOS Specific.
        /// This is called as part of Apple's 'Ask to buy' functionality,
        /// when a purchase is requested by a minor and referred to a parent
        /// for approval.
        /// 
        /// When the purchase is approved or rejected, the normal purchase events
        /// will fire.
        /// </summary>
        /// <param name="item">Item.</param>
        private static void OnDeferred(Product item)
        {
            Debug.Log("Purchase deferred: " + item.definition.id);
            if (OnIAPPurchaseDeferred != null) OnIAPPurchaseDeferred(ConvertFromProduct(item));
        }

        /// <summary>
        /// Raises the transactions restored event.
        /// </summary>
        /// <param name="success">If set to <c>true</c> success.</param>
        private static void OnTransactionsRestored(bool success)
        {
            Debug.Log("Transactions restored.");
            if (OnIAPTransactionsRestored != null) OnIAPTransactionsRestored(success);
        }

        /// <summary>
        /// Processes the purchase.
        /// </summary>
        /// <returns>The purchase.</returns>
        /// <param name="args">Arguments.</param>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {

            if (args != null && args.purchasedProduct != null)
            {

                IAPProduct product = ConvertFromProduct(args.purchasedProduct);

#if RECEIPT_VALIDATION
			if (Application.platform == RuntimePlatform.Android || 
				Application.platform == RuntimePlatform.IPhonePlayer || 
				Application.platform == RuntimePlatform.OSXPlayer) {
			
				try {						
					//var result =
                _ReceiptValidator.Validate(args.purchasedProduct.receipt);
					//Debug.Log("Receipt is valid. Contents:");					
					//foreach (IPurchaseReceipt productReceipt in result) {
					//	Debug.Log(productReceipt.productID);
					//	Debug.Log(productReceipt.purchaseDate);
					//	Debug.Log(productReceipt.transactionID);

					//	GooglePlayReceipt google = productReceipt as GooglePlayReceipt;						
					//	if (null != google) {
					//		Debug.Log(google.purchaseState);
					//		Debug.Log(google.purchaseToken);
					//	}

					//	AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
					//	if (null != apple) {
					//		Debug.Log(apple.originalTransactionIdentifier);
					//		Debug.Log(apple.subscriptionExpirationDate);
					//		Debug.Log(apple.cancellationDate);
					//		Debug.Log(apple.quantity);
					//	}
					//}
				} catch (IAPSecurityException) {
					Debug.Log("Invalid receipt, not unlocking content");
					if(OnIAPProcessPurchase!=null) OnIAPProcessPurchase(null,null,null);
					return PurchaseProcessingResult.Complete;
				}
			}

                // update subscription info
#if UNITY_IOS
				Dictionary<string, string> introductory_info_dict = _AppleExtensions.GetIntroductoryPriceDictionary();
				_UpdateReceipt(args.purchasedProduct,ref product,introductory_info_dict);
#endif
#if UNITY_ANDROID
                Dictionary<string, string> introductory_info_dict = _GooglePlayStoreExtensions.GetProductJSONDictionary();
                _UpdateReceipt(args.purchasedProduct,ref product,introductory_info_dict);
#endif

#endif

                // CloudMoolah purchase completion / finishing currently requires using the API 
                // extension IMoolahExtension.RequestPayout to finish a transaction.
                if (_isCloudMoolahStore)
                {
                    // Finish transaction with CloudMoolah server
                    _MoolahExtensions.ValidateReceipt(args.purchasedProduct.transactionID,
                        args.purchasedProduct.receipt,
                        (string transactionID, ValidateReceiptState state, string message) =>
                        {
                            if (state == ValidateReceiptState.ValidateSucceed)
                            {
                            // Finally, finish transaction with Unity IAP's local
                            // transaction log, recording the transaction id there
                            _StoreController.ConfirmPendingPurchase(args.purchasedProduct);

                            // Unlock content here.
                        }
                            else
                            {
                                Debug.Log("RequestPayOut: failed. transactionID: " + transactionID +
                                    ", state: " + state + ", message: " + message);
                            // Finishing failed. Retry later.
                        }
                        });
                }

                if (OnIAPProcessPurchase != null) OnIAPProcessPurchase(
                       product,
                       args.purchasedProduct.transactionID,
                       args.purchasedProduct.receipt);
                return PurchaseProcessingResult.Complete;
            }
            else
            {
                if (OnIAPProcessPurchase != null) OnIAPProcessPurchase(null, null, null);
            }

            // still pending
            return PurchaseProcessingResult.Pending;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            if (OnIAPPurchaseFailed != null) OnIAPPurchaseFailed(ConvertFromProduct(product), failureReason.ToString());
        }

        /// <summary>
        /// Updates the receipt.
        /// </summary>
        /// <param name="pp">Pp.</param>
        /// <param name="p">P.</param>
        /// <param name="introductory_info_dict">Introductory info dict.</param>
        private void _UpdateReceipt(Product pp, ref IAPProduct p, Dictionary<string, string> introductory_info_dict)
        {
#if RECEIPT_VALIDATION
#if UNITY_EDITOR
#else
			// Get subscription information						
			if (pp.definition.type == ProductType.Subscription)
			{							
                // Get the json which store the introductory info
                string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(pp.definition.storeSpecificId)) ? null : introductory_info_dict[pp.definition.storeSpecificId];                      					
				if(pp.receipt!=null&&checkIfProductIsAvailableForSubscriptionManager(pp.receipt)) 
				{													
						SubscriptionManager manager = new SubscriptionManager(pp, intro_json);
						SubscriptionInfo info = manager.getSubscriptionInfo();							
						p.subsciprtionInfo = IAPSubscriptionInfo.GetInstance(info);
						// Debug.Log("[SubscriptionInfo] input: " + pp.receipt);
						// Debug.Log("[SubscriptionInfo]: " + info);
						// Debug.Log("[SubscriptionInfo] result: " + p.subsciprtionInfo);
						// Debug.Log("[SubscriptionInfo] purchaseDate: " + p.subsciprtionInfo.purchaseDate);
						// Debug.Log("[SubscriptionInfo] expireDate: " + p.subsciprtionInfo.expireDate);
						// Debug.Log("[SubscriptionInfo] introductoryPrice: " + p.subsciprtionInfo.introductoryPrice);
				
				}
                // Decode introductoryPrice
                if (intro_json != null) {
                    IntroductoryPrice ip = JsonUtility.FromJson<IntroductoryPrice>(intro_json);
                    //Debug.Log("IntroductoryPrice> " + ip);
                    if(ip != null)
                    {
                        if (string.IsNullOrEmpty(ip.introductoryPrice)||ip.introductoryPrice=="0")
                        {
                            // No introductory price
                        }
                        else
                        {
                            //p.priceString = ip.introductoryPrice;
                            //p.price = Convert.ToDecimal(Regex.Replace(ip.introductoryPrice, "[^0-9.]", ""));                                   
                            p.introductoryPriceString = ip.introductoryPrice;
                            p.introductoryPrice = Convert.ToDecimal(Regex.Replace(ip.introductoryPrice, "[^0-9.]", ""));                                   
                        }
                    }                   
                }   
			}	                        	
#endif
#endif
        }

    }

}
