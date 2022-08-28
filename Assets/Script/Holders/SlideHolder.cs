using System.Collections.Generic;
using UnityEngine;

namespace Script.Holders
{/// <summary>
 /// A slider that contains changing slides
 /// </summary>
    public sealed class SlideHolder : MonoBehaviour
    {
        
        public List<RectTransform> itemPrefab { get; private set; }
    
        public static  SlideHolder Instance { get; private set; }
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
        
        public void AddItemPrefab(GameObject prefab)
        {
            itemPrefab ??= new List<RectTransform>();
            
            itemPrefab.Add(prefab.GetComponent<RectTransform>());
        }
    }
}
