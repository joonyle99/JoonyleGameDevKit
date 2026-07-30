using UnityEngine;

namespace JoonyleGameDevKit
{
    public class InGameUIController : MonoBehaviour, IGameStateListener<InGameState>
    {
        private void OnDestroy()
        {

        }

        public void Initialize()
        {

        }

        public void OnStateChanged(InGameState prevState, InGameState currState)
        {
            
        }
    }
}
