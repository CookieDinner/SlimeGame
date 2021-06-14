using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    public int coinValue = 1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            FindObjectOfType<AudioManager>().Play("KeyCollect");

            Destroy(other.gameObject);
            ScoreManager.instance.ChangeScore(coinValue);

        }
    }
}
