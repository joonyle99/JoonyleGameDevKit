using UnityEngine;

namespace JoonyleGameDevKit
{
    public class Player : MonoBehaviour
    {
        private StateMachine<Player> _fsm;

        private void Awake()
        {
            _fsm = new StateMachine<Player>(this);
            _fsm.AddState(new IdleState());
            _fsm.AddState(new MoveState());
            _fsm.AddState(new JumpState());
        }
        private void Start()
        {
            _fsm?.ChangeState<IdleState>();
        }
        private void Update()
        {
            _fsm?.Update();
        }
    }
}
