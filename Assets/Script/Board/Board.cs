using System.Collections.Generic;
using System.Linq;
using Script.Holders;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Board
{
    /// <summary>
    ///     Responsible for the formation and reset of the board
    /// </summary>
    [RequireComponent(typeof(ContentSizeFitter))]
    public class Board : MonoBehaviour
    {
        [SerializeField] private int countCardInRow = 6;
        [SerializeField] private List<Row> rows;

        private ContentSizeFitter _contentSize;
        private MusicHandler _musicHandler;
        private SaveLoadSceneData _saveLoadSceneData;

        private Board()
        {
        }

        public int CountCardInRow => countCardInRow;

        private void FindHandlers()
        {
            if (_musicHandler == null) _musicHandler = FindObjectOfType<MusicHandler>();
            if (_saveLoadSceneData == null) _saveLoadSceneData = FindObjectOfType<SaveLoadSceneData>();
        }

        public void AddRow(Row row)
        {
            rows.Add(row);
            var currentCards = GetSaveData(row.Num);

            if (currentCards.Count != 0)
                CreateCardFromSaveData(currentCards, row);
            else
                CreateCardRandomly(row);
        }

        private List<CardData> GetSaveData(int rowNum)
        {
            List<CardData> cardData = new();
            FindHandlers();
            var sceneData = _saveLoadSceneData.GetSaveData();

            if (sceneData != null) cardData = sceneData.cardData.ToList();

            var currentCards = cardData.Where(card => card.rowNum == rowNum).ToList();

            currentCards.Sort((x, y) => x.indexInList.CompareTo(y.indexInList));

            return currentCards;
        }

        private void CreateCardFromSaveData(List<CardData> currentCards, Row row)
        {
            for (var i = 0; i < CountCardInRow; i++)
            {
                CardHandler cardPrefab = null;
                foreach (var card in CardHolder.GetInstance().CardPrefab.Where(card =>
                             currentCards[i].cardImageName == card.GetComponent<Image>().sprite.name))
                    cardPrefab = card;

                var cardHandler = Instantiate(cardPrefab, row.transform);
                cardHandler.Init(_saveLoadSceneData, _musicHandler);
                row.SetCard(cardHandler, currentCards[i]);
            }
        }

        private void CreateCardRandomly(Row row)
        {
            for (var i = 0; i < CountCardInRow; i++)
            {
                var index = Random.Range(0, CardHolder.GetInstance().CardPrefab.Count);
                var card = Instantiate(CardHolder.GetInstance().CardPrefab[index], row.transform);
                card.Init(_saveLoadSceneData, _musicHandler);
                row.SetCard(card, null);
            }
        }

        public void ResetAll()
        {
            if (_contentSize == null) _contentSize = GetComponent<ContentSizeFitter>();
            _contentSize.enabled = false;
            foreach (var row in rows) row.ResetCards();
            _contentSize.enabled = true;
        }
    }
}