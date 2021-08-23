using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : Singleton<BackgroundController>
{
    [System.Serializable]
    public struct Layer
    {
        public Transform LayerTransform;
        public float ParalaxMultiplier;
        [HideInInspector] public float SpriteSizeX;
        [HideInInspector] public float SpriteSizeY;
    }
    [SerializeField] private Layer[] _layers;
    private Transform _cameraTransform;
    private Vector3 _lastCamerPosition;
    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCamerPosition = _cameraTransform.position;
        for (int i = 0; i < _layers.Length; i++)
        {
            var sprite = _layers[i].LayerTransform.GetComponent<SpriteRenderer>().sprite;
            var spriteTexture = sprite.texture;
            _layers[i].SpriteSizeX = spriteTexture.width / sprite.pixelsPerUnit;
            _layers[i].SpriteSizeY = spriteTexture.height / sprite.pixelsPerUnit;
        }
    }

    public void Move()
    {
        foreach (Layer layer in _layers)
        {
            var cameraPos = _cameraTransform.position;
            var layerPos = layer.LayerTransform.position;
            var delta = cameraPos - _lastCamerPosition;
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
            _lastCamerPosition = _cameraTransform.position;
    }
}
