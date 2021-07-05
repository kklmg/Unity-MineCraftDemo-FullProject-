using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading;

using UnityEngine;

using Assets.Scripts.NNoise;
using Assets.Scripts.NData;
using Assets.Scripts.NMesh;
using Assets.Scripts.NWorld;
using System;

namespace Assets.Scripts.Tester
{
    class TestScript : MonoBehaviour
    {
        INoiseMaker noiseMaker;
        IHashMaker hashMaker;

        public string seed = "123456";
        public float m_Frequency = 0.02f;

        public int m_Width = 100;
        public int m_Height = 100;

        public int m_Ocatave = 8;

        public Vector2 Offset = Vector2.zero;

        public Texture2D m_tex;

        public Renderer m_renderer;

        private void Start()
        {
            MakeTexture();
        }

        public void MakeTexture()
        {
            hashMaker = new HashMakerBase(seed);
            noiseMaker = new PerlinNoiseMaker(hashMaker);

            m_tex = new Texture2D(m_Width, m_Height);

            float noise;
            for (int i = 0; i < m_Width; ++i)
            {
                for (int j = 0; j < m_Height; ++j)
                {

                    noise = noiseMaker.MakeOctave_2D(new Vector2(i + Offset.x, j + Offset.y), m_Frequency, 1,m_Ocatave);

                    Debug.Assert(noise > 0 && noise < 1);

                    if (noise > 0 && noise < 0.2)
                    {
                        m_tex.SetPixel(i, j, Color.yellow);
                    }
                    else if (noise > 0.2 && noise < 0.4)
                    {
                        m_tex.SetPixel(i, j, Color.blue);
                    }
                    else if (noise > 0.4 && noise < 0.6)
                    {
                        m_tex.SetPixel(i, j, Color.green);
                    }
                    else if (noise > 0.6 && noise < 0.8)
                    {
                        m_tex.SetPixel(i, j, Color.yellow);
                    }
                    else if (noise > 0.8 && noise < 0.99)
                    {
                        m_tex.SetPixel(i, j, Color.white);
                    }
                    else
                    {
                        m_tex.SetPixel(i, j, Color.yellow);
                    }

                }
            }
            //m_tex.SetPixel(0, 0, Color.black);

            Debug.Log(m_tex.GetPixel(0, 0));
            m_tex.Apply();
            m_renderer.material.mainTexture = m_tex;
        }


    }
}







