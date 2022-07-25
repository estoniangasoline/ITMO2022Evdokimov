using System;
using System.Collections.Generic;
using Ubavar.core.Data.Static;
using UnityEngine;

namespace Ubavar.game.Data
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloorFormConfig", menuName = "Game/CreateFloorFormConfig", order = 1)]
    public class FloorFormConfig : BaseStaticData
    {
        public List<Vector2> Positions;
    }
}