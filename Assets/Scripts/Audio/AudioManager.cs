using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
  public static AudioManager Instance { get; private set; }

  [SerializeField] List<SoundEffectData> soundEffects;
  [SerializeField] List<MusicData> musics;
  [SerializeField] AudioSource sfxSource;
  [SerializeField] AudioSource musicSource;

  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);
  }

  public void PlaySFX(SoundEffect sound)
  {
    SoundEffectData soundData = soundEffects.Find(x => x.type == sound);

    if (soundData != null)
    {
      sfxSource.PlayOneShot(soundData.clip);
    }
  }

  public void PlayMusic(Music music)
  {
    MusicData musicData = musics.Find(x => x.type == music);

    if (musicData != null)
    {
      musicSource.loop = true;
      musicSource.volume = 0.3f;
      musicSource.clip = musicData.clip;
      musicSource.Play();
    }
  }

  public void LowerMusic()
  {
    musicSource.volume = 0.15f;
  }

  public void RiseMusic()
  {
    musicSource.volume = 0.3f;
  }
}