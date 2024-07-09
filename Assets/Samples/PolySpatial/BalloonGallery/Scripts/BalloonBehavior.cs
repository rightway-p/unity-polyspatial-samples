using System.Collections;
using UnityEngine;

namespace PolySpatial.Samples
{
    public class BalloonBehavior : MonoBehaviour
    {
        [SerializeField]
        Material m_WhiteMaterial;

        [SerializeField]
        Material m_BlueMaterial;

        [SerializeField]
        Material m_GoldMaterial;

        [SerializeField]
        GameObject m_BlueParticlePrefab;

        [SerializeField]
        GameObject m_WhiteParticlePrefab;

        [SerializeField]
        GameObject m_GoldParticlePrefab;

        [SerializeField]
        Animator m_Animator;

        [SerializeField]
        float m_Speed = 0.1f;

        [SerializeField]
        float m_WindIntensity = 0.1f;
        
        [SerializeField]
        float m_WindFrequency = 1.0f;

        private bool isFail = false;

        public int ScoreValue => m_ScoreValue;

        MeshRenderer m_MeshRenderer;
        int m_ScoreValue;
        GameObject m_ParticlePrefab;

        public BalloonGalleryManager m_Manager;
        bool m_Popped;

        const int k_BlueScore = 1;
        const int k_WhiteScore = 1;
        const int k_GoldScore = 10;
        const float k_ParticleOffset = 0.05f;
        const float k_PopDelay = 0.12f;
        const string k_PopAnimTrigger = "Pop";

        void Start()
        {
            m_MeshRenderer = GetComponent<MeshRenderer>();

            var balloonType = Random.Range(0, 100);

            // blue
            if (balloonType < 49)
            {
                SetBalloonValues(m_BlueMaterial, k_BlueScore, m_BlueParticlePrefab);
                m_Manager.GoodBalloonAdded();
            }

            // white
            else if (balloonType > 49 && balloonType < 98)
            {
                SetBalloonValues(m_WhiteMaterial, k_WhiteScore, m_WhiteParticlePrefab);
            }

            // gold
            else
            {
                SetBalloonValues(m_GoldMaterial, k_GoldScore, m_GoldParticlePrefab);
                m_Manager.GoodBalloonAdded();
            }
        }
        
        void OnEnable() 
        {
            Debug.Log("OnEnabled");
            StartCoroutine(MoveUpwards());
        }

        IEnumerator MoveUpwards()
        {
            float time = 0f;
            Vector3 initialPosition = transform.position;

            while (true)
            {
                float x = (Mathf.PerlinNoise(time * m_WindFrequency, 0.0f) * 2 - 1) * m_WindIntensity;
                float z = (Mathf.PerlinNoise(0.0f, time * m_WindFrequency) * 2 - 1) * m_WindIntensity;
                Vector3 windEffect = new Vector3(x, 0, z);

                transform.position = initialPosition + Vector3.up * m_Speed * time + windEffect;

                time += Time.deltaTime;
                yield return null;
            }
        }

        void Update() 
        {
            if (transform.position.y >= 0.5f)
            {
                isFail = true;
                this.Pop();
            }

        }

        void SetBalloonValues(Material mat, int score, GameObject particlePrefab)
        {
            m_MeshRenderer.material = mat;
            m_ScoreValue = score;
            m_ParticlePrefab = particlePrefab;
        }

        public void Pop()
        {
            if (!m_Popped)
            {
                StartCoroutine(PopSequence());
            }
        }

        IEnumerator PopSequence()
        {
            m_Popped = true;

            if (m_Animator != null)
            {
                m_Animator.SetTrigger(k_PopAnimTrigger);
            }
            yield return new WaitForSeconds(k_PopDelay);
            Instantiate(m_ParticlePrefab, transform.position + new Vector3(0, k_ParticleOffset, 0), Quaternion.identity);
            if (!isFail)
                m_Manager.BalloonPopped(m_ScoreValue);
            

            Destroy(this.gameObject);
        }
    }
}
