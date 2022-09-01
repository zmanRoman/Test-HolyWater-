using System.Collections;
using UnityEngine;

public sealed class WindowAnimateHandler : MonoBehaviour
{
    private const float Delay = 0.3f;

    private static readonly int Show = Animator.StringToHash("Show"), Hide = Animator.StringToHash("Hide");
    [SerializeField] private bool showOnStart;

    private Animator _animator;

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
        yield return new WaitForSeconds(Delay);
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