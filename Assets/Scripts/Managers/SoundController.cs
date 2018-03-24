/////////
//To the extent possible under law, 
//Tim Glasser 
//tim_glasser@hotmail.com
//has waived all copyright and related or neighboring rights and responsibilties to
//Unity PhysX3.3 SoundController C# Classes. - ported from Unity Assets
//This work is published from:
//Oakland, California.

// As indicated by the Creative Commons, the text on this page may be copied, 
// modified and adapted for your use, without any other permission from the author.

// Please do not remove this notice

using System;
using UnityEngine;
using System.Collections.Generic;
using UnityStandardAssets.Vehicles.Car;
//
// This script controls the sound for a vehicle. It automatically creates the needed AudioSources, and ensures
// that only a certain number of sound are played at any time, by limiting the number of OneShot
// audio clip that can be played at any time. This is to ensure that it does not play more sounds than Unity
// can handle.
// The script handles the sound for the engine, both idle and running, gearshifts, skidding and crashing.
// PlayOneShot is used for the non-looping sounds are needed. A separate AudioSource is create for the OneShot
// AudioClips, since the should not be affected by the pitch-changes applied to other AudioSources.


[RequireComponent(typeof (Rigidbody))]
public class SoundController : MonoBehaviour{

public CarController vehicle;

public AudioClip D  = null;
public float DVolume = 1.0f;
public AudioClip E = null;
public float EVolume = 1.0f;
public AudioClip F = null;
public float FVolume = 1.0f;
public AudioClip K = null;
public float KVolume = 1.0f;
public AudioClip L = null;
public float LVolume = 1.0f;

public AudioClip wind = null;
public float windVolume = 1.0f;
public AudioClip tunnelSound = null;
public float tunnelVolume = 1.0f;

public AudioClip crashLowSpeedSound = null;
public float crashLowVolume = 1.0f;

public AudioClip crashHighSpeedSound = null;
public float crashHighVolume = 1.0f;

public AudioClip skidSound = null;

public AudioClip BackgroundMusic = null;
public float BackgroundMusicVolume = 0.6f;

AudioSource DAudio = null;
AudioSource EAudio  = null;
AudioSource FAudio = null;
AudioSource KAudio  = null;
AudioSource LAudio = null;

AudioSource tunnelAudio  = null;
AudioSource windAudio = null;
AudioSource skidAudio = null;
AudioSource carAudio = null;

AudioSource backgroundMusic  = null;


float gearShiftTime = 0.1f;
bool shiftingGear = false;
int gearShiftsStarted  = 0;
int crashesStarted  = 0;
float crashTime  = 0.2f;
int oneShotLimit = 8;

float idleFadeStartSpeed = 3.0f;
float idleFadeStopSpeed  = 10.0f;
float idleFadeSpeedDiff  = 7.0f;
float speedFadeStartSpeed  = 0.0f;
float speedFadeStopSpeed  = 8.0f;
float speedFadeSpeedDiff  = 8.0f;

bool soundsSet  = false;

float inputFactor  = 0.0f;
int gear  = 0;
int topGear  = 6;

float idlePitch  = 0.7f;
float startPitch  = 0.85f;
float lowPitch = 1.17f;
float medPitch = 1.25f;
float highPitchFirst  = 1.65f;
float highPitchSecond  = 1.76f;
float highPitchThird  = 1.80f;
float highPitchFourth  = 1.86f;
float shiftPitch  = 1.44f;

float prevPitchFactor = 0.0f;

// Create the needed AudioSources
void Awake()
{
	vehicle = GetComponent<CarController>();
	
	DVolume *= 0.4f;
	EVolume *= 0.4f;
	FVolume *= 0.4f;
	KVolume *= 0.7f;
	LVolume *= 0.4f;
	windVolume *= 0.4f;
	
	DAudio = gameObject.AddComponent<AudioSource>();
	DAudio.loop = true;
	DAudio.clip = D;
	DAudio.volume = DVolume;
	DAudio.Play();

	EAudio = gameObject.AddComponent<AudioSource>();
	EAudio.loop = true;
	EAudio.clip = E;
	EAudio.volume = EVolume;
	EAudio.Play();
	
	FAudio = gameObject.AddComponent<AudioSource>();
	FAudio.loop = true;
	FAudio.clip = F;
	FAudio.volume = FVolume;
	FAudio.Play();
	
	KAudio = gameObject.AddComponent<AudioSource>();
	KAudio.loop = true;
	KAudio.clip = K;
	KAudio.volume = KVolume;
	KAudio.Play();
	
	LAudio = gameObject.AddComponent<AudioSource>();
	LAudio.loop = true;
	LAudio.clip = L;
	LAudio.volume = LVolume;
	LAudio.Play();
	
	windAudio = gameObject.AddComponent<AudioSource>();
	windAudio.loop = true;
	windAudio.clip = wind;
	windAudio.volume = windVolume;
	windAudio.Play();
	
	tunnelAudio = gameObject.AddComponent<AudioSource>();
	tunnelAudio.loop = true;
	tunnelAudio.clip = tunnelSound;
//	tunnelAudio.maxVolume = tunnelVolume;
	tunnelAudio.volume = tunnelVolume;
	
	skidAudio = gameObject.AddComponent<AudioSource>();
	skidAudio.loop = true;
	skidAudio.clip = skidSound;
	skidAudio.volume = 0.0f;
	skidAudio.Play();
	
	carAudio = gameObject.AddComponent<AudioSource>();
	carAudio.loop = false;
	carAudio.playOnAwake = false;
	carAudio.Stop();
	
	crashTime = Mathf.Max(crashLowSpeedSound.length, crashHighSpeedSound.length);
	soundsSet = false;
	
	idleFadeSpeedDiff = idleFadeStopSpeed - idleFadeStartSpeed;
	speedFadeSpeedDiff = speedFadeStopSpeed - speedFadeStartSpeed;
	
	backgroundMusic = gameObject.AddComponent<AudioSource>();
	backgroundMusic.loop = true;
	backgroundMusic.clip = BackgroundMusic;
//	backgroundMusic.maxVolume = BackgroundMusicVolume;
//	backgroundMusic.minVolume = BackgroundMusicVolume;
	backgroundMusic.volume = BackgroundMusicVolume;
	backgroundMusic.Play();
}

void Update()
{
	float carSpeed = GetComponent<Rigidbody>().velocity.magnitude;
	float carSpeedFactor  = Mathf.Clamp01(carSpeed / vehicle.MaxSpeed);
	
	KAudio.volume = Mathf.Lerp(0, KVolume, carSpeedFactor);
	windAudio.volume = Mathf.Lerp(0, windVolume, carSpeedFactor * 2);
	
	if(shiftingGear)
		return;
	
	float pitchFactor  = Sinerp(0, topGear, carSpeedFactor);
	int newGear = (int)pitchFactor;
	
	pitchFactor -= newGear;
	float throttleFactor = pitchFactor;
	pitchFactor *= 0.3f;
    pitchFactor += throttleFactor * 0.7f * Mathf.Clamp01(vehicle.AccelInput * 2.0f);
	
	if(newGear != gear)
	{
		if(newGear > gear)
			GearShift(prevPitchFactor, pitchFactor, gear, true);
		else
			GearShift(prevPitchFactor, pitchFactor, gear, false);
		gear = newGear;
	}
	else
	{
		float newPitch  = 0.0f;
		if(gear == 0)
			newPitch = Mathf.Lerp(idlePitch, highPitchFirst, pitchFactor);
		else
		if(gear == 1)
			newPitch = Mathf.Lerp(startPitch, highPitchSecond, pitchFactor);
		else
		if(gear == 2)
			newPitch = Mathf.Lerp(lowPitch, highPitchThird, pitchFactor);
		else
			newPitch = Mathf.Lerp(medPitch, highPitchFourth, pitchFactor);
		SetPitch(newPitch);
		SetVolume(newPitch);
	}
	prevPitchFactor = pitchFactor;
}

float Coserp(float start, float end,  float value) 
{
	return Mathf.Lerp(start, end, 1.0f - Mathf.Cos(value * Mathf.PI * 0.5f));
}

float Sinerp(float start ,float end , float value ) 
{
    return Mathf.Lerp(start, end, Mathf.Sin(value * Mathf.PI * 0.5f));
}

void SetPitch(float pitch )
{
    DAudio.pitch = pitch;
	EAudio.pitch = pitch;
	FAudio.pitch = pitch;
	LAudio.pitch = pitch;
	tunnelAudio.pitch = pitch;
}

void SetVolume(float pitch)
{
	float pitchFactor = Mathf.Lerp(0.0f, 1.0f, (pitch - startPitch) / (highPitchSecond - startPitch));
	DAudio.volume = Mathf.Lerp(0, DVolume, pitchFactor);
	float fVolume  = Mathf.Lerp(FVolume * 0.8f, FVolume, pitchFactor);
    FAudio.volume = fVolume * 0.7f + fVolume * 0.3f * Mathf.Clamp01(vehicle.Revs);
	float eVolume = Mathf.Lerp(EVolume * 0.89f, EVolume, pitchFactor);
    EAudio.volume = eVolume * 0.8f + eVolume * 0.2f * Mathf.Clamp01(vehicle.Revs);
}

void GearShift(float oldPitchFactor, float newPitchFactor, int gear, bool shiftUp )
{
	shiftingGear = true;
	
	float timer = 0.0f;
	float pitchFactor 	= 0.0f;
	float newPitch  = 0.0f;
	
	if(shiftUp)
	{
		while(timer < gearShiftTime)
		{
			pitchFactor = Mathf.Lerp(oldPitchFactor, 0.0f, timer / gearShiftTime);
			if(gear == 0)
				newPitch = Mathf.Lerp(lowPitch, highPitchFirst, pitchFactor);
			else
				newPitch = Mathf.Lerp(lowPitch, highPitchSecond, pitchFactor);
			SetPitch(newPitch);
			SetVolume(newPitch);
			timer += Time.deltaTime;
			//yield;
		}
	}
	else
	{
		while(timer < gearShiftTime)
		{
			pitchFactor = Mathf.Lerp(0.0f, 1.0f, timer / gearShiftTime);
			newPitch = Mathf.Lerp(lowPitch, shiftPitch, pitchFactor);
			SetPitch(newPitch);
			SetVolume(newPitch);
			timer += Time.deltaTime;
		//	yield;
		}
	}
		
	shiftingGear = false;
}

void Skid(bool play, float volumeFactor )
{
	if(!skidAudio)
		return;
	if(play)
	{
		skidAudio.volume = Mathf.Clamp01(volumeFactor + 0.3f);
	}
	else
		skidAudio.volume = 0.0f;
}

// Checks if the max amount of crash sounds has been started, and
// if the max amount of total one shot sounds has been started.
public void Crash(float volumeFactor)
{
		Debug.Log ("Crash sound " + volumeFactor);
	if((crashesStarted > 3) ||( OneShotLimitReached()))
		return;


	if(volumeFactor > 0.9f)
		carAudio.PlayOneShot(crashHighSpeedSound, Mathf.Clamp01((0.5f + volumeFactor * 0.5f) * crashHighVolume));
	carAudio.PlayOneShot(crashLowSpeedSound, Mathf.Clamp01(volumeFactor * crashLowVolume));
	crashesStarted++;
	
    new WaitForSeconds(crashTime);
	//yield new WaitForSeconds(crashTime);
	
	crashesStarted--;
}

// A function for testing if the maximum amount of OneShot AudioClips
// has been started.
bool OneShotLimitReached()
{
	return (crashesStarted + gearShiftsStarted) > oneShotLimit;
}

void OnTriggerEnter(Collider coll )
{
//	SoundToggler st  = coll.transform.GetComponent<SoundToggler>();
//	if(st)
	//	ControlSound(true, st.fadeTime);
}

void OnTriggerExit(Collider coll )
{
//	SoundToggler st  = coll.transform.GetComponent<SoundToggler>();
//	if(st)
	//	ControlSound(false, st.fadeTime);
}

void ControlSound(bool play, float fadeTime )
{
	float timer  = 0.0f;
	if(play && !tunnelAudio.isPlaying)
	{
		tunnelAudio.volume = 0.0f;
		tunnelAudio.Play();
		while(timer < fadeTime)
		{
			tunnelAudio.volume = Mathf.Lerp(0.0f, tunnelVolume, timer / fadeTime);
			timer += Time.deltaTime;
			//yield;
		}
	}
	else if(!play && tunnelAudio.isPlaying)
	{
		while(timer < fadeTime)
		{
			tunnelAudio.volume = Mathf.Lerp(0.0f, tunnelVolume, timer / fadeTime);
			timer += Time.deltaTime;
			//yield;
		}
		tunnelAudio.Stop();
	}
}
}