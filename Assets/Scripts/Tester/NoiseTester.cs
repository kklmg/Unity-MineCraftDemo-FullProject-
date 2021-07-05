using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.NNoise;

namespace Tester
{
    class NoiseTester : MonoBehaviour
    {
        public GameObject TargetPrefab;

        private PerlinNoiseMaker m_NoiseMaker;
        private MeshRenderer m_MeshRenderer;

        public int m_Amplitude = 128;
        public float m_Frequency = 1.0f;
        public int m_Octaves = 6;

        public List<float> result1;
        public List<float> result2;

        private void Awake()
        {
            m_NoiseMaker = new PerlinNoiseMaker(9876543);
            result1 = new List<float>();
            result2 = new List<float>();
        }

        private void Start()
        {
            m_MeshRenderer = gameObject.GetComponent<MeshRenderer>();

            //getTexture();
            test_1D();
            //test_2D();
        }

        void test_1D()
        {
            for (float x = 10.0f; x < 20f; x += 0.1f)
            {
                //float y1 = Mathf.PerlinNoise(x + 0.01f, 0.9f);
                //float y2 = Mathf.PerlinNoise(x + 0.001f, 0.2f);

                float y1 = m_NoiseMaker.MakeOctave_2D(new Vector2(x+0.01f, 0.1f),0.8f,20,8)*20;
                //float y2 = m_NoiseMaker.GetNoise_2D(new Vector2(x + 0.01f, 0.1f), 0.8f, 20);
                //float y1 = m_NoiseMaker.GetOctaveNoise_2D(new Vector2(x, 0.9f), 1.0f, 128.0f, 4);
                //float y2 = m_NoiseMaker.GetNoise_2D(new Vector2(x, 0.2f));

                result1.Add(y1);
                //result2.Add(y2);

                Instantiate(TargetPrefab, new Vector3(x*10, y1*10, 1.0f),Quaternion.Euler(0,180,0));
                //Instantiate(targetprefab, new Vector3(x * 10, y2 * 10, 5.0f), Quaternion.Euler(0, 180, 0));
            }

        }
        void getTexture()
        {

            Texture2D Tex;
            int width = 255;
            int height = 255;
            Tex = new Texture2D(width, height);
            int i, j;
            int blackcount = 0;
            for (i = 0; i < width; i++)
            {
                for (j = 0; j < height; j++)
                {
                    //float k = Mathf.PerlinNoise((float)i / (float)width, (float)j / (float)height);
                    //float k = m_NoiseMaker.GetNoise_2D(new Vector2(i, j), 1.0f/255, 1.0f);
                    //float k = m_NoiseMaker.GetOctaveNoise_2D(new Vector2(i, j), 1.0f / 255, 128.0f, 8);
                    //Tex.SetPixel(i, j, Color.red*k);
                    //Debug.Log(Color.white * k);
                }
            }
            Tex.Apply();
            Debug.Log(blackcount);
            m_MeshRenderer.materials = new Material[1];
            m_MeshRenderer.materials[0] = new Material(Shader.Find("Unlit/Texture"));
            if (m_MeshRenderer.materials[0] == null)
            {
                Debug.Log("Can't find Shader");
            }
            m_MeshRenderer.materials[0].mainTexture = Tex;


        }



        void test_2D()
        {
            for (float x = 0.0f; x < 1.0f; x += 0.05f)
            {
                for (float z = 0.0f; z < 1.0f; z += 0.05f)
                {
                    //float y = Mathf.PerlinNoise(x, z);
                    //float y = m_NoiseMaker.GetNoise_2D(new Vector2(x* m_Frequency, z));
                    float y = m_NoiseMaker.MakeOctave_2D(new Vector2(x ,z),m_Frequency,1.0f,6);
                    Instantiate(TargetPrefab, new Vector3(x * m_Frequency, y * m_Frequency, z * m_Frequency), Quaternion.identity);
                }

            }

        }
    }
}
