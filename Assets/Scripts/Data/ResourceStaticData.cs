using System;
using Ubavar.core.Data.Static;
using UnityEngine;

namespace Ubavar.core.Data.Static
{
    [CreateAssetMenu(fileName = "ResourceStaticData", menuName = "Game/CreateResource", order = 1)]
    [Serializable]
    public class ResourceStaticData : BaseStaticData
    {
        public override string GetPrefabSubFolderName()
        {
            return "Resources";
        }
    }
}