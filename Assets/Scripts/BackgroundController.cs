using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static UnityEngine.Camera;

public class BackgroundController : Singleton<BackgroundController>
{
    private enum LayerName
    {
        Back,
        Middle,
        Front
    }
    [System.Serializable]
    public struct Layer
    {
        public Transform LayerTransform;
        public float ParalaxMultiplier;
        [HideInInspector] public float SpriteSizeX;
        [HideInInspector] public float SpriteSizeY;
    }

    [SerializeField]private Layer[] _layers;
    private Transform _cameraTransform;
    private Vector3 _lastCameraPosition;

    private void Start()
    {
        if (main is { }) _cameraTransform = main.transform;
        _lastCameraPosition = _cameraTransform.position;
        for (int i = 0; i < _layers.Length; i++)
        {
            SetLayerSpriteSize(i, _layers[i].LayerTransform.GetComponent<SpriteRenderer>().sprite);
        }
    }

    private void SetLayerSpriteSize(int layerIndex, Sprite sprite)
    {
        var spriteTexture = sprite.texture;
        _layers[layerIndex].SpriteSizeX = spriteTexture.width / sprite.pixelsPerUnit;
        _layers[layerIndex].SpriteSizeY = spriteTexture.height / sprite.pixelsPerUnit;

    }

    private async System.Threading.Tasks.Task SetBackgroundSprite(string spriteName, LayerName layerName, float paralaxMultiplier)
    {
        var layerObjTransform = transform.GetChild((int) layerName);

        _layers[(int)layerName].LayerTransform = layerObjTransform;

        _layers[(int)layerName].ParalaxMultiplier = paralaxMultiplier;

        var sprite = await Addressables.LoadAssetAsync<Sprite>(spriteName).Task;
        layerObjTransform.GetComponent<SpriteRenderer>().sprite = sprite;
        
        SetLayerSpriteSize((int)layerName,sprite);
    }
    public void Move()
    {
        foreach (var layer in _layers)
        {
            
            var cameraPos = _cameraTransform.position;
            var layerPos = layer.LayerTransform.position;
            var delta = cameraPos - _lastCameraPosition;
            layer.LayerTransform.position = (Vector2)layerPos + (Vector2)delta * layer.ParalaxMultiplier;

            if(Mathf.Abs(cameraPos.x - layerPos.x) >= layer.SpriteSizeX)
            {
                float offsetX = (cameraPos.x - layerPos.x) % layer.SpriteSizeX;
                layer.LayerTransform.position = new Vector3(cameraPos.x + offsetX, cameraPos.y, layerPos.z);
            }
            if (Mathf.Abs(cameraPos.y - layerPos.y) >= layer.SpriteSizeY)
            {
                float offsetY = (cameraPos.y - layerPos.y) % layer.SpriteSizeY;
                layer.LayerTransform.position = new Vector3(cameraPos.x, cameraPos.y + offsetY, layerPos.z);
            }
        }
        _lastCameraPosition = _cameraTransform.position;
    }
}
