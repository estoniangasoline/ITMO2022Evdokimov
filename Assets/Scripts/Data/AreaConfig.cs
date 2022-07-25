using System;
using System.Collections.Generic;
using Ubavar.core.Data.Static;
using UnityEngine;

namespace Ubavar.game.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "AreaConfig", menuName = "Game/CreateAreaConfig", order = 1)]
    public class AreaConfig : BaseStaticData
    {
        public GameObject DecorPrefab;
        public Material WaterMaterial;
    }
}