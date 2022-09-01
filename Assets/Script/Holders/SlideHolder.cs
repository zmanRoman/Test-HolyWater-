using System.Collections.Generic;
using UnityEngine;

namespace Script.Holders
{
    public sealed class SlideHolder : MonoBehaviour
    {
        private static SlideHolder _instance;

        private SlideHolder()
        {
        }

        public List<RectTransform> itemPrefab { get; private set; }

        private void Awake()
        {
            if (_instance != null) Destroy(this);
            DontDestroyOnLoad(this);
        }

        public static SlideHolder GetInstance()
        {
            if (_instance == null) _instance = new GameObject().AddComponent<SlideHolder>();
            return _instance;
        }

        public void AddItemPrefab(GameObject prefab)
        {
            if (itemPrefab == null) itemPrefab = new List<RectTransform>();

            itemPrefab.Add(prefab.GetComponent<RectTransform>());
        }
    }
}