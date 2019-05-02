using UnityEngine;
using System.Collections;

public class DamageScript : MonoBehaviour {

	public float damageImpactSpeed;
	public int hitPoints=2;
	public Sprite damagedSprite;

	private float damageImpactSpeedSqr;
	private int currentHitPoints;
	private SpriteRenderer spriteRenderer;
	private Collider2D collider;
	private Rigidbody2D rBody;


	void Start () {
		collider = GetComponent<Collider2D> ();
		rBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
	
	}
	
	void OnCollisionEnter2D(Collision2D col){
		if (col.collider.tag != "Damager" && col.collider.tag != "Player") {
			return;
		}
		if (col.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr) {
			return;
		}
		spriteRenderer.sprite = damagedSprite;
		currentHitPoints--;
		if (currentHitPoints <= 0) {
			Kill ();
		}
	}

	void Kill(){
		spriteRenderer.enabled = false;
		collider.enabled = false;
		rBody.isKinematic = true;
	}
}
