using UnityEngine;
using UnityEngine.UI;

namespace AudioLibrary
{
    public class SampleSceneController : MonoBehaviour
    {
        [SerializeField]
        private Button _btnBGM;

        [SerializeField]
        private Button _btnSfx;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioLibraryAsset _audioLibrary;

        void Start()
        {
            _btnBGM.onClick.AddListener(() => {
                _audioSource.clip = _audioLibrary.GetBGMAudio("Background", "Level1");
                _audioSource.Play();
            });
            _btnSfx.onClick.AddListener(() => {
                _audioSource.clip = _audioLibrary.GetSFXAudio("SoundEffect", "Fire");
                _audioSource.Play();
            });
        }
    }
}
