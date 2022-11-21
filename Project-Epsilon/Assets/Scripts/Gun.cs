using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public InputActionReference toggleToTeleport = null;
    
    public AudioClip chamberEmpty;
    public AudioClip gunReload;
    
    protected int _currentBullets;
    public int maxBullets = 30;
    protected int _reloadTime;

    private TextMeshProUGUI _bulletCountText; 
    
    public GameObject bulletController;// Reference to the bullet controller object
    protected BulletController _bc;
    public Transform bulletSpawnPoint;
    
    public AudioSource audioSource;
    public AudioClip gunshotClip;

    public bool teleportOn;

    protected virtual void Awake()
    {
        toggleToTeleport.action.started += ToggleTeleport;
    }

    public GameObject GetCurrentPlayer()
    {
        return gameObject.GetComponent<XRGrabInteractable>().GetOldestInteractorSelecting().transform.root.gameObject;;
    }

    public virtual void Start()
    {
        _currentBullets = maxBullets;
        _bulletCountText = gameObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        _bulletCountText.text = _currentBullets.ToString();
        print(_currentBullets);
        bulletController = GameObject.Find("Bullet Controller");
        _bc = bulletController.GetComponent<BulletController>();
        
        Reload();
    }

    void ToggleTeleport(InputAction.CallbackContext context)
    {
        print($"Teleport is: {teleportOn}");
        teleportOn = !teleportOn;
    }
    
    /// <summary>
    /// Fires a Bullet.
    /// If there are any inactive bullets, an inactive bullet is taken and removed from the inactive list.
    /// If there are no inactive bullets, a new bullet is instantiated.
    /// </summary>
    public void Fire()
    {
        if (_currentBullets == 0)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(chamberEmpty);
            return;
        }
        GameObject bullet = _bc.GetBullet(this);

        //bullet.transform.SetParent(gameObject.transform);
        bullet.GetComponent<Bullet>().barrel = gameObject.transform.Find("Bullet Spawn Point");

        var position = bulletSpawnPoint.position;
        bullet.transform.position = position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;

        
        AudioSource.PlayClipAtPoint(gunshotClip,position);

        _currentBullets--;
        _bulletCountText.text = _currentBullets.ToString();

        if (teleportOn) teleportOn = false;
    }

    public void Reload()
    {
        _currentBullets = maxBullets;
        _bulletCountText.text = _currentBullets.ToString();
        
        AudioSource.PlayClipAtPoint(gunReload,transform.position);
    }
    
    /// <summary>
    /// This originally set the teleport area of the game to the player holding the gun.
    ///
    /// Since teleportation has been given to all players, this is no longer in use. It doesn't help that it no longer works
    /// </summary>
    public void OnSelected()
    {
        print("HERE");
        GetComponent<XRGrabInteractable>().GetOldestInteractorSelecting(). //grab interactor
            transform.root.GetComponent <Multiplayer.NetworkPlayer>().SetAreaProvider();
    }
}
