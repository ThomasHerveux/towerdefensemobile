using System.Xml.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bulletPerSeconds = 1f;
    [SerializeField] private int baseUpgradeCost = 100;
    
    private float bpsBase;
    private float targetingRangeBase;
    private Transform target;
    private float timeUntilFire;
    private int level = 1;
    private float lastClickTime = 0f;
    private float doubleClickTimeThreshold = 0.3f;
    private void Start()
    {
        bpsBase = bulletPerSeconds;
        targetingRangeBase = targetingRange;
    }
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
        if (Input.GetMouseButtonDown(0)) {
            float timeSinceLastClick = Time.time - lastClickTime;
            if (timeSinceLastClick <= doubleClickTimeThreshold)
                Upgrade();
            lastClickTime = Time.time;
        }
    }
    private void Upgrade()
    {
        int cost = Mathf.RoundToInt(baseUpgradeCost * Mathf.Pow(level, 0.8f));
        if (cost > LevelManager.main.currency) return;

        LevelManager.main.SpendCurrency(cost);
        level++;

        bulletPerSeconds = bpsBase * Mathf.Pow(level, 0.6f);
        targetingRange = targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        Bullet bulletscript = bulletObj.GetComponent<Bullet>();
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
