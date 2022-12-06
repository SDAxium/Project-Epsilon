using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets
{
    public class StrafingTarget : HitTarget
    {
    //     private Vector3 _spawnPoint;
    //     private Vector3 _pointOne;
    //     private Vector3 _pointTwo;
    //     private int _strafeCase;
    //     private float _strafeDistance;
    //     private float _distanceFromPlayer;
    //
    //     private void Start()
    //     {
    //         SetNewRandomValues();
    //     }
    //
    //     public void SetNewRandomValues()
    //     {
    //         _distanceFromPlayer = Random.Range(3, 11);
    //         /*
    //          * Sets the spawn point to a random value around the player
    //          * If the target spawns below the player, moves the target so that it is level or above the player
    //          */
    //         _spawnPoint = Random.onUnitSphere*_distanceFromPlayer + targetPlayer.transform.position;
    //         _spawnPoint.y = MathF.Abs(_spawnPoint.y);
    //         
    //         // Cases: X,Y,Z,XY,XZ,YZ
    //         _strafeCase = Random.Range(1, 7);
    //
    //         _strafeDistance = Random.Range(2, 6);
    //         targetSpeed = Random.Range(1, 3); 
    //     }
    //     public override  void UpdateLocation()
    //     {
    //         Timer += Time.deltaTime*targetSpeed;
    //         switch (_strafeCase)
    //         {
    //             case 1:
    //                 _pointOne = _spawnPoint + Vector3.left * _strafeDistance;
    //                 _pointTwo = _spawnPoint + Vector3.right * _strafeDistance;
    //                 break;
    //             case 2:
    //                 if (_spawnPoint.y - _strafeDistance < targetPlayer.transform.position.y)
    //                 {
    //                     _spawnPoint += new Vector3(0, Mathf.Abs(_spawnPoint.y - _strafeDistance), 0);
    //                 }
    //                 _pointOne = _spawnPoint + Vector3.down * _strafeDistance;
    //                 _pointTwo = _spawnPoint + Vector3.up * _strafeDistance;
    //                 break;
    //             case 3:
    //                 _pointOne = _spawnPoint + Vector3.back * _strafeDistance;
    //                 _pointTwo = _spawnPoint + Vector3.forward * _strafeDistance;
    //                 break;
    //             case 4:
    //                 if (_spawnPoint.y - _strafeDistance/2f < targetPlayer.transform.position.y)
    //                 {
    //                     _spawnPoint += new Vector3(0, Mathf.Abs(_spawnPoint.y - _strafeDistance), 0);
    //                 }
    //                 _pointOne = _spawnPoint + new Vector3(_strafeDistance/2f,_strafeDistance/2f,0);
    //                 _pointTwo = _spawnPoint - new Vector3(_strafeDistance/2f,_strafeDistance/2f,0);
    //                 break;
    //             case 5:
    //                 _pointOne = _spawnPoint + new Vector3(_strafeDistance/2f,0,_strafeDistance/2f);
    //                 _pointTwo = _spawnPoint - new Vector3(_strafeDistance/2f,0,_strafeDistance/2f);
    //                 break;
    //             case 6:
    //                 if (_spawnPoint.y - _strafeDistance/2f < targetPlayer.transform.position.y)
    //                 {
    //                     _spawnPoint += new Vector3(0, Mathf.Abs(_spawnPoint.y - _strafeDistance), 0);
    //                 }
    //                 _pointOne = _spawnPoint + new Vector3(0,_strafeDistance/2f,_strafeDistance/2f);
    //                 _pointTwo = _spawnPoint - new Vector3(0,_strafeDistance/2f,_strafeDistance/2f);
    //                 break;
    //             default:
    //                 _pointOne = _spawnPoint + Vector3.left * _strafeDistance;
    //                 _pointTwo = _spawnPoint + Vector3.right * _strafeDistance;
    //                 break;
    //         }
    //         transform.position = Vector3.Lerp(_pointOne, _pointTwo, (Mathf.Sin(Timer)+1.0f)/2.0f);
    //         transform.LookAt(targetPlayer.transform);
    //         transform.Rotate(Vector3.right,90f);
    //     }
    }
}
