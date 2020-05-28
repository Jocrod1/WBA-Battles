using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;

using Digicrafts.IAP.Pro.UI;
using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;
using Digicrafts.IAP.Pro.Datastore;
using Digicrafts.IAP.Pro.Events;

namespace Digicrafts.IAP.Pro 
{		

	/// <summary>
	/// IAP inventory manager provides methods to managing the data in IAP Manager Pro.
	/// </summary>
	public class IAPInventoryManager 
	{

		// Events
		public static event CurrencyUpdatedDelegate OnCurrencyUpdated;
		public static event InventoryUpdatedDelegate OnInventoryUpdated;
		public static event AbilityUpdatedDelegate OnAbilityUpdated;
		public static event PackageUpdatedDelegate OnPackageUpdated;
		public static event GameLevelUpdatedDelegate OnGameLevelUpdated;
		public static event CurrencyNotEnoughDelegate OnCurrencyNotEnough;

		public static event InitializedDelegate OnIAPInitialized;
		public static event InitializeFailedDelegate OnIAPInitializeFailed;
		public static event PurchaseStartDelegate OnIAPPurchaseStart;
		public static event ProcessPurchaseDelegate OnIAPProcessPurchase;
		public static event PurchaseFailedDelegate OnIAPPurchaseFailed;
		public static event PurchaseDeferredDelegate OnIAPPurchaseDeferred;
		public static event TransactionsRestoredDelegate OnIAPTransactionsRestored;

		// Private

		private static IAPInventoryManager _instance;
		private static IAPDatastore _datastore;
		private static IAPUISettings _uiSettings;
		private static List<IAPPackageSetting> _packageSettings;
		private static IAPAdvancedSetting _advancedSetting;

		private static Dictionary<string, IAPCurrency> _currencyIndex;
		private static Dictionary<string, IAPInventory> _inventoryIndex;
		private static Dictionary<string, IAPAbility> _abilityIndex;
		private static Dictionary<string, IAPPackage> _packageIndex;
		private static Dictionary<string, IAPGameLevel> _gameIndex;

		private static Dictionary<string, List<IAPInventory>> _inventoryTagDictionary;
		private static Dictionary<string, List<IAPAbility>> _abilityTagDictionary;
		private static Dictionary<string, List<IAPPackage>> _packageTagDictionary;
		private static List<string> _tags;

		private static bool _initing=false;

		/// <summary>
		/// Gets the user interface settings.
		/// </summary>
		/// <value>The user interface settings.</value>
		public static IAPUISettings uiSettings
		{
			get{
				return _uiSettings;
			}
		}

		/// <summary>
		/// Create the IAPInventoryManager instance. Call this before using any method.
		/// </summary>
		/// <param name="settings">Settings.</param>
		public static IAPInventoryManager Create(IAPSettings settings)
		{			

			// Create instance of the IAPManager
			if (_instance == null){

				// Check if settings valid
				if (settings!=null)
				{													

					_packageSettings = settings.packageList;
					_uiSettings=settings.uiSettings;
					_advancedSetting=settings.advancedSettings;

					// Create datastore
					switch(settings.databaseSettings.datastoreType){
					case IAPDatastoreType.PlayerPref:
						_datastore=new IAPPlayerPrefDatastore(settings.databaseSettings.playerPrefPrefix);
						break;
					case IAPDatastoreType.TextFile:
						_datastore=new IAPTextFileDatastore(settings.databaseSettings.datastorePath);
						break;
					case IAPDatastoreType.XORBinaryFile:
						_datastore=new IAPBinaryFileDatastore(settings.databaseSettings.datastorePath);
						break;
					case IAPDatastoreType.AESEncryptedFile:
						string hash = settings.databaseSettings.cryptoHash;
						string salt = SystemInfo.deviceUniqueIdentifier.Substring(0,8);
						string key = PlayerPrefs.GetString("RANDOM_KEY");
						if(string.IsNullOrEmpty(key)){
							key = IAPDatastore.Md5(DateTime.Now.ToString()).Substring(0,16);
							PlayerPrefs.SetString("RANDOM_KEY",key);
						}
						_datastore=new IAPEncryptedFileDatastore(settings.databaseSettings.datastorePath,hash,key,salt);

						break;
					}	
					_datastore.Build();


					// Read the data from database
					Dictionary<string,IAPObjectData> currencyListData = _datastore.GetGroup("currencyList");
					Dictionary<string,IAPObjectData> abilityListData = _datastore.GetGroup("abilityList");
					Dictionary<string,IAPObjectData> inventoryListData = _datastore.GetGroup("inventoryList");
					Dictionary<string,IAPObjectData> packageListData = _datastore.GetGroup("inventoryList");
					Dictionary<string,IAPObjectData> gameListData = _datastore.GetGroup("gameList");

					// Create the object context list
					_currencyIndex = new Dictionary<string, IAPCurrency>();
					_inventoryIndex = new Dictionary<string, IAPInventory>();
					_abilityIndex = new Dictionary<string, IAPAbility>();
					_packageIndex = new Dictionary<string, IAPPackage>();
					_gameIndex = new Dictionary<string, IAPGameLevel>();

					_inventoryTagDictionary = new Dictionary<string, List<IAPInventory>>();
					_abilityTagDictionary = new Dictionary<string, List<IAPAbility>>();
					_packageTagDictionary = new Dictionary<string, List<IAPPackage>>();
					_tags=settings.tagList;

					// check if need to save
					bool needSave = false;
					// Tags
					if(settings.tagList != null){
						// Loop the settings and fill in the list
						foreach(string s in IAPSettings.DefaultTag)
						{
							_inventoryTagDictionary.Add(s,new List<IAPInventory>());
							_packageTagDictionary.Add(s,new List<IAPPackage>());
							_abilityTagDictionary.Add(s,new List<IAPAbility>());
						}
					}

					// Check if currency list and inventory list valid
					if(settings.currencyList != null){
						// Loop the settings and fill in the list
						foreach(IAPCurrencySetting s in settings.currencyList)
						{
							if(_currencyIndex.ContainsKey(s.uid)){
								Log("Currency identify [" + s.uid + "] already exisit");
							} else {
								IAPCurrency currency;

								// check if have saved data
								if(currencyListData.ContainsKey(s.uid)){
									currency = new IAPCurrency(s.uid,s,currencyListData[s.uid],handleCurrencySave, handleCurrencyNotEnough);
								} else {
                                    // No saved data
                                    IAPObjectData d = s.data;
									currency = new IAPCurrency(s.uid,s,d,handleCurrencySave, handleCurrencyNotEnough);
									currencyListData.Add(s.uid,d);
									needSave=true;
								}
								_currencyIndex.Add(s.uid,currency);
							}
						}
					}
					// Check if ability list and inventory list valid
					if(settings.abilityList != null){						
						// Loop the settings and fill in the list
						foreach(IAPAbilitySetting s in settings.abilityList)
						{
							if(_abilityIndex.ContainsKey(s.uid)){
								Log("Currency identify [" + s.uid + "] already exisit");
							} else {
								IAPAbility ability;
//								Debug.Log("ability "+s.data);
								// check if have saved data
								if(abilityListData.ContainsKey(s.uid)){
									ability = new IAPAbility(s.uid,s,abilityListData[s.uid],handleAbilitySave);
								} else {
									IAPObjectData d = s.data;
									ability = new IAPAbility(s.uid,s,d,handleAbilitySave);
									abilityListData.Add(s.uid,d);
									needSave=true;
								}

								// Fill the tags
								for(int i = 0; i<_tags.Count;i++){
									string tag=_tags[i];
									if(tag=="Ability"){
										ability.tags=ability.tags|4;
										_abilityTagDictionary[tag].Add(ability);
									} else {
										int index=1<<i;
										if((index&s.tags)> 0){										
											if(!_abilityTagDictionary.ContainsKey(tag))
												_abilityTagDictionary.Add(tag,new List<IAPAbility>());
											_abilityTagDictionary[tag].Add(ability);
										}
									}
								}

								_abilityIndex.Add(s.uid,ability);
							}
						}
					}
					// Check if inventory list and inventory list valid
					if(settings.inventoryList != null){
						foreach(IAPInventorySetting s in settings.inventoryList)
						{
							if(_inventoryIndex.ContainsKey(s.uid)){
								Log("inventory identify [" + s.uid + "] already exisit");
							} else {
								IAPInventory inventory;
								if(inventoryListData.ContainsKey(s.uid)){
									inventory = new IAPInventory(s.uid,s,inventoryListData[s.uid],handleInventorySave);
								} else {
									IAPObjectData d = s.data;
									inventory = new IAPInventory(s.uid,s,d,handleInventorySave);
									inventoryListData.Add(s.uid,d);
									needSave=true;
								}
								// Fill the tags
								for(int i = 0; i<_tags.Count;i++){
									string tag=_tags[i];
									int index=1<<i;
									if((index&s.tags)> 0){
										if(!_inventoryTagDictionary.ContainsKey(tag))
											_inventoryTagDictionary.Add(tag,new List<IAPInventory>());
										_inventoryTagDictionary[tag].Add(inventory);
									}
								}

								_inventoryIndex.Add(s.uid,inventory);
							}
						}
					}

					// Check if game list and inventory list valid
					if(settings.gameList != null){						
						// Loop the settings and fill in the list
						foreach(IAPGameLevelSetting s in settings.gameList)
						{
							if(_gameIndex.ContainsKey(s.uid)){
								Log("Game identify [" + s.uid + "] already exisit");
							} else {
								IAPGameLevel game;
								//								Debug.LogFormat("Game {0}",s.uid);
								// check if have saved data
								if(gameListData.ContainsKey(s.uid)){
									game = new IAPGameLevel(s.uid,s,gameListData[s.uid],handleGameLevelSave);
								} else {
									IAPObjectData d = s.data;
									game = new IAPGameLevel(s.uid,s,d,handleGameLevelSave);
									gameListData.Add(s.uid,d);
									needSave=true;
								}

								_gameIndex.Add(s.uid,game);
							}
						}
					}

					// Check if package list valid
					if(settings.packageList != null){						
						foreach(IAPPackageSetting s in settings.packageList)
						{														
							if(_packageIndex.ContainsKey(s.productId)){
								Log("Product ID [" + s.productId + "] already exisit");
							} else {				
								IAPPackage package;
								if(packageListData.ContainsKey(s.productId)){
									package = new IAPPackage(s.productId,s,packageListData[s.productId],handlePackageSave);
								} else {
									IAPObjectData d = s.data;
									package = new IAPPackage(s.productId,s,d,handlePackageSave);
									inventoryListData.Add(s.productId,d);
									needSave=true;
								}

								_packageIndex.Add(s.productId,package);
								// Fill the tags
								for(int i = 0;i<_tags.Count;i++){
									string tag=_tags[i];
									if(tag=="IAP"){
										package.tags=package.tags|1;
										_packageTagDictionary[tag].Add(package);
									} else if(tag=="IAP_Fetch"&&package.fetchFromStore){
										package.tags=package.tags|2;
										_packageTagDictionary[tag].Add(package);
									} else {
										int index=1<<i;
										if((index&s.tags)> 0){		
											if(!_packageTagDictionary.ContainsKey(tag))
												_packageTagDictionary.Add(tag,new List<IAPPackage>());
											_packageTagDictionary[tag].Add(package);
										
										}
									}
								}
							}
						}
					}						

					// Create instance
					_instance = new IAPInventoryManager();

					// Save change
					if(needSave) Save();
				}
					
				return null;
			}				

			return _instance;
		}

		/// <summary>
		/// Initialize the IAP Manager.
		/// </summary>
		public static void InitIAPManager()
		{			
			// Init the IAPManager
			if(_instance!=null && _initing==false && _packageSettings != null )
			{
				_initing=true;

				// Get the product list need to fetch
				List<IAPProductSetting> products = new List<IAPProductSetting>();
				foreach(IAPPackageSetting p in _packageSettings){
					if(p.fetchFromStore)products.Add(p);
				}

				// Add events
				IAPManager.OnIAPInitialized+=handleOnIAPInitialized;
				IAPManager.OnIAPInitializeFailed+=handleOnIAPInitializeFailed;
				IAPManager.OnIAPProcessPurchase+=handleOnIAPProcessPurchase;
				IAPManager.OnIAPPurchaseDeferred+=handleOnIAPProcessDeferred;
				IAPManager.OnIAPPurchaseFailed+=handleOnIAPPurchaseFailed;
				IAPManager.OnIAPPurchaseStart+=handleOnIAPPurchaseStart;
				IAPManager.OnIAPTransactionsRestored+=handleOnIAPTransactionsRestored;

				IAPManager.Create(products, _advancedSetting);
			}					
		}

//		public static T GetObject<T>(string uid)
//		{
//			if(uid!=null){
//				
//				Type type = typeof(T);
//
//				if(type == typeof(IAPCurrency)&&_currencyIndex.ContainsKey(uid)){
//					return (T)Convert.ChangeType(_currencyIndex[uid],typeof(T));
//				} else if(type == typeof(IAPAbility)&&_abilityIndex.ContainsKey(uid)){
//					return (T)Convert.ChangeType(_abilityIndex[uid],typeof(T));
//				} else if(type == typeof(IAPInventory)&&_inventoryIndex.ContainsKey(uid)){
//					return (T)Convert.ChangeType(_inventoryIndex[uid],typeof(T));
//				} else if(type == typeof(IAPPackage)&&_packageIndex.ContainsKey(uid)){
//					return (T)Convert.ChangeType(_packageIndex[uid],typeof(T));
//				} else if(type == typeof(IAPGameLevel)&&_gameIndex.ContainsKey(uid)){
//					return (T)Convert.ChangeType(_gameIndex[uid],typeof(T));
//				}					
//			}
//			return default(T);
//		}
//
//		public static List<T> GetObjectList<T>()
//		{
//			Type type = typeof(T);
//
//			if(type == typeof(IAPCurrency)){
//				return List<IAPCurrency>(_currencyIndex.Values);
//			} else if(type == typeof(IAPAbility)){
//				return List<IAPCurrency>(_currencyIndex.Values);
//			} else if(type == typeof(IAPInventory)){
//				return List<IAPCurrency>(_currencyIndex.Values);
//			} else if(type == typeof(IAPPackage)){
//				return List<IAPCurrency>(_currencyIndex.Values);
//			} else if(type == typeof(IAPGameLevel)){
//				return List<IAPCurrency>(_currencyIndex.Values);
//			}
//
//			return default(List<T>);
//		}

//		public static List<T> GetListByTag<T>(string tags)
//		{
//
//			List<T> list = new List<T>();
//			if(_packageIndex!=null){				
//				foreach( KeyValuePair<string, T> item in _packageIndex )
//				{					
//					T v = item.Value;
//					if((v.tags&tags)>0 || (v.tags==-1&&tags==-1) ){						
//						list.Add(v);
//					}
//				}
//			}
//			return list;
//
//			if(!string.IsNullOrEmpty(tags)){
//
//				Type type = typeof(T);
//
//				if(type == typeof(IAPCurrency)){					
//					if(_currencyIndex.ContainsKey(uid)){
//						return (T)Convert.ChangeType(_currencyIndex[uid],typeof(T));
//					}
//				} else if(type == typeof(IAPAbility)){					
//					if(_abilityIndex.ContainsKey(uid)){
//						return (T)Convert.ChangeType(_abilityIndex[uid],typeof(T));
//					}
//				} else if(type == typeof(IAPInventory)){					
//					if(_inventoryIndex.ContainsKey(uid)){
//						return (T)Convert.ChangeType(_inventoryIndex[uid],typeof(T));
//					}
//				} else if(type == typeof(IAPPackage)){					
//					if(_packageIndex.ContainsKey(uid)){
//						return (T)Convert.ChangeType(_packageIndex[uid],typeof(T));
//					}
//				} else if(type == typeof(IAPGameLevel)){					
//					if(_gameIndex.ContainsKey(uid)){
//						return (T)Convert.ChangeType(_gameIndex[uid],typeof(T));
//					}
//				}
//
//			}
//			return default(List<T>);
//		}

		/// <summary>
		/// Gets the currency by uid.
		/// </summary>
		/// <returns>The currency.</returns>
		/// <param name="uid">Uid.</param>
		public static IAPCurrency GetCurrency(string uid)
		{
			if(uid!=null&&_currencyIndex.ContainsKey(uid)){

				return _currencyIndex[uid];
			}
			return null;
		}

		/// <summary>
		/// Gets the inventory by uid.
		/// </summary>
		/// <returns>The inventory.</returns>
		/// <param name="uid">Uid.</param>
		public static IAPInventory GetInventory(string uid)
		{
			if(uid!=null&&_inventoryIndex.ContainsKey(uid)){

				return _inventoryIndex[uid];
			}
			return null;
		}

		/// <summary>
		/// Gets the ability by uid.
		/// </summary>
		/// <returns>The ability.</returns>
		/// <param name="uid">Uid.</param>
		public static IAPAbility GetAbility(string uid)
		{
			if(uid!=null&&_abilityIndex.ContainsKey(uid)){

				return _abilityIndex[uid];
			}
			return null;
		}
			

		/// <summary>
		/// Gets the package by uid.
		/// </summary>
		/// <returns>The package.</returns>
		/// <param name="uid">Uid.</param>
		public static IAPPackage GetPackage(string uid)
		{
			if(_packageIndex.ContainsKey(uid)){

				return _packageIndex[uid];
			}
			return null;
		}

		/// <summary>
		/// Gets the game level by uid.
		/// </summary>
		/// <returns>The game level.</returns>
		/// <param name="uid">Uid.</param>
		public static IAPGameLevel GetGameLevel(string uid)
		{
			if(_gameIndex.ContainsKey(uid)){

				return _gameIndex[uid];
			}
			return null;
		}
			
		/// <summary>
		/// Gets the currency list.
		/// </summary>
		/// <returns>The currency list.</returns>
		public static List<IAPCurrency> GetCurrencyList()
		{			
			return new List<IAPCurrency>(_currencyIndex.Values);
		}

		/// <summary>
		/// Gets the inventory list.
		/// </summary>
		/// <returns>The inventory list.</returns>
		public static List<IAPInventory> GetInventoryList()
		{
			return new List<IAPInventory>(_inventoryIndex.Values);
		}			

		/// <summary>
		/// Gets the ability list.
		/// </summary>
		/// <returns>The ability list.</returns>
		public static List<IAPAbility> GetAbilityList()
		{
			return new List<IAPAbility>(_abilityIndex.Values);
		}			

		/// <summary>
		/// Gets the package list.
		/// </summary>
		/// <returns>The package list.</returns>
		public static List<IAPPackage> GetPackageList()
		{			
			return new List<IAPPackage>(_packageIndex.Values);
		}	

		/// <summary>
		/// Gets the game level list.
		/// </summary>
		/// <returns>The game level list.</returns>
		public static List<IAPGameLevel> GetGameLevelList()
		{			
			return new List<IAPGameLevel>(_gameIndex.Values);
		}	

		/// <summary>
		/// Gets the inventory list by tag name.
		/// </summary>
		/// <returns>The inventory list.</returns>
		/// <param name="tag">Tag.</param>
		public static List<IAPInventory> GetInventoryListByTag(string tag)
		{
			if(tag!=null && _inventoryTagDictionary!=null && _inventoryTagDictionary.ContainsKey(tag)){

				return _inventoryTagDictionary[tag];
			}
			return null;
		}

		/// <summary>
		/// Gets the ability list by tag name.
		/// </summary>
		/// <returns>The ability list.</returns>
		/// <param name="tag">Tag.</param>
		public static List<IAPAbility> GetAbilityListByTag(string tag)
		{
			if(_abilityTagDictionary.ContainsKey(tag)){

				return _abilityTagDictionary[tag];
			}
			return null;
		}

		/// <summary>
		/// Gets the package list by tag name.
		/// </summary>
		/// <returns>The package list.</returns>
		/// <param name="tag">Tag.</param>
		public static List<IAPPackage> GetPackageListByTag(string tag)
		{
			if(_packageTagDictionary.ContainsKey(tag)){

				return _packageTagDictionary[tag];
			}
			return null;
		}

		/// <summary>
		/// Gets the inventory list by tags index.
		/// </summary>
		/// <returns>The inventory list.</returns>
		/// <param name="tags">Tags.</param>
		public static List<IAPInventory> GetInventoryListByTags(int tags)
		{				
			List<IAPInventory> list = new List<IAPInventory>();
			if(_inventoryIndex!=null){
				foreach( KeyValuePair<string, IAPInventory> item in _inventoryIndex )
				{
					IAPInventory inventory = item.Value;
					if((inventory.tags&tags)>0 || (inventory.tags==-1&&tags==-1) ){
						list.Add(inventory);
					}
				}
			}
			return list;
		}
			
		/// <summary>
		/// Gets the ability list by tags index.
		/// </summary>
		/// <returns>The ability list.</returns>
		/// <param name="tags">Tags.</param>
		public static List<IAPAbility> GetAbilityListByTags(int tags)
		{
			List<IAPAbility> list = new List<IAPAbility>();
			if(_abilityIndex!=null){
				foreach( KeyValuePair<string, IAPAbility> item in _abilityIndex )
				{
					IAPAbility ability = item.Value;
					if((ability.tags&tags)>0 || (ability.tags==-1&&tags==-1) ){
						list.Add(ability);
					}
				}
			}
			return list;
		}

		/// <summary>
		/// Gets the package list by tags index.
		/// </summary>
		/// <returns>The package list.</returns>
		/// <param name="tags">Tags.</param>
		public static List<IAPPackage> GetPackageListByTags(int tags)
		{			
			List<IAPPackage> list = new List<IAPPackage>();
			if(_packageIndex!=null){				
				foreach( KeyValuePair<string, IAPPackage> item in _packageIndex )
				{					
					IAPPackage package = item.Value;
					if((package.tags&tags)>0 || (package.tags==-1&&tags==-1) ){						
						list.Add(package);
					}
				}
			}
			return list;
		}

		/// <summary>
		/// Gets the tags name by tags index.
		/// </summary>
		/// <returns>The tags list.</returns>
		/// <param name="tags">Tags.</param>
		public static string[] GetTags(int tags)
		{
			List<string> l = new List<string>();
			// Fill the tags
			for(int i = 0;i<_tags.Count;i++){
				string tag=_tags[i];
				int index=1<<i;
				if((index&tags)> 0){		
					l.Add(tag);
				}
			}
			return l.ToArray();
		}

		/// <summary>
		/// Gets all the name of the tags.
		/// </summary>
		/// <returns>The tags name.</returns>
		public static string[] GetTagsName()
		{
			return _tags.ToArray();
		}

		/// <summary>
		/// Save current data to data store.
		/// </summary>
		public static void Save()
		{
			_datastore.Save();
		}

		/// <summary>
		/// Shows the confirm dialog. It will invoke the callback directly if no confirmDialog template set.
		/// </summary>
		/// <param name="msg">Message.</param>
		public static void ShowConfirmDialog(string msg, ConfirmButtonPressedDelegate confirmCallback = null , CancelButtonPressedDelegate cancelCallback = null)
		{			
			if(_uiSettings.confirmDialog==null){
				confirmCallback.Invoke(null);
			} else {
				IAPDialog.Show(_uiSettings.confirmDialog,msg,confirmCallback,cancelCallback);				
			}
		}

		/// <summary>
		/// Shows the error dialog. It will invoke the callback with null args if errorDialog template not set.
		/// </summary>
		/// <param name="msg">Message.</param>
		public static void ShowErrorDialog(string msg, CancelButtonPressedDelegate cancelCallback = null)
		{
			IAPDialog.Show(_uiSettings.errorDialog,msg,null,cancelCallback);
		}

		/// <summary>
		/// Log the specified message.
		/// </summary>
		/// <param name="message">Message.</param>
		public static void Log(object message)
		{
			#if UNITY_EDITOR || DEBUG
			Debug.Log("[DEBUG:IAPManagerPro] " + message);
			#endif
		}

		/// <summary>
		/// Log the specified format and args.
		/// </summary>
		/// <param name="format">Format.</param>
		/// <param name="args">Arguments.</param>
		public static void LogFormat(string format, params object[] args)
		{
			#if UNITY_EDITOR || DEBUG
			Debug.LogFormat("[DEBUG:IAPManagerPro]" + format, args);
			#endif
		}

		///////////////////////////////////////////////////////////////
		// Private

		//--- IIAPDelegate

		private static void handleOnIAPInitialized (Dictionary<string, IAPProduct> products)
		{			
			_initing=false;
			if (products != null) {
				// Loop Settings
//				Debug.Log("handleOnIAPInitialized " + OnIAPInitialized);
				List<IAPPackage> packages = IAPInventoryManager.GetPackageList();
				foreach(IAPPackage package in packages){

					//Debug.Log("Loop package: " + package);

					if(products.ContainsKey(package.uid)){	

						//Get the products
						IAPProduct product = products[package.uid];	

						// Set the title and description
						if(string.IsNullOrEmpty(package.title)) package.title=product.title;
						if(string.IsNullOrEmpty(package.description)) package.description=product.description;
						package.priceString=product.priceString;
						package.isoCurrencyCode=product.isoCurrencyCode;
						package.product=product;
//						Debug.LogFormat("uid: {0} hasReceipt {1} {2} amount {3}", package.uid, package.product.hasReceipt, package.product.rawProduct.hasReceipt, package.amount);

						// Check Subsciprtion Valid
						if(product.productType==IAPProductType.Subscription){
							//Debug.Log("Subsciprtion: " + product.id + " receipt: " + product.hasReceipt);
							if(product.subscriptionInfo.isSubscribed){
								package.Unlock();
							} else { 
								package.Lock();
							}
						}
					}
				}
			}
			IAPManager.OnIAPInitialized-=handleOnIAPInitialized;
			if(OnIAPInitialized!=null) OnIAPInitialized.Invoke(products);
		}

		private static void handleOnIAPInitializeFailed (string error)
		{
			if(OnIAPInitializeFailed!=null) OnIAPInitializeFailed.Invoke(error);
		}


		private static void handleOnIAPProcessPurchase(IAPProduct product, string transactionID, string receipt)
		{
			if(OnIAPProcessPurchase!=null) OnIAPProcessPurchase.Invoke(product,transactionID,receipt);
		}

		// Event when a purchase started
		private static void handleOnIAPPurchaseStart(IAPProduct product) {
			if(OnIAPPurchaseStart!=null) OnIAPPurchaseStart.Invoke(product);
		}			

		// Event when a purchase failed
		private static void handleOnIAPPurchaseFailed(IAPProduct product, string failureReason){
			if(OnIAPPurchaseFailed!=null) OnIAPPurchaseFailed.Invoke(product,failureReason);
		}

		// Event for deferred purcahse
		// On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
		// On non-Apple platforms this will have no effect; OnDeferred will never be called.
		private static void handleOnIAPProcessDeferred(IAPProduct product) {
			if(OnIAPPurchaseDeferred!=null) OnIAPPurchaseDeferred.Invoke(product);
		}			

		// Event for restore purchase
		// Success set to true if restore success
		private static void handleOnIAPTransactionsRestored(bool success) {
			if(OnIAPTransactionsRestored!=null) OnIAPTransactionsRestored.Invoke(success);
		}


		// Events of IAPInventoryManager
		private static void handleCurrencyNotEnough(IAPCurrency currency)
		{			
			if(OnCurrencyNotEnough!=null) OnCurrencyNotEnough(currency);
		}

		// Callback for saving
			
		private static void handleCurrencySave(IAPObject obj)
		{
            //Debug.Log("handleCurrencySave: " + obj);
			_datastore.SetValue("currencyList",obj.uid,obj.data);
			_datastore.Save();
			if(OnCurrencyUpdated!=null) OnCurrencyUpdated.Invoke(obj as IAPCurrency);
		}

		private static void handleAbilitySave(IAPObject obj)
		{
			_datastore.SetValue("abilityList",obj.uid,obj.data);
			_datastore.Save();
			if(OnAbilityUpdated!=null) OnAbilityUpdated.Invoke(obj as IAPAbility);
		}

		private static void handleInventorySave(IAPObject obj)
		{
			// Debug.Log("handleInventorySave: " + obj.uid);
			_datastore.SetValue("inventoryList",obj.uid,obj.data);
			_datastore.Save();
			if(OnInventoryUpdated!=null) OnInventoryUpdated.Invoke(obj as IAPInventory);
		}

		private static void handleGameLevelSave(IAPObject obj)
		{
			_datastore.SetValue("gameList",obj.uid,obj.data);
			_datastore.Save();
			if(OnGameLevelUpdated!=null) OnGameLevelUpdated.Invoke(obj as IAPGameLevel);
		}

		private static void handlePackageSave(IAPObject obj)
		{
			_datastore.SetValue("packageList",obj.uid,obj.data);
			_datastore.Save();
			if(OnPackageUpdated!=null) OnPackageUpdated.Invoke(obj as IAPPackage);
		}

	}		

}