using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
  [SerializeField] List<SoundEffectData> soundEffects;
  AudioSource audioSource;

  public static AudioManager Instance { get; private set; }

  void Awake()
  {
    if (Instance != null && Instance != this)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    DontDestroyOnLoad(gameObject);

    audioSource = GetComponent<AudioSource>();
  }

  public void PlaySFX(SoundEffect sound)
  {
    SoundEffectData soundData = soundEffects.Find(x => x.type == sound);

    if (soundData != null)
    {
      audioSource.PlayOneShot(soundData.clip);
    }
  }
}