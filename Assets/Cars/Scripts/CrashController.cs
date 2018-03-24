

using System;
using UnityEngine;
using System.Collections.Generic;
//
// This script controls the sound for a vehicle. It automatically creates the needed AudioSources, and ensures
// that only a certain number of sound are played at any time, by limiting the number of OneShot
// audio clip that can be played at any time. This is to ensure that it does not play more sounds than Unity
// can handle.
// The script handles the sound for the engine, both idle and running, gearshifts, skidding and crashing.
// PlayOneShot is used for the non-looping sounds are needed. A separate AudioSource is create for the OneShot
// AudioClips, since the should not be affected by the pitch-changes applied to other AudioSources.
namespace UnityStandardAssets.Vehicles.Car
{
    public class CrashController : MonoBehaviour
    {

        private CarAudio sound;

        void Awake()
        {
            sound = GetComponent<CarAudio>();
        }

        void OnCollisionEnter(Collision collInfo)
        {
            if (enabled && collInfo.contacts.Length > 0)
            {
                float volumeFactor = Mathf.Clamp01(collInfo.relativeVelocity.magnitude * 0.08f);
                volumeFactor *= Mathf.Clamp01(0.3f + Mathf.Abs(Vector3.Dot(collInfo.relativeVelocity.normalized, collInfo.contacts[0].normal)));
                volumeFactor = volumeFactor * 0.5f + 0.5f;
                sound.Crash(volumeFactor);
            }
        }
    }
}