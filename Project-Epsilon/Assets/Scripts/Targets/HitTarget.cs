using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets
{
    public class HitTarget : MonoBehaviour
    {
        public AudioClip blindPing;
        public AudioClip spawnSound;

        public GameObject player;

        protected float Timer;

        public bool targetActive;

        protected float TargetSpeed;
        // public bool notMoving;
        private void Awake()
        {
            targetActive = true;
            player = GameObject.Find("Player");
        }

        public virtual void SetNewRandomValues()
        {
            
        }
        public virtual void UpdateLocation()
        {
        
        }

        public virtual void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Bullet")) return;
            targetActive = false;
        }
    }
}
