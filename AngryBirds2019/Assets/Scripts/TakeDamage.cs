using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private float damageImpactSpeed;
    [SerializeField] private Sprite damagedSprite;
    [SerializeField] private int hitPoints;

    private float damageImpactSpeedSqr;
    private SpriteRenderer spriteRenderer;



    private void Start()
    {
        damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag != "Damager" && collision.collider.tag != "Player" ||
            collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr)
        {
            return;
        }
        spriteRenderer.sprite = damagedSprite;
        hitPoints--;
        if (hitPoints <= 0)
        {
            Kill();
        }
    }

    private void Kill()
    {
        gameObject.SetActive(false);
        
    }
}
