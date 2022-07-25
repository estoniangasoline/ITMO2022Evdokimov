using System;
using UnityEngine;

namespace Ubavar.core.Data.Static
{
    [Serializable]
    public class BaseStaticData : ScriptableObject
    {
        [Header("Base")] 
        public string Ident;
        public string Type;

        public string NameKey
        {
            get { return GetPrefabSubFolderName() + "." + Ident + ".Name"; }
        }

        public GameObject Prefab
        {
            get { return Resources.Load<GameObject>(Path); }
        }

        public virtual string Path
        {
            get { return "Prefabs/" + GetPrefabSubFolderName() + "/" + Ident; }
        }

        public Sprite Icon
        {
            get
            {
                string iconPath = "Icons/" + GetPrefabSubFolderName() + "/" + Ident;
                return Resources.Load<Sprite>(iconPath);
            }
        }

        public virtual string GetPrefabSubFolderName()
        {
            return Type;
        }
    }
}