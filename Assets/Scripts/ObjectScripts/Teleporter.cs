using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject Teleport2;
    public GameObject Player;
    public Transform RespawnPoint;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Teleport();
        }
    }

    void Teleport()
    {
        Player.transform.position = new Vector2(Teleport2.transform.position.x, Teleport2.transform.position.y);
        RespawnPoint.position = new Vector2(Teleport2.transform.position.x, Teleport2.transform.position.y);
    }
}
