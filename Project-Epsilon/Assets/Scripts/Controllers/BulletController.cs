using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
    public class BulletController : MonoBehaviour
    {
        public GameObject bulletPrefab;
    
        public List<GameObject> inactiveBullets = new List<GameObject>();
        public List<GameObject> activeBullets = new List<GameObject>();

        private void Start()
        {
       
        }

        public GameObject GetBullet()
        {
            GameObject bullet;
        
            if (inactiveBullets.Count > 0)
            {
                bullet = inactiveBullets[0];
                bullet.SetActive(true);
                inactiveBullets.Remove(bullet);
                activeBullets.Add(bullet);
            }
            else
            {
                bullet = Instantiate(bulletPrefab);
                activeBullets.Add(bullet);
            }
        
            bullet.GetComponent<Bullet>().active = true;
            return bullet;
        }
        private void Update()
        {
            if(activeBullets.Count > 0) UpdateBullets();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void UpdateBullets()
        {
            // ReSharper disable once ForCanBeConvertedToForeach
            // It cannot be converted to a foreach loop. The contents of activeBullets get changed so it would break
            for (var bulletIndex = 0; bulletIndex < activeBullets.Count; bulletIndex++)
            {
                var bullet = activeBullets[bulletIndex];
                bullet.GetComponent<Bullet>().UpdateLocation();
                if (!bullet.GetComponent<Bullet>().active)
                {
                    PutAway(bullet);
                }
            }
        }

        // Resets bullet velocity and disables bullet
        private void PutAway(GameObject bullet)
        {
            bullet.GetComponent<Rigidbody>().velocity = Vector3.zero;
            activeBullets.Remove(bullet);
            inactiveBullets.Add(bullet);
            bullet.SetActive(false);
        }
    }
}