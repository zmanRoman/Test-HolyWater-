using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Loading
{
    public class LoadingHelper : MonoBehaviour
    {
        private const int GameScene = 1;
        private const float SceneMinimumDuration = 3f;

        private const int MaxProgress = 100;

        [SerializeField] private string _textLoad;
        [SerializeField] private TextMeshProUGUI textLoad;
        [SerializeField] private Image progressBar;
        private bool _loadNextScene;

        private LoadingHelper()
        {
        }

        private string TextLoad
        {
            set
            {
                _textLoad = value;
                textLoad.text = _textLoad;
            }
        }

        private void Start()
        {
            StartCoroutine(SceneWaitDuration());
        }

        public void ShowProgress(float progress)
        {
            if (progress >= MaxProgress) StartCoroutine(LoadGameScene());
            progressBar.fillAmount = progress / MaxProgress;
            TextLoad = progress + "%";
        }

        private IEnumerator SceneWaitDuration()
        {
            yield return new WaitForSeconds(SceneMinimumDuration);
            _loadNextScene = true;
        }

        private IEnumerator LoadGameScene()
        {
            yield return new WaitUntil(() => _loadNextScene);
            SceneManager.LoadScene(GameScene);
        }
    }
}