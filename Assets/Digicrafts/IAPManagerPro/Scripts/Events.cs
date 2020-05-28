using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;
using Digicrafts.IAP.Pro.Core;

namespace Digicrafts.IAP.Pro.Events
{	
	[Serializable]
	public class CurrencyUpdatedEvent : UnityEvent<IAPCurrency>{}
	[Serializable]
	public class InventoryUpdatedEvent : UnityEvent<IAPInventory>{}
	[Serializable]
	public class AbilityUpdatedEvent : UnityEvent<IAPAbility>{}
	[Serializable]
	public class PackageUpdatedEvent : UnityEvent<IAPPackage>{}
	[Serializable]
	public class GameLevelUpdatedEvent : UnityEvent<IAPGameLevel>{}
	[Serializable]
	public class CurrencyNotEnoughEvent : UnityEvent<IAPCurrency>{}

	[Serializable]
	public class GameLevelSelectEvent : UnityEvent<IAPGameLevel,int>{}

	[Serializable]
	public class ConsumeSuccessEvent : UnityEvent<IAPObject>{}
	[Serializable]
	public class ConsumeFailEvent : UnityEvent<IAPObject>{}

	// delegate

	public delegate void CurrencyUpdatedDelegate (IAPCurrency currency);
	public delegate void InventoryUpdatedDelegate (IAPInventory inventory);
	public delegate void AbilityUpdatedDelegate (IAPAbility ability);
	public delegate void PackageUpdatedDelegate (IAPPackage package);
	public delegate void GameLevelUpdatedDelegate (IAPGameLevel level);
	public delegate void CurrencyNotEnoughDelegate (IAPCurrency currency);
	public delegate void ConsumeSuccessDelegate (IAPObject obj);
	public delegate void ConsumeFailDelegate (IAPObject obj);

}