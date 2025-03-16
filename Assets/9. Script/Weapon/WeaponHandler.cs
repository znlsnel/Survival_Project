using UnityEngine;

public class WeaponHandler: MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    public float Delay { get => delay; set => delay = value; }
    
    [SerializeField] private float power = 1f;
    
    [SerializeField] private float knockbackPower = 0.1f;
}