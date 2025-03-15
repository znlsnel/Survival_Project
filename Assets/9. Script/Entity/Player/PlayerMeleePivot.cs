using UnityEngine;

// 그냥 child로 지정하면 되지 않을까?
public class PlayerMeleePivot: MonoBehaviour
{
    public Transform hand;

    private void FixedUpdate()
    {
        transform.position = hand.position;
        transform.rotation = hand.rotation;
    }
}