using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public Slider HealthBar;
    public float health = 100;
    private float currHealth;

    // Start is called before the first frame update
    void Start()
    {
        currHealth = health;
    }

    void Update()
    {
        if (currHealth <= 0)
            SceneManager.LoadScene("Title Screen");
    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
        HealthBar.value = currHealth;
    }
}