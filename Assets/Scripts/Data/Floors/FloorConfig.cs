using System;
using Ubavar.core.Data.Static;
using UnityEngine;

namespace Ubavar.game.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloorConfig", menuName = "Game/CreateFloorConfig", order = 1)]
    public class FloorConfig : BaseStaticData
    {
        public bool Destroyable = true;
        public FloorFormConfig FormConfig;
        public FloorColorConfig ColorConfig;
    }
}