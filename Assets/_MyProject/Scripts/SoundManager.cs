using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
	public List<AudioClip> footstepList;
	public List<AudioClip> gunshotList;
	public AudioSource audio;
	public AudioSource footstepsAudioSource;
	public AudioSource dechargeAudioSource;
	public AudioClip reloadClip;
	public AudioClip footstepClip;
	public AudioClip gunshotClip;
	public AudioClip pickupClip;
	public AudioClip gunDeChargeClip;
	public AudioClip uiClip;
	public AudioMixerGroup gunDeChargeMixer;
	public AudioMixerGroup gunshotMixer;
	public AudioMixerGroup footstepMixer;
	public AudioMixerGroup reloadMixer;
	public AudioMixerGroup pickupMixer;
	

	public float footstepVolume;
	public float gunshotVolume;
	public float gunDeChargeVolume;
	public float reloadVolume;
	public float currentSpeed;
	public float walkSpeed;
	public float runSpeed;
	private float nextTimeToPlay;

	// Use this for initialization
	void Start()
	{
		audio = this.GetComponent<AudioSource>();
	}
    private void Update()
    {
		if(!Input.GetKey(KeyCode.LeftShift))
        {
			currentSpeed = walkSpeed;

			if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && Time.time >= nextTimeToPlay)
			{
				nextTimeToPlay = Time.time + 1f / currentSpeed;
				PlayFootsteps();
			}

		}
		else
        {
			currentSpeed = runSpeed;

			if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 ) && Time.time >= nextTimeToPlay)
			{
				nextTimeToPlay = Time.time + 1f / currentSpeed;
				PlayFootsteps();
			}
		}

	}
	public void PlayGunDecharge()
	{

		dechargeAudioSource.PlayOneShot(gunDeChargeClip);
		//audio.volume = volume;
		dechargeAudioSource.outputAudioMixerGroup = gunDeChargeMixer;
	}

	void PlayFootsteps()
	{
		int footstepIndex = Random.Range(0, footstepList.Count);
		footstepsAudioSource.PlayOneShot(footstepList[footstepIndex]);
		//audio.volume = volume;
		footstepsAudioSource.outputAudioMixerGroup = footstepMixer;
	}
	public void PlayGunShot()
	{
		int gunShotIndex = Random.Range(0, gunshotList.Count);
		audio.PlayOneShot(gunshotList[gunShotIndex]);
		//audio.volume = volume;
		audio.outputAudioMixerGroup = gunshotMixer;
	}
	public void PlayReload()
	{
		audio.PlayOneShot(reloadClip);
		//audio.volume = volume;
		audio.outputAudioMixerGroup = reloadMixer;

	}
	public void PlayPickup()
	{
		audio.PlayOneShot(pickupClip);
		//audio.volume = volume;
		audio.outputAudioMixerGroup = pickupMixer;

	}
	public void PlayUISound(float volume)
	{
		audio.PlayOneShot(uiClip);
		//audio.volume = volume;
		audio.volume = volume;

	}


}
