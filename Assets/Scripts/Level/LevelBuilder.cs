using System.Collections.Generic;
using Ubavar.game.Data;
using UnityEngine;

namespace Ubavar.game.Level
{
    public class LevelBuilder : MonoBehaviour
    {
        public float FloorYGap = 10;

        private List<Grid> grids = new List<Grid>();
        private LevelConfig config;

        public List<Grid> Generate(LevelConfig levelConfig, AreaConfig areaConfig)
        {
            GenerateGround(areaConfig);
            RedrawWater(areaConfig);

            config = levelConfig;

            for (int i = 0; i < config.FormConfigs.Count; i++)
            {
                GameObject floor = new GameObject("Floor_" + i.ToString());
                floor.transform.parent = gameObject.transform;
                floor.transform.localPosition = new Vector3(0, -FloorYGap * i, 0);

                FloorConfig floorConfig = new FloorConfig();
                floorConfig.Destroyable = (i != 0);
                floorConfig.FormConfig = config.FormConfigs[i];
                floorConfig.ColorConfig = config.ColorConfigs[i];


                Grid grid = floor.AddComponent<Grid>();
                grid.HexPrefab = LevelSettings.Instance.GetHexPrefab();
                grid.Config = floorConfig;
                grid.Generate();

                grids.Add(grid);
            }

            return grids;
        }

        private void RedrawWater(AreaConfig areaConfig)
        {
            core.Utils.GameObjectUtils.FindDescendentTransformByPathOnRootTransform("Water").GetComponent<Renderer>().sharedMaterial = areaConfig.WaterMaterial;
        }

        private void GenerateGround(AreaConfig areaConfig)
        {
            Instantiate(areaConfig.DecorPrefab);
        }

        public void StopDestroy()
        {
            foreach (Grid grid in grids)
            {
                grid.StopDestroy();
            }
        }
    }
}