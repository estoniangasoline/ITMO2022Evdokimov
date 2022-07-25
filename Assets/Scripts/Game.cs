using Cinemachine;
using System.Collections.Generic;
using Ubavar.core.Utils;
using Ubavar.game.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ubavar.game.Level
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private CameraSwitcher cameraSwitcher;
        [SerializeField] private LevelBuilder builder;
        [SerializeField] private BotGenerator botGenerator;
        [SerializeField] private Character user;
        [SerializeField] private ResultWindow results;

        private List<Bot> bots;
        private Grid startGrid;

        private void OnEnable()
        {
            botGenerator.BotCountChanged += OnBotCountChanged;
        }

        private void OnDisable()
        {
            botGenerator.BotCountChanged -= OnBotCountChanged;
        }

        private void Start()
        {
            List<Grid> grids = builder.Generate(LevelSettings.Instance.GetRandomLevelConfig(), LevelSettings.Instance.GetRandomArea());
            startGrid = grids[0];

            PrepareUser(startGrid);
            cameraSwitcher.ActivateGameCamera();
        }

        public void StartLevel()
        {
            bots = botGenerator.Generate(startGrid);

            user.Moveable = true;

            foreach (Bot bot in bots)
            {
                bot.GetComponent<Character>().Moveable = true;
            }

            startGrid.StartDestroy(true);
        }

        private void PrepareUser(Grid grid)
        {
            user.Moveable = false;
            user.Skin = LevelSettings.Instance.GetPlayerSkin();
            user.Name = "Ubavar";
            user.transform.position = grid.GetRandomFreePosition();
            user.gameObject.SetActive(true);
            user.Init();
            user.CharacterDead += OnUserDead;
        }

        private void UpdateCameraTarget()
        {
            DG.Tweening.DOVirtual.DelayedCall(1, () => UpdateGameCameraTargetInternal());
        }

        private void UpdateGameCameraTargetInternal()
        {
            GameObject topBot = null;

            if (user != null)
                topBot = user.gameObject;

            if (topBot == null)
            {
                foreach (Bot bot in bots)
                {
                    if (topBot == null || bot.transform.position.y > topBot.transform.position.y)
                    {
                        topBot = bot.gameObject;
                    }
                }
            }

            if (topBot != null)
                SetCamerasTarget(topBot.transform);
            else
                SetCamerasTarget(null);
        }

        private void SetCamerasTarget(Transform target)
        {
            GameObjectUtils.FindDescendentTransformByPathOnRootTransform("GameVirtualCamera")
     .GetComponent<CinemachineVirtualCamera>().Follow = target;
            GameObjectUtils.FindDescendentTransformByPathOnRootTransform("GameVirtualCamera")
                .GetComponent<CinemachineVirtualCamera>().LookAt = target;

            GameObjectUtils.FindDescendentTransformByPathOnRootTransform("ObserverVirtualCamera")
                .GetComponent<CinemachineVirtualCamera>().Follow = target;
            GameObjectUtils.FindDescendentTransformByPathOnRootTransform("ObserverVirtualCamera")
                .GetComponent<CinemachineVirtualCamera>().LookAt = target;
        }

        private void Lose()
        {
            cameraSwitcher.ActivateObserverCamera();
            results.Lose();
        }

        private void Win()
        {
            builder.StopDestroy();
            user.Moveable = false;
            user.StartDancing();
            cameraSwitcher.ActivateObserverCamera();
            results.Win();
        }

        private void OnUserDead(Character character)
        {
            character.CharacterDead -= OnUserDead;

            if (bots.Count != 0)
            {
                UpdateCameraTarget();
            }

            Lose();
        }

        private void OnBotCountChanged(int count)
        {
            if (count == 0)
            {
                Win();
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}