using System.Collections;
using System.Collections.Generic;
using Script.Holders;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public sealed class SlideUpHandler : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{
    private const float Duration = 0.2f, Delay = 3f;

    [SerializeField] private RectTransform pointView;
    [SerializeField] private RectTransform contentHolder;
    [SerializeField] private List<RectTransform> content = new();

    private Vector3 _pView;
    private ScrollRect _slider;

    /// <summary>
    ///     processing and changing the state of the slider, saving / scrolling
    /// </summary>
    public int Current { get; private set; }

    private void Start()
    {
        _pView = _slider.transform.InverseTransformPoint(pointView.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StopAllCoroutines();
        _slider.StopMovement();
        FindCurrentItem();
        StartCoroutine(SlideItem());
    }

    public void LoadSaveCurrentItem(SaveLoadSceneData saveLoadSceneData)
    {
        Current = saveLoadSceneData.CurrentItemSlider;
        CreateSlide();
    }

    private void CreateSlide()
    {
        foreach (var item in SlideHolder.GetInstance().itemPrefab) content.Add(Instantiate(item, contentHolder));

        StartCoroutine(SlideItem());
    }

    private void FindCurrentItem()
    {
        float saveDist = 0;
        RectTransform currentContent = null;
        foreach (var cont in content)
        {
            var currentDist = Vector2.Distance(_slider.transform.InverseTransformPoint(cont.position), _pView);
            if (saveDist == 0 || saveDist > currentDist)
            {
                saveDist = currentDist;
                currentContent = cont;
            }
        }

        var index = content.IndexOf(currentContent);
        Current = index;
    }


    private void SnapTo(int currentItem)
    {
        Canvas.ForceUpdateCanvases();
        if (_slider == null) _slider = gameObject.GetComponent<ScrollRect>();

        var cHolder = _slider.transform.InverseTransformPoint(contentHolder.position);
        var cContent = _slider.transform.InverseTransformPoint(content[currentItem].position);

        var endPosition = cHolder - cContent;
        StartCoroutine(LerpRect(contentHolder, endPosition));
    }

    private IEnumerator LerpRect(RectTransform rect, Vector2 endPosition)
    {
        float currentTime = 0;
        var startPosition = rect.anchoredPosition;
        while (currentTime <= Duration)
        {
            currentTime += Time.deltaTime;
            var normalizedValue = currentTime / Duration;

            rect.anchoredPosition = Vector2.Lerp(startPosition, endPosition, normalizedValue);
            yield return null;
        }

        rect.anchoredPosition = endPosition;
        _slider.StopMovement();
    }

    private IEnumerator SlideItem()
    {
        SnapTo(Current);
        yield return new WaitForSeconds(Delay);
        Current++;
        if (content.Count == Current) Current = 0;

        StartCoroutine(SlideItem());
    }
}