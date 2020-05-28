using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

using Digicrafts.Serialization;
using Digicrafts.IAP.Pro.Settings;
using Digicrafts.IAP.Pro.Datastore;

namespace Digicrafts.IAP.Pro.Core
{	

	/// <summary>
	/// IAP type.
	/// </summary>
	public enum IAPType
	{
		Currency,
		Inventory,
		Ability,
		GameLevel,
		InAppPurchase
	}		

	/// <summary>
	/// IAP inventory type.
	/// </summary>
	public enum IAPInventoryType
	{
		Consumable,
		NonConsumable
	}		

	/// <summary>
	/// IAP property.
	/// </summary>
	[Serializable]
	public class IAPProperty
	{
		public string name = "name_here";
		public int value = 0;

		public IAPProperty Clone()
		{
			IAPProperty obj = new IAPProperty();
			obj.name=this.name;
			obj.value=this.value;
			return obj;
		}
	}

	// Data Objects

	[Serializable]
	public class IAPObjectData
	{		
		public int a=0; // amount
		public int av=-1; // available
		public bool l = false; // locked
		public int lv = 0; // level
		public string ps; // propertiesString

		[NonSerialized]
		public Dictionary<string,string> propertiesString;
		[NonSerialized]
		public Dictionary<string,int[]> properties;

		public IAPObjectData(){
			
		}
	}
		

	// Data Context

	/// <summary>
	/// IAP object.
	/// </summary>
	public class IAPObject 
	{

		protected IAPObjectData _data;
		protected Action<IAPObject> _callback;

		public string uid;
		public string title = "Title Here";
		public string description = "Description Here";
		public string currency;
		public int tags = 0;
		public int price = 0;
		public Sprite icon;

		// Getter/Setter
		public int amount
		{
			get { return _data.a; }
		}
		public int available
		{
			get { return _data.av; }
		}
		public IAPObjectData data
		{
			get { return _data; }
		}

		public IAPObject(string uid, IAPObjectData data, Action<IAPObject> callback = null)
		{			
			this.uid=uid;
			this._callback=callback;
			InitData(data);
		}			

		/// <summary>
		/// Consume the specified amount.
		/// </summary>
		/// <param name="amount">Amount.</param>
		public bool Consume(int amount, bool delaySave = false)
		{
			if(amount>0&&_data.a>=amount)
			{
				_data.a -= amount;
				Save(delaySave);
			} else {				
				// Error not enough currency
				Error("not_enough_currency");
				return false;
			}

			return true;
		}

		/// <summary>
		/// Refill the specified amount.
		/// </summary>
		/// <param name="amount">Amount.</param>
		public bool Refill(int amount, bool delaySave = false)
		{
			if(amount>0)
			{
				_data.a += amount;
                if (!(this is IAPCurrency)){
                    if (_data.av != -1) {
                        if (_data.av >= amount)
                            _data.av -= amount;
                        else
                            return false;
                    }
                }
				Save(delaySave);
			} else {
				return false;
			}
			return true;
		}			

		/// <summary>
		/// Unlock the specified package.
		/// </summary>
		/// <param name="delaySave">If set to <c>true</c> delay save.</param>
		virtual public void Unlock(bool delaySave = false)
		{
			_data.l=false;
			Save(delaySave);
		}

		/// <summary>
		/// Lock the specified package
		/// </summary>
		/// <param name="delaySave"></param>
		virtual public void Lock(bool delaySave = false)
		{
			_data.l=true;
			Save(delaySave);
		}

		// Private Helpers

		virtual protected void InitData(IAPObjectData data)
		{
			_data=data;
		}

		protected void Save(bool delaySave=false)
		{
            //Debug.Log("Save: " + uid);
			if(delaySave)
				IAPDatastore.DelaySave();	
			if(_callback!=null) _callback(this);
		}
			
		virtual protected void Error(string msg)
		{
			if(_callback!=null)
				_callback(null);
		}
	}
		

	/// <summary>
	/// IAP currency.
	/// </summary>
	public class IAPCurrency : IAPObject
	{
		private Action<IAPCurrency> _errorCallback;

		public IAPCurrency(string uid, IAPCurrencySetting s, IAPObjectData d, Action<IAPObject> callback = null,  Action<IAPCurrency> errorCallback = null) : base(uid,d,callback){

			title=s.title;
			icon=s.icon;
			tags=s.tags;
			_errorCallback=errorCallback;
		}	

		override protected void Error(string msg)
		{			
			if(_errorCallback!=null) _errorCallback(this);
		}
	}

	/// <summary>
	/// IAP item.
	/// </summary>
	public class IAPInventory :IAPObject
	{				
		public IAPInventoryType type;
		public Dictionary<string,IAPProperty> properties;		

		public IAPInventory(string uid, IAPInventorySetting s, IAPObjectData d, Action<IAPObject> callback = null) : base(uid,d,callback)
		{
			title=s.title;
			description=s.description;
			currency=s.currency;
			price=s.price;
			icon=s.icon;
			tags=s.tags;
			// 
			properties=new Dictionary<string, IAPProperty>();
			foreach(IAPProperty c in s.properties){
				properties.Add(c.name,c);
			}

			// Convert the data from json to dictionary			
			_data.propertiesString=new Dictionary<string, string>();

			// Build the new data
			if(string.IsNullOrEmpty(d.ps)){

				// Loop each properties and build a new array
				foreach(IAPProperty c in s.properties){				
					_data.propertiesString.Add(c.name,c.value.ToString());
				}				

				// Convert the properties to json string
				if(s.properties.Count>0)
					_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));


			} else {
			// Update the data if data exist

				bool needSave = false;
				// Convert properties from json string
				_data.propertiesString=JsonUtility.FromJson<Serializer<string, string>>(d.ps).ToDictionary();

				// loop each properties
				foreach(KeyValuePair<string,IAPProperty> prop in properties){									
					// if already in the json string
					if(_data.propertiesString.ContainsKey(prop.Key)){																								
						prop.Value.value=Int32.Parse(_data.propertiesString[prop.Key]);						
						// needSave=true;
					} else {
						_data.propertiesString.Add(prop.Key,prop.Value.value.ToString());
						needSave=true;
					}
				}							

				// Convert the properties to json string
				if(s.properties.Count>0)
					_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));

				if(needSave) Save();
			}		
		}	


		/// <summary>
		/// 
		/// </summary>
		/// <param name="prop"></param>
		/// <returns></returns>
		public int GetPropertyValue(string prop)
		{
			int result=0;
			if(properties.ContainsKey(prop)){				
				result=properties[prop].value;				
			}
			return result;
		}

		/// <summary>
		/// Set the property value
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool SetPropertyValue(string prop, int value)
		{
			bool result=false;
			if(properties.ContainsKey(prop)){				
				properties[prop].value=value;				
				_data.propertiesString[prop]=value.ToString();
				_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));
				result=true;
				Save();				
			}
			return result;
		}	

		public void SaveProperties()
		{
			// Convert the data from json to dictionary			
			_data.propertiesString=new Dictionary<string, string>();
						
			// Loop each properties and build a new array
			foreach(KeyValuePair<string,IAPProperty> c in properties){				
				_data.propertiesString.Add(c.Key,c.Value.value.ToString());
			}				

			// Convert the properties to json string
			if(properties.Count>0)
				_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));

			Save();
		}		

		/// <summary>
		/// Ises the locked.
		/// </summary>
		/// <returns><c>true</c>, if locked was ised, <c>false</c> otherwise.</returns>
		public bool isLocked()
		{
			return _data.l;
		}
	}

	/// <summary>
	/// IAP ability level.
	/// </summary>
	[Serializable]
	public class IAPAbilityLevel
	{						
		public string description = "Description Here";
		public int price = 0;
		public Sprite icon;
		public List<IAPProperty> properties;

		public IAPAbilityLevel Clone()
		{
			IAPAbilityLevel obj = new IAPAbilityLevel();
			obj.description=this.description;
			obj.price=this.price;
			obj.icon=this.icon;

			return obj;
		}

	}
		
	/// <summary>
	/// IAP ability.
	/// </summary>
	public class IAPAbility :IAPObject
	{				

		public int level
		{
			get { return _data.lv; }
		}

		public string maxString = "Max";
		public string lockedString = "Locked";
		public List<IAPAbilityLevel> levels;

		public IAPAbility(string uid, IAPAbilitySetting s, IAPObjectData d, Action<IAPObject> callback = null) : base(uid,d,callback)
		{
			title=s.title;
			description=s.description;
			currency=s.currency;
			price=s.price;
			icon=s.icon;
			tags=s.tags;
			maxString=s.maxString;
			lockedString=s.lockedString;
			levels=s.levels;

		}			

		/// <summary>
		/// Purchase this instance.
		/// </summary>
		public bool Upgrade()
		{
			bool result = false;
			// Get the currency
			IAPCurrency cur = IAPInventoryManager.GetCurrency(this.currency);
			// Get the Level Object
			IAPAbilityLevel lv = null;
			if(levels!=null&&level<levels.Count-1){
				lv = levels[level];
				// Check if enough currency
				// Consume package, delay the save process until next save
				if(cur != null && cur.Consume(lv.price, true)){

					// Upgrade
					_data.lv+=1;
					result=true;
					//
					Save();
				} 

			}
			return result;
		}

		public IAPAbilityLevel GetCurrentLevel()
		{
			IAPAbilityLevel lv = null;

			if(levels!=null&&level<levels.Count){
				lv = levels[level];
			}

			return lv;
		}

		/// <summary>
		/// Ises the locked.
		/// </summary>
		/// <returns><c>true</c>, if locked was ised, <c>false</c> otherwise.</returns>
		public bool isLocked()
		{
			return _data.l;
		}
	}

	/// <summary>
	/// IAP package content.
	/// </summary>
	public class IAPPackageContent
	{				
		public int amount = 0;
		public IAPObject obj;
	}

	/// <summary>
	/// IAP package.
	/// </summary>
	public class IAPPackage : IAPObject
	{		

		public bool fetchFromStore = true;
		public string priceString;
		public string isoCurrencyCode;
		public List<IAPPackageContent> content;
		public IAPProductType productType;
		public IAPProduct product = null;

		private Action<IAPPackage> _purchaseCallback;
		private Dictionary<string,IAPPackageContent> _contentDictionary;

		public IAPPackage(string uid, IAPPackageSetting s, IAPObjectData d, Action<IAPObject> callback = null) : base(uid,d,callback)
		{
//			uid=s.productId;
			fetchFromStore=s.fetchFromStore;
			currency=s.currency;
			price=s.price;
			content=new List<IAPPackageContent>();
			icon=s.icon;
			title=s.title;
			description=s.description;
			tags=s.tags;
			productType=s.productType;

			// Create the dictionary for finding IAPPackageContent
			_contentDictionary=new Dictionary<string, IAPPackageContent>();

			// Loop and create IAPPackageContent for each setting
			foreach(IAPContentSetting c in s.content){
				IAPPackageContent item = new IAPPackageContent();
				item.amount=c.amount;
				if(c.type==IAPType.Currency){
					item.obj=IAPInventoryManager.GetCurrency(c.uid);
				} else if(c.type==IAPType.Inventory){
					item.obj=IAPInventoryManager.GetInventory(c.uid);
				} else if(c.type==IAPType.GameLevel){
					item.obj=IAPInventoryManager.GetGameLevel(c.uid);
				}
				content.Add(item);
				_contentDictionary.Add(c.uid,item);
			}

			// Add event for real money purchase
			if(fetchFromStore){
				// Debug.Log("fetchFromStore");
				IAPManager.OnIAPProcessPurchase+=handleOnIAPProcessPurchase;				
			}
		}

		/// <summary>
		/// Gets the content.
		/// </summary>
		/// <returns>The content.</returns>
		/// <param name="uid">Uid.</param>
		public IAPPackageContent GetContent(string uid)
		{
			if(_contentDictionary.ContainsKey(uid))
				return _contentDictionary[uid];
			else
				return null;
		}			

		/// <summary>
		/// Purchase this instance.
		/// </summary>
		public bool Purchase(Action<IAPPackage> callback = null)
		{
			bool result = false;
			if(fetchFromStore){
//				Debug.Log("purcahse " + _callback);
				_purchaseCallback=callback;
				IAPManager.OnIAPProcessPurchase+=handleOnIAPProcessPurchaseInternal;
				IAPManager.OnIAPPurchaseFailed+=handleOnIAPPurchaseFailedInternal;
				IAPManager.PurchaseProduct(uid);
			} else {
				if(productType==IAPProductType.NonConsumable&&amount>0)
				{
					//For non-consumable products, only purcahse one
				} else {
					// Get the currency
					IAPCurrency cur = IAPInventoryManager.GetCurrency(this.currency);
					// Check if enough currency
					if(cur != null && cur.Consume(this.price)){				
						DeliveryProduct();
						result=true;
					} 
				}
			}
			return result;
		}

		private void DeliveryProduct()
		{				
			// Refill this package
			Refill(1,true);
			// Loop the content in package and delivery the contents
			foreach(IAPPackageContent item in content)
			{												
				if(item!=null){                    
                    // IAPGameLevel, unlock
                    if (item.obj is IAPGameLevel){
						item.obj.Refill(1,true);
						item.obj.Unlock(true);
					} else if(item.obj is IAPInventory){						
						item.obj.Unlock(true);
						item.obj.Refill(item.amount,true);
					} else {
                        //Debug.Log("Refill: " + item.obj.uid + " amount: " + item.amount);
                        item.obj.Refill(item.amount,true);
					}
				}
			}
			Save();
		}
		
		/// <summary>
		/// Unlock the specified package.
		/// </summary>
		/// <param name="delaySave">If set to <c>true</c> delay save.</param>
		override public void Unlock(bool delaySave = false)
		{
			//Debug.Log("Unlock  Item: " + uid + " content: " + content.Count);

			foreach(IAPPackageContent item in content)
			{												
				// if(item!=null){	
					//Debug.Log("Unlock Package Item: " + item.obj.uid);			
					// IAPGameLevel, unlock
					if(item.obj is IAPGameLevel){						
						item.obj.Unlock(true);
					} else if(item.obj is IAPInventory){						
						item.obj.Unlock(true);						
					}
				// }
			}			
			base.Unlock(false);
		}

		/// <summary>
		/// Lock the specified package
		/// </summary>
		/// <param name="delaySave"></param>
		override public void Lock(bool delaySave = false)
		{
			foreach(IAPPackageContent item in content)
			{												
				if(item!=null){					
					// IAPGameLevel, unlock
					if(item.obj is IAPGameLevel){						
						item.obj.Lock(true);
					} else if(item.obj is IAPInventory){						
						item.obj.Lock(true);						
					}
				}
			}			
			base.Lock(false);
		}



		// Event when purchase ok
		private void handleOnIAPProcessPurchase(IAPProduct p, string transactionID, string receipt)
		{			
			// Debug.Log("handleOnIAPProcessPurchase"+product.id+" this is" + this.uid);
			if(this.uid==p.id) DeliveryProduct();			
		}		

		private void handleOnIAPProcessPurchaseInternal(IAPProduct p, string transactionID, string receipt)
		{						
			if(_purchaseCallback!=null) _purchaseCallback(this);			
			IAPManager.OnIAPProcessPurchase-=handleOnIAPProcessPurchaseInternal;
			IAPManager.OnIAPPurchaseFailed-=handleOnIAPPurchaseFailedInternal;
		}

		// Event when a purchase failed
		private void handleOnIAPPurchaseFailedInternal(IAPProduct p, string failureReason){
			IAPManager.OnIAPProcessPurchase-=handleOnIAPProcessPurchaseInternal;
			IAPManager.OnIAPPurchaseFailed-=handleOnIAPPurchaseFailedInternal;
		}

	}

	[Serializable]
	public class IAPGameSubLevel
	{
		// info
		public string title = "";
		public Sprite icon;	
		public bool locked = true;
		public int price = 0;

		public IAPGameSubLevel Clone()
		{
			IAPGameSubLevel obj = new IAPGameSubLevel();
			obj.title=this.title;
			obj.locked=this.locked;
			obj.icon=this.icon;

			return obj;
		}
	}

	public class IAPGameLevel : IAPObject
	{		
		public Sprite lockedIcon;
		public List<IAPGameSubLevel> levels;
		public Dictionary<string,int[]> properties
		{
			get{
				return _data.properties;
			}
		}

		public IAPGameLevel(string uid, IAPGameLevelSetting s, IAPObjectData d, Action<IAPObject> callback = null) : base(uid,d,callback)
		{
			title=s.title;
			description=s.description;
			currency=s.currency;
			price=s.price;
			icon=s.icon;
			tags=s.tags;
			//
			lockedIcon=s.lockedIcon;
			levels=s.levels;

			// Convert the data from json to dictionary
			_data.properties=new Dictionary<string, int[]>();
			_data.propertiesString=new Dictionary<string, string>();

			// add default "locked" property
			if(!s.properties.Contains<string>("locked"))
				s.properties.Add("locked");

			// Build the new data
			if(string.IsNullOrEmpty(d.ps)){

				// Loop each properties and build a new array
				foreach(string prop in s.properties){
					int[] p = Enumerable.Repeat(0, levels.Count).ToArray();
					if(prop=="locked")
						for(int i=0;i<p.Length;i++) p[i]=s.levels[i].locked?1:0;
					_data.properties.Add(prop,p);
					_data.propertiesString.Add(prop,JsonHelper.ToJson(p));
				}					

				// Convert the properties to json string
				if(s.properties.Count>0)
					_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));

//				Save();

			} else {
			// Update the data if data exist

				bool needSave = false;
				// Convert properties from json string
				_data.propertiesString=JsonUtility.FromJson<Serializer<string, string>>(d.ps).ToDictionary();

				// loop each properties
				foreach(string prop in s.properties){
					// Debug.Log(prop);
					// if already in the json string
					if(_data.propertiesString.ContainsKey(prop)){						
						// convert json to array
						int[] p = JsonHelper.FromJson<int>(_data.propertiesString[prop]);
						// rebuild array if length < levels.count
						if(p.Length<levels.Count){							
							List<int> temp = new List<int>(p);
							// Add extra data
							for(int i=p.Length;i<levels.Count;i++) temp.Add(0);
							// convert to array
							p=temp.ToArray();
							// for locked properties
							if(prop=="locked")
								for(int i=0;i<p.Length;i++) p[i]=s.levels[i].locked?1:0;
							// Re assign the data
							_data.propertiesString[prop]=JsonHelper.ToJson(p);
							needSave=true;
						}
//						Debug.LogFormat("prop {0} json {1}",prop,_data.propertiesString[prop]);
						//
						_data.properties.Add(prop,p);
					} else {
					// if not, build a new one
						int[] p = Enumerable.Repeat(0, levels.Count).ToArray();
						if(prop=="locked")
							for(int i=0;i<p.Length;i++) p[i]=s.levels[i].locked?1:0;
						_data.properties.Add(prop,p);
						_data.propertiesString.Add(prop,JsonHelper.ToJson(p));
						needSave=true;
					}
						
				}

				// Convert the properties to json string
				if(s.properties.Count>0)
					_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));

				if(needSave) Save();
			}
				
		}

		/// <summary>
		/// Ises the locked.
		/// </summary>
		/// <returns><c>true</c>, if locked was ised, <c>false</c> otherwise.</returns>
		public bool isLocked()
		{
			return _data.l;
		}

		/// <summary>
		/// Gets the property value.
		/// </summary>
		/// <returns>The property value.</returns>
		/// <param name="prop">Property.</param>
		/// <param name="level">Level.</param>
		public int GetPropertyValue(string prop, int level)
		{
			int result=0;
			if(properties.ContainsKey(prop)){
				if(level<properties[prop].Length){
					result=properties[prop][level];
				}
			}
			return result;
		}

		/// <summary>
		/// Sets the property value.
		/// </summary>
		/// <returns><c>true</c>, if property value was set, <c>false</c> otherwise.</returns>
		/// <param name="prop">Property.</param>
		/// <param name="level">Level.</param>
		/// <param name="value">Value.</param>
		public bool SetPropertyValue(string prop, int level, int value)
		{
			bool result=false;
			if(properties.ContainsKey(prop)){
				if(level<properties[prop].Length){
					properties[prop][level]=value;
					// _data.propertiesString.Add(prop,JsonHelper.ToJson(properties[prop]));
					_data.propertiesString[prop]=JsonHelper.ToJson(properties[prop]);
					_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));
					result=true;
					Save();
				}
			}
			return result;
		}

		public bool UnlockLevel(int level)
		{
			bool result=false;
			if(properties.ContainsKey("locked")){
				if(level<properties["locked"].Length){
					// Debug.LogFormat("level {0} locked {1}",level, properties["locked"][level]);
					properties["locked"][level]=0;
					// Convert the data to json
					_data.propertiesString["locked"]=JsonHelper.ToJson(properties["locked"]);
					// Serialize the data
					_data.ps=JsonUtility.ToJson(new Serializer<string,string>(_data.propertiesString));
					result=true;
					Save();
				}
			}
			return result;
		}
	}

}