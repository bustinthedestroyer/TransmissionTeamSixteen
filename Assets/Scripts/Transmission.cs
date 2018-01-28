using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Transmission : MonoBehaviour {
	public Text IncomingTransmissionText, OutgoingTransmissionText, TransmissionStatusText, TransmissionMessageText, TextScore, TextHighScore, TextScoreMulti;

	public Transform Operatior, Telegraph;
	public InputField testingTextInput;

	public Button ResetButton;
	public AudioSource SoundDot, SoundDash;
	private bool playing = false;	
	public string MessageString;
	[HideInInspector]
	public string TargetCodeString, InputCodeString;
	public float CodeSoundPause;
	private bool telegraphDown = false;
	private bool perfectWord = true;
	private bool timesUp = false;
	private float telegraphDownTime;
	public float dotInputPause;
	public float dashInputPause;
	private int scoreValue = 0, highValue, multiValue;
	public GameObject LevelSelectRow;
	public GameObject LevelSelectPanel;

	public SpriteRenderer OperatorHand;
	public Sprite HandUp;
	public Sprite HandDown;

	void Start () {
		//TextHighScore.text = "High: " + PlayerPrefs.GetInt("HighScore", 0);
		//StartGame();
	}

	void Update()
	{
		//if(true){
		if(playing){
			GetTelegraphInput();
		}
	}
	
	public int LevelNumber;
	public void StartGame(int levelNumber, string levelTitle, string levelMessage){
		ResetButton.gameObject.SetActive(false);

		// if(testingTextInput.text.Length == 0){
		// 	MessageString = "testing text";
		// }else{
		// 	MessageString = testingTextInput.text;
		// }
		LevelNumber = levelNumber;
		MessageString = levelMessage;

		OutgoingTransmissionText.text = ""; 
		IncomingTransmissionText.text = "";
		TransmissionMessageText.text = "";
		TransmissionStatusText.text = "";
		multiValue = 0;
		TextScoreMulti.text = "X" + multiValue.ToString();
		scoreValue = 0;
		TextScore.text = "Score: " + scoreValue.ToString();
		highValue = PlayerPrefs.GetInt("HighScore", 0);
		TextHighScore.text = "High: " + highValue;
		StartCoroutine(PlayGame());
	}

#region TheTelegraph Stuff
	void GetTelegraphInput() {
		if(Input.GetKeyDown("space") && !telegraphDown){
			telegraphDownTime = Time.time;
			telegraphDown = true;
			PoorAnimation();
		}

		if(Input.GetKeyUp("space") && telegraphDown){
			if(Time.time < telegraphDownTime + dotInputPause){
				SoundDot.Play();
				InputCodeString += ".";
			}else{
				SoundDash.Play();
				InputCodeString += "-";
			}
			OutgoingTransmissionText.text = InputCodeString; 
			telegraphDown = false;
			PoorAnimation();
		}

		// if(telegraphDown && Time.time > telegraphDownTime + dashInputPause){
		// 	timesUp = true;
		// }
		// if(Time.time > telegraphDownTime + dashInputPause){
		// 	timesUp = true;
		// }
		
	}

#endregion

#region Transmissions
	IEnumerator PlayCode(string codeToPlay){
		IncomingTransmissionText.text = "";

		List<char> codeList = new List<char>(codeToPlay.ToCharArray());

		foreach(char character in codeList){
			if(character == '.'){
				SoundDot.Play();
				IncomingTransmissionText.text += '.';
				yield return StartCoroutine(waitForSound(SoundDot));
			}else if(character == '-'){
				SoundDash.Play();
				IncomingTransmissionText.text += '-';
				yield return StartCoroutine(waitForSound(SoundDash));
			}
            yield return new WaitForSeconds(CodeSoundPause);
		}
	}

	IEnumerator waitForSound(AudioSource sound){
        while (sound.isPlaying)
        {
            yield return null;
        }
	}

#endregion

#region GameLoop

	private string transmission;

	IEnumerator PlayGame(){

		yield return new WaitForSeconds(2f);

		string[] wordArray = MessageString.Split(' ');

		foreach(string word in wordArray){

			char[] charArray = word.ToCharArray();

			foreach(char character in charArray){

				string testCharacter = character.ToString();
				string testCode = MorseCode.EnglishToMorseCode(testCharacter.ToString());
				TargetCodeString = testCode;
				
				transmission += character;
				if(transmission.Length == 1){
					TransmissionMessageText.text = "<color=#ff0000ff>" + character + "</color>";
				}else{
					TransmissionMessageText.text = transmission.Substring(0, transmission.Length-1) + "<color=#ff0000ff>" + character + "</color>";
				}
				
				bool correct = false;
				while(!correct){

					yield return StartCoroutine(PlayCode(testCode));
					playing = true;

					while(playing){
						string stringToCompare = TargetCodeString.Substring(0, InputCodeString.Length);


						if(InputCodeString != stringToCompare || timesUp){
							if(timesUp){
								timesUp = false;
								TransmissionStatusText.text = "Times Up";
							}else{
								TransmissionStatusText.text = "Incorrect!";
							}
							///TODO play noise?
							perfectWord = false;
							playing = false;
							yield return new WaitForSeconds(.5f);
							InputCodeString = "";
							OutgoingTransmissionText.text = "";

							TransmissionStatusText.text = "Try Again!";
						}
						if(TargetCodeString.Length == InputCodeString.Length){
							TransmissionStatusText.text = "Correct!";
							UpdateScore();
							playing = false;
							correct = true;
							yield return new WaitForSeconds(.25f);

							InputCodeString = "";
							OutgoingTransmissionText.text = "";
							IncomingTransmissionText.text = "";
							TransmissionStatusText.text = "";
						}
						
						yield return null;
					}					
					yield return null;
				}

				
			}

			WordPerfect(perfectWord);


			transmission += " ";

		}
		ResetButton.gameObject.SetActive(true);

		if(scoreValue > highValue){
			PlayerPrefs.SetInt("HighScore", scoreValue);
		}
		transmission = "";
		TransmissionMessageText.text = MessageString;
		TransmissionStatusText.text = "Transmission Recieved!";

		if(LevelNumber > PlayerPrefs.GetInt("HighestLevel", 0)){
			PlayerPrefs.SetInt("HighestLevel", LevelNumber);
		}

		Debug.Log("done");
	}




#endregion

#region Scoring

	void UpdateScore(){
		if(multiValue != 0){
			scoreValue += multiValue;
		}else{
			scoreValue += 1;
		}
		TextScore.text = "Score: " + scoreValue.ToString();
	}

	void WordPerfect(bool isPerfect){
		//MultiValue
		if(perfectWord){
			if(multiValue == 0){
				multiValue = 2;
			}else if(multiValue == 16){
				multiValue = 16;
			}else{
				multiValue = multiValue * 2;
			}
			TextScoreMulti.text = "X" + multiValue.ToString();
		}
		perfectWord = true;
	}

#endregion

#region Animation
	void PoorAnimation(){
		if(telegraphDown){
			///Telegraph.rotation = Quaternion.Euler(Telegraph.rotation.x,Telegraph.rotation.y,Telegraph.rotation.z - 5);
			//Operatior.position = new Vector3(Operatior.position.x,Operatior.position.y + .25f,Operatior.position.z);
			OperatorHand.sprite = HandDown;
		}else{
			//Telegraph.rotation = Quaternion.Euler(Telegraph.rotation.x,Telegraph.rotation.y,Telegraph.rotation.z + 5);
			//Operatior.position = new Vector3(Operatior.position.x,Operatior.position.y - .25f,Operatior.position.z);
			OperatorHand.sprite = HandUp;
		}
	}
#endregion


	public void ExitGame(){
		Application.Quit();
	}
}
