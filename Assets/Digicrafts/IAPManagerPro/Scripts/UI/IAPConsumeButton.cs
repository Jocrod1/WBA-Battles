using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using Digicrafts.IAP.Pro;
using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;
using Digicrafts.IAP.Pro.Events;


namespace Digicrafts.IAP.Pro.UI
{
	[DisallowMultipleComponent]
	public class IAPConsumeButton : IAPUI {

		public int amount = 1;
		public bool useConfirmDialog = true;

		public ConsumeSuccessEvent OnConsumeSuccess;
		public ConsumeFailEvent OnConsumeFail;

		// Use this for initialization
		void Start () {

			if(uid!=null){

				IAPObject obj=null;

				//
				if(targetType==IAPType.Inventory){
					obj = IAPInventoryManager.GetInventory(uid);			
				} else if(targetType==IAPType.Currency){
					obj = IAPInventoryManager.GetCurrency(uid);			
				}
					
				if(obj!=null){

					UpdateTemplate(obj);

					// Update text
					Text[] txts = gameObject.GetComponentsInChildren<Text>();
					// InAppPurchase					
					foreach(Text txt in txts){
						if(txt.name=="amount_consume"){
							txt.text=amount.ToString();
						}
					}			

					IAPUIUtility.AddButtonCallback(
						gameObject,(GameObject go)=>{							
							if(useConfirmDialog){								
								// Construct confirm msg
								string msg = IAPInventoryManager.uiSettings.consumeConfirmString.Replace("%title%",obj.title);
								msg = msg.Replace("%description%",obj.description);
								msg = msg.Replace("%amount_consume%",amount.ToString());
								// Show confirm diaglog
								IAPInventoryManager.ShowConfirmDialog(msg,
									delegate(IAPDialog diag){
										if(obj.Consume(amount)){
											UpdateTemplate(obj);
										}
									}
								);		

							} else {
								obj.Consume(amount);
							}

						}
					);

				}	
			}				
		}							

	}
}