using UnityEngine;

namespace Targets
{
    public class OscillatingTarget : HitTarget
    {
        // private float _rotationHeight;
        // private float _rotationWidth;
        //
        // private float _rotationCenterX;
        // private float _rotationCenterY;
        // private float _rotationCenterZ;
        //
        // private int _oscillationCase;
        // public void SetNewRandomValues()
        // {
        //     Timer = Random.Range(0, 181);
        //     targetSpeed = Random.Range(1, 3);
        //     _rotationHeight = Random.Range(4, 16);
        //     _rotationWidth = Random.Range(4, 16);
        //     _oscillationCase = Random.Range(1, 4);
        //     SetRotationCenter(targetPlayer.transform.position);
        // }
        //
        // public override void UpdateLocation()
        // {
        //     Timer += Time.deltaTime*targetSpeed;
        //     float horizontalRotation;
        //     float verticalRotation;
        //
        //     switch (_oscillationCase)
        //     {
        //         case 1:
        //             horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterX;
        //             verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterY;
        //             transform.position = new Vector3(horizontalRotation, verticalRotation, _rotationCenterZ);
        //             break;
        //         case 2:
        //             horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterX;
        //             verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterZ;
        //             transform.position = new Vector3(horizontalRotation, _rotationCenterY, verticalRotation);
        //             break;
        //         case 3:
        //             horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterY;
        //             verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterZ;
        //             transform.position = new Vector3(_rotationCenterX, verticalRotation, horizontalRotation);
        //             break;
        //         default:
        //             horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterX;
        //             verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterY;
        //             transform.position = new Vector3(horizontalRotation, verticalRotation, _rotationCenterZ);
        //             break;
        //     }
        //     transform.LookAt(targetPlayer.transform);
        //     transform.Rotate(Vector3.right,90f);
        // }
        //
        // private void SetRotationCenter(Vector3 rotationCenter)
        // {
        //     _rotationCenterX = rotationCenter.x;
        //     _rotationCenterY = rotationCenter.y;
        //     _rotationCenterZ = rotationCenter.z;
        // }
    }
}
