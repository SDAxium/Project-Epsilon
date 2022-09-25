using System;
using System.Collections;
using System.Collections.Generic;
using Targets;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class TargetController : MonoBehaviour
{
    public List<GameObject> inactiveTargets = new List<GameObject>();
    public List<GameObject> activeTargets = new List<GameObject>();

    public GameObject oscillatingTargetPrefab,strafingTargetPrefab;
    
    private GameObject _bulletTarget;
    private int _spawningCase;

    public int maxTargets = 20;

    public bool simulationOn = false;
    // Start is called before the first frame update
    // void Start()
    // {
    //     //StartCoroutine(TargetSpawning());
    //     //Invoke(nameof(StartSimulation),0.5f);
    // }

    public void SetSpawningCase(int caseNumber)
    {
        _spawningCase = caseNumber;
    }
    // Update is called once per frame
    public void Update()
    {
        if (activeTargets.Count <= 0) return;
        // ReSharper disable once ForCanBeConvertedToForeach
        for(var index = 0; index < activeTargets.Count; index++)
        {
            var target = activeTargets[index]; // The target parent object
            // Target child object. The parent is being used because all of its transforms are at default
           // var targetChild = target.transform.GetChild(0).gameObject; 
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
        
        // If active target list is full, wait until it isn't
        if (activeTargets.Count >= maxTargets) yield return new WaitUntil(() => activeTargets.Count < maxTargets);
        GameObject bulletTarget;
        switch (_spawningCase)
        {
            case 0://Strafing Targets Only
                if (inactiveTargets.Count(target => target.GetComponent<StrafingTarget>() != null) > 0)
                {
                    bulletTarget = inactiveTargets.Find(target => target.GetComponent<StrafingTarget>() != null);
                    inactiveTargets.Remove(bulletTarget);
                }
                else
                {
                    //_bulletTarget = strafingTargetPrefab;
                    bulletTarget = Instantiate(strafingTargetPrefab);
                }
                break;
            
            case 1://Oscillating Targets Only
                if (inactiveTargets.Count(target => target.GetComponent<OscillatingTarget>() != null) > 0)
                {
                    bulletTarget = inactiveTargets.Find(target => target.GetComponent<OscillatingTarget>() != null);
                    inactiveTargets.Remove(bulletTarget);
                }
                else
                {
                    //_bulletTarget = oscillatingTargetPrefab;
                    bulletTarget = Instantiate(oscillatingTargetPrefab);
                }
                break;
            
            case 2://All Targets
                var osci = Random.value >= 0.5f;
                if (inactiveTargets.Count > 0)
                {
                    if (osci)
                    {
                        bulletTarget = inactiveTargets.Count(target => target.GetComponent<OscillatingTarget>() != null) > 0 ?
                            inactiveTargets.Find(target => target.GetComponent<OscillatingTarget>() != null) : 
                            Instantiate(oscillatingTargetPrefab);
                    }
                    else
                    {
                        bulletTarget = inactiveTargets.Count(target => target.GetComponent<StrafingTarget>() != null) > 0 ? 
                            inactiveTargets.Find(target => target.GetComponent<StrafingTarget>() != null) : 
                            Instantiate(strafingTargetPrefab);
                    }
                    inactiveTargets.Remove(bulletTarget);
                }
                else
                {
                    _bulletTarget = osci ? oscillatingTargetPrefab : strafingTargetPrefab;
                    bulletTarget = Instantiate(_bulletTarget);
                }
                break;
            default://Strafing
                if (inactiveTargets.Count > 0)
                {
                    bulletTarget = inactiveTargets.Find(target => target.GetComponent<StrafingTarget>() != null);
                    inactiveTargets.Remove(bulletTarget);
                }
                else
                {
                    //_bulletTarget = strafingTargetPrefab;
                    bulletTarget = Instantiate(strafingTargetPrefab);
                }
                break;
        }
        bulletTarget.GetComponent<HitTarget>().SetNewRandomValues();
        activeTargets.Add(bulletTarget);
        bulletTarget.SetActive(true);
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
        simulationOn = true;
        StartCoroutine(TargetSpawning());
    }
    
    public void EndSimulation()
    {
        simulationOn = false;
        for(var i = activeTargets.Count-1; i > 0;i--)
        {
            var target = activeTargets[i];
            PutAwayTarget(target);
        }
    }
}
