using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Weapons weapon;

    WeaponLoader weaponLoader;

    AudioSource audioSource;
    public AudioClip[] swings;
    public AudioClip[] powerUp;

    GameObject model;
    Collider modelHB;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        weaponLoader = GetComponentInChildren<WeaponLoader>();
        model = Instantiate(weapon.modelPrefab) as GameObject;
        modelHB = model.GetComponentInChildren<BoxCollider>();
    }

    void Start()
    {
        weaponLoader.LoadWeaponModel(model, true);
    }

    public void LoadWeapon(bool set)
    {
        weaponLoader.LoadWeaponModel(model, set);
    }

    public bool isWeaponLoaded()
    {
        if (model.activeInHierarchy)
        {
            return true;
        }

        return false;
    }

    public void EnableWeaponCollider()
    {
        modelHB.enabled = true;
    }

    public void DisableWeaponCollider()
    {
        modelHB.enabled = false;
    }

    public void PlayRandomSwoosh()
    {
        int i = Random.Range(0, swings.Length);

        audioSource.pitch = Random.Range(0.2f, 0.3f);

        audioSource.PlayOneShot(swings[i]);
    }

    public void PlayPowerUp()
    {
        audioSource.pitch = 0.8f;
        audioSource.PlayOneShot(powerUp[0]);
    }

    public void PlayPowerUp2()
    {
        audioSource.pitch = 0.8f;
        audioSource.PlayOneShot(powerUp[1]);
    }
}
