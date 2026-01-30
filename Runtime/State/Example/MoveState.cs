using UnityEngine;

namespace JoonyleGameDevKit
{
    public sealed class MoveState : StateBase<Player>
    {
        public override void Enter(Player owner)
        {
            Debug.Log($"Enter MoveState - owner type: {owner.GetType()}");
        }

        public override void Exit(Player owner)
        {
            Debug.Log($"Exit MoveState - owner type: {owner.GetType()}");
        }

        public override void Update(Player owner)
        {
            Debug.Log($"Update MoveState - owner type: {owner.GetType()}");
        }
    }
}
