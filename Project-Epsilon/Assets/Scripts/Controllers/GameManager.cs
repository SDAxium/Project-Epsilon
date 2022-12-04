using Photon.Pun;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        //Instance of the game manager
        public GameManager instance;

        public Transform player1Spawn;
        // Game Object that holds the reference to the player 
        public GameObject player;

        // Scene Travel allows Unity to move to the next scene. This will be used in conjunction with the scene transitions
        public bool sceneTravel;
    
        // Holds the next scene to be loaded in the background
        private AsyncOperation _scene;
        private void Awake()
        {
            
        }

        void Start()
        {
            PhotonNetwork.OfflineMode = true;
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                transform.GetChild(2).GetComponent<TargetController>().StartSimulation();
            }
            if (GameObject.FindWithTag("Player")) return;
            
            Instantiate(player,player1Spawn.position,Quaternion.identity);
        }

        void Update()
        {
            if (sceneTravel)
            {
                _scene.allowSceneActivation = true;
            }
        }
    
        /*
        * Loads scene in background depending on what scene player is currently in
        *
        * This method is not currently in use. Probably will be used again in the future
        */ 
        void LoadBackgroundScene()
        {
            if (SceneManager.GetActiveScene().name.Equals("Main Menu"))
            {
                _scene = SceneManager.LoadSceneAsync(1);
            }
            if (SceneManager.GetActiveScene().name.Equals("SampleScene"))
            {
                _scene = SceneManager.LoadSceneAsync(0);
            }
            _scene.allowSceneActivation = false;
        }
    
        // Opens scene at build index
        public void OpenSceneAt(int buildIndex)
        {
            SceneManager.LoadScene(buildIndex);
        }
    
        /*
     * There's probably a more efficient way to do this
     * Sets scene travel bool to true, moving player to the next preloaded scene
     */
        public void LoadNextScene()
        {
            sceneTravel = true;
        }
    
        // Close game
        public void Exit()
        {
            Application.Quit();
        }
    }
}