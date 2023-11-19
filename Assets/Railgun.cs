using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoints;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform firingPoint;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float bulletPerSeconds = 1f;
    
    private Transform target;
    private float timeUntilFire;
    public int BeamBaseDamage = 1;
    public int BeamScaleDamage = 2;
    
    private int bulletDamage = 1;
    private LineRenderer lineRenderer;
    public Color lineColor = Color.red;

    private void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(lineColor.r, lineColor.g, lineColor.b, 1.0f);

        lineRenderer.material = material;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.1f;
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
    }
    private void Shoot()
    {
        target.GetComponent<Health>().TakeDamage(bulletDamage);
        bulletDamage += BeamScaleDamage;
    }
    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,(Vector2) transform.position, 0f, enemyMask);
        if (hits.Length > 0) {
            bulletDamage = BeamBaseDamage;
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
        lineRenderer.SetPosition(0, firingPoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

}