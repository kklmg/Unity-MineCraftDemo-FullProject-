using System;
using System.IO;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using Assets.Scripts.NWorld;
using Assets.Scripts.NData;

namespace Assets.Scripts.NGameSystem
{
    [System.Serializable]
    public class GameSetting
    {
        public RangedInt PlayerView;
        public RangedInt PlayerSpeed;
        public RangedFloat JumpForce;
        public RangedFloat RotateSensitivity;
        public RangedInt PickingDistance;

        public GameSetting()
        {
            PlayerView = new RangedInt(1, 5, 3);
            PlayerSpeed = new RangedInt(1, 9, 4);
            JumpForce = new RangedFloat(0.5f, 5.0f, 0.5f);
            RotateSensitivity = new RangedFloat(0.2f, 5.0f, 1.0f);
            PickingDistance = new RangedInt(12, 32, 16);
        }

    }
}
