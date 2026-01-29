using UnityEngine;

namespace JoonyleGameDevKit
{
    public sealed class JumpState : StateBase<Player>
    {
        public override void Enter(Player owner)
        {
            Debug.Log($"Enter JumpState - owner type: {owner.GetType()}");
        }

        public override void Exit(Player owner)
        {
            Debug.Log($"Exit JumpState - owner type: {owner.GetType()}");
        }

        public override void Update(Player owner)
        {
            Debug.Log($"Update JumpState - owner type: {owner.GetType()}");
        }
    }
}
