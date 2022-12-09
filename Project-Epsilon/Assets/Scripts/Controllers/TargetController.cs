using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Targets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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


        private PhotonView _photonView;
        /// <summary>
        /// Time between target spawns
        /// </summary>
        public float waitTime;
        // Start is called before the first frame update
        void Start()
        {
            _photonView = GetComponent<PhotonView>();
            //StartCoroutine(TargetSpawning());
            for (int i = 0; i < 40; i++)
            {
                
            }
        }
        
        public void Update()
        {
            _photonView.RPC(nameof(UpdateTargets),RpcTarget.All);
        }

        [PunRPC]
        public void UpdateTargets()
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
                GameObject bulletTarget = null; 
                if (inactiveTargets.Count > 0)
                {
                    bulletTarget = inactiveTargets[0];
                    inactiveTargets.Remove(bulletTarget);
                }
                else
                {
                    if (PhotonNetwork.IsConnectedAndReady) // If connected to network, instantiate using photon
                    {
                        bulletTarget = PhotonNetwork.Instantiate(targetPrefab.name,Vector3.zero, Quaternion.identity);   
                    }
                    
                }
            
                HitTarget bulletTargetHitScript = bulletTarget!.GetComponent<HitTarget>(); // The hit target script of the target spawned
                int canTakeBulletsFrom;
                int distance;
                int strafeCase;
                int strafeDistance;
                int timer;
                float rotationHeight;
                float rotationWidth;
                int oscillationCase;
                Vector3 spawnPoint;
                Vector3 origin = SceneManager.GetActiveScene().buildIndex == 0
                    ? Vector3.zero
                    : new Vector3(0, 7.5f, 0);
               float targetSpeed = Random.Range(0.5f, 1.3f); 
                if (SceneManager.GetActiveScene().buildIndex == 0) _spawningCase = 3;
                switch (_spawningCase)
                {
                    case 0:// Stationary Targets
                        bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,0); 
                        
                        //int canTakeBulletsFrom,int distance,int spawnPointX, int spawnPointY,int spawnPointZ
                        canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                        distance = Random.Range(6, 11);
                        spawnPoint = Random.onUnitSphere*distance + origin;
                        spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                        
                        bulletTargetHitScript.photonView.RPC("SetStationaryValues",RpcTarget.All,
                            canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z);
                        break;
                    case 1:// Strafing Targets Only
                        bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,1);
                        
                        //int canTakeBulletsFrom,int distance,int spawnPointX, int spawnPointY,int spawnPointZ, int strafeCase, int strafeDistance
                        canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                        distance = Random.Range(3, 11);
                        spawnPoint = Random.onUnitSphere * distance + origin;
                        spawnPoint.y = MathF.Abs(spawnPoint.y);
                        strafeCase = Random.Range(1, 7);
                        strafeDistance = Random.Range(2, 6);
                        
                        bulletTargetHitScript.photonView.RPC("SetStrafingValues",RpcTarget.All,
                            canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z,strafeCase,strafeDistance,targetSpeed);
                        break;
                    case 2:// Oscillating Targets Only
                        bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,2);
                        //int canTakeBulletsFrom,float timer,float rotationHeight, float rotationWidth, int oscillationCase, int rotationCenterX,int rotationCenterY,int rotationCenterZ
                        canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                        timer = Random.Range(0, 181);
                        rotationHeight = Random.Range(4.0f, 16.0f);
                        rotationWidth = Random.Range(4.0f, 16.0f);
                        oscillationCase = Random.Range(1, 4);
                        
                        bulletTargetHitScript.photonView.RPC("SetOscillatingValues",RpcTarget.All,
                            canTakeBulletsFrom,timer,rotationHeight,rotationWidth,origin.x,origin.y,origin.z,targetSpeed);
                        break;
                    case 3://All Targets
                        int tm = Random.Range(0, 4);
                        switch (tm)
                        {
                            case 0:
                                bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,0); 
                        
                                //int canTakeBulletsFrom,int distance,int spawnPointX, int spawnPointY,int spawnPointZ
                                canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                                distance = Random.Range(6, 11);
                                spawnPoint = Random.onUnitSphere*distance + origin;
                                spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                        
                                bulletTargetHitScript.photonView.RPC("SetStationaryValues",RpcTarget.All,
                                    canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z);
                                break;
                            case 1:
                                bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,1);
                        
                                //int canTakeBulletsFrom,int distance,int spawnPointX, int spawnPointY,int spawnPointZ, int strafeCase, int strafeDistance
                                canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                                distance = Random.Range(3, 11);
                                spawnPoint = Random.onUnitSphere * distance + origin;
                                spawnPoint.y = MathF.Abs(spawnPoint.y);
                                strafeCase = Random.Range(1, 7);
                                strafeDistance = Random.Range(2, 6);
                        
                                bulletTargetHitScript.photonView.RPC("SetStrafingValues",RpcTarget.All,
                                    canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z,strafeCase,strafeDistance,targetSpeed);
                                break;
                            case 2:
                                bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,2);
                                //int canTakeBulletsFrom,float timer,float rotationHeight, float rotationWidth, int oscillationCase, int rotationCenterX,int rotationCenterY,int rotationCenterZ
                                canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                                timer = Random.Range(0, 181);
                                rotationHeight = Random.Range(4.0f, 16.0f);
                                rotationWidth = Random.Range(4.0f, 16.0f);
                                oscillationCase = Random.Range(1, 4);
                        
                                bulletTargetHitScript.photonView.RPC("SetOscillatingValues",RpcTarget.All,
                                    canTakeBulletsFrom,timer,rotationHeight,rotationWidth,origin.x,origin.y,origin.z,targetSpeed);
                                break;
                            default:
                                bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,0); 
                        
                                //int canTakeBulletsFrom,int distance,int spawnPointX, int spawnPointY,int spawnPointZ
                                canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                                distance = Random.Range(6, 11);
                                spawnPoint = Random.onUnitSphere*distance + origin;
                                spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                        
                                bulletTargetHitScript.photonView.RPC("SetStationaryValues",RpcTarget.All,
                                    canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z);
                                break;
                        }
                        break;
                    default:
                        bulletTargetHitScript.photonView.RPC("SetTargetMode",RpcTarget.All,0); 
                        
                        //int canTakeBulletsFrom,int distance,int spawnPointX, int spawnPointY,int spawnPointZ
                        canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
                        distance = Random.Range(6, 11);
                        spawnPoint = Random.onUnitSphere*distance + origin;
                        spawnPoint.y = MathF.Abs(spawnPoint.y);
                        
                        
                        bulletTargetHitScript.photonView.RPC("SetStationaryValues",RpcTarget.All,
                            canTakeBulletsFrom,distance,spawnPoint.x,spawnPoint.y,spawnPoint.z); 
                        break;
                }
                bulletTargetHitScript.SetNewRandomValues();
                
                activeTargets.Add(bulletTarget);
                bulletTargetHitScript.targetActive = true;
                bulletTarget.SetActive(true);
            }
            yield return new WaitForSeconds(waitTime);
            yield return TargetSpawning();
        }
        
        /// <summary>
        /// Takes a target and removes it from the active targets list. Removed targets are also deactivated 
        /// </summary>
        /// <param name="target">The target to put away</param>
        [PunRPC]
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
            _spawningCase = 3; //0- Stationary only, 1- Strafing Only, 2- Oscillating only, 3- All types
            simulationOn = true;
            StartCoroutine(TargetSpawning());
        }
    
        /// <summary>
        /// Stops spawning targets and removes all targets in the scene 
        /// </summary>
        [PunRPC]
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