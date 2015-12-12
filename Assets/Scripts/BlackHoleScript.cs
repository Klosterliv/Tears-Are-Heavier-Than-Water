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
	void OnTriggerEnter(Collision2D _collider)
	{
		if(_collider.layer == "blackHole")
		{
			Debug.Log("Collided with Blackhole nr"+ _collider.gameObject.GetInstanceID());
			Destroy(this.gameObject);
		}
	}
	void KillBlackhole(float _lifeTime)
	{
		if(lifeT<=0)
		{
			Debug.Log(this.gameObject.GetInstanceID()+" ran out of time");
			Destroy(this.gameObject);
		}
		else
		{
			_lifeTime = _lifeTime - 1*Time.fixedDeltaTime();
		}
	}
}
