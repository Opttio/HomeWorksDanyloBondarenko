using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace _Project.Scripts.Runtime.Controllers
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController Instance;
        
        [SerializeField] private AudioMixerGroup _musicGroup;
        [SerializeField] private AudioMixerGroup _enemyGroup;
        [SerializeField] private AudioMixerGroup _sfxGroup;
        [SerializeField] private AudioMixerGroup _uiGroup; 
        
        private AudioSource _musicSource;
        private AudioSource _enemySource;
        private AudioSource _sfxSource;
        private AudioSource _uiSource;
        
        private Coroutine _playMusicRoutine;

        private void Awake()
        {
            Initialise();
        }

        public void StopAllAudioSources()
        {
            _musicSource.Stop();
            _enemySource.Stop();
            _sfxSource.Stop();
            _uiSource.Stop();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (_musicSource)
                _musicSource.Stop();
            _musicSource.clip = clip;
            _musicSource.Play();
        }

        public void PlayMusic(AudioClip clip, float duration)
        {
            if (_musicSource)
                _musicSource.Stop();
            _musicSource.clip = clip;
            _musicSource.Play();
            if (_playMusicRoutine != null)
                StopCoroutine(_playMusicRoutine);
            _playMusicRoutine = StartCoroutine(FadeMixerVolume(_musicGroup.audioMixer, "MusicVolume", -60f, -10f, duration));
        }

        private IEnumerator  FadeMixerVolume(AudioMixer musicMixer, string musicVolume, float start, float end, float duration)
        {
            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                musicMixer.SetFloat(musicVolume, Mathf.Lerp(start, end, time / duration));
                yield return null;
            }
            musicMixer.SetFloat(musicVolume, end);
        }

        public void PlayEnemy(AudioClip clip)
        {
            if (_enemySource)
                _enemySource.Stop();
            _enemySource.clip = clip;
            _enemySource.Play();
        }

        public void PlaySfx(AudioClip clip, float pitch = 1f)
        {
            _sfxSource.pitch = pitch;
            _sfxSource.PlayOneShot(clip);
        }

        public void PlayUi(AudioClip clip)
        {
            if(_uiSource)
                _uiSource.Stop();
            _uiSource.clip = clip;
            _uiSource.Play();
        }

        private void Initialise()
        {
            ToSingleTone();
            CreateMusicAudioSource();
            CreateSfxAudioSource();
            CreateUiAudioSource();
            CreateEnemyAudioSource();
        }

        private void ToSingleTone()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void CreateMusicAudioSource()
        {
            _musicSource = CreateAudioSource("Music", _musicGroup, 1f, true);
        }

        private void CreateEnemyAudioSource()
        {
            _enemySource = CreateAudioSource("Enemy", _enemyGroup, 1f, true, true);
        }

        private void CreateSfxAudioSource()
        {
            _sfxSource = CreateAudioSource("SFX", _sfxGroup);
        }

        private void CreateUiAudioSource()
        {
            _uiSource = CreateAudioSource("UI", _uiGroup);
        }

        private AudioSource CreateAudioSource(string objectNames, AudioMixerGroup group, float volume = 1f, bool loop = false, bool plaOnAwake = false)
        {
            var source = new GameObject(objectNames).AddComponent<AudioSource>();
            source.transform.SetParent(transform);
            
            source.outputAudioMixerGroup = group;
            source.volume = volume;
            source.loop = loop;
            source.playOnAwake = plaOnAwake;

            return source;
        }
    }
}