using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Spikes") || collision.gameObject.CompareTag("Enemy"))
        {
            if (!GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeController>().isInvulnerable)
            {
                FindObjectOfType<AudioManager>().Play("SlimeDeath");
                Destroy(gameObject);
                LevelManager.instance.Respawn();
            }
            else
            {

            }
        }
    }
}
