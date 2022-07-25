using System.Collections.Generic;
using Ubavar.core.Data.Static;
using Ubavar.game.Data;
using UnityEngine;

namespace Ubavar.game.Level
{
    public class LevelSettings : MonoBehaviour
    {
        public static LevelSettings Instance { get; private set; }

        [Header("Сложность ботов")]
        public BotDifficult Difficult = BotDifficult.Normal;
        [Header("Варианты уровней")]
        [SerializeField] private LevelConfig[] levelConfig;
        [Header("Использовать кровь при смерте")]
        [SerializeField] private bool useBloodEffect = false;
        [SerializeField] private GameObject prefabBloodEffect;

        [Header("Префаб плитки пола")]
        [SerializeField] private GameObject classicHexPrefab;

        [Header("Использовать скин игрока")]
        [SerializeField] private TypeSkinPlayer playerSkin = TypeSkinPlayer.Boy;
        [SerializeField] private ResourcesPlayer[] resourcesPlayer;

        [Header("Окружение")]
        [SerializeField] private AreaConfig[] areaConfigs;

        [Header("Боты")]
        [SerializeField] private Vector2Int botCountRange;
        [SerializeField] private ResourcesBot[] botSkins;
        [SerializeField] private string[] botNames;

        public bool UseBloodEffect => useBloodEffect;
        public GameObject BloodEffect => prefabBloodEffect;
        public int BotCount => Random.Range(botCountRange.x, botCountRange.y + 1);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(transform);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public GameObject GetHexPrefab()
        {
            return classicHexPrefab;
        }

        public ResourceStaticData GetPlayerSkin()
        {
            for (int i = 0; i < resourcesPlayer.Length; i++)
            {
                if (resourcesPlayer[i].skin == playerSkin)
                {
                    return resourcesPlayer[i].data;
                }
            }

            Debug.LogError("Ошибка! Нет ResourceStaticData");
            return null;
        }

        public ResourceStaticData GetRandomBotSkin()
        {
            return botSkins[Random.Range(0, botSkins.Length)].data;
        }

        public string GetRandomName()
        {
            int botNumber = Random.Range(0, botNames.Length);
            return botNames[botNumber];
        }

        public AreaConfig GetRandomArea()
        {
            return areaConfigs[Random.Range(0, areaConfigs.Length)];
        }

        public LevelConfig GetRandomLevelConfig()
        {
            return levelConfig[Random.Range(0, levelConfig.Length)];
        }
    }

    public enum TypeSkinPlayer { Classic, Boy }

    public enum TypeFloorMaterials { Classic = -1 }

    [System.Serializable]
    public class ResourcesPlayer
    {
        public TypeSkinPlayer skin = TypeSkinPlayer.Boy;
        public ResourceStaticData data;
    }

    [System.Serializable]
    public class ResourcesBot
    {
        public ResourceStaticData data;
    }
}