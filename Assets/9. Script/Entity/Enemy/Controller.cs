using System;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Animation), typeof(Audio), typeof(Movement))]
    [RequireComponent(typeof(Navigation), typeof(Resource))]
    public class Controller : MonoBehaviour
    {
        private Animation _animation;
        private Audio _audio;
        private Movement _movement;
        private Navigation _navigation;
        private Resource _resource;

        void Awake()
        {
            _animation = GetComponent<Animation>();
            _audio = GetComponent<Audio>();
            _movement = GetComponent<Movement>();
            _navigation = GetComponent<Navigation>();
            _resource = GetComponent<Resource>();
        }
    }
    
}
