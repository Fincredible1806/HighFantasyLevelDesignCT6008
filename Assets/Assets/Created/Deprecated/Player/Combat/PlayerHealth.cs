using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    private int health = 25;
    [SerializeField] TextMeshProUGUI healthText;

    private void Awake()
    {
        healthText.text = ("Health: " +  health).ToString();   
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("TakenDamage");
        health = health - damage;
        healthText.text = ("Health: " + health).ToString();
        if (health <= 0)
        {
            SceneManager.LoadScene("MainScene");
        }
    }    
}
