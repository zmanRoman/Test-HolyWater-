using System.Collections.Generic;
using Script.Board;
using UnityEngine;

namespace Script.Holders
{/// <summary>
 /// contains all generated cards
 /// </summary>
    public sealed class CardHolder : MonoBehaviour
    {
        public List<CardHandler> CardPrefab{ get; private set; }
    
        public static  CardHolder Instance { get; private set; }
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

            DontDestroyOnLoad(this);
        }
        public void AddCardPrefab(CardHandler prefab)
        {
            CardPrefab ??= new List<CardHandler>();
            CardPrefab.Add(prefab);
        }
    
    }
}
