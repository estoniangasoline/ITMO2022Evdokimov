using System;
using System.Collections.Generic;
using Ubavar.core.Data.Static;
using UnityEngine;

namespace Ubavar.game.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloorColorConfig", menuName = "Game/CreateFloorColorConfig", order = 1)]
    public class FloorColorConfig : BaseStaticData
    {
        public List<Material> Materials;
    }
}