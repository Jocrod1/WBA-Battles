using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

using Digicrafts.IAP.Pro.Core;
using Digicrafts.IAP.Pro;

public class Demo_Game : MonoBehaviour {

	public GameObject storePanel;
	public GameObject gemPanel;
	public GameObject coinPanel;
	public GameObject upgradePanel;


	public Button gemButton;
	public Button coinButton;
	public Button upgradeButton;

	void Awake () {

		if(storePanel!=null){
			storePanel.SetActive(false);
			gemPanel.SetActive(false);
			coinPanel.SetActive(false);
			upgradePanel.SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void GotoGame(string name){
		
		SceneManager.LoadScene(name);
	}

	public void ShowStore(string type)
	{
		storePanel.SetActive(true);
		switch(type){
		case "coins":
			ShowCoinStore();
			break;
		case "gems":
			ShowGemStore();
			break;
		case "upgrade":
			ShowUpgradeStore();
			break;
		}
	}

	public void HideStore()
	{
		HideGemStore();
		HideCoinStore();
		HideUpgradeStore();
		storePanel.SetActive(false);
	}

	public void ShowGemStore()
	{
		gemPanel.SetActive(true);
		gemButton.interactable=false;
		HideCoinStore();
		HideUpgradeStore();
	}

	public void ShowCoinStore()
	{
		coinPanel.SetActive(true);
		coinButton.interactable=false;
		HideGemStore();
		HideUpgradeStore();
	}

	public void ShowUpgradeStore()
	{
		upgradePanel.SetActive(true);
		upgradeButton.interactable=false;
		HideCoinStore();
		HideGemStore();
	}

	public void HideGemStore()
	{
		gemPanel.SetActive(false);
		gemButton.interactable=true;
	}

	public void HideCoinStore()
	{
		coinPanel.SetActive(false);
		coinButton.interactable=true;
	}

	public void HideUpgradeStore()
	{
		upgradePanel.SetActive(false);
		upgradeButton.interactable=true;
	}

	public void GotoSubLevel(IAPGameLevel level, int index)
	{
		Debug.Log(level.levels[index].locked);
		if(level.levels[index].locked){

			// Do somthing if locked

		} else {
			GotoGame("Demo_Game_Play");

		}
	}
}
