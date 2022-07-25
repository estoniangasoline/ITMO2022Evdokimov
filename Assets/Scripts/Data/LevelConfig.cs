using System;
using System.Collections.Generic;
using Ubavar.core.Data.Static;
using UnityEngine;

namespace Ubavar.game.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/CreateLevelConfig", order = 1)]
    public class LevelConfig : BaseStaticData
    {
        public List<FloorFormConfig> FormConfigs;
        public List<FloorColorConfig> ColorConfigs;
    }
}