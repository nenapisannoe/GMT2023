using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

[DefaultExecutionOrder(-300)]
public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static AudioManager o;
    public AudioSource audioSource;
    public float masterVolume;
    public float masterVolumeMixer;
    public float musicVolumeMixer;
    public float soundVolumeMixer;
    public AudioMixer mainMixer;


    public TextMeshProUGUI audioMasterText;
    public Slider masterSlider;

    public TextMeshProUGUI audioMusicText;
    public Slider musicSlider;

    public TextMeshProUGUI audioSoundText;
    public Slider soundSlider;

    public float soundVolume;
    public float musicVolume;
    public GameObject soundprefab;
    public string prefabname;


    public Dictionary<string, int> musicsDic = new Dictionary<string, int>();
    public Sound[] musics = new Sound[10];


    public Sound[] sounds = new Sound[10];
    public Dictionary<string, int> soundsDic = new Dictionary<string, int>();

    public Sound testSound;

    void Start()
    {
        o = this;
        prefabname = soundprefab.name;

        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < musics.Length; i++)
        {
            if ((musics[i].clip != null))
                musicsDic.Add(musics[i].clip.name, i);
        }

        for (int i = 0; i < sounds.Length; i++)
        {
            if ((sounds[i].clip != null))
                soundsDic.Add(sounds[i].clip.name, i);
        }

        PlayMusic(musics[0].clip.name);
        //SetVolume(masterVolumeNew);
        //SetDefVolume(masterVolumeMixer,musicVolumeMixer, soundVolumeMixer);

    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        foreach (Sound snd in sounds)
        {
            if (snd.adaptive)
            {
                snd.adaptCount -= snd.adaptCount * dt * 0.5f;
            }
        }
    }


    public static void PlaySound(AudioClip aud, float vol = 1, float xpos = 0, float ypos = 0)
    {
        PlaySound(aud.name, vol, xpos,ypos);
    }

    public static void PlaySound(string nam, float vol = 1, float xpos = 0, float ypos = 0)
    {
        AudioSource snd = Instantiate(o.soundprefab).GetComponent<AudioSource>();


        int i = o.soundsDic[nam];
        Sound mySound = o.sounds[i];
        snd.clip = mySound.clip;

        float adap = 1f;
        if (mySound.adaptive)
        {
            mySound.adaptCount++;
            adap = (5f / (5f + mySound.adaptCount));
            if (Random.Range(0f, mySound.adaptCount) < 1)
            {
                //adap = 1;
            }
        }

        snd.volume = vol * mySound.volume * o.masterVolume * o.soundVolume * adap * Random.Range(1.0f / (1f + mySound.volumeVariance), 1.0f * (1f + mySound.volumeVariance));
        snd.loop = mySound.loop;
        snd.pitch = mySound.pitch * Random.Range(1.0f / (1f + mySound.pitchVariance), 1.0f * (1f + mySound.pitchVariance));

        Vector2 pos = new Vector2(xpos, ypos);
        if (pos != new Vector2(0, 0))
        {
            //snd.spatialBlend = 1; //here goes 3d
            snd.transform.position = pos;
        }
        else
        {
            snd.spatialBlend = 0; //here goes 2d
        }

        snd.Play();

    }


    public static void PlayAmbient(string nam, float vol = 1, float xpos = 0, float ypos = 0)
    {

    }

    public static float GetSoundVolume()
    {
        return o.masterVolume* o.soundVolume;
    }


    public static void PlayMusic(string nam)
    {
        AudioSource snd = o.audioSource;


        int i = o.musicsDic[nam];
        Sound mySound = o.musics[i];
        snd.clip = mySound.clip;

        Debug.Log($"{mySound.volume} {o.masterVolume} {o.musicVolume}");
        snd.volume = mySound.volume * o.masterVolume * o.musicVolume;
        snd.loop = mySound.loop;
        snd.Play();



    }

    public void SetVolumeByName(float sliderValue, string name, TextMeshProUGUI text)
    {
        if (text != null)
        {
            text.text = Mathf.FloorToInt(sliderValue * 100).ToString();
        }
        masterVolumeMixer = sliderValue;

        mainMixer.SetFloat(name, Mathf.Log10(sliderValue) * 20);
    }

    public void SetVolumeMaster(float sliderValue)
    {
        SetVolumeByName(sliderValue, "Master", audioMasterText);
    }

    public void SetVolumeMusic(float sliderValue)
    {
        SetVolumeByName(sliderValue, "Music", audioMusicText);
    }

    public void SetVolumeSound(float sliderValue)
    {
        SetVolumeByName(sliderValue, "SFX", audioSoundText);
    }

    public void SetDefVolume(float master, float music, float sound)
    {
        SetVolumeMaster(master);
        masterSlider.value = master;

        SetVolumeMusic(music);
        musicSlider.value = music;

        SetVolumeSound(sound);
        soundSlider.value = sound;

    }


    private AudioSource MyPlay(string nam)
    {
        return null; //temp
    }

}



[System.Serializable]
public class Sound
{
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume = .75f;
    [Range(0f, 1f)]
    public float volumeVariance = .1f;

    [Range(.1f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float pitchVariance = .1f;

    public bool loop = false;
    public bool adaptive = false;
    [HideInInspector]
    public float adaptCount = 0;

    public AudioMixerGroup mixerGroup;

    [HideInInspector]
    public AudioSource source;
    // Default Constructor
    public Sound()
    {
        Init();
    }

    public void Init()
    {
        volume = .75f;
        volumeVariance = .1f;
        pitch = 1f;
        pitchVariance = .1f;
        loop = false;
        adaptive = false;
    }

}

