using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poison_turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoints;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bulletPerSeconds = 1f;
    
    private Transform target;
    private float timeUntilFire;

    private void Update()
    {
        if (target == null) {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange()) {
            target = null;
        } else {
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= (1f / bulletPerSeconds)) {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }
    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Poison_bullet bulletscript = bulletObj.GetComponent<Poison_bullet>();
        bulletscript.SetTarget(target);
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,(Vector2) transform.position, 0f, enemyMask);
        if (hits.Length > 0) {
            target = hits[0].transform;
        }
    }
    private bool CheckTargetIsInRange()
    {
        if (Vector2.Distance(target.position, transform.position) <= targetingRange)
            return true;
        else
            return false;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoints.rotation = Quaternion.RotateTowards(turretRotationPoints.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}