using System;
using TMPro;
using UnityEngine;

namespace Controllers
{
    public class ScoreManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public int p1Score;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void UpdateScore(int value)
        {
            p1Score += value;
            SetScoreText();
        }


        public void ResetScore()
        {
            p1Score = 0;
            SetScoreText();
        }

        public void SetScoreText()
        {
            scoreText.text = p1Score.ToString();
        }
    }
}
