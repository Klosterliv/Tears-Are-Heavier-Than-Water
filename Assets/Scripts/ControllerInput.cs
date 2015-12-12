using UnityEngine;
using System.Collections;

public class ControllerInput : MonoBehaviour {

	public GameObject hand;

	public GameObject shrimp;

	[SerializeField] float speed = 1;
	[SerializeField] float maxSpeed = 10;

	[SerializeField] float shootCoolDown = 1;
	[SerializeField] string shootInput = "ShootP1";
	[SerializeField] float projectileForce = 10;

	[SerializeField] bool speedRelative = false;
	[SerializeField] bool maxSpeedOn = true;

	[SerializeField] GameObject projectile;


	bool fire = false;
	float coolDown;

	// Use this for initialization
	void Start () {
		coolDown = shootCoolDown;
	
	}

	void Update () {
		coolDown -= Time.deltaTime;
		if (Input.GetButtonDown(shootInput) && coolDown <= 0f) {
			fire = true;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		Steer ();
		Aim ();

	
	}

	void Steer () {
		Vector2 axisInput = new Vector2(0,0);

		axisInput.x = Input.GetAxis("Horizontal");
		axisInput.y = Input.GetAxis("Vertical");

		axisInput = Vector2.ClampMagnitude(axisInput,1f);

		float dot = Mathf.Clamp01(Vector2.Dot(axisInput, Vector2.up));

		Vector2 force = axisInput * dot * speed * Time.fixedDeltaTime;


		Rigidbody2D rigid = shrimp.GetComponent<Rigidbody2D>();

		if(speedRelative) {
			float speedDot = Mathf.Clamp01(Vector2.Dot(rigid.velocity, force));
			force /= (rigid.velocity.magnitude*speedDot+0.1f);
		} 

		if(maxSpeedOn) {
			float speedDot = Mathf.Clamp01(Vector2.Dot(rigid.velocity, force));         
			float percentSpeed = rigid.velocity.magnitude / maxSpeed;
			force -= force*( percentSpeed*speedDot ); 
		}



		//hand.transform.position += (Vector3) axisInput*speed*Time.deltaTime;


		rigid.AddForce(force);

		Debug.Log(""+force+"     "+dot);
		Debug.DrawRay(shrimp.transform.position, force, Color.cyan, 0.01f);

	}

	void Aim() {

		Vector2 axisInput = new Vector2(0,0);

		axisInput.x = Input.GetAxis("HorizontalRight");
		axisInput.y = Input.GetAxis("VerticalRight");

		Debug.DrawRay(shrimp.transform.position, axisInput, Color.red, 1f);

		if(fire) Fire(shrimp.transform.position, axisInput);

	}

	void Fire(Vector2 pos, Vector2 dir) {
		
		GameObject fired;
		fired = (GameObject) Instantiate(projectile, pos, Quaternion.identity);

		Rigidbody2D projectileRigid; 
		projectileRigid = fired.GetComponent<Rigidbody2D>();
		Rigidbody2D shrimpRigid;
		shrimpRigid = shrimp.GetComponent<Rigidbody2D>();

		projectileRigid.velocity = (shrimpRigid.velocity+dir*projectileForce);



		coolDown = shootCoolDown;
		fire = false;
	}
}
