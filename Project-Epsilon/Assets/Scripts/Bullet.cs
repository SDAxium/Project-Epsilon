using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float Speed = 40;
    public Transform barrel;
    public bool active;

    public int playerRef;

    public void Awake()
    {
        //barrel = transform.parent.Find("Bullet Spawn Point").transform;
        active = true;
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
        if (!collision.gameObject.CompareTag("Bullet")) active = false;
    }
}
