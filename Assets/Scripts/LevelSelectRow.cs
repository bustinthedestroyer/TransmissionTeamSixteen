using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectRow : MonoBehaviour {

	public Text LevelNumber, LevelName;
	public Button SelectButton;
	public Transmission transmission;

	public int LevelId;
	public string LevelTitle;
	public string LevelTransmission;
	

	void Start(){
		Refresh();
	}

	public void Refresh()
	{
		LevelNumber.text = LevelId.ToString() + ".";
		LevelName.text = LevelTitle;

		Debug.Log(PlayerPrefs.GetInt("HighestLevel",0));
		int HighestLevel = PlayerPrefs.GetInt("HighestLevel",0);

		if(HighestLevel+1 >= LevelId){
			SelectButton.interactable  = true;
		}else{
			SelectButton.interactable  = false;
		}
	}

	public void StartLevel(){
		transmission.StartGame(LevelId, LevelTitle, LevelTransmission);
	}
}
