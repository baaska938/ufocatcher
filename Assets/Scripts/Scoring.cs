using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Scoring : MonoBehaviour {
	Text text;
	TriggerSens tr;

	// Use this for initialization
	void Awake () {
		text = GetComponent<Text> ();
		tr = FindObjectOfType<TriggerSens> ();
	}

	public void ToRed(){
		text.color = Color.Lerp (Color.red, Color.black, Time.deltaTime);
	}

	public void ToBlack(){
		text.color = Color.Lerp (Color.black, Color.red, Time.deltaTime);
	}
	
	// Update is called once per frame
	void Update () {

		text.text = "Score: " + tr.ReturnScore ();
	}
}
