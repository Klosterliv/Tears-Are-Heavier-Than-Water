using UnityEngine;
using System.Collections;

public class PlasmaShotScript : MonoBehaviour {
	float speed;
	public float lifeTime;
	public float stunTime;
	public GameObject blackHole;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime = lifeTime-1*Time.deltaTime;
		if(lifeTime<=0)
		{
			Instantiate(blackHole,this.transform.position,this.transform.rotation);
			Destroy(this.gameObject);
		}

	}
	void OnTriggerEnter2d(Collision2D _collider)
	{
		if (_collider.collider.CompareTag("Shrimp")) {
			Debug.Log("Collided");
			Damage damage = (Damage) _collider.collider.gameObject.GetComponent(typeof(Damage));
			ControllerInput shrimp = damage.shrimp;
			Debug.Log("STUN"+ _collider.collider.tag);
			shrimp.Stun(stunTime);
		}
	}
	void SetupPlasma(float _lifeTime, float _speed)
	{
		lifeTime = _lifeTime;
		speed = _speed;
	}
			
}
