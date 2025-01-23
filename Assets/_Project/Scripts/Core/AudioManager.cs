using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;

namespace ShadowWorker.Core
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudio();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeAudio()
        {
            if (!musicSource || !sfxSource)
            {
                Debug.LogError("Audio sources not set in AudioManager!");
                return;
            }
        }

        public void PlayMusic(AudioClip clip, bool loop = true)
        {
            if (!clip) return;
            
            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }

        public void PlaySFX(AudioClip clip)
        {
            if (!clip) return;
            sfxSource.PlayOneShot(clip);
        }

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        }
    }
} 