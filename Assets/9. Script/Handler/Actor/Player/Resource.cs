
using UnityEngine;

namespace Player
{
    public class Resource: MonoBehaviour
    {
        [HideInInspector] public int health = 500;

        public void Modify(int amount)
        { 
            health += amount;
        }
    }
}