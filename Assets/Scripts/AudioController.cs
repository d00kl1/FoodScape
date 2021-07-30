using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioController : MonoBehaviour
{
    [Tooltip("The time used in the audio transitions.")]
    public float transitionTime = .05f;    
    [Tooltip("The audio source used for the random note.")]
    public AudioSource noteClipSource;
    [Space]

    [Tooltip("The default audio mixer.")]
    public AudioMixer mainMixer;
    [Tooltip("The audio mixer snapshot used for the default mode.")]
    public AudioMixerSnapshot defaultAudioMix;
    [Tooltip("The audio mixer snapshot used for the low pass mode.")]
    public AudioMixerSnapshot lowPassSnapshot;

    [Tooltip("The notes used when an element is spawned.")]
    public AudioClip[] forwardNoteClips;
    [Tooltip("The reverse notes used when an element is destroyed.")]
    public AudioClip[] reverseNoteClips;

    AudioSource[] audioSourceArray;
	public AudioClip[] audioClipArray;
	
	int nextClip = 0;
	double nextStartTime = AudioSettings.dspTime + 0.5;
	int toggle = 0;
    bool firstTime = true;

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }

        firstTime = false;
        yield break;
    }    

	void Awake() {
		audioSourceArray = new AudioSource[2];
		audioSourceArray[0] = gameObject.AddComponent<AudioSource>();
		audioSourceArray[1] = gameObject.AddComponent<AudioSource>();		
	}	

	void Update () {
	    if (AudioSettings.dspTime > nextStartTime - 1) {
		    AudioClip clipToPlay = audioClipArray[nextClip];

		    // Loads the next Clip to play and schedules when it will start
		    audioSourceArray[toggle].clip = clipToPlay;

            if (firstTime) {                
                StartCoroutine(StartFade(audioSourceArray[toggle], 10, 1.0f));              
            }
            
		    audioSourceArray[toggle].PlayScheduled(nextStartTime);

		    // Checks how long the Clip will last and updates the Next Start Time with a new value
		    double duration = (double)clipToPlay.samples / clipToPlay.frequency;
		    nextStartTime = nextStartTime + duration;

		    // Switches the toggle to use the other Audio Source next
		    toggle = 1 - toggle;

		    // Increase the clip index number, reset if it runs out of clips
		    nextClip = nextClip < audioClipArray.Length - 1 ? nextClip + 1 : 0;
	    }
	}


    //Reset our mix when the game starts
    private void Start()
    {
        FullSpeedAudio();
        AudioForward();
    }

    /// <summary>
    /// Play random note from "forwardNoteClips" and the click sound.
    /// </summary>
    public void PlayRandomClip(AudioClip[] audioClips)
    {
        noteClipSource.clip = audioClips[Random.Range(0, audioClips.Length)];
        noteClipSource.Play();
    }

    /// <summary>
    /// Makes a transition to the default audio snapshot.
    /// </summary>
    public void RemoveLowpassEffect()
    {
        defaultAudioMix.TransitionTo(transitionTime);    
    }

    /// <summary>
    /// Makes a transition to the low pass audio snapshot.
    /// </summary>
    public void LowpassAudio()
    {
        lowPassSnapshot.TransitionTo(transitionTime);
    }
    
    /// <summary>
    /// Rescale the sounds pitch to 1.
    /// </summary>
    public void FullSpeedAudio()
    {     
        noteClipSource.pitch = 1f;
    }

    /// <summary>
    /// Rescale the sounds pitch to .5.
    /// </summary>
    public void HalfPitchAudio()
    {
        noteClipSource.pitch = .5f;
    }
    /// <summary>
    ///Turn up the mixer group for the loop of the music playing forwards and turn down backwards audio
    /// </summary>
    public void AudioForward()
    {
        //SetFloat is independent of audio mixer snapshots, so this can change even when snapshots are changing
        mainMixer.SetFloat("ForwardMusicVolume", 0f);
        mainMixer.SetFloat("ReversedMusicVolume", -80f);
    }

    /// <summary>
    /// Turn up the backwards audio and turn down the forwards loop group in the mixer
    /// </summary>
    public void AudioReverse()
    {
        //SetFloat is independent of audio mixer snapshots, so this can change even when snapshots are changing
        mainMixer.SetFloat("ForwardMusicVolume", -80f);
        mainMixer.SetFloat("ReversedMusicVolume", 0f);
    }
}
