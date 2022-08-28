using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Script.Board
{/// <summary>
 /// Forming a column from cards
 /// </summary>
    public sealed class Row : MonoBehaviour
    {
        private const float Duration = 0.3f;
        [SerializeField] private List<CardHandler> cards;
        [SerializeField] private int rowNum;

        public int Num => rowNum;

        public bool EndSetup { get; private set; }

        private void Start()
        {
            Board.Instance.AddRow(this);
        }
        
        public async void StartSwap(CardHandler currentCard)
        {
       
            var index =  cards.IndexOf(currentCard);
            var sequence = DOTween.Sequence();
        
            var oldPos = currentCard.ImageCard;
            for (var i = 0; i < cards.Count; i++)
            {
                if (i <= index) continue;
                var newPos = cards[i].ImageCard;
                sequence.Join(newPos.transform.DOMove(oldPos.transform.position, Duration));
                oldPos = newPos;
            }
            await sequence.Play().AsyncWaitForCompletion();
        }

        public void SetCard(CardHandler cardHandler, CardData data)
        {
            if (data == null)
            {
                cardHandler.SetPositionOnList(cards.IndexOf(cardHandler), rowNum);
            }
            else
            {
                cardHandler.SetupLoadData(data);
            }
            cards.Add(cardHandler);
            EndSetup = true;
        }
        public void ResetCards()
        {
            foreach (var card in cards)
            {
                card.Reset();
            }
        }
    }
}
