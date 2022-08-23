
using System;
using System.Collections;
using System.Collections.Generic;
using Script.Holders;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]

public sealed class SlideUpHandler : MonoBehaviour, IEndDragHandler, IBeginDragHandler
{

    [SerializeField] private RectTransform pointView;
    private Vector3 _pView;
    [SerializeField] private RectTransform contentHolder;
    [SerializeField] private List<RectTransform> content = new ();
    private ScrollRect _slider;
    
    private Coroutine _currentSlideItem;

    [field: SerializeField] public int Current { get; private set; }
    public static SlideUpHandler Instance { get; private set; }
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
      
    }

    private void Start()
    {
        if (_slider == null) _slider = gameObject.GetComponent<ScrollRect>();
        _pView = _slider.transform.InverseTransformPoint(pointView.position);
        Current = SaveLoadSceneData.Instance.CurrentItemSlider;
        CreateSlide();

    }

    private void CreateSlide()
    {
        foreach (var item in  SlideHolder.Instance.itemPrefab)
        {
            content.Add(Instantiate(item, contentHolder));
        }
        _currentSlideItem = StartCoroutine(SlideItem());
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        StopCoroutine(_currentSlideItem);
    }
        
    public void OnEndDrag(PointerEventData eventData)
    {
        StopCoroutine(_currentSlideItem);
        _slider.StopMovement();
        FindCurrentItem();
        _currentSlideItem = StartCoroutine(SlideItem());
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
            
        int index = content.IndexOf(currentContent);
        Current = index;
    }
        

    public void SnapTo(int currentItem)
    {
        Canvas.ForceUpdateCanvases();
        
        var cHolder = _slider.transform.InverseTransformPoint(contentHolder.position);
        var cContent = _slider.transform.InverseTransformPoint(content[currentItem].position);
            
        contentHolder.anchoredPosition = cHolder -  cContent;
    }
        
    private IEnumerator SlideItem()
    {
        SnapTo(Current);
        yield return new WaitForSeconds(4f);
        Current++;
        if (content.Count == Current)
        {
            Current = 0;
        }
        _currentSlideItem =  StartCoroutine(SlideItem());
    }
}
