using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TurretSlowMo : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask enemyMask;

    [Header("Attributes")]
    [SerializeField] private float targetingRange = 3f;
    [SerializeField] private float attackPerSecond = 4f;
    [SerializeField] private float freezeTime = 1f;
    [SerializeField] private int baseUpgradeCost = 100;
    
    private float apsBase;
    private float targetingRangeBase;
    private float timeUntilFire;
    private int level = 1;
    private float lastClickTime = 0f;
    private float doubleClickTimeThreshold = 0.3f;

    private void Start()
    {
        apsBase = attackPerSecond;
        targetingRangeBase = targetingRange;
    } 
    private void Update()
    {
        timeUntilFire += Time.deltaTime;

        if (timeUntilFire >= (1f / attackPerSecond)) {
            Freeze();
            timeUntilFire = 0f;
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

        attackPerSecond = apsBase * Mathf.Pow(level, 0.6f);
        targetingRange = targetingRangeBase * Mathf.Pow(level, 0.4f);
    }
    private void Freeze()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange,(Vector2) transform.position, 0f, enemyMask);
        if (hits.Length > 0) {
            for (int i = 0; i < hits.Length; i++) {
                RaycastHit2D hit = hits[i];

                EnnemyMovement em = hit.transform.GetComponent<EnnemyMovement>();
                em.UpdateSpeed(0.5f);

                StartCoroutine(ResetEnemySpeed(em));
            }
        }
    }
    private IEnumerator ResetEnemySpeed(EnnemyMovement em)
    {
        yield return new WaitForSeconds(freezeTime);

        em.ResetSpeed();
    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
