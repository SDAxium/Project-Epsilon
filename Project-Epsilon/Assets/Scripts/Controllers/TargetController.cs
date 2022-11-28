using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Targets;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class TargetController : MonoBehaviour
    {
        public List<GameObject> inactiveTargets = new List<GameObject>();
        public List<GameObject> activeTargets = new List<GameObject>();

        public GameObject targetPrefab;
    
        private GameObject _bulletTarget;
        private int _spawningCase;

        public int maxTargets = 20;

        public bool simulationOn = false;
        // Start is called before the first frame update
        void Start()
        {
            //StartCoroutine(TargetSpawning());
        }
        
        public void Update()
        {
            if (activeTargets.Count <= 0) return;
        
            for(int index = 0; index < activeTargets.Count; index++)
            {
                GameObject target = activeTargets[index];
                if (!target.GetComponent<HitTarget>().targetActive)
                {
                    Debug.Log("HIT");
                    PutAwayTarget(target);
                    continue;
                }
                target.GetComponent<HitTarget>().UpdateLocation();
            }
        }
    
        /// <summary>
        ///     Spawns a target a every three seconds as long as there are less than 20 active targets in the scene. 
        /// </summary>
        /// <returns></returns>
        [PunRPC]
        private IEnumerator TargetSpawning()
        {
            if (!simulationOn)
            {
                StopCoroutine(nameof(TargetSpawning));
                yield break;
            }
        
            // If active target list is not full, spawn a new target
            if (!(activeTargets.Count >= maxTargets))
            {
                GameObject bulletTarget; 
                if (inactiveTargets.Count > 0)
                {
                    bulletTarget = inactiveTargets[0];
                    inactiveTargets.Remove(bulletTarget);
                }
                else
                {
                    bulletTarget = PhotonNetwork.Instantiate(targetPrefab.name,Vector3.zero, Quaternion.identity);
                }
            
                HitTarget bulletTargetHitScript = bulletTarget.GetComponent<HitTarget>();
            
                switch (_spawningCase)
                {
                    case 0:// Stationary Targets
                        bulletTargetHitScript.SetTargetMode(0); 
                        break;
                    case 1:// Strafing Targets Only
                        bulletTargetHitScript.SetTargetMode(1); 
                        break;
                    case 2:// Oscillating Targets Only
                        bulletTargetHitScript.SetTargetMode(2);
                        break;
                    case 3://All Targets
                        bulletTargetHitScript.SetTargetMode(Random.Range(0,4));
                        break;
                    default:
                        bulletTargetHitScript.SetTargetMode(0); 
                        break;
                }
                bulletTargetHitScript.SetNewRandomValues();
                
                print("adding target to active targets");
                activeTargets.Add(bulletTarget);
                bulletTargetHitScript.targetActive = true;
                bulletTarget.SetActive(true);
            }
            yield return new WaitForSeconds(1f);
            yield return TargetSpawning();
        }
        
        /// <summary>
        /// Takes a target and removes it from the active targets list. Removed targets are also deactivated 
        /// </summary>
        /// <param name="target">The target to put away</param>
        private void PutAwayTarget(GameObject target)
        {
            activeTargets.Remove(target); // Remove from active targets
            inactiveTargets.Add(target); // Add to inactive targets
            target.SetActive(false); // Turn target visibility off
        }
    
        /// <summary>
        /// Begins spawning targets 
        /// </summary>
        public void StartSimulation()
        {
            _spawningCase = 3;
            simulationOn = true;
            StartCoroutine(TargetSpawning());
        }
    
        /// <summary>
        /// Stops spawning targets and removes all targets in the scene 
        /// </summary>
        public void EndSimulation()
        {
            simulationOn = false;
            
            for(int i = activeTargets.Count-1; i >= 0;i--)
            {
                GameObject target = activeTargets[i];
                PutAwayTarget(target);
            }
        }
    }
}