using UnityEngine;
using System.Collections;

public class ControllerInput : MonoBehaviour {

	public GameObject hand;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 axisInput = new Vector2(0,0);

		axisInput.x = Input.GetAxis("Horizontal");
		axisInput.y = Input.GetAxis("Vertical");

		hand.transform.position += (Vector3) axisInput*Time.deltaTime;
	
	}
}
