using Photon.Pun;
using TMPro;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

namespace Controllers
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        //Instance of the game manager
        public static GameManager instance;

        public Transform player1Spawn;
        // Game Object that holds the reference to the player 
        public GameObject player;
        

        public TextMeshProUGUI playerOneScoreText, playerTwoScoreText;
        public static int p1Score, p2Score;
        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            PhotonNetwork.OfflineMode = true;
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                transform.GetChild(2).GetComponent<TargetController>().StartSimulation();
            }
            if (GameObject.FindWithTag("Player")) return;
            
            // Instantiate(player,player1Spawn.position,Quaternion.identity);
        }

        void Update()
        {
            
        }

        /// Close game
        public void Exit()
        {
            Application.Quit();
        }

        void SetScoreText()
        {
            playerOneScoreText.text = p1Score.ToString();
            playerTwoScoreText.text = p2Score.ToString();
        }
    }
}