using UnityEngine;

public class PlayerMeleePivot: MonoBehaviour
{
    public Transform hand;

    private void FixedUpdate()
    {
        transform.position = hand.position;
        transform.rotation = hand.rotation;
    }
}