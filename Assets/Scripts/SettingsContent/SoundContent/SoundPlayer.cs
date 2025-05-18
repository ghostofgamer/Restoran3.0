using UnityEngine;

namespace SettingsContent.SoundContent
{
    public class SoundPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _buttonClick;
        [SerializeField] private AudioClip _wheelNeedle;

        public static SoundPlayer Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlayButtonClick()
        {
            _audioSource.PlayOneShot(_buttonClick);
        }

        public void PlayWheelFortune()
        {
            _audioSource.PlayOneShot(_wheelNeedle);
        }
    }
}