using UnityEngine;
using System.Collections;

public class SarwuuController : MonoBehaviour {
	public Transform leftSar;
	public Transform rightSar;
	public Rigidbody left;
	public Rigidbody right;
	public float sarAngleEnd = 10;
	private float sarAngle = 0;
	private float sarSpeed = 0.5f;
	private float downHight = 0.6f; //normal 0.55
	private Vector3 FirstPosition;
	private Vector3 SecondPosition;
	private Quaternion FirstLeftRotation;
	private Quaternion FirstRightRotation;
	private Quaternion SecondLeftRotation;
	private Quaternion SecondRightRotation;
	public float downSpeed = 1;
	public float HorizontalSpeed = 0.8f;
	public float VerticalSpeed = 0.8f;
	private int i = 0;
	private int j;
	private int timer = 0;
	private bool rightflag;
	private bool upflag;
	private BoxCollider downhighsens;
	private DownHeightSens downheightsens;


	void Awake () {
		FirstPosition = transform.position;				//save first position
		FirstLeftRotation = left.transform.rotation;	//save first left sarwuu rotation
		FirstRightRotation = right.transform.rotation;	//save first right sarwuu rotation
		downheightsens = FindObjectOfType<DownHeightSens> ();
	}

	// Use this for initialization
	void Start () {
		rightflag = true;	//right move possible
		upflag = false;		//up move impossible
	}

	// Update is called once per frame

	void Update() {  
		if (Input.GetKeyDown (KeyCode.Return)) {  
			Application.LoadLevel (0);  
		}  
	}
	void FixedUpdate () {
		//on unity
		#if UNITY_EDITOR

		if (rightflag && Input.GetKey (KeyCode.RightArrow)) {
			MoveRight(rightflag);		
		}

		if (rightflag && Input.GetKeyUp (KeyCode.RightArrow)) {
			rightflag = false;
			upflag = true;
			MoveRight(rightflag);		
		}

		if (Input.GetKey (KeyCode.RightArrow) && Input.GetKey (KeyCode.UpArrow)){
			Debug.Log("both pushed");
		}
		#endif

		//on android
		#if UNITY_ANDROID
		if (rightflag && j==1) {
			MoveRight(rightflag);		
		}
		
		if (rightflag && j==2) {
			rightflag = false;
			upflag = true;
			MoveRight(rightflag);		
		}
		#endif

		//up move
		if (upflag && (Input.GetKey(KeyCode.UpArrow) || j==3)) {
			MoveUp(upflag);
		}
		if (upflag && (Input.GetKeyUp (KeyCode.UpArrow) || j==4)) {
			upflag = false;
			SecondPosition = transform.position;
			MoveUp(upflag);	
		}
		
		//opensarwuu
		if (!rightflag && !upflag && sarAngle <= sarAngleEnd && i==0){
			left.rigidbody.AddTorque (Vector3.forward*sarSpeed * 0.3f, ForceMode.VelocityChange);
			right.rigidbody.AddTorque (-Vector3.forward*sarSpeed * 0.3f, ForceMode.VelocityChange);
			sarAngle ++ ;
			if(sarAngle == sarAngleEnd){
				

				left.rigidbody.velocity = Vector3.zero;
				left.rigidbody.angularVelocity = Vector3.zero;
				left.rigidbody.isKinematic = true;
				right.rigidbody.velocity = Vector3.zero;
				right.rigidbody.angularVelocity = Vector3.zero;
				right.rigidbody.isKinematic = true;
				SecondLeftRotation = left.rigidbody.rotation;
				SecondRightRotation = right.rigidbody.rotation;

				this.i=3;
			}
		}
		
		//move down

		if (i == 3 && transform.position.y >= downHight && downheightsens.DownPossible()) {
			transform.Translate (Vector3.down * downSpeed * Time.deltaTime);
		}
		if (i == 3 && (transform.position.y <= downHight || !downheightsens.DownPossible())) {
			left.rigidbody.isKinematic = false;
			right.rigidbody.isKinematic = false;
			this.i = 4;
		}


		//closesarwuu
		if (i == 4 && sarAngle >=0){
			left.rigidbody.AddTorque (Vector3.back * sarSpeed * 0.3f, ForceMode.VelocityChange);
			right.rigidbody.AddTorque (-Vector3.back * sarSpeed * 0.3f, ForceMode.VelocityChange);
			sarAngle-- ;

		}
		if (i == 4 && sarAngle == 0){
			left.rigidbody.velocity = Vector3.zero;
			left.rigidbody.angularVelocity = Vector3.zero;
			left.rigidbody.isKinematic = true;
			right.rigidbody.velocity = Vector3.zero;
			right.rigidbody.angularVelocity = Vector3.zero;
			right.rigidbody.isKinematic = true;
			this.i = 5;
			
		}
		
		//move up
		if (i == 5 && transform.position.y <= SecondPosition.y) {

			//left.rigidbody.AddTorque (Vector3.back * sarSpeed*0.1, ForceMode.VelocityChange);
			//right.rigidbody.AddTorque (-Vector3.back * sarSpeed*0.1, ForceMode.VelocityChange);
			transform.Translate (-Vector3.down * downSpeed * 0.8f * Time.deltaTime);
		} 
		else if (i == 5) {
						this.i = 6;
		}
	
		

		
		//move original position
		if (i == 6){

			if(transform.position.x > FirstPosition.x)transform.Translate (-Vector3.right * HorizontalSpeed * Time.deltaTime);
			if(transform.position.z > FirstPosition.z)transform.Translate (-Vector3.forward * VerticalSpeed * Time.deltaTime);
		}

		//original position open sarwuu
		if (i == 6 && transform.position.x == FirstPosition.x && transform.position.z == FirstPosition.z){
			left.rigidbody.isKinematic = false;
			right.rigidbody.isKinematic = false;
			//leftSar.transform.rotation = Quaternion.FromToRotation(Vector3.up, SecondLeftRotation);
			leftSar.transform.rotation = Quaternion.Lerp(leftSar.transform.rotation, SecondLeftRotation, Time.deltaTime * 10f);
			rightSar.transform.rotation = Quaternion.Lerp(rightSar.transform.rotation, SecondRightRotation, Time.deltaTime * 10f);
			
			if(timer != 10)
				this.timer++;
		}

		//timer 10
		if (i == 6 && timer == 10) 
		{
			left.rigidbody.velocity = Vector3.zero;
			left.rigidbody.angularVelocity = Vector3.zero;
			right.rigidbody.velocity = Vector3.zero;
			right.rigidbody.angularVelocity = Vector3.zero;
			this.i = 7;
		}

		//original position close sarwuu
		if (i == 7)
		{
			//Debug.Log ("ey");
			leftSar.transform.rotation = Quaternion.Lerp (leftSar.transform.rotation, FirstLeftRotation, Time.deltaTime * 5f);
			rightSar.transform.rotation = Quaternion.Lerp (rightSar.transform.rotation, FirstRightRotation, Time.deltaTime * 5f);
			if (timer != 100) 
				this.timer++;
		}

		//timer 100
		if (i == 7 && timer == 100) 
		{
			left.rigidbody.velocity = Vector3.zero;
			left.rigidbody.angularVelocity = Vector3.zero;
			right.rigidbody.velocity = Vector3.zero;
			right.rigidbody.angularVelocity = Vector3.zero;
			this.i=0;
			this.timer=0;
			rightflag = true;
			upflag = false;
		}
	}

	//right move
	public void MoveRight(bool flag){
			if (flag && transform.position.x <= 0.68f) {
						transform.Translate (Vector3.right * HorizontalSpeed * Time.deltaTime);
				} 
			else if (flag == false)
						rightflag = false;
		}

	//up move
	public void MoveUp(bool flag){
			if (flag && transform.position.z <= 0.68f) {
						transform.Translate (Vector3.forward * VerticalSpeed * Time.deltaTime);

				} 
			else if (flag == false) {
						
						upflag = false;
						
				}
	}

	//touch right move
	public void rightTouched(bool flag){
		if (flag)
						this.j = 1;
				else
						this.j = 2;
	}


	//touch up move
	public void upTouched(bool flag){
		if (flag)
			this.j = 3;
		else
			this.j = 4;
	}

	public void restartTouched(){
		Application.LoadLevel (0);
	}

}
