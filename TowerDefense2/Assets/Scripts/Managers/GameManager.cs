using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Variable")]
    [SerializeField] private int stage;
    [SerializeField] private int startMoney;
    [SerializeField] private int waveAmount;
    [SerializeField] private int spawnAmount;
    [Header("Reference")]
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject boss;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private MainCastle mainCastle;
    [SerializeField] private Castle[] subCastles;
    [SerializeField] private DisappearRecon reconPrefeb;

    public bool ExistSubCastles
    {
        get
        {
            if (subCastles == null) return false;
            if (subCastles.Length == 0) return false;

            foreach (var castle in subCastles)
                if (castle != null) return true;

            return false;
        }
    }
    public int Money { get; set; }
    public int Wave { get; private set; }
    public MainCastle MainCastle => mainCastle;
    public Castle[] SubCastles => subCastles;
    public int MaxWave => waveAmount;
    public float WaveProgress { get; private set; }
    public int Stage => stage;
    public bool IsGameEnd { get; private set; }

    //effect
    public float EnemySlowTime { get; private set; }
    public float GainAdditiveGoldTime { get; private set; }
    public float ReduceAttackDelayTime { get; private set; }
    public float StopEnemyAttackTime { get; private set; }

    public void ItemEffect(Define.ItemType type)
    {
        switch (type)
        {
            case Define.ItemType.TowerHeal:
                foreach(var tower in TowerBase.towers)
                    tower.HealEffect();
                break;
            case Define.ItemType.SlowEnemy: EnemySlowTime = 10; break;
            case Define.ItemType.GainAdditiveGold: GainAdditiveGoldTime = 60; break;
            case Define.ItemType.ReduceAttackDelay: ReduceAttackDelayTime = 10; break;
            case Define.ItemType.StopEnemyAttack: StopEnemyAttackTime = 10; break;
            case Define.ItemType.SpawnRecon:
                Instantiate(reconPrefeb).Init();
                break;
        }
    }

    private void Awake()
    {
        Singleton.Register(this);

        Tower.towers.Clear();
        Enemy.enemies.Clear();
        Recon.recons.Clear();
        Money = startMoney;
    }

    private void Start()
    {
        StartCoroutine(GameRoutine());
    }

    private void Update()
    {
        if(EnemySlowTime > 0) EnemySlowTime -= Time.deltaTime;
        if(GainAdditiveGoldTime > 0) GainAdditiveGoldTime -= Time.deltaTime;
        if(ReduceAttackDelayTime > 0) ReduceAttackDelayTime -= Time.deltaTime;
        if(StopEnemyAttackTime > 0) StopEnemyAttackTime -= Time.deltaTime;

        if(mainCastle.IsDestroyed && !IsGameEnd)
        {
            IsGameEnd = true;
            StartCoroutine(GameEndRoutine());
        }
    }

    private IEnumerator GameEndRoutine()
    {
        var waitReal = new WaitForSecondsRealtime(0);

        Singleton.Get<GameTimeManager>().TimeScale = 1;
        Singleton.Get<GameTimeManager>().IsGameStopped = true;
        yield return new WaitForSecondsRealtime(2f);

        ScreenFade.LoadScene("RankWrite");
        while(!ScreenFade.EndLoading) yield return waitReal;
        Singleton.Get<GameTimeManager>().IsGameStopped = false;
    }

    private IEnumerator GameRoutine()
    {
        yield return new WaitForSeconds(3f);

        for (Wave = 0; Wave < waveAmount; Wave++)
        {
            for (int spawn = 0; spawn < spawnAmount; spawn++)
            {
                WaveProgress = (spawn + 1) / (float)spawnAmount;
                Instantiate(
                    enemies[Random.Range(0, enemies.Length)],
                    spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                    Quaternion.identity);

                yield return new WaitForSeconds(Random.Range(0.3f, 3.5f));
            }
        }
        GameObject boss = Instantiate(this.boss, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        yield return new WaitUntil(() => !boss);
    }
}
