using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Datastore;

namespace Digicrafts.IAP.Pro.Settings
{		

	/// <summary>
	/// IAP object setting.
	/// </summary>
	[Serializable]
	public class IAPObjectSetting:ICloneable
	{		
		public string uid = "UID";
		public int tags = 0;
		public string title = "Title";
		public string description = "Description";
		public Sprite icon;

		virtual public System.Object Clone()
		{			
			return null;
		}
	}
		
	/// <summary>
	/// IAP currency setting.
	/// </summary>
	[Serializable]
	public class IAPCurrencySetting : IAPObjectSetting
	{
		public int amount = 10;
		public int available = -1; // -1: infinite

		public IAPObjectData data{
			get {
				IAPObjectData d = new IAPObjectData();
				d.a=this.amount;
				d.av=this.available;
				return d;
			}				
		}

		override public System.Object Clone()
		{
			IAPCurrencySetting obj = new IAPCurrencySetting();
			//
			obj.uid=this.uid;
			obj.tags=this.tags;
			obj.title=this.title;
			obj.description=this.description;
			obj.icon=this.icon;
			//
			obj.amount=this.amount;
			obj.available=this.available;
			return obj;
		}
	}

	/// <summary>
	/// IAP inventory setting.
	/// </summary>
	[Serializable]
	public class IAPInventorySetting : IAPObjectSetting
	{		
		public int price = 100;
		public string currency;
		public bool locked = false;
		public int amount = 1;
		public int available = -1; // -1: infinite
		public List<IAPProperty> properties;

		public IAPObjectData data{
			get {
				IAPObjectData d = new IAPObjectData();
				d.a=this.amount;
				d.l=this.locked;
				d.av=this.available;
				return d;
			}
		}

		public IAPInventorySetting()
		{
			this.available=-1;
		}

		override public System.Object Clone()
		{
			IAPInventorySetting obj = new IAPInventorySetting();
			//
			obj.uid=this.uid;
			obj.tags=this.tags;
			obj.title=this.title;
			obj.description=this.description;
			obj.icon=this.icon;
			//
			obj.currency=this.currency;
			obj.price=this.price;
			obj.amount=this.amount;
			obj.locked=this.locked;
			obj.available=this.available;
			obj.properties = new List<IAPProperty>();

			foreach(IAPProperty item in this.properties){
				obj.properties.Add(item.Clone());
			}

			return obj;
		}
	}
		

	/// <summary>
	/// IAP ability setting.
	/// </summary>
	[Serializable]
	public class IAPAbilitySetting : IAPObjectSetting
	{
		public int price = 100;
		public string currency;
		public bool locked = false;
		public int level = 0;
		public string maxString = "Max";
		public string lockedString = "Locked";
		public List<IAPAbilityLevel> levels;

		public IAPObjectData data{
			get {
				IAPObjectData d = new IAPObjectData();
				d.l=this.locked;
				d.lv=this.level;
				return d;
			}
		}

//		public IAPAbilitySetting()
//		{
//
//		}

		override public System.Object Clone()
		{
			IAPAbilitySetting obj = new IAPAbilitySetting();
			//
			obj.uid=this.uid;
			obj.tags=this.tags;
			obj.title=this.title;
			obj.description=this.description;
			obj.icon=this.icon;
			//
			obj.currency=this.currency;
			obj.price=this.price;
			obj.locked=this.locked;
			obj.level=this.level;
			obj.maxString=this.maxString;
			obj.lockedString=this.lockedString;
			obj.levels = new List<IAPAbilityLevel>();

			foreach(IAPAbilityLevel item in this.levels){
				obj.levels.Add(item.Clone());
			}

			return obj;
		}
	}		

	/// <summary>
	/// IAP package content.
	/// </summary>
	[Serializable]
	public class IAPContentSetting
	{		
		public string uid;
		public int amount = 1;
		public IAPType type = IAPType.Currency;

		public IAPContentSetting Clone()
		{
			IAPContentSetting obj = new IAPContentSetting();
			this.uid=obj.uid;
			this.amount=obj.amount;
			this.type=obj.type;
			return obj;
		}
	}

	/// <summary>
	/// IAP package setting.
	/// </summary>
	[Serializable]
	public class IAPPackageSetting : IAPProductSetting, ICloneable
	{		
		public bool fetchFromStore = true;
		public string currency;
		public string title = "";	
		public string description = "";	
		public int price = 0;
		public int tags = 0;
		public Sprite icon;
		public List<IAPContentSetting> content;

		public IAPObjectData data{
			get {
				IAPObjectData d = new IAPObjectData();
//				d.locked=this.locked;
				return d;
			}
		}			

		public IAPPackageSetting (string id, IAPProductType type = IAPProductType.Consumable) : base(id,type)
		{			

		}	

		public System.Object Clone()
		{
			IAPPackageSetting obj = new IAPPackageSetting(this.productId,this.productType);
			//
			obj.fetchFromStore=this.fetchFromStore;
			obj.currency=this.currency;
			obj.title=this.title;
			obj.description=this.description;
			obj.price=this.price;
			obj.tags=this.tags;
			obj.icon=this.icon;
			//
			obj.amazonProductId=this.amazonProductId;
			obj.appleProductId=this.appleProductId;
			obj.googleProductId=this.googleProductId;
			obj.macProductId=this.macProductId;
			obj.moolahProductId=this.moolahProductId;
			obj.productId=this.productId;
			obj.productType=this.productType;
			obj.samsungProductId=this.samsungProductId;
			obj.tizenProductId=this.tizenProductId;

			obj.content = new List<IAPContentSetting>();

			foreach(IAPContentSetting item in this.content){
				obj.content.Add(item.Clone());
			}

			return obj;
		}
	}		

	[Serializable]
	public class IAPGameLevelSetting : IAPObjectSetting
	{		
		
		public int price = 0;
		public string currency;
		public bool locked = true;
		public Sprite lockedIcon;
		public List<string> properties;
		public List<IAPGameSubLevel> levels;

		public IAPObjectData data{
			get {
				IAPObjectData d = new IAPObjectData();
				d.l=this.locked;
				return d;
			}
		}			

		override public System.Object Clone()
		{
			IAPGameLevelSetting obj = new IAPGameLevelSetting();
			//
			obj.uid=this.uid;
			obj.tags=this.tags;
			obj.title=this.title;
			obj.description=this.description;
			obj.icon=this.icon;
			//
			obj.currency=this.currency;
			obj.price=this.price;
			obj.locked=this.locked;
			obj.lockedIcon=this.lockedIcon;

			obj.properties= new List<string>();
			obj.levels = new List<IAPGameSubLevel>();

			foreach(string item in this.properties){
				obj.properties.Add(item);
			}

			foreach(IAPGameSubLevel item in this.levels){
				obj.levels.Add(item.Clone());
			}

			return obj;
		}
	}		

	/// <summary>
	/// IAPUI settings.
	/// </summary>
	[Serializable]
	public class IAPUISettings
	{			
		public GameObject errorDialog;
		public GameObject confirmDialog;

		public string currencyErrorString = "Not enough %currency_title%.";
		public string buyConfirmString = "Buy %title% with %price% %currency_title%.";
		public string abilityConfirmString = "Upgrade %title% with %price% %currency_title%.";
		public string iapConfirmString = "Buy %title% with %price% %currency_title% ?";
		public string consumeConfirmString = "Use %amount_consume% of %title%?";

	}

	[Serializable]
	public class IAPDatabaseSettings
	{			
		public string datastorePath = "gamesave/data.dat";
		public string playerPrefPrefix = "GAMESAVE_";
		public IAPDatastoreType datastoreType = IAPDatastoreType.PlayerPref;
		public string cryptoHash = "HASH_KEY_HERE";
	}

	/// <summary>
	/// IAP inventory settings.
	/// </summary>
	[Serializable]
	public class IAPSettings
	{			
		public static string[] DefaultTag = {"IAP","IAP_Fetch","Ability","Reserved 1","Reserved 2"};

		public List<IAPCurrencySetting> currencyList;
		public List<IAPInventorySetting> inventoryList;
		public List<IAPAbilitySetting> abilityList;
		public List<IAPGameLevelSetting> gameList;
		public List<IAPPackageSetting> packageList;
		public List<string> tagList = new List<string>(DefaultTag);
		public IAPAdvancedSetting advancedSettings;
		public IAPDatabaseSettings databaseSettings;
		public IAPUISettings uiSettings;
		public bool fetchDataOnStart = false;
	}

}
