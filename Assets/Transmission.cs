using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Transmission : MonoBehaviour {
	public Text IncomingTransmissionText;
	public Text OutgoingTransmissionText;
	public Text TransmissionStatusText;

	public AudioSource SoundDot;
	public AudioSource SoundDash;

	private bool playing;

	void Start () {
		OutgoingTransmissionText.text = ""; 
		IncomingTransmissionText.text = "";
		StartGame();
	}

	void Update()
	{
		if(playing){
			GetTelegraphInput();
		}
	}

#region TheTelegraph Stuff
	private bool telegraphDown = false;
	private float telegraphDownTime;
	public float dotPause;
	public float dashPause;
	public string PlayerInputCode;

	void GetTelegraphInput() {
		if(Input.GetKeyDown("space") && !telegraphDown){
			telegraphDownTime = Time.time;
			telegraphDown = true;
		}

		if(Input.GetKeyUp("space") && telegraphDown){
			if(Time.time < telegraphDownTime + dotPause){
				SoundDot.Play();
				PlayerInputCode += ".";
			}else{
				SoundDash.Play();
				PlayerInputCode += "_";
			}
			OutgoingTransmissionText.text = PlayerInputCode; 
			telegraphDown = false;

			////Validate player input
			string stringToCompare = TargetCode.Substring(0, PlayerInputCode.Length);
			if(PlayerInputCode != stringToCompare){
				TransmissionStatusText.text = "bad!";
			}else{
				TransmissionStatusText.text = "good!";
			}
			Debug.Log(stringToCompare);
		}
		

	}

#endregion

#region Audio

	public float CodePause;
	IEnumerator PlayCode(string codeToPlay){


		IncomingTransmissionText.text = "";

		List<char> codeList = new List<char>(codeToPlay.ToCharArray());

		foreach(char character in codeList){
			if(character == '.'){
				SoundDot.Play();
				IncomingTransmissionText.text += '.';
				yield return StartCoroutine(waitForSound(SoundDot));
			}else if(character == '−'){
				SoundDash.Play();
				IncomingTransmissionText.text += '−';
				yield return StartCoroutine(waitForSound(SoundDash));
			}
			//IncomingTransmissionText.text += " ";
            yield return new WaitForSeconds(CodePause);
		}
	}

	IEnumerator waitForSound(AudioSource sound){
        while (sound.isPlaying)
        {
            yield return null;
        }
	}

#endregion

	public string TargetCode;

	public void StartGame(){
		string testWord = "h";
		string testCode = MorseCode.EnglishToMorseCode(testWord);
		StartCoroutine(PlayCode(testCode));
		TargetCode = testCode;
		playing = true;

		////Wait for player feedback

	}

}