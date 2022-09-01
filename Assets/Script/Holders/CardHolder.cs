using System.Collections.Generic;
using Script.Board;
using UnityEngine;

namespace Script.Holders
{
    public sealed class CardHolder : MonoBehaviour
    {
        private static CardHolder _instance;

        private CardHolder()
        {
        }

        public List<CardHandler> CardPrefab { get; private set; }

        private void Awake()
        {
            if (_instance != null) Destroy(this);
            DontDestroyOnLoad(this);
        }

        public static CardHolder GetInstance()
        {
            if (_instance == null) _instance = new GameObject().AddComponent<CardHolder>();
            return _instance;
        }

        public void AddCardPrefab(CardHandler prefab)
        {
            if (CardPrefab == null) CardPrefab = new List<CardHandler>();
            CardPrefab.Add(prefab);
        }
    }
}