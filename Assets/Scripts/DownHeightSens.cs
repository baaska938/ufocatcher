using UnityEngine;
using System.Collections;

public class DownHeightSens : MonoBehaviour {
	bool flag;
	void Start(){
		flag = true;
	}
	void OnTriggerEnter(Collider other) {
		this.flag = false;
		Debug.Log (flag);
	}

	void OnTriggerStay(Collider other) {
		this.flag = true;
		Debug.Log (flag);
	}

	void OnTriggerExit(Collider other){
		this.flag = true;
		Debug.Log (flag);
	}

	public bool DownPossible(){
		return flag;
	}
}
