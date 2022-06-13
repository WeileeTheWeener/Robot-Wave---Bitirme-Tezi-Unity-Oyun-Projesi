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
		//EGER SOL SHIFT TUSU BASILI DEGIL ISE YURUYUS HIZINI WALK SPEEDE ESITLE VE SESI OYNAT
		if(!Input.GetKey(KeyCode.LeftShift))
        {
			currentSpeed = walkSpeed;

			if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && Time.time >= nextTimeToPlay)
			{
				nextTimeToPlay = Time.time + 1f / currentSpeed;
				PlayFootsteps();
			}

		}
		//EGER SOL SHIFT TUSU BASILI  ISE YURUYUS HIZINI RUN SPEEDE ESITLE VE SESI OYNAT
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
	//SÝLAH DEÞARJ SESÝ OYNAT
	public void PlayGunDecharge()
	{

		dechargeAudioSource.PlayOneShot(gunDeChargeClip);
		dechargeAudioSource.outputAudioMixerGroup = gunDeChargeMixer;
	}
	//AYAK SESÝ OYNAT
	void PlayFootsteps()
	{
		//LÝSTEDEN SESLERI RASTGELE OYNAT
		int footstepIndex = Random.Range(0, footstepList.Count);
		footstepsAudioSource.PlayOneShot(footstepList[footstepIndex]);
		//SES CIKISINI MIXERE ATA
		footstepsAudioSource.outputAudioMixerGroup = footstepMixer;
	}
	//SILAH ATES SESI OYNAT
	public void PlayGunShot()
	{
		int gunShotIndex = Random.Range(0, gunshotList.Count);
		audio.PlayOneShot(gunshotList[gunShotIndex]);
		audio.outputAudioMixerGroup = gunshotMixer;
	}
	//SARJOR DEGISTIRME SESI OYNAT
	public void PlayReload()
	{
		audio.PlayOneShot(reloadClip);
		audio.outputAudioMixerGroup = reloadMixer;

	}
	//ITEM ALINCA CIKACAK SESI OYNAT
	public void PlayPickup()
	{
		audio.PlayOneShot(pickupClip);
		audio.outputAudioMixerGroup = pickupMixer;

	}
	//ARAYUZ SESI OYNAT
	public void PlayUISound(float volume)
	{
		audio.PlayOneShot(uiClip);
		audio.volume = volume;

	}


}
