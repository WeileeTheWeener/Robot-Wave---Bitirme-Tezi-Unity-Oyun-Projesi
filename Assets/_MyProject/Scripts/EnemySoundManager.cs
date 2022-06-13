using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    public List<AudioClip> footstepList;
    public List<AudioClip> gunshotList;
    public AudioSource audio;
    public AudioClip reloadClip;
    public AudioClip footstepClip;
    public AudioClip gunshotClip;
    public float footstepVolume;
    public float gunshotVolume;
    public float reloadVolume;
    public float FootstepSpeed;
    private float nextTimeToPlay;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    // Use this for initialization
    void Start()
    {
        //NAVMESH AGENT VE SES OYNATMAK ICIN GEREKLI AUDIO SOURCE COMPONENTI AL
        navMeshAgent = gameObject.transform.root.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        audio = this.GetComponent<AudioSource>();
    }
    private void Update()
    {
        //EGER HEDEFE UZAKLIK VAR ISE VE BIR SONRAKI CALMA ZAMANI GELMIS ISE AYAK SESÝ OYNAT
        if (navMeshAgent.remainingDistance > 0.1f && Time.time >= nextTimeToPlay)
        {
            nextTimeToPlay = Time.time + FootstepSpeed;
            PlayFootsteps(footstepVolume);
        }        
    }

    //AYAK SESÝ OYNAT
    void PlayFootsteps(float volume)
    {
        //LÝSTEDEN SESLERI RASTGELE OYNAT
        int footstepIndex = Random.Range(0, footstepList.Count);
        audio.PlayOneShot(footstepList[footstepIndex]);
        audio.volume = volume;
    }
    //SÝLAH SESÝ OYNAT
    public  void PlayGunShot(float volume)
    {
        //LÝSTEDEN SESLERI RASTGELE OYNAT
        int gunShotIndex = Random.Range(0, gunshotList.Count);
        audio.PlayOneShot(gunshotList[gunShotIndex]);
        audio.volume = volume;
    }
    //SARJOR DEGISTIRME SESI OYNAT
    public void PlayReload(float volume)
    {
        audio.PlayOneShot(reloadClip);
        audio.volume = volume;
    }

}
