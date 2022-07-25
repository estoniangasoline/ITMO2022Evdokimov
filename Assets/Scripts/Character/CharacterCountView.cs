using TMPro;
using UnityEngine;

namespace Ubavar.game.Level
{
    public class CharacterCountView : MonoBehaviour
    {
        [SerializeField] private TMP_Text countField;
        [SerializeField] private BotGenerator botGenerator;
        [SerializeField] private Character user;

        private void OnEnable()
        {
            botGenerator.BotCountChanged += OnBotCountChanged;
        }

        private void OnDisable()
        {
            botGenerator.BotCountChanged -= OnBotCountChanged;
        }

        private void OnBotCountChanged(int count)
        {
            if (user != null)
                count++;

            countField.text = count.ToString();
        }
    }
}