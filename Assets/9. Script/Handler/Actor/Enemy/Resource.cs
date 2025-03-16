using UnityEngine;

namespace Enemy
{
    public class Resource: MonoBehaviour
    {
        public int health = 100;


        public void Modify(int amount)
        {
            health += amount;
        }
    }
}