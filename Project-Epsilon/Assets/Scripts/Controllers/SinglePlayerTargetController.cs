using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Targets;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class SinglePlayerTargetController : MonoBehaviour
    {
        public List<GameObject> inactiveTargets = new List<GameObject>();
        public List<GameObject> activeTargets = new List<GameObject>();

        public GameObject targetPrefab;
    
        private GameObject _bulletTarget;
        private int _spawningCase;

        public int maxTargets = 20;

        public bool simulationOn = false;

        
        /// <summary>
        /// Time between target spawns
        /// </summary>
        public float waitTime;
        // Start is called before the first frame update

        public void Update()
        {
            // If every  target currently active is one the player would lose points shooting, clear them all away
            // Todo: give the player extra points when this happens
            if (activeTargets.All(target => target.GetComponent<HitTarget>()._canTakeBulletsFrom == 2)) 
            {
                PutAwayAll();
            }
            // Update location of all targets
            UpdateTargets();
        }

        private void UpdateTargets()
        {
            if (activeTargets.Count <= 0) return;
        
            // DO NOT REPLACE WITH A FOR EACH LOOP. IT WILL BREAK AND YOU WILL CRY
            // Iterates through the active list, putting targets away if they've been shot at. Otherwise the position of 
            // the target is updated.
            for(int index = 0; index < activeTargets.Count; index++)
            {
                GameObject target = activeTargets[index];
                if (!target.GetComponent<HitTarget>().targetActive)
                {
                    PutAwayTarget(target);
                    continue;
                }
                target.GetComponent<HitTarget>().UpdateLocation();
            }
        }

        /// <summary>
        ///   Spawns a target in regular intervals as long as there are less than 20 active targets in the scene. 
        /// </summary>
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
                GameObject bulletTarget = null; 
                if (inactiveTargets.Count > 0) // Check the inactive targets list for a target
                {
                    bulletTarget = inactiveTargets[0];
                    inactiveTargets.Remove(bulletTarget);
                }
                else // if there are no targets in the inactive list, instantiate a new one
                {
                    // Todo: The check for photon connection is no longer needed as everything is photon dependent
                    if (PhotonNetwork.IsConnectedAndReady) // If connected to network, instantiate using photon
                    {
                        bulletTarget = PhotonNetwork.Instantiate(targetPrefab.name,Vector3.zero, Quaternion.identity);   
                    }
                    
                }
            
                HitTarget bulletTargetHitScript = bulletTarget!.GetComponent<HitTarget>(); // The hit target script of the target spawned

                #region Target variables
                int canTakeBulletsFrom; // The player that can shoot the target. 0 is both. 1 is Player 1. 2 is player 2. 
                int distance; // How far from the player the target starts
                int strafeCase; // The direction that the target moves back and forth along. X,Y,Z,XY,XZ,YZ
                int strafeDistance; // The max distance the target moves away from the spawn point
                float timer; // Used to track the position of the target in it's orbit
                float rotationHeight; // Self explanatory. I hope
                float rotationWidth; // Self explanatory. I hope
                int oscillationCase; // Whether the target rotates around XY,XZ, or YZ
                Vector3 spawnPoint; // The spawn point of the target
                Vector3 origin = SceneManager.GetActiveScene().buildIndex == 0 // Convoluted way of choosing the center pivot of all targets. Todo: make an origin variable in the game manager script
                    ? Vector3.zero
                    : new Vector3(0, 7.5f, 0); 
                float targetSpeed = Random.Range(0.5f, 1.3f); // Speed of the targets. Should probably play around with variables a bit more so this can be changed to meters per second
                #endregion
                
                if (SceneManager.GetActiveScene().buildIndex == 0) _spawningCase = 3; // If in the main menu, spawn all target types. 
                canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                switch (_spawningCase)
                {
                    case 0:// Stationary Targets
                        bulletTargetHitScript.SetTargetMode(0); 
                        
                        
                        distance = Random.Range(6, 9);
                        spawnPoint = Random.onUnitSphere*distance + Vector3.zero;
                        spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                        bulletTargetHitScript.SetStationaryValues(canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z);
                        break;
                    case 1:// Strafing Targets Only
                        bulletTargetHitScript.SetTargetMode(1);
                        
                        distance = Random.Range(3, 10);
                        spawnPoint = Random.onUnitSphere * distance + origin;
                        spawnPoint.y = MathF.Abs(spawnPoint.y);
                        strafeCase = Random.Range(1, 7);
                        strafeDistance = Random.Range(2, 6);
                        
                        bulletTargetHitScript.SetStrafingValues(canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z,strafeCase,strafeDistance,targetSpeed);
                        break;
                    case 2:// Oscillating Targets Only
                        bulletTargetHitScript.SetTargetMode(2);
                        
                        timer = Random.Range(0f, 181f);
                        rotationHeight = Random.Range(4.0f, 12.0f);
                        rotationWidth = Random.Range(4.0f, 12.0f);
                        oscillationCase = Random.Range(1, 4);
                        
                        bulletTargetHitScript.SetOscillatingValues(canTakeBulletsFrom,timer,rotationHeight,rotationWidth,oscillationCase,origin.x,origin.y,origin.z,targetSpeed);
                        break;
                    
                    // All Targets. This code is not DRY in anyway right now. I'll figure out a better way of doing it eventually.
                    // As it is, case 3 is just the switch statement up to the case above copied and pasted. 
                    case 3:
                        int tm = Random.Range(0, 4);
                        switch (tm)
                        {
                            case 0:
                                bulletTargetHitScript.SetTargetMode(0); 
                                
                                distance = Random.Range(6, 9);
                                spawnPoint = Random.onUnitSphere*distance + Vector3.zero;
                                spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                                bulletTargetHitScript.SetStationaryValues(canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z);
                                break;
                            case 1:
                                bulletTargetHitScript.SetTargetMode(1);
                        
                                distance = Random.Range(3, 10);
                                spawnPoint = Random.onUnitSphere * distance + origin;
                                spawnPoint.y = MathF.Abs(spawnPoint.y);
                                strafeCase = Random.Range(1, 7);
                                strafeDistance = Random.Range(2, 6);
                        
                                bulletTargetHitScript.SetStrafingValues(canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z,strafeCase,strafeDistance,targetSpeed);
                                break;
                            case 2:
                                bulletTargetHitScript.SetTargetMode(2);
                                
                                timer = Random.Range(0f, 181f);
                                rotationHeight = Random.Range(4.0f, 12.0f);
                                rotationWidth = Random.Range(4.0f, 12.0f);
                                oscillationCase = Random.Range(1, 4);
                        
                                bulletTargetHitScript.SetOscillatingValues(canTakeBulletsFrom,timer,rotationHeight,rotationWidth,oscillationCase,origin.x,origin.y,origin.z,targetSpeed);
                                break;
                            default:
                                bulletTargetHitScript.SetTargetMode(0);  
                        
                                //int canTakeBulletsFrom,int distance,int spawnPointX, int spawnPointY,int spawnPointZ
                                canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                                distance = Random.Range(6, 10);
                                spawnPoint = Random.onUnitSphere*distance + origin;
                                spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                        
                                bulletTargetHitScript.SetStationaryValues(canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z);
                                break;
                        }
                        break;
                    default: // Stationary only
                        bulletTargetHitScript.SetTargetMode(0); 
                        
                        canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                        distance = Random.Range(6, 11);
                        spawnPoint = Random.onUnitSphere*distance + origin;
                        spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                        bulletTargetHitScript.SetStationaryValues(canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z);
                        break;
                }
                activeTargets.Add(bulletTarget);
                bulletTargetHitScript.targetActive = true;
                bulletTarget.SetActive(true);
            }
            yield return new WaitForSeconds(waitTime); // Wait the designated time before spawning the next target
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
        /// Disables all targets
        /// </summary>
        private void PutAwayAll()
        {
            for(int i = activeTargets.Count-1; i >= 0;i--)
            {
                GameObject target = activeTargets[i];
                PutAwayTarget(target);
            }
        }

        /// <summary>
        /// Begins spawning targets 
        /// </summary>
        public void StartSimulation()
        {
            _spawningCase = 3; //0- Stationary only, 1- Strafing Only, 2- Oscillating only, 3- All types
            simulationOn = true;
            StartCoroutine(TargetSpawning());
        }
    
        /// <summary>
        /// Stops spawning targets and removes all targets in the scene 
        /// </summary>
        public void EndSimulation()
        {
            simulationOn = false;
            PutAwayAll();
        }
    }
}
