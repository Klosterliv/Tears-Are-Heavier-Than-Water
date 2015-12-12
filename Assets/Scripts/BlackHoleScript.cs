using UnityEngine;
using System.Collections;

public class BlackHoleScript : MonoBehaviour {
	public float radius;
	public float startRadius = 1.0f;
	public float startSpeed;
	public float lifeTime;
	// Use this for initialization
	void Start () 
	{
		KillBlackhole(lifeTime);
	}
	void OnCollisionEnter2D(Collision2D _collider)
	{
		if(_collider.gameObject.layer == 8)
		{
			Debug.Log("Collided with Blackhole nr"+ _collider.gameObject.GetInstanceID());
			Destroy(this.gameObject);
		}
	}
	void KillBlackhole(float _lifeTime)
	{
		if(_lifeTime<=0)
		{
			Debug.Log(this.gameObject.GetInstanceID()+" ran out of time");
			Destroy(this.gameObject);
		}
		else
		{
			_lifeTime = _lifeTime - 1*Time.fixedDeltaTime;
		}
	}
}
