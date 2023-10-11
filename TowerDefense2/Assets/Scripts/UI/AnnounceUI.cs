using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum AnnounceType
{
    CheatEnemyStop,
    CheatGainMoney,
    KillEnemyWithoutMoney,
    KillEnemy,

    ItemGainTowerHeal,
    ItemGainSlowEnemy,
    ItemGainGainAdditiveGold,
    ItemGainReduceAttackDelay,
    ItemGainStopEnemyAttack,
    ItemGainSpawnRecon,
    ItemUseTowerHeal,
    ItemUseSlowEnemy,
    ItemUseGainAdditiveGold,
    ItemUseReduceAttackDelay,
    ItemUseStopEnemyAttack,
    ItemUseSpawnRecon,
    ItemFull,
    
    TowerCannotSet,

    LackMoney
}

public class AnnounceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private float time;

    public void DisplayText(AnnounceType type)
    {
        text.text = type switch
        {
            AnnounceType.CheatEnemyStop => "현재 적이 정지되었습니다.",
            AnnounceType.CheatGainMoney => "돈 +1000",
            AnnounceType.KillEnemyWithoutMoney => "현재 나와있는 적이 모두 사망합니다.",
            AnnounceType.KillEnemy => "현재 나와있는 적이 모두 사망합니다. 돈을 얻습니다.",

            AnnounceType.ItemGainTowerHeal => "획득 : 타워 회복",
            AnnounceType.ItemGainSlowEnemy => "획득 : 적 이속 감소",
            AnnounceType.ItemGainGainAdditiveGold => "획득 : 골드 획득량 증가",
            AnnounceType.ItemGainReduceAttackDelay => "획득 : 타워 공격 딜레이 감소",
            AnnounceType.ItemGainStopEnemyAttack => "획득 : 공격 정지",
            AnnounceType.ItemGainSpawnRecon => "획득 : 순찰 유닛 생성",

            AnnounceType.ItemUseTowerHeal => "모든 타워 현재 체력 +30%",
            AnnounceType.ItemUseSlowEnemy => "10초간 모든적의 이동속도 -50%",
            AnnounceType.ItemUseGainAdditiveGold => "1분간 골드 획득량 +100%",
            AnnounceType.ItemUseReduceAttackDelay => "10초간 타워 공격 딜레이 -50%",
            AnnounceType.ItemUseStopEnemyAttack => "10초간 모든 적 공격 중지",
            AnnounceType.ItemUseSpawnRecon => "1분동안 유지되는 순찰유닛 생성",
            AnnounceType.ItemFull => "아이템이 가득 찼습니다.",

            AnnounceType.TowerCannotSet => "현재 위치에 타워를 설치할 수 없습니다.",
            AnnounceType.LackMoney => "돈이 부족합니다.",

            _ => ""
        };

        time = 3;
    }

    private void Awake()
    {
        Singleton.Register(this);
    }

    private void Update()
    {
        if(time > 0) time -= Time.deltaTime;
        text.enabled = time > 0;
    }
}
