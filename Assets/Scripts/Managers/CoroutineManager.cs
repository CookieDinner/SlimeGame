using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public void RespawnPowerUp(GameObject powerUp, float seconds)
    {
        StartCoroutine(RespawnPower(powerUp, seconds));
    }

    IEnumerator RespawnPower(GameObject powerUp, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        powerUp.SetActive(true);
    }
}
