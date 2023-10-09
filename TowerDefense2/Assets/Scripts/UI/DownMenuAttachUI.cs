using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DownMenuAttachUI : MonoBehaviour
{
    [SerializeField] protected float openY;
    [SerializeField] protected float closeY;
    [SerializeField] protected float lerpSpeed;

    protected RectTransform rt => transform as RectTransform;

    protected abstract bool OpenCriteria { get; }
    protected DownMenuUI downMenuUI => Singleton.Get<DownMenuUI>();

    protected virtual void Update()
    {
        var targetpos = new Vector2(rt.anchoredPosition.x, OpenCriteria ? openY : closeY);
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, targetpos, Time.unscaledDeltaTime * lerpSpeed);
    }
}
