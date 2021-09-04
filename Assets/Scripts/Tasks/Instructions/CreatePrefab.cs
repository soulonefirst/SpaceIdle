using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Instructions
{
    public class CreatePrefab : Instruction , IInstruction<Vector3>
    {
    
        private readonly GameObject _prefab;
        public Vector3 Value { get; set;}
        public CreatePrefab(MonoBehaviour parent, string prefabName, Vector3 position) : base(parent)
        {
            Value = position;
            _prefab = Addressables.LoadAssetAsync<GameObject>(prefabName).WaitForCompletion();
        }
        protected override bool Update()
        {
            return false;
        }

        protected override void Done()
        {
            var transform = Parent.transform;
            var position = transform.position + Value;
            Object.Instantiate(_prefab, position, Quaternion.identity, transform);
        }

    }
}
