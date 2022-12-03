using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using NetworkPlayer = Multiplayer.NetworkPlayer;

public class Gun : MonoBehaviour
{
    public List<InputActionReference> toggleToTeleport;
    
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
    public int currentPlayerReference;

    public ParticleSystem ps;
    protected virtual void Awake()
    {
        foreach (InputActionReference reference in toggleToTeleport)
        {
            reference.action.started += ToggleTeleport;
        }
    }

    public GameObject GetCurrentPlayer()
    {
        return gameObject.GetComponent<XRGrabInteractable>().GetOldestInteractorSelecting().transform.root.gameObject;;
    }

    public void Start()
    {
        _currentBullets = maxBullets;
        _bulletCountText = gameObject.transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        _bulletCountText.text = _currentBullets.ToString();
        bulletController = GameObject.Find("Bullet Controller");
        _bc = bulletController.GetComponent<BulletController>();
        
        GetComponent<XRGrabInteractable>().selectEntered.AddListener(UpdateOwner);
        Reload();
    }
    
    /// <summary>
    /// check the interacting player's reference against the index currently on the gun. If the two don't match, update
    /// the gun's stored index and reload the gun 
    /// </summary>
    /// <param name="args"></param>
    void UpdateOwner(SelectEnterEventArgs args)
    {
        // check new reference against previous reference
        // if new reference is different, reload gun
        var reference = GetComponent<XRGrabInteractable>().GetOldestInteractorSelecting(). //grab interactor
            transform.root.GetComponent <NetworkPlayer>().reference;

        if (reference != currentPlayerReference)
        {
            currentPlayerReference = reference;
            Reload();
        }

        print($"reference is now {currentPlayerReference}");
    }

    void ToggleTeleport(InputAction.CallbackContext context)
    {
        print($"Teleport is: {teleportOn}");
        if (GetComponent<XRGrabInteractable>().interactorsSelecting != null)
        {
            teleportOn = !teleportOn;
        }
        
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
        
        bullet.GetComponent<Bullet>().barrel = gameObject.transform.Find("Bullet Spawn Point");
        bullet.GetComponent<Bullet>().playerRef = currentPlayerReference;
        
        var position = bulletSpawnPoint.position;
        bullet.transform.position = position;
        bullet.transform.rotation = bulletSpawnPoint.rotation;
        
        //print($"Last interactor is: {GetComponent<XRGrabInteractable>().GetOldestInteractorSelecting().transform.gameObject.name}");
        GetComponent<XRGrabInteractable>().GetOldestInteractorSelecting().transform.gameObject.GetComponent<ActionBasedController>().SendHapticImpulse(0.7f, .1f);
        AudioSource.PlayClipAtPoint(gunshotClip,position);
        var em = ps.emission;
        var dur = ps.main.duration;
        em.enabled = true;
        
        ps.Play();
        StartCoroutine(DisableEmitter(dur));

        _currentBullets--;
        _bulletCountText.text = _currentBullets.ToString();

        if (teleportOn)
        {
            bullet.GetComponent<Bullet>().isTeleportEnabled = teleportOn;
            teleportOn = false;
        }
        
    }

    private IEnumerator DisableEmitter(float duration)
    {
        yield return new WaitForSeconds(duration);
        ps.Stop();
        var em = ps.emission;
        em.enabled = false;
        

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
            transform.root.GetComponent <NetworkPlayer>().SetAreaProvider();
    }
}