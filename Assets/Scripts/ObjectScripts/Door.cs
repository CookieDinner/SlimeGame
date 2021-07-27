using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject Teleport2;
    public GameObject Player;
    public Transform RespawnPoint;
    public Animator animator;
    public int nextScene;
    public bool open = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (open == false)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            if (collision.gameObject.tag == "Player")
            {
                if (ScoreManager.instance.score >= 4)
                {
                    FindObjectOfType<AudioManager>().Play("DoorOpen");
                    animator.SetInteger("OpenDoor", 1);
                    ScoreManager.instance.ChangeScore(-4);
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
        /*Player.transform.position = new Vector2(Teleport2.transform.position.x, Teleport2.transform.position.y);
        RespawnPoint.position = new Vector2(Teleport2.transform.position.x, Teleport2.transform.position.y);*/
        switch (nextScene)
        {
            case 1:
                SceneManager.LoadScene("level_1");
                break;
            case 2:
                SceneManager.LoadScene("level_2");
                break;
            case 3:
                SceneManager.LoadScene("level_3");
                break;
            case 4:
                SceneManager.LoadScene("level_4");
                break;
            case 5:
                SceneManager.LoadScene("GameOver");
                break;

        }
    }

    IEnumerator ExampleCoroutine()
    {
        yield return new WaitForSeconds(1);
        Teleport();
        open = true;
    }
}
