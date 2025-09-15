using UnityEngine;

namespace CustomizationContent
{
    public class StyleCustomization : MonoBehaviour
    {
        [SerializeField] private Texture[] _floorTextures;
        [SerializeField] private Texture[] _outsideWallTextures;
        [SerializeField] private Texture[] _insideWallTextures;
        [SerializeField] private Texture[] _kitchenTextures;
        [SerializeField] private StyleOption[] _visorOptions;
        [SerializeField] private Material _floorMaterial;
        [SerializeField] private Material _outsideWallMaterial;
        [SerializeField] private Material _insideWallMaterial;
        [SerializeField] private Material _kitchenMaterial;
        [SerializeField] private Material _visorMaterial;

        public void ChangeFloorTexture(int index) =>
            ApplyTexture(_floorTextures, _floorMaterial, index);

        public void ChangeOutsideWallTexture(int index) =>
            ApplyTexture(_outsideWallTextures, _outsideWallMaterial, index);

        public void ChangeInsideWallTexture(int index) =>
            ApplyTexture(_insideWallTextures, _insideWallMaterial, index);

        public void ChangeKitchenTexture(int index) =>
            ApplyTexture(_kitchenTextures, _kitchenMaterial, index);

        public void ChangeVisorTexture(int index)
        {
            _visorMaterial.mainTexture = _visorOptions[index].texture != null ? _visorOptions[index].texture : null;
            _visorMaterial.color = _visorOptions[index].color;
        }

        private void ApplyTexture(Texture[] textures, Material material, int index)
        {
            if (textures == null || textures.Length == 0)
            {
                Debug.LogWarning("Текстуры не заданы!");
                return;
            }

            if (material == null)
            {
                Debug.LogWarning("Материал не назначен!");
                return;
            }

            if (index < 0 || index >= textures.Length)
            {
                Debug.LogWarning("Индекс вне диапазона!");
                return;
            }

            material.mainTexture = textures[index];
        }
    }
}

[System.Serializable]
public struct StyleOption
{
    public bool isTexture; // true = texture, false = color
    public Texture texture; // можно оставить null, если color
    public Color color; // можно игнорировать, если texture
}