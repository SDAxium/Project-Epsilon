using System;
using Controllers;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Targets
{
    public class HitTarget : MonoBehaviour
    {
        public Color defaultColor = new(128,0,202);
        public Color defaultColor2 = new(69,0,108);
        public Color playerOneColor = new(17,0,200);
        public Color playerOneColor2 = new(12,0,137);
        public Color playerTwoColor = new(209,22,0);
        public Color playerTwoColor2 = new(132,14,0);
        
        public GameObject origin;

        // Multiplayer Thing
        /// <summary>
        /// Using this to track which player can shoot this target. 0 is both. 1 is player 1. 2 is player 2
        /// </summary>
        private int _canTakeBulletsFrom = 0;

        private Material _targetBaseMaterial, _targetBase2Material, _targetEyeMaterial;

        //Base Color and Eye Color are the same
        private Color _baseColor, _base2Color;

        private const int Stationary = 0;
        private const int Strafing = 1;
        private const int Oscillating = 2;
        private int _targetMode;
        
        // GLOBAL TARGET VALUES
        public GameObject targetPlayer;
        private Vector3 _spawnPoint;
        public AudioClip spawnSound;

        private float Timer;
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

        private MeshRenderer _meshRenderer;
        private PhotonView _photonView;

        // OSCILLATING TARGET VALUES
        private void Awake()
        {
            _photonView = GetComponent<PhotonView>();
            _meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            targetActive = true;
            if (PhotonNetwork.IsConnectedAndReady)
            {
                int index = 0;
                if (_canTakeBulletsFrom == 0)
                {
                    
                    if (PhotonNetwork.PlayerList.Length == 1)
                    {
                        index = 0;
                    }
                    else
                    {
                        index = Random.Range(0, 2) > 0 ? 0 : 1;
                    }
                    
                }
                else if (_canTakeBulletsFrom == 1)
                {
                    index = 0;
                }
                else
                {
                    index = 1;
                }

                targetPlayer = PhotonNetwork.PlayerList[index].TagObject as GameObject;
                
                if (targetPlayer == null)
                {
                    print("Whoops, still null");
                    if (_canTakeBulletsFrom == 1)
                    {
                        targetPlayer = GameObject.Find("Player One");
                    }
                    else if (_canTakeBulletsFrom == 2)
                    {
                        targetPlayer = GameObject.Find("Player Two");
                    }
                }
                //targetPlayer = PhotonNetwork.LocalPlayer.TagObject as GameObject;
                // if (targetPlayer != null) print(targetPlayer.name);
                print(targetPlayer != null ? targetPlayer.name:"WHAT");
            }
            origin = GameObject.Find("origin");
        }

        private void Start()
        {
            _photonView = GetComponent<PhotonView>();
            // SetNewRandomValues();
        }

        private void Update()
        {
            
        }

        [PunRPC]
        void UpdateColors()
        {
            var materials = _meshRenderer.materials;
            switch (_canTakeBulletsFrom)
            {
                case 0:
                    materials[1].color = defaultColor;
                    materials[4].color = defaultColor;
                    materials[5].color = defaultColor2;
                    break;
                case 1:
                    materials[1].color = playerOneColor;
                    materials[4].color = playerOneColor;
                    materials[5].color = playerOneColor2;
                    break;
                case 2: 
                    materials[1].color = playerTwoColor;
                    materials[4].color = playerTwoColor;
                    materials[5].color = playerTwoColor2;
                    break;
                default:
                    materials[1].color = defaultColor;
                    materials[4].color = defaultColor;
                    materials[5].color = defaultColor2;
                    break;
            }
        }
        
        public void SetNewRandomValues()
        {
            //_canTakeBulletsFrom = Random.Range(0, SceneManager.GetActiveScene().buildIndex == 0 ? 2 : 3);
            _canTakeBulletsFrom = SceneManager.GetActiveScene().buildIndex == 0 ? 1 : Random.Range(0, 3);
            _photonView.RPC(nameof(UpdateColors),RpcTarget.All);
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
            targetSpeed = Random.Range(0.5f, 1.3f); 
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
        
        public void OnCollisionEnter(Collision other)
        {
            
            if (!other.gameObject.CompareTag("Bullet")) return;
            if (other.gameObject.GetComponent<Bullet>().playerRef != _canTakeBulletsFrom && _canTakeBulletsFrom != 0) return;
            _photonView.RPC(nameof(DeactivateTarget),RpcTarget.All);
            
            // targetActive = false;
            // gameObject.SetActive(false);
            
        }
        
        [PunRPC] 
        void DeactivateTarget()
        {
            // Check if bullet was shot by the player that can hit this target and deactivate target if it can 
            targetActive = false; 
            gameObject.SetActive(false);
        }
    }
}