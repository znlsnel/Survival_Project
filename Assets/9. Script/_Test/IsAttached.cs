using UnityEngine;

public class IsAttached: MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }
}