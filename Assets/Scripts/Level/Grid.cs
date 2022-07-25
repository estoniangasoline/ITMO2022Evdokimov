using System.Collections.Generic;
using Ubavar.game.Data;
using Ubavar.game.HexUtils;
using UnityEngine;

namespace Ubavar.game.Level
{
    public class Grid : MonoBehaviour
    {
        public GameObject HexPrefab;
        public FloorConfig Config;

        private LevelSettings modSettings;
        private List<GameObject> cells = new List<GameObject>();
        private List<Vector3> busyPosition;

        public void Generate()
        {
            busyPosition = new List<Vector3>();

            if (modSettings == null)
                modSettings = LevelSettings.Instance;

            foreach (Vector2 position in Config.FormConfig.Positions)
            {
                Hex hexPosition = new Hex(position.x, position.y);
                CreateCell(hexPosition);
            }
        }

        public void StartDestroy(bool immediate = false)
        {
            foreach (GameObject cell in cells)
            {
                cell.GetComponent<Cell>().StartDestroy(immediate);
            }
        }

        public void StopDestroy()
        {
            foreach (GameObject cell in cells)
            {
                if (cell != null)
                {
                    cell.GetComponent<Cell>().StopDestroy();
                }
            }
        }

        public Vector3 GetRandomFreePosition()
        {
            Vector3 randomPosition;
            int index;

            do
            {
                index = Random.Range(0, cells.Count);
                randomPosition = cells[index].transform.position;
            } while (busyPosition.Contains(randomPosition));


            busyPosition.Add(randomPosition);
            return randomPosition;
        }

        private void CreateCell(Hex hexPosition)
        {
            Vector3 newPosition = hexPosition.ToWorld(0f);
            newPosition.y = transform.position.y;

            GameObject cell = Instantiate(HexPrefab, transform);
            cell.transform.position = newPosition;

            cell.name = newPosition.x + "_" + newPosition.y;

            Cell currentCell = cell.GetComponent<Cell>();

            Material material = Config.ColorConfig.Materials[Random.Range(0, Config.ColorConfig.Materials.Count)];

            currentCell.SetMaterial(material);

            currentCell.Destroyable = Config.Destroyable;

            cells.Add(cell);
        }
    }
}