using System;
using UnityEngine;

namespace Enemy.Skeleton
{
    public class ResourceHandler: Enemy.ResourceHandler
    {
        private NavigationHandler _navigationHandler;

        private void Awake()
        {
            _navigationHandler = GetComponent<NavigationHandler>();
        }

        public override void ModifyHealth(int amount)
        {
            base.ModifyHealth(amount);
            if (health <= 50 && !_navigationHandler.isEscape)
            {
                _navigationHandler.isEscape = true;
            } 
        }
    }
}