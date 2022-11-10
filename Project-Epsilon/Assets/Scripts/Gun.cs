using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    public AudioClip chamberEmpty;
    public AudioClip gunReload;
    
    private int _currentBullets;
    public int maxBullets;
    private int _reloadTime;

    private TextMeshProUGUI _bulletCountText; 
    
    public GameObject bulletController;// Reference to the bullet controller object
    private BulletController _bc;
    public Transform bulletSpawnPoint;
    
    public AudioSource audioSource;
    public AudioClip gunshotClip;
    void Start()
    {
        _currentBullets = maxBullets;
        _bulletCountText = gameObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        _bulletCountText.text = maxBullets.ToString();
        
        bulletController = GameObject.Find("Bullet Controller");
        _bc = bulletController.GetComponent<BulletController>();

    }

    /*
     * Fires a Bullet
     * If there are any inactive bullets, an inactive bullet is taken and removed from the inactive list
     * If there are no inactive bullets, a new bullet is instantiated
     */
    public void Fire()
    {
        if (_currentBullets == 0)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(chamberEmpty);
            return;
        }
        GameObject bullet = _bc.GetBullet();
        
        //bullet.transform.SetParent(gameObject.transform);
        bullet.GetComponent<Bullet>().barrel = gameObject.transform.Find("Bullet Spawn Point");

        var position = bulletSpawnPoint.position;
        bullet.transform.position = position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        
        AudioSource.PlayClipAtPoint(gunshotClip,position);

        _currentBullets--;
        _bulletCountText.text = _currentBullets.ToString();
    }

    public void Reload()
    {
        _currentBullets = maxBullets;
        _bulletCountText.text = _currentBullets.ToString();
        
        AudioSource.PlayClipAtPoint(gunReload,transform.position);
    }
}
