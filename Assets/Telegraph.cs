using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Telegraph : MonoBehaviour {

	public AudioSource dot;
	public AudioSource dash;

	private float time;

	public float dotPause;
	public float dashPasue;
	private float timeTimer;
	public string PlayerInput;
	public Text PlayerMorseCode;


	private bool telegraphing = false;

	void Start () {
		PlayerInput = "";
	}

	void Update () {
		if(Input.GetKeyDown("space") && !telegraphing){
			timeTimer = Time.time;
			telegraphing = true;
		}

		if(Input.GetKeyUp("space") && telegraphing){
			if(Time.time < timeTimer + dotPause){
				dot.Play();
				PlayerInput += ".";
			}else{
				dash.Play();
				PlayerInput += "_";
			}
			PlayerMorseCode.text = PlayerInput;
			telegraphing = false;
		}
	}
}
