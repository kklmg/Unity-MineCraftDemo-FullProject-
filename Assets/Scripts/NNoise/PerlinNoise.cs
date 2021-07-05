using UnityEngine;

using Assets.Scripts.NCache;


namespace Assets.Scripts.NNoise
{
    //delegate fade function
    public delegate float del_FadeFunction(float t);

    //defined fade functions
    public static class FadeFunction
    {
        public static del_FadeFunction[] Version;

        static FadeFunction()
        {
            Version = new del_FadeFunction[2];

            //Vesion 0 : 3t^2 − 2t^3
            Version[0] = (float t) => { return t * t * (3 - 2 * t); };

            //Vesion 1 : 6t^5 - 15t^4 + 10t^3
            Version[1] = (float t) => { return t * t * t * (t * (t * 6 - 15) + 10); };
        }
    }



    /// <summary>
    /// class perlin noise maker
    /// </summary>
    public class PerlinNoiseMaker : INoiseMaker
    {
        //Fileld
        const float NOISE_OFFSET = 0.001f;
        private IHashMaker m_HashMaker;                    //Hash Maker

        //Caches
        Vector2[] m_VerticiesCache = new Vector2[4];

        float m_Weight_LT;
        float m_Weight_RT;
        float m_Weight_LB;
        float m_Weight_RB;

        float m_lerpAbove;
        float m_lerpBottom;

        //Delegate----------------------------------------------------------------
        private del_FadeFunction m_delFadeFunction;       //Fade Function
      
        //Property----------------------------------------------------------------
        public del_FadeFunction FadeFun {set { m_delFadeFunction = value; } }


        //Constructor----------------------------------------------------------------
        public PerlinNoiseMaker(uint _seed, byte version = 0)
        {
            //set default fade function
            m_delFadeFunction = new del_FadeFunction(FadeFunction.Version[version]);

            //Init Hash Maker
            m_HashMaker = new HashMakerBase(_seed);
        }

        public PerlinNoiseMaker(IHashMaker hashMaker, byte version = 0)
        {
            //set default fade function
            m_delFadeFunction = new del_FadeFunction(FadeFunction.Version[version]);

            //Init Hash Maker
            m_HashMaker = hashMaker;
        }




        //Public Function---------------------------------------------------------------
        /// <summary>
        /// get a 2D perlin noise 
        /// </summary>
        /// <param name="point">Arbitraty Vector2</param>
        /// <returns> Noise (float value between -1 and 1) </returns>
        public float Make_2D(Vector2 point, bool GetPositiveRes = true)
        {
            //for avoid integer input
            point.x += NOISE_OFFSET;
            point.y += NOISE_OFFSET;

            //Debug.Log("offset " + NOISE_OFFSET);

            //Debug.Log("point x" + point.x);
            //Debug.Log("point y" + point.y);
            //Debug.Log("point " + point);
            //       _______________
            //      |               |
            //      |  point        |
            //      |    *          |
            //      |               |
            //      |_______________|
            //    p_int

            //keep interger
            Vector2 p_int;

            p_int.x = point.x >= 0 ? (int)point.x : (int)point.x - 1;
            p_int.y = point.y >= 0 ? (int)point.y : (int)point.y - 1;


            //offset between  p_int and point: (0.0f,0.0f) ~ (1.0f,1.0f)
            Vector2 offset = point - p_int;

            //vector positon to Lattice  surrounded the point
            m_VerticiesCache[0] = new Vector2(p_int.x, p_int.y + 1);      //left_top
            m_VerticiesCache[1] = new Vector2(p_int.x + 1, p_int.y + 1);  //right_top
            m_VerticiesCache[2] = new Vector2(p_int.x, p_int.y);          //left_bottom
            m_VerticiesCache[3] = new Vector2(p_int.x + 1, p_int.y);      //right_bottom


            // weight : product of random gradient and vector(position -> lattice)
            m_Weight_LT = _Grad_2D(m_HashMaker.GetHash_2D(m_VerticiesCache[0]), offset - new Vector2(0.0f, 1.0f));
            m_Weight_RT = _Grad_2D(m_HashMaker.GetHash_2D(m_VerticiesCache[1]), offset - new Vector2(1.0f, 1.0f));
            m_Weight_LB = _Grad_2D(m_HashMaker.GetHash_2D(m_VerticiesCache[2]), offset /*- new Vector2(0.0f, 0.0f)*/);
            m_Weight_RB = _Grad_2D(m_HashMaker.GetHash_2D(m_VerticiesCache[3]), offset - new Vector2(1.0f, 0.0f));           

            //learp
            m_lerpAbove = _FadeLerp(m_Weight_LT, m_Weight_RT, offset.x);

            m_lerpBottom = _FadeLerp(m_Weight_LB, m_Weight_RB, offset.x);
            

            return GetPositiveRes == true ?  
                (_FadeLerp(m_lerpBottom, m_lerpAbove, offset.y)+1)/2.0f  //result Range: (0,1)
                :_FadeLerp(m_lerpBottom, m_lerpAbove, offset.y);         //result Range: (-1,1)


        }
        public float Make_2D(Vector2 point, float frequency, float amplitude = 1.0f, bool GetPositiveRes = true)
        {
            return Make_2D(new Vector2(point.x * frequency, point.y * frequency), GetPositiveRes) * amplitude;
        }

        public float MakeOctave_2D(Vector2 point, float frequency, float amplitude, int octave = 8, bool GetPositiveRes = true)
        {
            float res = 0;
            float maxValue = 0;  // sum of all amplitude (possible max amplitude)
            float scale = amplitude;

            for (int i = 0; i < octave; i++)
            {
                res += Make_2D(point,frequency,amplitude, GetPositiveRes);

                maxValue += amplitude;

                amplitude /= 2;
                frequency *= 2;
            }

            return res / maxValue*scale;
        }


        public float Make_3D(Vector3 point, bool GetPositiveRes = true)
        {
            throw new System.NotImplementedException();
        }


        //Private Function----------------------------------------------------------------

        //get weight (product of random gradient and the vector)
        private float _Grad_2D(int hash, Vector2 p)
        {   
            switch ((hash & 3))      //get fandom gradient
            {
                case 0x0: return  p.x + p.y;     //gradient(1,1)   * p => p.x *  1 + p.y *  1 
                case 0x1: return -p.x + p.y;    //gradient(-1,1)  * p => p.x * -1 + p.y *  1
                case 0x2: return  p.x - p.y;     //gradient(1,-1)  * p => p.x *  1 + p.y * -1
                case 0x3: return -p.x - p.y;    //gradient(-1,-1) * p => p.x * -1 + p.y * -1
                default: return 0;
            }
        }

        private float _Grad_3D(int hash, Vector3 p)
        {
            switch (hash & 0xF)    //get fandom gradient          
            {
                case 0x0: return  p.x + p.y;
                case 0x1: return -p.x + p.y;
                case 0x2: return  p.x - p.y;
                case 0x3: return -p.x - p.y;
                case 0x4: return  p.x + p.z;
                case 0x5: return -p.x + p.z;
                case 0x6: return  p.x - p.z;
                case 0x7: return -p.x - p.z;
                case 0x8: return  p.y + p.z;
                case 0x9: return -p.y + p.z;
                case 0xA: return  p.y - p.z;
                case 0xB: return -p.y - p.z;
                case 0xC: return  p.y + p.x;
                case 0xD: return -p.y + p.z;
                case 0xE: return  p.y - p.x;
                case 0xF: return -p.y - p.z;
                default: return 0; 
            }        
        }

        private float _FadeLerp(float left, float right, float t)
        {
            //case: no fade function setted, use liner lerp 
            if (m_delFadeFunction == null)
                return Mathf.Lerp(left, right, t);

            return Mathf.Lerp(left, right, m_delFadeFunction(t));
        }
    }
}
