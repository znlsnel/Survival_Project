using UnityEngine;

public class ChomperResourceHandler: MonoBehaviour
{
    public float health = 100f;
    public float power = 10f;
    
    // refactor status로 구분해야하는 건지?
    [HideInInspector] public bool isAttacking = false;
    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
}