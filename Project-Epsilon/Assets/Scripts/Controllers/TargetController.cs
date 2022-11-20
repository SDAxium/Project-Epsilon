using System.Collections;
using System.Collections.Generic;
using Targets;
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

        public void SetSpawningCase(int caseNumber)
        {
            _spawningCase = caseNumber;
        }
        // Update is called once per frame
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
                GameObject bulletTarget; //= inactiveTargets.Count > 0 ? inactiveTargets[0] : Instantiate(targetPrefab);
                if (inactiveTargets.Count > 0)
                {
                    bulletTarget = inactiveTargets[0];
                    inactiveTargets.Remove(bulletTarget);
                }
                else
                {
                    bulletTarget = Instantiate(targetPrefab);
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
                bulletTargetHitScript.targetActive = true;
                activeTargets.Add(bulletTarget);
                bulletTarget.SetActive(true);
            }
            yield return new WaitForSeconds(3f);
            yield return TargetSpawning();
        }
    
        private void PutAwayTarget(GameObject target)
        {
            activeTargets.Remove(target); // Remove from active targets
            inactiveTargets.Add(target); // Add to inactive targets
            target.SetActive(false); // Turn target visibility off
        }
    
        public void StartSimulation()
        {
            _spawningCase = 1;
            simulationOn = true;
            StartCoroutine(TargetSpawning());
        }
    
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