using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameResetScript : MonoBehaviour {

	public Rigidbody2D asteroid;
	public float resetSpeed=0.025f;

	private SpringJoint2D spring;
	private float resetSpeedSqr;


	void Start(){
		resetSpeedSqr = resetSpeed * resetSpeed;
		spring = asteroid.GetComponent<SpringJoint2D> ();
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Asteroid") {
			Reset ();
		}
	}
	

	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Reset ();
		}
		if (spring == null && asteroid.velocity.sqrMagnitude < resetSpeedSqr) {
			Reset ();
		}
	
	}

	void Reset(){
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
