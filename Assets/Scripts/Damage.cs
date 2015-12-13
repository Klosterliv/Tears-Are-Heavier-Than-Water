using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

	public ControllerInput shrimp;
	[SerializeField] float armor = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision) {

		float multiplier = 0.3f;

		Vector2 ray1 = collision.contacts[0].normal*1;
		Vector2 ray2 = collision.relativeVelocity*1;
			
		if(ray2.magnitude >= 3)
		Debug.DrawRay(transform.position, collision.contacts[0].normal*2, Color.blue, 5f);
		if(ray2.magnitude >= 3)
		Debug.DrawRay(transform.position, collision.relativeVelocity*1, Color.yellow, 5f);
		
		/*
		if(Mathf.Abs(Vector2.Dot(ray1,ray2)) >= 10)
			Debug.LogError(Vector2.Dot(ray1,ray2));
		*/

		if(collision.contacts[0].collider.CompareTag("Shrimp")) {
			Damage collDamage = (Damage) collision.contacts[0].collider.gameObject.GetComponent(typeof(Damage));
			if(collDamage.shrimp != shrimp) {
				multiplier += 2;
			}
			else multiplier = 0;
		}
		
		shrimp.Damage( Mathf.Clamp(Mathf.Abs(Vector2.Dot(ray1,ray2))*multiplier - armor, 0, 1337 ));


	}
}
