using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionManagerClass : MonoBehaviour {

	public delegate void onCollision(Transform collidedObject, Vector3 collisionPoint);
	public event onCollision onCollisionCallback;
	public delegate void onTrigger(Transform collidedObject);
	public event onTrigger onTriggerCallback;

	List<string> collisionTags=new List<string>();
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addCollisionForTag(string tagName){
		collisionTags.Add (tagName);
	}

	void OnCollisionEnter(Collision collision) {
		if (collisionTags.Contains (collision.transform.tag)) {
			if (onCollisionCallback != null) {
				foreach(ContactPoint contact in collision.contacts){
					onCollisionCallback (collision.transform, contact.point);
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider) {
		if (collisionTags.Contains (collider.tag)) {
			if (onTriggerCallback != null) {
				onTriggerCallback (collider.transform);
			}
		}
	}
}
