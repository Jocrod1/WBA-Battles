using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Digicrafts.IAP.Pro.Editor
{	
	[Serializable]
	public class IAPEditorSettings {				

		public string key;
		public List<string> currencyList;
		public List<string> inventoryList;
		public List<string> abilityList;
		public List<string> gameList;
		public List<string> packageList;
		public List<string> tagList;

	}

}
