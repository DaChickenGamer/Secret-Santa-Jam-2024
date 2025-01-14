using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private bool _isBattleMusicPlaying = false;
    private Coroutine _battleMusicCoroutine;

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

    public void PlayMusic(string name)
    {
        AudioClip sound = Array.Find(musicSounds, song => song.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = sound;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void StopMusic()
    {
        if (!musicSource.isPlaying) return;
        musicSource.Stop();

        _isBattleMusicPlaying = false;
    }

    public void PlaySFX(string name)
    {
        AudioClip sound = Array.Find(sfxSounds, sfx => sfx.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(sound);
        }
    }

    public float GetSfxLength(string name)
    {
        AudioClip sound = Array.Find(sfxSounds, sfx => sfx.name == name);

        if (sound == null)
        {
            Debug.Log("Sound Not Found");
            return 0f;
        }
        else
        {
            return sound.length;
        }
    }

    public void StartBattleMusic()
    {
        if (_isBattleMusicPlaying) return;

        _isBattleMusicPlaying = true;

        _battleMusicCoroutine = StartCoroutine(PlayShuffledMusic());
    }

    public void StopBattleMusic()
    {
        if (!_isBattleMusicPlaying) return;

        _isBattleMusicPlaying = false;

        if (_battleMusicCoroutine != null)
        {
            StopCoroutine(_battleMusicCoroutine);
            _battleMusicCoroutine = null;
        }

        musicSource.Stop();
    }

    private IEnumerator<WaitForSeconds> PlayShuffledMusic()
    {
        List<AudioClip> shuffledMusic = ShuffleSongs(new List<AudioClip>(musicSounds));

        while (_isBattleMusicPlaying)
        {
            foreach (AudioClip song in shuffledMusic)
            {
                musicSource.clip = song;
                musicSource.Play();

                yield return new WaitForSeconds(song.length);

                if (!_isBattleMusicPlaying) yield break;
            }
        }
    }

    private List<AudioClip> ShuffleSongs(List<AudioClip> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            AudioClip temp = list[i];
            int randomIndex = UnityEngine.Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        return list;
    }

    public void SetSfxVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public float GetSfxVolume()
    {
        return sfxSource.volume;
    }

    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
    
}
