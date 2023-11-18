using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager main;
    [Header("References")]
    [SerializeField] private Tower[] towers;
    private int selcetedTower = 0;
    private void Awake() {
        main = this;
    }
    public Tower GetSelectedTower() {
        return towers[selcetedTower];
    }
    public void SetSelectedTower(int _selectedTower) {
        selcetedTower = _selectedTower;
    }
}
