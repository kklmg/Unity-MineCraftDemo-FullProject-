using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assets.Scripts.NNoise;
using Assets.Scripts.NGlobal.ServiceLocator;

using UnityEngine;

namespace Assets.Scripts.NGameSystem
{
    class GameUtility : MonoBehaviour
    {
        public INoiseMaker NoiseMaker {private set; get; }
        public IHashMaker HashMaker { private set; get; }

        public void Init(string Seed)
        {
            //Init Hash Maker
            HashMaker = new HashMakerBase(Seed);
            //Init Noise Maker
            NoiseMaker = new PerlinNoiseMaker(HashMaker);

            Locator<IHashMaker>.ProvideService(HashMaker);
            Locator<INoiseMaker>.ProvideService(NoiseMaker);
        }

    }
}
