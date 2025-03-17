using UnityEngine;

namespace Enemy.Skeleton
{
    public class NavigationHandler: Enemy.NavigationHandler
    {
        
        public new void UpdateStatus()
        {
            if (!target) throw new UnityException("enemy navigation: target not set");
    
            var distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (_currStatus != Status.Attacking)
            {
                _currStatus = Status.Idle;
                if(distanceToTarget <= chaseRange) _currStatus = Status.Detected;
                if(distanceToTarget <= stoppingDistance) _currStatus = Status.Attackable;
            }

            if (_currStatus != _prevStatus) WhenChangedStatus?.Invoke(_currStatus);
            _prevStatus = _currStatus;
        }
    }
}