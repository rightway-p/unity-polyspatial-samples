using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PolySpatial.Samples
{
    public class BalloonGalleryManager : MonoBehaviour
    {
        [SerializeField]
        TMP_Text m_ScoreText;

        [SerializeField]
        GameObject m_WinConfetti;

        [SerializeField]
        public List<GameObject> m_Balloons;

        int m_CurrentScore = 0;
        int m_NumberOfBalloonsPopped;
        int m_NumberOfGoodBalloons;
        const int k_NumberOfBalloons = 36;

        void Awake()
        {
            m_CurrentScore = 0;
            var balloons = FindObjectsOfType<BalloonBehavior>();

            foreach (var balloon in balloons)
            {
                balloon.m_Manager = this;
                m_Balloons.Add(balloon.gameObject);
                balloon.gameObject.SetActive(false);
            }
        }

        void Start() 
        {
            StartCoroutine(EnableBalloons());
        }

        IEnumerator EnableBalloons() 
        {
            foreach (var balloon in m_Balloons)
            {
                balloon.gameObject.SetActive(true);
                yield return new WaitForSeconds(Random.Range(0.6f, 1.5f));
            }
        }

        public void BalloonPopped(int scoreValue)
        {
            m_CurrentScore += scoreValue;
            m_ScoreText.text = m_CurrentScore.ToString();
            // good balloon
            if (scoreValue > 0)
            {
                m_NumberOfBalloonsPopped++;
            }

            // all balloons popped, Show confetti
            if (m_NumberOfBalloonsPopped == k_NumberOfBalloons)
            {
                Instantiate(m_WinConfetti);
            }
        }

        public void GoodBalloonAdded()
        {
            m_NumberOfGoodBalloons++;
        }
    }
}
