// sound system by ted bissada to allow for sounds to be loaded in and out of a pre defined number of audio channels on demand

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;
    [SerializeField] AudioClip mainTheme; // the background music of the area

    [SerializeField] AudioClip chopClip; // add whatever sounds u want here
    [SerializeField] AudioClip cookClip;
    [SerializeField] AudioClip dropClip;
    [SerializeField] AudioClip cheerClip;
    [SerializeField] AudioClip host1Clip;

    private int maxAudioSourceCount = 6; //max number of channels that can be played at any time
    private List<AudioSource> sources = null;
    private AudioSource mainThemeChannel; // seperate channel for main theme
    private AudioSource hostAudioChannel; //seperate channel for host speech for audio analysis

    float[] samples = new float[64]; // for host audio analysis

    private void Awake() // only one sound manager allowed active
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        mainThemeChannel = this.gameObject.AddComponent<AudioSource>(); // starts main theme on begin
        mainThemeChannel.volume = 0.5f;
        mainThemeChannel.clip = mainTheme;
        mainThemeChannel.loop = true;
        hostAudioChannel = this.gameObject.AddComponent<AudioSource>();
    }

    public void playChopSound()
    {
        AudioSource source = GetAvailableSource();
        source.clip = chopClip;
        source.Play();
    }

    public void playCookSound()
    {
        AudioSource source = GetAvailableSource();
        source.clip = cookClip;
        source.Play();
    }
    public void playDropSound()
    {
        AudioSource source = GetAvailableSource();
        source.clip = dropClip;
        source.Play();
    }
    public void playCheerSound()
    {
        AudioSource source = GetAvailableSource();
        source.clip = cheerClip;
        source.Play();
    }

    public void playHost1Sound()
    {
        AudioSource source = hostAudioChannel;
        source.clip = host1Clip;
        source.Play();
    }

    public void playMainTheme()
    {
        if (mainThemeChannel.isPlaying)
            return;
        mainThemeChannel.Play();
    }

    public void stopMainTheme()
    {
        if (!mainThemeChannel.isPlaying)
            return;
        mainThemeChannel.Stop();
    }


    public float[] getSamples()
    {
        return samples;
    }

    void Update()
    {
        hostAudioChannel.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
    }

    private AudioSource GetAvailableSource() // generates the specified number of audio channels and gives the oldest audio channel for new sound playback 
    {
        if (this.sources == null)
        {
            this.sources = new List<AudioSource>();
            for (int i = 0; i < this.maxAudioSourceCount; i++)
            {
                AudioSource newSource = this.gameObject.AddComponent<AudioSource>();
                this.sources.Add(newSource);
            }
            return this.sources[0];
        }
        for (int i = 0; i < this.maxAudioSourceCount; i++)
        {
            AudioSource source = this.sources[i];
            if (source.isPlaying == false)
            {
                return source;
            }
        }
        AudioSource oldest = this.sources[0];
        for (int i = 0; i < this.maxAudioSourceCount; i++)
        {
            AudioSource source = this.sources[i];
            if (source.time >= oldest.time)
                oldest = source;
        }
        return oldest;
    }
}
