using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AudioLibrary
{
    [System.Serializable]
    internal class AudioLabel
    {
        [SerializeField] private string _label;
        [SerializeField] private AudioClip _audioClip;

        public string Label { set => _label = value; get => _label; }
        public AudioClip AudioClip { set => _audioClip = value; get => _audioClip; }
    }

    [System.Serializable]
    internal class AudioCategory
    {
        [SerializeField] private string _categoryName;
        [SerializeField] private List<AudioLabel> _audioLabelList = new List<AudioLabel>();

        public string CategoryName { set => _categoryName = value; get => _categoryName; }
        public List<AudioLabel> AudioLabel { set => _audioLabelList = value; get => _audioLabelList; }
    }

    [System.Serializable]
    internal class AudioData
    {
        [SerializeField] List<AudioCategory> _audioCategoryList = new List<AudioCategory>();

        public List<AudioCategory> AudioCategoryList { set => _audioCategoryList = value; get => _audioCategoryList; }
    }

    [CreateAssetMenu(fileName = "AudioLibraryAsset",menuName = "AudioLibrary/AudioLibraryAsset")]
    public sealed class AudioLibraryAsset : ScriptableObject
    {
        [SerializeField]
        AudioData AudioBGM;
        [SerializeField]
        AudioData AudioSFX;

        public AudioClip GetSFXAudio(string categoryName,string labelName)
        {
            var audioCategory = AudioSFX.AudioCategoryList.FirstOrDefault(catName => catName.CategoryName == categoryName);

            if(audioCategory != null)
            {
                var audioLabel = audioCategory.AudioLabel.FirstOrDefault(lblName => lblName.Label == labelName);
                if(audioLabel != null)
                {
                    return audioLabel.AudioClip;
                }
            }
            return null;
        }

        public AudioClip GetBGMAudio(string categoryName, string labelName)
        {
            var audioCategory = AudioBGM.AudioCategoryList.FirstOrDefault(catName => catName.CategoryName == categoryName);

            if (audioCategory != null)
            {
                var audioLabel = audioCategory.AudioLabel.FirstOrDefault(lblName => lblName.Label == labelName);
                if (audioLabel != null)
                {
                    return audioLabel.AudioClip;
                }
            }
            return null;
        }

    }
}
