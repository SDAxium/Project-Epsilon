using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Targets
{
    public class HitTarget : MonoBehaviour
    {
        public GameObject origin;

        public Material defaultMaterial, p1Material, p2Material;
        
        // Multiplayer Thing
        // Using to this to track which player can shoot this target. 0 is both. 1 is player 1. 2 is player 2
        private int _canTakeBulletsFrom;
        //
        
        private const int Stationary = 0;
        private const int Strafing = 1;
        private const int Oscillating = 2;
        private int _targetMode;
        
        // GLOBAL TARGET VALUES
        public GameObject targetPlayer;
        private Vector3 _spawnPoint;
        public AudioClip spawnSound;

        protected float Timer;
        public bool targetActive;
        public float targetSpeed;
        // GLOBAL TARGET VALUES
        
        // STRAFING TARGET VALUES
        private Vector3 _pointOne;
        private Vector3 _pointTwo;
        private int _strafeCase;
        private float _strafeDistance;
        private float _distanceFromPlayer;
        // STRAFING TARGET VALUES
        
        // OSCILLATING TARGET VALUES
        private float _rotationHeight;
        private float _rotationWidth;
        
        private float _rotationCenterX;
        private float _rotationCenterY;
        private float _rotationCenterZ;
        
        private int _oscillationCase;
        // OSCILLATING TARGET VALUES
        private void Awake()
        {
            targetActive = true;
            targetPlayer = GameObject.FindWithTag("Player");
            origin = GameObject.Find("origin");
        }

        private void Start()
        {
            SetNewRandomValues();
        }

        private void Update()
        {
            targetPlayer = GameObject.FindWithTag("Player");
        }

        public virtual void SetNewRandomValues()
        {
            _canTakeBulletsFrom = Random.Range(0, 3);
            switch (_targetMode)
            {
                case Stationary:
                    _distanceFromPlayer = Random.Range(6, 11);
                    _spawnPoint = Random.onUnitSphere*_distanceFromPlayer + origin.transform.position;
                    _spawnPoint.y = MathF.Abs(_spawnPoint.y);
                    break;
                case Strafing:
                    _distanceFromPlayer = Random.Range(3, 11);
                    _spawnPoint = Random.onUnitSphere*_distanceFromPlayer + origin.transform.position;
                    _spawnPoint.y = MathF.Abs(_spawnPoint.y);
            
                    // Cases: X,Y,Z,XY,XZ,YZ
                    _strafeCase = Random.Range(1, 7);

                    _strafeDistance = Random.Range(2, 6);
                    break;
                case Oscillating:
                    Timer = Random.Range(0, 181);
                    _rotationHeight = Random.Range(4.0f, 16.0f);
                    _rotationWidth = Random.Range(4.0f, 16.0f);
                    _oscillationCase = Random.Range(1, 4);
                    SetRotationCenter(origin.transform.position);
                    break;
                default:
                    _distanceFromPlayer = Random.Range(6, 11);
                    _spawnPoint = Random.onUnitSphere*_distanceFromPlayer + origin.transform.position;
                    _spawnPoint.y = MathF.Abs(_spawnPoint.y);
                    break;
            }
            targetSpeed = Random.Range(0.5f, 2.5f); 
        }
        public virtual void UpdateLocation()
        {
            Timer += Time.deltaTime*targetSpeed;
            switch (_targetMode)
            {
                case Stationary:
                    // TODO: make it jiggle left and right or something. Being absolutely still is kinda wack 
                    transform.position = _spawnPoint;
                    break;
                case Strafing:
                    switch (_strafeCase)
                    {
                        case 1:
                            _pointOne = _spawnPoint + Vector3.left * _strafeDistance;
                            _pointTwo = _spawnPoint + Vector3.right * _strafeDistance;
                            break;
                        case 2:
                            _spawnPoint.y = MathF.Abs(_spawnPoint.y);
                            _pointOne = _spawnPoint + Vector3.down * _strafeDistance;
                            _pointTwo = _spawnPoint + Vector3.up * _strafeDistance;
                            break;
                        case 3:
                            _pointOne = _spawnPoint + Vector3.back * _strafeDistance;
                            _pointTwo = _spawnPoint + Vector3.forward * _strafeDistance;
                            break;
                        case 4:
                            _spawnPoint.y = MathF.Abs(_spawnPoint.y);
                            _pointOne = _spawnPoint + new Vector3(_strafeDistance/2f,_strafeDistance/2f,0);
                            _pointTwo = _spawnPoint - new Vector3(_strafeDistance/2f,_strafeDistance/2f,0);
                            break;
                        case 5:
                            _pointOne = _spawnPoint + new Vector3(_strafeDistance/2f,0,_strafeDistance/2f);
                            _pointTwo = _spawnPoint - new Vector3(_strafeDistance/2f,0,_strafeDistance/2f);
                            break;
                        case 6:
                            _spawnPoint.y = MathF.Abs(_spawnPoint.y);
                            _pointOne = _spawnPoint + new Vector3(0,_strafeDistance/2f,_strafeDistance/2f);
                            _pointTwo = _spawnPoint - new Vector3(0,_strafeDistance/2f,_strafeDistance/2f);
                            break;
                        default:
                            _pointOne = _spawnPoint + Vector3.left * _strafeDistance;
                            _pointTwo = _spawnPoint + Vector3.right * _strafeDistance;
                            break;
                    }
                    transform.position = Vector3.Lerp(_pointOne, _pointTwo, (Mathf.Sin(Timer)+1.0f)/2.0f);
                    break;
                case Oscillating:
                    float horizontalRotation;
                    float verticalRotation;

                    switch (_oscillationCase)
                    {
                        case 1:
                            horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterX;
                            verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterY;
                            transform.position = new Vector3(horizontalRotation, verticalRotation, _rotationCenterZ);
                            break;
                        case 2:
                            horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterX;
                            verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterZ;
                            transform.position = new Vector3(horizontalRotation, _rotationCenterY, verticalRotation);
                            break;
                        case 3:
                            horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterY;
                            verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterZ;
                            transform.position = new Vector3(_rotationCenterX, verticalRotation, horizontalRotation);
                            break;
                        default:
                            horizontalRotation = Mathf.Cos(Timer) * _rotationWidth + _rotationCenterX;
                            verticalRotation = Mathf.Sin(Timer) * _rotationHeight + _rotationCenterY;
                            transform.position = new Vector3(horizontalRotation, verticalRotation, _rotationCenterZ);
                            break;
                    }
                    break;
                default:
                    transform.position = _spawnPoint;
                    break;
            }
            transform.LookAt(targetPlayer.transform);
            transform.Rotate(Vector3.right,90f);
        }

        public void SetTargetMode(int mode)
        {
            _targetMode = mode;
        }
        
        private void SetRotationCenter(Vector3 rotationCenter)
        {
            _rotationCenterX = rotationCenter.x;
            _rotationCenterY = rotationCenter.y;
            _rotationCenterZ = rotationCenter.z;
        }
        
        public virtual void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Bullet")) return;
            if (_canTakeBulletsFrom == 0)
            {
                targetActive = false;
            }
            else
            {
                // Check if bullet was shot by the player that can hit this target and deactivate target if it can 
                targetActive = !other.gameObject.GetComponent<Bullet>().playerRef.Equals(_canTakeBulletsFrom);
            }
        }
    }
}