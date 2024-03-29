using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private GameObject bulletPrefab;
    public Transform target;
    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) {
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
