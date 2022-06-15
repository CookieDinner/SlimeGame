using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Start is called before the first frame update

    public bool activated = false;
    public GameObject Player;
    public ParticleSystem ps;
    public Transform RespawnPoint;
    private void Awake()
    {
        ps.Stop();
    }
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated == false)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            if (collision.gameObject.tag == "Player")
            {
                FindObjectOfType<AudioManager>().Play("KeyCollect");
                foreach (GameObject flag in GameObject.FindGameObjectsWithTag("Checkpoint"))
                {
                    flag.GetComponent<Checkpoint>().activated = false;
                    flag.GetComponent<ParticleSystem>().Stop();
                }
                RespawnPoint.position = new Vector2(transform.position.x, transform.position.y);
                activated = true;
                ps.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
