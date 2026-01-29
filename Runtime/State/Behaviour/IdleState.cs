using UnityEngine;

namespace JoonyleGameDevKit
{
    public sealed class IdleState : StateBase<Player>
    {
        public override void Enter(Player owner)
        {
            Debug.Log($"Enter IdleState - owner type: {owner.GetType()}");
        }

        public override void Exit(Player owner)
        {
            Debug.Log($"Exit IdleState - owner type: {owner.GetType()}");
        }

        public override void Update(Player owner)
        {
            Debug.Log($"Update IdleState - owner type: {owner.GetType()}");
        }
    }
}
