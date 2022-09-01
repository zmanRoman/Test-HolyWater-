using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Board
{
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class CardHandler : MonoBehaviour
    {
        private const int On = 1, Off = 0;
        private const float Duration = 0.3f;

        [SerializeField] private Row row;
        [SerializeField] private ParticleSystem fx;
        [SerializeField] private Button button;
        [SerializeField] private Material materialFx;
        [SerializeField] private CardData cardData = new();

        private readonly Color _invisible = new(1, 1, 1, 0), _visible = new(1, 1, 1, 1);

        private MusicHandler _musicHandler;
        private SaveLoadSceneData _saveLoadSceneData;
        public Image ImageCard { get; private set; }
        public CardData Data => cardData;

        private void Awake()
        {
            button = GetComponent<Button>();

            ImageCard = GetComponent<Image>();
            Data.cardImageName = ImageCard.sprite.name;

            row = GetComponentInParent<Row>();
        }

        public void Reset()
        {
            ImageCard.color = _visible;
            button.interactable = true;
            Data.isDestroy = Off;
        }


        public void Init(SaveLoadSceneData saveLoadSceneData, MusicHandler musicHandler)
        {
            _saveLoadSceneData = saveLoadSceneData;
            _musicHandler = musicHandler;
        }

        public void SetPositionOnList(int index, int rowNum)
        {
            Data.indexInList = index;
            Data.rowNum = rowNum;
            _saveLoadSceneData.SetCardData(ref cardData);
        }

        public void SetMaterial(Material material)
        {
            materialFx = material;
            fx = GetComponent<ParticleSystem>();
            fx.GetComponent<ParticleSystemRenderer>().material = materialFx;
        }

        public void SetupLoadData(CardData data)
        {
            StartCoroutine(CoroutineSetupLoadData(data));
        }

        private IEnumerator CoroutineSetupLoadData(CardData data)
        {
            yield return new WaitUntil(() => row.EndSetup);
            if (data.isDestroy == On) DestroyMute();
            cardData = data;
            _saveLoadSceneData.SetCardData(ref cardData);
            row.CardEndSetup();
        }

        private void DestroyMute()
        {
            button.interactable = false;
            ImageCard.color = _invisible;
            Data.isDestroy = On;
        }

        public void Destroy()
        {
            button.interactable = false;
            fx.Play();
            _musicHandler.PlayFX();
            ImageCard.color = _invisible;
            Data.isDestroy = On;
            row.Swap(this, Duration);
        }
    }
}