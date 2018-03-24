using System;
using UnityEngine;

[Serializable]
public class AudioSurface
{
    public string tag;              // The tag on the surfaces that play these sounds.
    public AudioClip[] clips;       // The different clips that can be played on this surface.
    public AudioSource source;      // The AudioSource that will play the clips.

    //private FisherYatesRandom randomSource = new FisherYatesRandom();       // For randomly reordering clips.

    public AudioSurface(string tag)
    {
        this.tag = tag;
    }


    public void SetSource(Animator animator)
    {
        // The audio source is on a specifically named child.
        source = animator.transform.Find("ScientistAudio/" + tag + "Audio").GetComponent<AudioSource>();
    }


    public void PlayRandomClip()
    {
        // If there are no clips to play return.
        if (clips == null || clips.Length == 0)
            return;

        // Find a random clip and play it.
       // int index = randomSource.Next(clips.Length);
        source.PlayOneShot(clips[0]);
    }
}