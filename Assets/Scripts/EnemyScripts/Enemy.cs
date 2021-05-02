using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;

        if(currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        //Die animation

        //Disable the enemy

        Destroy(gameObject);
    }

}
