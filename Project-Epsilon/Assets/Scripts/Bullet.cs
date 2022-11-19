using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Gun gunOrigin; 
    
    private const float Speed = 40;
    public Transform barrel;
    public bool active;

    public int playerRef;

    private bool _isTeleportEnabled = false;
    public void Awake()
    {
        //barrel = transform.parent.Find("Bullet Spawn Point").transform;
        active = true;
        
    }

    public void SetGun(Gun gun)
    {
        gunOrigin = gun;
        if (gunOrigin.teleportOn)
        {
            _isTeleportEnabled = true;
        }
    }
    private void Update()
    {
        if (CheckIfTargetOutOfRange()) active = false;
    }

    private bool CheckIfTargetOutOfRange()
    {
        var position = transform.position;
        var x = Mathf.Abs(position.x);
        var y = Mathf.Abs(position.y);
        var z = Mathf.Abs(position.z);

        return x > 70 || y > 70 || z > 70;
    }
    public void UpdateLocation()
    {
        if (GetComponent<Rigidbody>().velocity.Equals(Vector3.zero)) GetComponent<Rigidbody>().velocity = Speed * barrel.forward;
    }

    public void OnCollisionEnter(Collision collision)
    {
        // if (!collision.gameObject.CompareTag("Bullet")) active = false;

        if (_isTeleportEnabled)
        {
            if (collision.gameObject.CompareTag("TeleportArea"))
            {
                print("we made it");
                gunOrigin.GetCurrentPlayer().transform.position = transform.position;
                active = false;
            }
        }
        
    }
}
