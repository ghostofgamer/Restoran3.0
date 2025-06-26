using UnityEngine;

namespace CityTrafficContent
{
    public class CarNPC : AbstractNPC
    {
        [SerializeField] private Texture[] _textures;
        [SerializeField] private Renderer _renderer;

        private Material _material;
        
        public override void InitUniqueData()
        {
            if (_renderer != null)
            {
                _material = new Material(_renderer.material);
                _material.mainTexture = _textures[Random.Range(0, _textures.Length)];
                _renderer.material = _material;
            }
        }
    }
}