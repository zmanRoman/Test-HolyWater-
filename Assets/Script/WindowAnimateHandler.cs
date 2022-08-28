
using System.Collections;
using UnityEngine;

public sealed class WindowAnimateHandler : MonoBehaviour
{/// <summary>
 /// control and processing of window animations
 /// </summary>
    private Animator _animator;
    [SerializeField] private bool showOnStart;
    private static readonly int Show = Animator.StringToHash("Show"), Hide = Animator.StringToHash("Hide");
    private void Start()
    {
        if (_animator == null)
        {
            TryGetComponent(out Animator animator);
            _animator = animator;
        }

        StartCoroutine(ShowAfterStart());
    }

    private IEnumerator ShowAfterStart()
    {
        if (!showOnStart) yield break;
        yield return new WaitForSeconds(0.3f);
        AnimateWindow(true);
    }
    public void AnimateWindow(bool active)
    {
        switch (active)
        {
            case true:
                _animator.SetTrigger(Show);
                break;
            case false:
                _animator.SetTrigger(Hide);
                break;
        }
    }
}
