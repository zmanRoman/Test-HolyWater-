using System.Collections;
using System.IO;
using Script.Board;
using Script.Holders;
using UnityEngine;

namespace Script.Loading
{
    public class LoadAssetBundle : MonoBehaviour
    {
        private const string CardAsset = "card";
        private const string CardName = "card_";

        private const string MaterialAsset = "material";
        private const string MaterialName = "_material";

        private const string SlideAsset = "slide";
        private const string SlideName = "slide_";

        private const float Duration = 0.1f;
        private const float MaxProgress = 100;

        [SerializeField] private int objectSetup;
        [SerializeField] private int allObjectCount;
        private LoadingHelper _loadingHelper;
        private float _progress;

        private int ObjectSetup
        {
            get => objectSetup;
            set
            {
                objectSetup = value;
                SendProgressLoad();
            }
        }

        private void Start()
        {
            StartCoroutine(LoadCard());
            StartCoroutine(LoadSlideItem());
        }

        private void SendProgressLoad()
        {
            _progress = (float)ObjectSetup / allObjectCount * MaxProgress;
            if (_loadingHelper == null) _loadingHelper = FindObjectOfType<LoadingHelper>();
            _loadingHelper.ShowProgress(_progress);
        }

        private IEnumerator LoadCard()
        {
            var cardAssetBundle = LoadBundle(CardAsset);
            var materialAssetBundle = LoadBundle(MaterialAsset);

            var count = cardAssetBundle.GetAllAssetNames().Length / 2;
            allObjectCount += count;

            for (var i = 1; i <= count; i++)
            {
                var card = cardAssetBundle.LoadAsset<GameObject>(CardName + i);
                var cardHandler = card.GetComponent<CardHandler>();
                CardHolder.GetInstance().AddCardPrefab(cardHandler);

                var materialCard = materialAssetBundle.LoadAsset<Material>(CardName + i + MaterialName);

                cardHandler.SetMaterial(materialCard);
                ObjectSetup++;
                yield return new WaitForSeconds(Duration);
            }
        }

        private IEnumerator LoadSlideItem()
        {
            var assetBundle = LoadBundle(SlideAsset);

            var count = assetBundle.GetAllAssetNames().Length;
            allObjectCount += count;

            for (var i = 1; i <= count; i++)
            {
                var item = assetBundle.LoadAsset<GameObject>(SlideName + i);
                SlideHolder.GetInstance().AddItemPrefab(item);

                ObjectSetup++;
                yield return new WaitForSeconds(Duration);
            }
        }

        private AssetBundle LoadBundle(string name)
        {
            var assetBundle = AssetBundle.LoadFromFile(Path.Combine("Assets/AssetBundles", name));

            if (assetBundle == null) Debug.Log("Failed to load AssetBundle" + " " + name);

            return assetBundle;
        }
    }
}