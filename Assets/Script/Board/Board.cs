using System.Collections.Generic;
using System.Linq;
using Script.Holders;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Board
{
    [RequireComponent(typeof(ContentSizeFitter))]
    public class Board : MonoBehaviour
    {
        [SerializeField] private int countCardInRow = 3;

        [SerializeField] private List<Row> rows;
        public static Board Instance { get; private set; }

        private ContentSizeFitter _contentSize;
       
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        public void AddRow(Row row)
        {
            rows.Add(row);
            List<CardData> cardData = new();
            var sceneData = SaveLoadSceneData.Instance.GetSaveData();
            if (sceneData != null )
            {
                cardData = sceneData.cardData.ToList();
            }

            var currentCards = cardData.Where(card => card.rowNum == row.Num).ToList();
            
            currentCards.Sort((x, y) => x.indexInList.CompareTo(y.indexInList));
            
            if (currentCards.Count != 0)
            {//создаем из сохраненніх данных
                for (int i = 0; i < countCardInRow; i++)
                {
                    CardHandler cardPrefab = null;
                    foreach (var card in CardHolder.Instance.CardPrefab.Where(card =>
                                 currentCards[i].cardImageName == card.GetComponent<Image>().sprite.name))
                    {
                        cardPrefab = card;
                    }
                    row.SetCard(Instantiate(cardPrefab, row.transform),currentCards[i]);
                }
            }
            else
            {//создаем рандомно
                for (int i = 0; i < countCardInRow; i++)
                {
                    var index = Random.Range(0, CardHolder.Instance.CardPrefab.Count);
                    row.SetCard(Instantiate(CardHolder.Instance.CardPrefab[index], row.transform), null);
                }
            }
        }

        public void ResetAll()
        {
            if (_contentSize == null)
                _contentSize = GetComponent<ContentSizeFitter>();
            _contentSize.enabled = false;
            foreach (var row in rows)
            {
                row.ResetCards();
            }
            _contentSize.enabled = true;
        }
    }
}
