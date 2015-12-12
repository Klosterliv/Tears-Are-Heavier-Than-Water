using UnityEngine;
using System.Collections;

public class BlackHoleScript : MonoBehaviour {
	public float radius;
	public float startRadius = 1.0f;
	public float startSpeed;
	public float lifeTime;

	public PointEffector2D gravity;
	public CircleCollider2D effectorArea;
	public CircleCollider2D area;
	public Rigidbody2D rigid;

	bool collided = false;

	public float mass = 1;
	// Use this for initialization
	void Start () 
	{
		//KillBlackhole(lifeTime);
		//area = (CircleCollider2D) GetComponent(typeof(CircleCollider2D));

	}
	void OnCollisionEnter2D(Collision2D _collider)
	{
		if(_collider.collider.CompareTag("BlackHole"))
		{
			Debug.Log("Collided with Blackhole nr"+ _collider.gameObject.GetInstanceID());

			BlackHoleScript other = (BlackHoleScript) _collider.collider.GetComponent(typeof(BlackHoleScript));
			if(collided) return;

			other.collided = true; collided = true;
			if(other.mass <= mass) {
				mass += other.mass;
				//area.radius += other.area.radius;
				gravity.forceMagnitude = -mass;
				rigid.mass = mass;

				//effectorArea.radius += other.effectorArea.radius;
				lifeTime+=1000f;

				transform.localScale = Vector3.one + Vector3.one * mass/10;
				Destroy(other.gameObject);
			}
			else {
				other.mass += mass;
				//other.area.radius += area.radius;
				other.gravity.forceMagnitude = -other.mass;
				//other.effectorArea.radius += effectorArea.radius;
				other.rigid.mass = other.mass;

				other.transform.localScale = Vector3.one + Vector3.one * other.mass/10;
				Destroy(gameObject);
			}


			//Destroy(_collider.collider.gameObject);


		}
	}
	void FixedUpdate() {
		collided = false;
	}
	void Update() {
		KillBlackhole();
	}
	void KillBlackhole()
	{
		if(lifeTime<=0)
		{
			Debug.Log(this.gameObject.GetInstanceID()+" ran out of time");
			Destroy(this.gameObject);
		}
		else
		{
			lifeTime = lifeTime - 1*Time.deltaTime;
		}
	}
}
