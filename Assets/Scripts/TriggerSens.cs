using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriggerSens : MonoBehaviour {
	public int score = 0;
	private Scoring colorchange;

	void Awake(){
		colorchange = FindObjectOfType<Scoring> ();
	}

	void OnTriggerEnter(Collider other) {
		this.score ++;
		colorchange.ToRed ();
	}

	void OnTriggerExit(Collider other) {
		colorchange.ToBlack ();
	}

	public int ReturnScore(){
				return this.score;
		}
}