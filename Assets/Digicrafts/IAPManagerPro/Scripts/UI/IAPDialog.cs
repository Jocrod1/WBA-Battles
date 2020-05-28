using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Digicrafts.IAP.Pro.UI
{
	public delegate void ConfirmButtonPressedDelegate (IAPDialog obj);
	public delegate void CancelButtonPressedDelegate (IAPDialog obj);

	public class IAPDialog : MonoBehaviour {

		public event ConfirmButtonPressedDelegate OnConfirmButtonPressed;
		public event CancelButtonPressedDelegate OnCancelButtonPressed;

		// Use this for initialization
		void Start () {
		
			IAPUIUtility.AddButtonCallback(
				gameObject,(GameObject go)=>{	
					if(go.name=="confirm_button"){
						if(OnConfirmButtonPressed!=null) OnConfirmButtonPressed(this);
					} else if(go.name=="cancel_button"){
						if(OnCancelButtonPressed!=null) OnCancelButtonPressed(this);
					}
					Destroy(gameObject);
				}
			);

		}

		/// <summary>
		/// Show the specified template, msg, confirmCallback and cancelCallback.
		/// </summary>
		/// <param name="template">Template.</param>
		/// <param name="msg">Message.</param>
		/// <param name="confirmCallback">Confirm callback.</param>
		/// <param name="cancelCallback">Cancel callback.</param>
		public static void Show(GameObject template, string msg, ConfirmButtonPressedDelegate confirmCallback = null , CancelButtonPressedDelegate cancelCallback = null){

			// Get the Canvas
			GameObject canvas = GameObject.Find("Canvas") as GameObject;

			if(canvas!=null && template!=null){

				GameObject obj = Instantiate(template);
				RectTransform dialogTransform = obj.GetComponent<RectTransform>();
				RectTransform canvasTransform = canvas.GetComponent<RectTransform>();

				if(dialogTransform!=null&&canvasTransform!=null){
					IAPDialog dialog = obj.AddComponent<IAPDialog>();
					dialogTransform.SetParent(canvasTransform);
					dialogTransform.offsetMin=new Vector2(0,0);
					dialogTransform.offsetMax=new Vector2(0,0);
					dialogTransform.localScale=new Vector3(1,1,1);

					Text[] txts = obj.GetComponentsInChildren<Text>();
					foreach(Text txt in txts){
						if(txt.name=="message")
							txt.text=msg;
					}

					if(confirmCallback!=null)
						dialog.OnConfirmButtonPressed+=confirmCallback;

					if(cancelCallback!=null)
						dialog.OnCancelButtonPressed+=cancelCallback;

				}

			}
		}

	}
}
