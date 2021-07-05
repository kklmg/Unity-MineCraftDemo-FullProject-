using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.NWorld
{
    [RequireComponent(typeof(Light))]
    class Sun : MonoBehaviour
    {
        public int MinutesInDay = 20;

        private Light m_Light;

        [SerializeField]
        private float m_RAngelPerSecond;



        private void Awake()
        {
            m_Light = GetComponent<Light>();
            m_RAngelPerSecond = 360.0f / MinutesInDay / 60.0f ;
        }

        private void Update()
        {
            transform.Rotate(Vector3.right, m_RAngelPerSecond * Time.deltaTime);
        }

    }
}
