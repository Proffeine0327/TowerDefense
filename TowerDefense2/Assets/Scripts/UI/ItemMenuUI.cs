using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenuUI : DownMenuAttachUI
{
    [SerializeField] private Button buyBtn;
    [SerializeField] private TextMeshProUGUI btnText;
    [SerializeField] private RectTransform cardGroup;
    [SerializeField] private Vector2 closedPos;

    private int requireMoney = 50;
    private List<ItemCard> close = new();
    private List<ItemCard> open = new();

    protected override bool OpenCriteria => downMenuUI.State == DownMenuState.item;
    public float CurDelay { get; private set; }

    public void Register(ItemCard card)
    {
        close.Add(card);
        card.Set(-1, closedPos);
    }

    public void UseCard(int index)
    {
        close.Add(open[index]);
        open[index].Set(-1, closedPos);
        (open[index].transform as RectTransform).anchoredPosition = closedPos;
        open.RemoveAt(index);
        ApplyChange();
        CurDelay = 5;
    }

    private void Start()
    {
        btnText.text = requireMoney.ToString();
        buyBtn.onClick.AddListener(() =>
        {
            if (Singleton.Get<GameManager>().Money < requireMoney) return;
            if (open.Count >= 3) return;

            var rand = Random.Range(0, close.Count);
            var card = close[rand];
            close.RemoveAt(rand);
            open.Add(card);
            ApplyChange();
            Singleton.Get<GameManager>().Money -= requireMoney;
            requireMoney += 50;
            btnText.text = requireMoney.ToString();
        });
    }

    private void ApplyChange()
    {
        for (int i = 0; i < open.Count; i++)
        {
            var w = cardGroup.sizeDelta.x / 3;

            var index = i;
            open[i].Set(index, new Vector2(w * 0.5f + w * i, 0));
        }
    }

    protected override void Update()
    {
        base.Update();
        foreach(var c in open) c.CoolTimeImage.fillAmount = CurDelay / 5;
        if(CurDelay > 0) CurDelay -= Time.deltaTime;
    }
}
