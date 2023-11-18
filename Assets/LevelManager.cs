using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform StartPoint;
    public Transform[] path;
    public int currency = 500;
    private void Awake()
    {
        main = this;
    }
    private void Start() {
        currency = 500;
    }
    public void IncreaseCurrency(int amount) {
        currency += amount;
    }
    public bool SpendCurrency(int amount) {
        if (amount <= currency) {
            currency -= amount;
            return true;
        }
        Debug.Log("No money");
        return false;
    }
}
