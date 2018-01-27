using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MorseCode : MonoBehaviour {

	public Text TextMorseCode;
	public InputField InputEnglish;

	public AudioSource SoundDot;
	public AudioSource SoundDash;


	void Start () {
		InitialiseDictionary();
	}

	public void Translate(){
		string TheMorseCode = translate(InputEnglish.text); 
		TextMorseCode.text = TheMorseCode;
		StartCoroutine(PlayCode(TheMorseCode));
	}

	public float CodePause;
	IEnumerator PlayCode(string codeToPlay){

		List<char> codeList = new List<char>(codeToPlay.ToCharArray());
		Debug.Log(codeList);
		foreach(char character in codeList){
			if(character == '.'){
				SoundDot.Play();
				yield return StartCoroutine(waitForSound(SoundDot));
			}else if(character == '−'){
				SoundDash.Play();
				yield return StartCoroutine(waitForSound(SoundDash));
			}
            yield return new WaitForSeconds(CodePause);
		}
	}

	IEnumerator waitForSound(AudioSource sound){
        while (sound.isPlaying)
        {
            yield return null;
        }
	}

	

	static Dictionary<char, string> translator;

	private static void InitialiseDictionary()
	{
		char dot = '.';
		char dash = '−';

		translator = new Dictionary<char, string>()
		{
			{'a', string.Concat(dot, dash)},
			{'b', string.Concat(dash, dot, dot, dot)},
			{'c', string.Concat(dash, dot, dash, dot)},
			{'d', string.Concat(dash, dot, dot)},
			{'e', dot.ToString()},
			{'f', string.Concat(dot, dot, dash, dot)},
			{'g', string.Concat(dash, dash, dot)},
			{'h', string.Concat(dot, dot, dot, dot)},
			{'i', string.Concat(dot, dot)},
			{'j', string.Concat(dot, dash, dash, dash)},
			{'k', string.Concat(dash, dot, dash)},
			{'l', string.Concat(dot, dash, dot, dot)},
			{'m', string.Concat(dash, dash)},
			{'n', string.Concat(dash, dot)},
			{'o', string.Concat(dash, dash, dash)},
			{'p', string.Concat(dot, dash, dash, dot)},
			{'q', string.Concat(dash, dash, dot, dash)},
			{'r', string.Concat(dot, dash, dot)},
			{'s', string.Concat(dot, dot, dot)},
			{'t', string.Concat(dash)},
			{'u', string.Concat(dot, dot, dash)},
			{'v', string.Concat(dot, dot, dot, dash)},
			{'w', string.Concat(dot, dash, dash)},
			{'x', string.Concat(dash, dot, dot, dash)},
			{'y', string.Concat(dash, dot, dash, dash)},
			{'z', string.Concat(dash, dash, dot, dot)},
			{'0', string.Concat(dash, dash, dash, dash, dash)},
			{'1', string.Concat(dot, dash, dash, dash, dash)},  
			{'2', string.Concat(dot, dot, dash, dash, dash)},
			{'3', string.Concat(dot, dot, dot, dash, dash)},
			{'4', string.Concat(dot, dot, dot, dot, dash)},
			{'5', string.Concat(dot, dot, dot, dot, dot)},
			{'6', string.Concat(dash, dot, dot, dot, dot)},
			{'7', string.Concat(dash, dash, dot, dot, dot)},
			{'8', string.Concat(dash, dash, dash, dot, dot)},
			{'9', string.Concat(dash, dash, dash, dash, dot)}
		};
	}

	private static string translate(string input)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		foreach(char character in input)
		{
			if(translator.ContainsKey(character))
			{
				sb.Append(translator[character] + " ");
			} else if (character == ' ')
			{
				sb.Append("/ ");
			} else 
			{
				sb.Append(character + " ");
			}
		}
		return sb.ToString();
	}
}