using System;
using System.Collections;
using Actor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy.Grenadier
{
    // [RequireComponent(typeof(AnimationHandler), typeof(AudioHandler), typeof(MovementHandler))]
    // [RequireComponent(typeof(NavigationHandler), typeof(ResourceHandler), typeof(RewardHandler))]
    public class Controller : Enemy.Controller
    {
        public GameObject Bomb;
        public Transform handTransform;

        public void GenerateBomb()
        {
            var bomb = Instantiate(Bomb, transform.position, Quaternion.identity);
            bomb.transform.parent = handTransform;
        }
    }
    
}
