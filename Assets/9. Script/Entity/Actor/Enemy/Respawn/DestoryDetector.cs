using UnityEngine;

namespace Respawn
{
    public class DestroyedDetector: MonoBehaviour
    {
        private RespawnArea _respawnArea;

        public void RegisterArea(RespawnArea respawnArea)
        {
            _respawnArea = respawnArea;
        }
        
        private void OnDestroy()
        {
            _respawnArea.UnRegister(gameObject);
        }
    }
}