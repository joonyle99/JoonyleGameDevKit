using UnityEngine;

namespace JoonyleGameDevKit
{
    public interface IGeneratable
    {
        public void OnGenerated(ItemGenerator itemGenerator);
        public void OnEliminated();
    }

    public class ItemBase : MonoBehaviour, IGeneratable
    {
        private ItemGenerator _fromGenerator;
        public ItemGenerator FromGenerator => _fromGenerator;

        public void OnGenerated(ItemGenerator itemGenerator)
        {
            _fromGenerator = itemGenerator;
            _fromGenerator.NowCount++;
        }

        public void OnEliminated()
        {
            _fromGenerator.NowCount--;
            _fromGenerator.ObjectPooling.ReturnObject(this);
        }
    }
}
