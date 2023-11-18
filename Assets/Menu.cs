using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator anim;

    private bool isOpen = true;

    public void ToggleMenu() {
        isOpen = !isOpen;
        anim.SetBool("isOpen", isOpen);
    }

    private void OnGUI() {
        currencyUI.text = LevelManager.main.currency.ToString();
    }
    private void SetSelected() {
        
    }
}
