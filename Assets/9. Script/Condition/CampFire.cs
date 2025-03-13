using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public float temperatureIncreaseRate = 10f; 
    private List<PlayerCondition> playersInRange = new List<PlayerCondition>();

    private void OnTriggerEnter(Collider other)
    {
        PlayerCondition playerCondition = other.GetComponent<PlayerCondition>();
        if (playerCondition != null)
        {
            playersInRange.Add(playerCondition);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerCondition playerCondition = other.GetComponent<PlayerCondition>();
        if (playerCondition != null)
        {
            playersInRange.Remove(playerCondition);
        }
    }

    private void Update()
    {
        foreach (PlayerCondition player in playersInRange)
        {
            player.Rest(temperatureIncreaseRate);
        }
    }
}
