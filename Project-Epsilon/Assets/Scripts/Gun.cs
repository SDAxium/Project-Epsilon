using System.Collections;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Unused Variables
    private int _magazineCount;
    private int _reloadTime; 
    
    //End of Unused Variables
    public GameObject bulletPrefab;
    public GameObject bulletController;// Reference to the bullet controller object
    private BulletController _bc;
    public Transform bulletSpawnPoint;
    
    public AudioSource audioSource;
    public AudioClip audioClip;
   
    void Start()
    {
        bulletController = GameObject.Find("Bullet Controller");
        _bc = bulletController.GetComponent<BulletController>();
        //InvokeRepeating(nameof(Fire),3,0.5f);
    }

    /*
     * Fires a Bullet
     * If there are any inactive bullets, an inactive bullet is taken and removed from the inactive list
     * If there are no inactive bullets, a new bullet is instantiated
     */
    public void Fire()
    {
        GameObject bullet = _bc.GetBullet();
        
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        
        AudioSource.PlayClipAtPoint(audioClip,bulletSpawnPoint.position);
    }
   
}
