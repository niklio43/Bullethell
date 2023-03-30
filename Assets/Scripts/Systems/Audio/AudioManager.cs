using System;
using System.Collections;
using UnityEngine;
/*
 * How to use
 * To make global sound call
 * AudioManager.instance.Play("name");
 * 
 * To make local sound call
 * AudioManager.instance.PlayLocal("name", gameObject);
 */

public class AudioManager : Singleton<AudioManager>
{
    #region Public Fields
    public Sound[] sounds;
    [HideInInspector] public float menuVolume;
    #endregion

    #region Private Fields
    [SerializeField] float multiplier = 30f;
    #endregion

    #region Private Methods
    void Awake()
    {
        foreach (Sound sound in sounds)
        {
            sound.audio = gameObject.AddComponent<AudioSource>();
            sound.audio.clip = sound.clip;
            sound.audio.volume = sound.volume;
            sound.audio.loop = sound.loop;
            sound.audio.outputAudioMixerGroup = sound.mixer;
        }
    }

    IEnumerator DestroyLocalSound(float time, GameObject obj)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }
    #endregion

    #region Public Methods
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.audio.Play();
    }

    public void PlayOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        s.audio.PlayOneShot(s.clip);
    }

    public void PlayLocal(string name, GameObject target)
    {
        GameObject obj = new GameObject("LocalSound");
        obj.transform.SetParent(target.transform);
        obj.transform.position = target.transform.position;

        var source = obj.AddComponent<AudioSource>();

        Sound s = Array.Find(sounds, sound => sound.name == name);

        source.clip = s.clip;
        source.volume = s.volume;
        source.loop = s.loop;
        source.outputAudioMixerGroup = s.mixer;
        source.spatialBlend = 1f;
        source.dopplerLevel = 0f;
        source.maxDistance = s.maxDistance;
        source.priority = 256;

        source.Play();
    }

    public void PlayOnceLocal(string name, GameObject target)
    {
        GameObject obj = new GameObject();
        obj.name = "LocalSound";
        obj.transform.SetParent(target.transform);
        obj.transform.position = target.transform.position;

        var source = obj.AddComponent<AudioSource>();

        Sound s = Array.Find(sounds, sound => sound.name == name);

        source.clip = s.clip;
        source.volume = s.volume;
        source.loop = s.loop;
        source.outputAudioMixerGroup = s.mixer;
        source.spatialBlend = 1f;
        source.dopplerLevel = 0f;
        source.maxDistance = s.maxDistance;

        s.audio.PlayOneShot(s.clip);
        StartCoroutine(DestroyLocalSound(source.clip.length, obj));
    }

    public string GetRandomAudio(string[] audio)
    {
        int randomIndex = UnityEngine.Random.Range(0, audio.Length);

        return audio[randomIndex];
    }

    public void PauseAllSound()
    {
        foreach (Sound sound in sounds)
        {
            sound.audio.Pause();
        }
    }

    public void PauseSound(string name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.audio.Pause();
            }
        }
    }

    public void SetMasterVolume(float soundLevel)
    {
        foreach (Sound sound in sounds)
        {
            sound.mixer.audioMixer.SetFloat("Master", Mathf.Log10(soundLevel) * multiplier);
        }
    }

    public void SetSfxVolume(float soundLevel)
    {
        foreach (Sound sound in sounds)
        {
            sound.mixer.audioMixer.SetFloat("SFX", Mathf.Log10(soundLevel) * multiplier);
        }
    }

    public void SetMusicVolume(float soundLevel)
    {
        foreach (Sound sound in sounds)
        {
            sound.mixer.audioMixer.SetFloat("Music", Mathf.Log10(soundLevel) * multiplier);
        }
    }
    #endregion
}