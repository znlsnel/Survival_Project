using UnityEngine;

namespace Enemy
{
    public class Resource: MonoBehaviour
    {
        public int maxHealth = 100;
        public int health = 100;

        public void ModifyHealth(int amount)
        {
            health = Mathf.Clamp(health + amount, 0, maxHealth);
            if (health <= 0) Destroy(gameObject);
        }
    }
}