using Player;
using UnityEngine;

namespace Actor
{
    public interface IController
    {
        public IMovement Movement { get; }
        public IAnimation Animation { get; }
    }

    public interface IAnimation
    {
        
    }

    public interface IResource {}

    public interface IMovement
    {
        void ApplyKnockBack(Vector3 knockBackDirection, float force);
    }
}