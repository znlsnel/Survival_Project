using System;
using UnityEngine;

namespace Enemy
{
    public class ResourceHandler: MonoBehaviour
    {
        public int maxHealth = 100;
        public int health = 100;
        
        public Action OnDeath;

        public void ModifyHealth(int amount)
        {
            health = Mathf.Clamp(health + amount, 0, maxHealth);
            if (health <= 0) OnDeath?.Invoke();
        }
    }
}