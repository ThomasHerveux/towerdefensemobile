using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_zone : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private int ZoneDamage = 1;
    [SerializeField] private float lifespan = 5f;

    private float timeUntildespawn;

    public Transform target;
    private void FixedUpdate()
    {
        timeUntildespawn += Time.deltaTime;
        if (timeUntildespawn >= (lifespan)) {
            Destroy(gameObject);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        other.gameObject.GetComponent<Health>().TakeDamage(ZoneDamage);
    }
}
