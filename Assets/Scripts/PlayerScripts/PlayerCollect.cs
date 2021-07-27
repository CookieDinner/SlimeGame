using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int coinValue = 1;
    public GameObject keyParticles;
    public GameObject powerUpParticles;
    public SpriteRenderer spriteRenderer;
    private CoroutineManager coroutineManager;

    private void Awake()
    {
        coroutineManager = FindObjectOfType<CoroutineManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        SlimeController slimeController = player.GetComponent<SlimeController>();
        if (other.gameObject.CompareTag("Key"))
        {
            FindObjectOfType<AudioManager>().Play("KeyCollect");

            Destroy(other.gameObject);
            ScoreManager.instance.ChangeScore(coinValue);

        }
        if (!slimeController.powerUpActive)
        {
            if (other.gameObject.CompareTag("JumpPower"))
            {
                FindObjectOfType<AudioManager>().Play("KeyCollect");

                GameObject particle = Instantiate(powerUpParticles, other.transform.position, Quaternion.identity);
                Destroy(particle, 5);
                other.gameObject.SetActive(false);


                coroutineManager.RespawnPowerUp(other.gameObject, 3);
                spriteRenderer.color = new Color(0.1556604f, 0.3800871f, 1f);
                slimeController.canJump = true;
                slimeController.canAttack = true;
                slimeController.isPowerJumping = true;
                slimeController.powerUpActive = true;
            }

            if (other.gameObject.CompareTag("InvulnPower"))
            {
                FindObjectOfType<AudioManager>().Play("KeyCollect");

                GameObject particle = Instantiate(powerUpParticles, other.transform.position, Quaternion.identity);
                Destroy(particle, 5);
                other.gameObject.SetActive(false);

                coroutineManager.RespawnPowerUp(other.gameObject, 5);

                spriteRenderer.color = new Color(1, 0.1428f, 0f);
                slimeController.isInvulnerable = true;
                slimeController.Attack();
                slimeController.powerUpActive = true;
            }
        }
        if (!slimeController.powerUpActive || slimeController.isHoverActive)
        {
            if (other.gameObject.CompareTag("HoverPower"))
            {
                FindObjectOfType<AudioManager>().Play("KeyCollect");

                GameObject particle = Instantiate(powerUpParticles, other.transform.position, Quaternion.identity);
                Destroy(particle, 5);
                other.gameObject.SetActive(false);

                coroutineManager.RespawnPowerUp(other.gameObject, 5);

                spriteRenderer.color = new Color(1, 1, 1, 0.23f);
                slimeController.powerUpActive = true;
                slimeController.isHoverActive = true;
                slimeController.activateHover();
            }
        }

    }
}
