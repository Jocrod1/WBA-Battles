using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using Digicrafts.IAP.Pro;
using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro.Settings;

namespace Digicrafts.IAP.Pro.UI
{
	/// <summary>
	/// IAP property image.
	/// </summary>
	[DisallowMultipleComponent]
	public class IAPPropertyImage : MonoBehaviour {

		// Animated number
		public string property;
		public Sprite[] icons;
		public IAPGameLevel obj;
		public int level=0;

		// Use this for initialization
		void Start () {
			UpdateImage();
		}			

		/// <summary>
		/// Updates the image.
		/// </summary>
		public void UpdateImage(){

			if(obj!=null){
				Image img = gameObject.GetComponent<Image>();
				if(img!=null){					
					if(obj.data!=null&&obj.data.properties!=null&&obj.data.properties.ContainsKey(property)){						
						if(level<obj.data.properties[property].Length){							
							int val = obj.data.properties[property][level];
							if(val<icons.Length&&icons[val]!=null){								
								img.sprite=icons[val];
							}
						}
					}					
				}
			}
		}

	}
}