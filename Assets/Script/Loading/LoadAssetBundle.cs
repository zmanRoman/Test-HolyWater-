using System.Collections;
using System.IO;
using Script.Board;
using Script.Holders;
using UnityEngine;

namespace Script.Loading
{/// <summary>
 /// loading and managing bundles
 /// </summary>
    public class LoadAssetBundle : MonoBehaviour
    {
        private const string CardAsset = "card";
        private const string CardName = "card_";

        private const string MaterialAsset = "material";
        private const string MaterialName = "_material";
        
        private const string SlideAsset = "slide";
        private const string SlideName = "slide_";

        private void Start()
        {
            StartCoroutine(LoadCard());
            StartCoroutine(LoadSlideItem());
        }


        private IEnumerator LoadCard()
        {
            var cardAssetBundle = LoadBundle(CardAsset);
            var materialAssetBundle = LoadBundle(MaterialAsset);
            
            var count = cardAssetBundle.GetAllAssetNames().Length/2;
            LoadingHelper.Instance.allObjectCount += count;
            
            for (var i = 1; i <= count; i++)
            {
                var card = cardAssetBundle.LoadAsset<GameObject>(CardName + i);
                var cardHandler = card.GetComponent<CardHandler>();
                CardHolder.Instance.AddCardPrefab( cardHandler);
                
                var materialCard = materialAssetBundle.LoadAsset<Material>(CardName+i+MaterialName);
                
                cardHandler.SetMaterial(materialCard);
                LoadingHelper.Instance.ObjectSetup ++;
                yield return new WaitForSeconds(0.1f);
            }
        }
        
        private IEnumerator LoadSlideItem()
        {
            var assetBundle = LoadBundle(SlideAsset);

            var count = assetBundle.GetAllAssetNames().Length;
            LoadingHelper.Instance.allObjectCount += count;
            
            for (var i = 1; i <= count; i++)
            {
                var item = assetBundle.LoadAsset<GameObject>(SlideName + i);
                SlideHolder.Instance.AddItemPrefab(item);
                
                LoadingHelper.Instance.ObjectSetup ++;
                yield return new WaitForSeconds(0.1f);
            }
        }

        private AssetBundle LoadBundle(string name)
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine("Assets/AssetBundles",name));
            
            if ( assetBundle == null) {
                Debug.Log("Failed to load AssetBundle" +" " + name);
            }

            return assetBundle;
        }
    }
}
