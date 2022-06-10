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

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;
        currentMagazines = maxMagazines;
    }

    // Update is called once per frame
    void Update()
    {
        ClampAmmo();
        SetText();
    }
    public void ReloadMagazine()
    {
        if (currentMagazines > 0)
        {
            currentAmmo = maxAmmo;
            currentMagazines -= 1;
            playerSoundManager.PlayReload();

        }

    }
    public void setCurrentAmmo(int ammo)
    {
        currentAmmo = ammo;

    }
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
    public void SetText()
    {
        ammoText.text = currentMagazines + "/" + currentAmmo;
    }
}
