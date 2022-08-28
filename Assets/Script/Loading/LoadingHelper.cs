using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Loading
{
    public class LoadingHelper : MonoBehaviour
    {
        public int allObjectCount;
        [SerializeField] private int objectSetup;
    
        [SerializeField] private string _textLoad;
        [SerializeField] private TextMeshProUGUI textLoad;
        [SerializeField] private Image progressBar;
        
        private const float SceneMinimumDuration = 3f;
        private bool _loadNextScene;

        private string TextLoad
        {
            set
            {
                _textLoad = value;
                textLoad.text = _textLoad;
            }
        }
        public int ObjectSetup
        {
            get => objectSetup;
            set
            {
                objectSetup = value;


                if ((float)ObjectSetup / allObjectCount * 100 >= 99 && allObjectCount >= 2)
                {
                    StartCoroutine(LoadGameScene());
                }
            }
        }
        
        public static LoadingHelper Instance { get; private set; }
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

            StartCoroutine(SceneWaitDuration());
        }
        
        private void Update()
        {
            if (allObjectCount == 0) return;
            var progress = Mathf.RoundToInt(ObjectSetup / allObjectCount * 100f);
            if (progress > 100)
            {
                TextLoad = "100%";
            }
            else
            {
                progressBar.fillAmount = (float)progress / 100;
                TextLoad = progress + "%";
            }
        }

        private IEnumerator SceneWaitDuration()
        {
            yield return new WaitForSeconds(SceneMinimumDuration);
            _loadNextScene = true;
        }
        
        private IEnumerator LoadGameScene()
        {
            yield return new WaitUntil(() => _loadNextScene);
            SceneManager.LoadScene(1);
        }
    }
}
