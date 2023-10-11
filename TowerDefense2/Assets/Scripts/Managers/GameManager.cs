using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private GameObject mainCastle;
    [SerializeField] private GameObject[] subCastles;

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
    public GameObject MainCastle => mainCastle;
    public GameObject[] SubCastles => subCastles;
    public int MaxWave => waveAmount;
    public float WaveProgress { get; private set; }
    public int Stage => stage;

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
                break;
        }
    }

    private void Awake()
    {
        Singleton.Register(this);

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
    }

    private IEnumerator GameRoutine()
    {
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
