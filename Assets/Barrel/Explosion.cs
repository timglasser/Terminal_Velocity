using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	[SerializeField]
	float explosionForce;
	[SerializeField]
	GameObject explosionPrefab;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}
	void OnTriggerEnter(Collider collider){
		if (collider.tag == "Player"){
			Explode();
			Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			gameObject.SetActive(false);
		}
	}

	void Explode(){
		Collider[] targets = Physics.OverlapSphere(transform.position, ((SphereCollider)GetComponent<Collider>()).radius);
		foreach (Collider t in targets){
            /*
			if (t.GetComponent<RagdollControl>()){
				Collider[] bones = t.GetComponent<RagdollControl>().Kill();
				foreach (Collider b in bones){
					b.attachedRigidbody.AddExplosionForce(explosionForce, transform.position, ((SphereCollider)GetComponent<Collider>()).radius);
//					Debug.Log(b.gameObject);
				}
			} else {
				Rigidbody rb = t.attachedRigidbody;
				
				if (rb) {
					rb.AddExplosionForce(explosionForce, transform.position, ((SphereCollider)GetComponent<Collider>()).radius);
//					Debug.Log(rb.gameObject);
				}
			}
            */
		}
	}
}
