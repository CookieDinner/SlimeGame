using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject Teleport2;
    public GameObject Player;
    public Transform RespawnPoint;
    public Animator animator;
    public bool open = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (open == false)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            if (collision.gameObject.tag == "Player")
            {
                if (ScoreManager.instance.score >= 1)
                {
                    FindObjectOfType<AudioManager>().Play("DoorOpen");
                    animator.SetInteger("OpenDoor", 1);
                    ScoreManager.instance.ChangeScore(-1);
                    StartCoroutine(ExampleCoroutine());
                }
                else
                {

                }
            }
        }
    }

    void Teleport()
    {
        Player.transform.position = new Vector2(Teleport2.transform.position.x, Teleport2.transform.position.y);
        RespawnPoint.position = new Vector2(Teleport2.transform.position.x, Teleport2.transform.position.y);
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(1);
        Teleport();
        open = true;
    }
}
