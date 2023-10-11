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
    public Define.ItemType Type => type;

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
            if (itemMenuUI.CurDelay > 0) return;
            Singleton.Get<GameManager>().ItemEffect(type);
            itemMenuUI.UseCard(index);

            switch (type)
            {
                case Define.ItemType.TowerHeal: Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.ItemUseTowerHeal); break;
                case Define.ItemType.SlowEnemy: Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.ItemUseSlowEnemy); break;
                case Define.ItemType.GainAdditiveGold: Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.ItemUseGainAdditiveGold); break;
                case Define.ItemType.ReduceAttackDelay: Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.ItemUseReduceAttackDelay); break;
                case Define.ItemType.StopEnemyAttack: Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.ItemUseStopEnemyAttack); break;
                case Define.ItemType.SpawnRecon: Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.ItemUseSpawnRecon); break;
            }
        });
    }

    private void Update()
    {
        rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, pos, Time.unscaledDeltaTime * lerpSpeed);
    }
}
