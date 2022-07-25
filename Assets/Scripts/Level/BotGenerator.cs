using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ubavar.game.Level
{
    public class BotGenerator : MonoBehaviour
    {
        [SerializeField] private Bot bot;

        private List<Bot> generatedBots;

        public event UnityAction<int> BotCountChanged;

        public List<Bot> Generate(Grid grid)
        {
            generatedBots = new List<Bot>();

            for (int i = 0; i < LevelSettings.Instance.BotCount; i++)
            {
                Bot newBot = Instantiate(bot);
                Character newBotCharacter = newBot.GetComponent<Character>();
                newBotCharacter.transform.position = grid.GetRandomFreePosition();
                newBotCharacter.Name = LevelSettings.Instance.GetRandomName();
                newBotCharacter.Skin = LevelSettings.Instance.GetRandomBotSkin();
                newBotCharacter.Init();
                newBotCharacter.CharacterDead += OnBotDead;
                newBotCharacter.Moveable = false;
                newBotCharacter.moveDirection = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));

                newBot.Difficult = LevelSettings.Instance.Difficult;

                generatedBots.Add(newBot);
                BotCountChanged?.Invoke(generatedBots.Count);
            }

            return generatedBots;
        }

        private void OnBotDead(Character character)
        {
            character.CharacterDead -= OnBotDead;
            generatedBots.Remove(character.GetComponent<Bot>());
            BotCountChanged?.Invoke(generatedBots.Count);
        }
    }
}