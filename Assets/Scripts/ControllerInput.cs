using UnityEngine;
using System.Collections;

public class ControllerInput : MonoBehaviour {

	public GameObject hand;

	[SerializeField] string horizontalAxisL;
	[SerializeField] string horizontalAxisR;
	[SerializeField] string verticalAxisL;
	[SerializeField] string verticalAxisR;

	public GameObject shrimp;

	[SerializeField] float speed = 1;
	[SerializeField] float maxSpeed = 10;
	[SerializeField] float dashForce = 200f;
	[SerializeField] float maxStamina = 1f;
	[SerializeField] float staminaRegen = 0.2f;
	[SerializeField] float maxHP = 100f;
	[SerializeField] float armor = 0f;
	//[SerializeField] float dashTempArmor = 20f;
	[SerializeField] AnimationCurve dashTempArmorDropOff;

	[SerializeField] float dashWindow = 0.1f;
	[SerializeField] float shootCoolDown = 1;
	[SerializeField] float dashCooldDown = 3;
	[SerializeField] string shootInput = "ShootP1";
	[SerializeField] string dashInput = "DashP1";
	[SerializeField] float projectileForce = 10;

	[SerializeField] bool speedRelative = false;
	[SerializeField] bool maxSpeedOn = true;

	[SerializeField] GameObject projectile;


	bool fire = false;
	bool dash = false;
	bool rush = false;
	bool stunned = false;

	float stunTimer = 0;
	float coolDown;
	float dashCD;
	float dashWindowTime = 0;
	public float stamina;
	public float hp;

	float aimRmagnitude = 0;
	float timeSinceDash = 10f;

	// Use this for initialization
	void Start () {
		coolDown = shootCoolDown;
		dashCD = dashCooldDown;
		stamina = maxStamina;
		hp = maxHP;
	
	}

	void Update () {

		stamina += staminaRegen*Time.deltaTime;
		if (stamina >= maxStamina) stamina = maxStamina;

		coolDown -= Time.deltaTime;
		dashCD -= Time.deltaTime;
		timeSinceDash += Time.deltaTime;
		stunTimer -= Time.deltaTime;

		if(stunTimer > 0) stunned = true; else stunned = false;
		if(stunned) return;
		if (Input.GetButtonDown(shootInput) && coolDown <= 0f) {
			fire = true;
		}
		if (Input.GetButtonDown(dashInput) && dashCD <= 0f && aimRmagnitude >= 0.05f  && stamina >= 0.2f) {
			dash = true;
		}			

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(stunned) return;
		Steer ();
		Aim ();

	
	}

	void Steer () {
		Vector2 axisInput = new Vector2(0,0);

		axisInput.x = Input.GetAxis(horizontalAxisL);
		axisInput.y = Input.GetAxis(verticalAxisL);

		axisInput = Vector2.ClampMagnitude(axisInput,1f);

		//float dot = Mathf.Clamp01(Vector2.Dot(axisInput, Vector2.up));

		Vector2 force = axisInput * speed * Time.fixedDeltaTime;


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

		//Debug.Log(""+force+"     "+dot);
		Debug.DrawRay(shrimp.transform.position, force, Color.cyan, 0.01f);

	}

	void Aim() {

		Vector2 axisInput = new Vector2(0,0);

		axisInput.x = Input.GetAxis(horizontalAxisR);
		axisInput.y = Input.GetAxis(verticalAxisR);

		Debug.DrawRay(shrimp.transform.position, axisInput, Color.red);

		if(fire) Fire(shrimp.transform.position, axisInput);

		if(dash) Dash(axisInput);

		aimRmagnitude = axisInput.magnitude;


	}

	void Fire(Vector2 pos, Vector2 dir) {
		
		GameObject fired;
		fired = (GameObject) Instantiate(projectile, pos+dir.normalized*2, Quaternion.identity);

		Rigidbody2D projectileRigid; 
		projectileRigid = fired.GetComponent<Rigidbody2D>();
		Rigidbody2D shrimpRigid;
		shrimpRigid = shrimp.GetComponent<Rigidbody2D>();

		projectileRigid.velocity = (shrimpRigid.velocity+dir*projectileForce);



		coolDown = shootCoolDown;
		fire = false;

	}

	void Dash(Vector2 dir) {


		Rigidbody2D shrimpRigid;
		shrimpRigid = shrimp.GetComponent<Rigidbody2D>();

		shrimpRigid.velocity = shrimpRigid.velocity/10f;


		//Mathf.Clamp01(Vector2.Dot(shrimpRigid.velocity, dir))

		Vector2 force = dir.normalized*dashForce;
		//force = force - Mathf.Clamp( (Mathf.Clamp01(Vector2.Dot(shrimpRigid.velocity, dir))*force) ,Vector2.zero,force);
		//force = force -  (Mathf.Clamp01(Vector2.Dot(shrimpRigid.velocity, dir))*force);

		//float dot = ((Vector2.Dot(shrimpRigid.velocity.normalized, dir)+1)/2);
		//float speedPercent = (shrimpRigid.velocity.magnitude*dot) / (maxSpeed*0.5f);

		//speedPercent = Mathf.Clamp01(speedPercent);

		//force = force - speedPercent*force;




		//shrimpRigid.AddForce(force,ForceMode2D.Impulse);
		shrimpRigid.AddForce(force);

		Debug.Log("DASH! :: "+force.magnitude);
		Debug.DrawRay(shrimpRigid.transform.position, force, Color.green, 1f);

		dashCD = dashCooldDown;
		dash = false;

		stamina -= 0.2f;
		timeSinceDash = 0f;
	}

	public void Damage(float dmg) {
		float dmgTaken = Mathf.Clamp(dmg - dashTempArmorDropOff.Evaluate(timeSinceDash),0,1337);
		//dmgTaken = Mathf.Clamp( dmg - armor , 0f, 1337f);
		hp -= dmgTaken;
		if (dmgTaken > 1) Debug.LogError(dmgTaken);

		if(hp <= 0) { Death (); }
	}

	void Death() {
		//Destroy(shrimp);
		//Destroy(gameObject);
	}

	public void Stun (float value) {
		stunTimer = value;
	}

}
