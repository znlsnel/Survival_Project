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

        void Start()
        {
            _animation.WhenAttack += _navigation.StopByAnimation;
            
            _navigation.WhenChangedStatus += (state) =>
            {
                if (state == Navigation.Status.Idle)
                {
                    _animation.animator.SetBool(Animation.HashBoolRun, false);
                }
                if (state == Navigation.Status.Detected)
                {
                    _animation.animator.SetBool(Animation.HashBoolRun, true);
                }

                if (state == Navigation.Status.Attackable)
                {
                    _animation.animator.SetBool(Animation.HashBoolRun, false);
                    _animation.animator.SetBool(Animation.HashBoolAttack, true);
                }
            };
        }
    }
    
}
