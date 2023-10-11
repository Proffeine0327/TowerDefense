using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private ItemMenuUI itemMenuUI;
    [SerializeField] private Image coolTime;
    [SerializeField] private Define.ItemType type;
    private Button btn;
    private int index;
    private Vector3 pos;

    private RectTransform rt => transform as RectTransform;
    public Image CoolTimeImage => coolTime;

    public void Set(int index, Vector3 pos)
    {
        this.index = index;
        this.pos = pos;
    }

    private void Awake()
    {
        itemMenuUI.Register(this);

        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            if(itemMenuUI.CurDelay > 0) return;
            Singleton.Get<GameManager>().ItemEffect(type);
            itemMenuUI.UseCard(index);
        });
    }

    private void Update()
    {
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, pos, Time.unscaledDeltaTime * lerpSpeed);
    }
}
