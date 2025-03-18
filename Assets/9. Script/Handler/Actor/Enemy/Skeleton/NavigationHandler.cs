using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Skeleton
{
    public class NavigationHandler: Enemy.NavigationHandler
    { 
        // private ResourceHandler _resourceHandler;
        public bool isEscape = false;
        
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateStatus()
        {
            if (!target) throw new UnityException("enemy navigation: target not set");
            
            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            
            
            if (_currStatus != Status.Attacking)
            {
                if(distanceToTarget <= chaseRange) _currStatus = Status.Detected;
                else if(distanceToTarget <= stoppingDistance) _currStatus = Status.Attackable;
                else _currStatus = Status.Idle;
            }
            
            if(isEscape) _currStatus = Status.Escape; 

            if (_currStatus != _prevStatus) WhenChangedStatus?.Invoke(_currStatus);
            _prevStatus = _currStatus;
        }
        
        protected virtual void Update()
        {
            // if (_resourceHandler.isDying) return;
            UpdateStatus();
            
            
            if (_currStatus == Status.Escape)
            {
                Debug.Log("escape");
                Vector3 escapeDirection = transform.position - target.position;
                Vector3 newTargetPosition = transform.position + escapeDirection.normalized * 3f;

                if (NavMesh.SamplePosition(newTargetPosition, out var hit, 10f, NavMesh.AllAreas))
                {
                    Agent.SetDestination(hit.position);
                }

                return;
            }
            
            if (_currStatus == Status.Detected && !Agent.isStopped)
            {
                Debug.Log(1);
                Agent.SetDestination(target.position);
            }
            if(_currStatus != Status.Idle && _currStatus != Status.Attacking) transform.rotation = Quaternion.Euler(0, Quaternion.LookRotation(target.position - transform.position).eulerAngles.y, 0);
         
        }
    }
}