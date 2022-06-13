using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAmmo : MonoBehaviour
{
    public Text ammoText;
    public int currentAmmo;
    public int maxAmmo;
    public int currentMagazines;
    public int maxMagazines;
    public SoundManager playerSoundManager;

    void Start()
    {
        currentAmmo = maxAmmo;
        currentMagazines = maxMagazines;
    }

    void Update()
    {
        ClampAmmo();
        SetText();
    }
    //SARJOR DEGISTIR
    public void ReloadMagazine()
    {
        if (currentMagazines > 0)
        {
            currentAmmo = maxAmmo;
            currentMagazines -= 1;
            playerSoundManager.PlayReload();

        }

    }
    //ANLIK MERMIYI AYARLA
    public void setCurrentAmmo(int ammo)
    {
        currentAmmo = ammo;

    }
    //MERMI VE SARJOR SAYI LIMITLERINI BELIRLE
    public void ClampAmmo()
    {
        if(currentAmmo > maxAmmo)
        {
            currentMagazines++;
            currentAmmo = currentAmmo % maxAmmo;
        }

        if(currentMagazines > maxMagazines)
        {
            currentMagazines = maxMagazines;
        }
    }
    //ARAYUZDEKI MERMI SAYISINI AYARLA
    public void SetText()
    {
        ammoText.text = currentMagazines + "/" + currentAmmo;
    }
}
