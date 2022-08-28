using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Board
{/// <summary>
 /// card state processing
 /// </summary>
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class CardHandler : MonoBehaviour
    {
        [SerializeField] private Row row;
        [SerializeField] private ParticleSystem fx;
        [SerializeField] private Button button;
        [SerializeField] private Material materialFx;
        
        [SerializeField]private Vector3 originPosition;

        [SerializeField] private CardData cardData = new ();

        public Image ImageCard { get; private set; }
        
        
        private readonly Color _invisible = new (1, 1, 1, 0),_visible = new (1, 1, 1, 1);

        private Vector3 _transform {
            get => transform.localPosition;
            set {
                transform.localPosition = value;
                cardData.localPositionX = transform.localPosition.x;
                cardData.localPositionX = transform.localPosition.y;
            }
        }
     
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(Destroy);
            
            ImageCard = GetComponent<Image>();
            cardData.cardImageName = ImageCard.sprite.name;
                
            row = GetComponentInParent<Row>();
        }
        
        public void SetPositionOnList(int index, int rowNum)
        {
            cardData.indexInList = index;
            cardData.rowNum = rowNum;
            SaveLoadSceneData.Instance.SetCardData(ref cardData);
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
            if (data.isDestroy == 1)
                DestroyMute();
            _transform = new Vector3(cardData.localPositionX, cardData.localPositionY, 0);
            cardData = data;
            SaveLoadSceneData.Instance.SetCardData(ref cardData);
        }
        public void Reset()
        {
            ImageCard.color = _visible;
            _transform = originPosition;
            
            cardData.isDestroy = 0;
        }

        private void DestroyMute()
        {
            originPosition = _transform;
            ImageCard.color = _invisible;
            row.StartSwap(this);
            
            cardData.isDestroy = 1;
        }
        
        public void Destroy()
        {
            originPosition = _transform;
            
            fx.Play();
            MusicHandler.Instance.PlayFX();
            ImageCard.color = _invisible;
            row.StartSwap(this);
            
            cardData.isDestroy = 1;
        }
    }
}
