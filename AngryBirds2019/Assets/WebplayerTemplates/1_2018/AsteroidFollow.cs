using UnityEngine;
using System.Collections;

public class AsteroidFollow : MonoBehaviour {

	public Transform asteroid;
	public Transform leftBoundary;
	public Transform rightBoundary;

	void Update () {
		Vector3 newPosition = transform.position;
		newPosition.x = asteroid.position.x;
		newPosition.x = Mathf.Clamp (newPosition.x, leftBoundary.position.x, rightBoundary.position.x);
		transform.position = newPosition;	
	}
}
