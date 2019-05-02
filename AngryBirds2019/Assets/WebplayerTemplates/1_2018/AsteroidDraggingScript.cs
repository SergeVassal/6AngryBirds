using UnityEngine;
using System.Collections;

public class AsteroidDraggingScript : MonoBehaviour {

	public LineRenderer backLine;
	public LineRenderer frontLine;
	public float maxStretch = 3f;

	private bool clickOn;
	private Transform catapult;
	private SpringJoint2D spring;
	private float maxStretchSqr;
	private Ray rayToMouse;
	private Ray frontCatapultToProjRay;
	private Rigidbody2D rBody;
	private Vector2 prevVelocity;
	private float circleRadius;

	void Awake(){
		rBody = GetComponent<Rigidbody2D> ();
		spring = GetComponent<SpringJoint2D> ();
		catapult = spring.connectedBody.transform;
	}

	void Start () {			
		LinesSetup ();
		maxStretchSqr = maxStretch * maxStretch;
		frontCatapultToProjRay = new Ray (frontLine.transform.position, Vector3.zero);
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		CircleCollider2D circle = GetComponent<CircleCollider2D> () as CircleCollider2D;
		circleRadius = circle.radius;

	}	

	void LinesSetup () {
		backLine.SetPosition (0, backLine.transform.position);
		frontLine.SetPosition (0, frontLine.transform.position);

		backLine.sortingLayerName = "FG";
		backLine.sortingOrder = 1;
		frontLine.sortingLayerName = "FG";
		frontLine.sortingOrder = 3;

	}

	void Update(){
		if (clickOn) {
			Dragging ();
		}

		if (spring != null) {
			if (!rBody.isKinematic && prevVelocity.sqrMagnitude > rBody.velocity.sqrMagnitude) {
				Destroy (spring);
				rBody.velocity = prevVelocity;
			}
			if (!clickOn) {
				prevVelocity = rBody.velocity;
			}
			LineUpdate ();
		} else {
			backLine.enabled = false;
			frontLine.enabled = false;
		}

	}

	void OnMouseDown(){
		spring.enabled = false;
		clickOn = true;
	}

	void OnMouseUp(){
		spring.enabled = true;
		rBody.isKinematic = false;
		clickOn = false;
	}

	void Dragging(){
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}
		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineUpdate(){
		Vector2 frontCatToProjectile = transform.position - frontLine.transform.position;
		frontCatapultToProjRay.direction = frontCatToProjectile;
		Vector3 holdPoint = frontCatapultToProjRay.GetPoint (frontCatToProjectile.magnitude + circleRadius);
		frontLine.SetPosition (1, holdPoint);
		backLine.SetPosition (1, holdPoint);
	}
}
