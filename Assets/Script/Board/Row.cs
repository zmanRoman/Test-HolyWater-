using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Board
{
    public sealed class Row : MonoBehaviour
    {
        private const float QuickDuration = 0.01f;

        [SerializeField] private List<CardHandler> cards;
        [SerializeField] private int rowNum;
        private Board _board;
        private int _cardsEndSetup;
        public int Num => rowNum;

        public bool EndSetup { get; private set; }

        private void Start()
        {
            _board = FindObjectOfType<Board>();
            _board.AddRow(this);
        }

        public void CardEndSetup()
        {
            _cardsEndSetup++;
            if (_cardsEndSetup == _board.CountCardInRow) StartCoroutine(QuickSwap());
        }

        private IEnumerator QuickSwap()
        {
            foreach (var card in cards)
            {
                Swap(card, QuickDuration);
                yield return new WaitForSeconds(QuickDuration);
            }
        }

        public async void Swap(CardHandler currentCard, float duration)
        {
            var index = currentCard.Data.indexInList;
            var sequence = DOTween.Sequence();
            Image oldPos = null;
            for (var i = index; i < cards.Count; i++)
            {
                if (oldPos)
                {
                    var newPos = cards[i].ImageCard;
                    sequence.Join(newPos.transform.DOMove(oldPos.transform.position, duration));
                    oldPos = newPos;
                }

                if (Convert.ToBoolean(cards[i].Data.isDestroy)) oldPos = cards[i].ImageCard;
            }

            await sequence.Play().AsyncWaitForCompletion();
        }

        public void SetCard(CardHandler cardHandler, CardData data)
        {
            cards.Add(cardHandler);
            if (data == null)
                cardHandler.SetPositionOnList(cards.IndexOf(cardHandler), rowNum);
            else
                cardHandler.SetupLoadData(data);
            EndSetup = true;
        }

        public void ResetCards()
        {
            foreach (var card in cards) card.Reset();
        }
    }
}