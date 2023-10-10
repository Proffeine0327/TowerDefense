using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Variable")]
    [SerializeField] private int endStageIndex;
    [SerializeField] private int startMoney;
    [Header("Reference")]
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject boss;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject mainCastle;

    public int Money { get; set; }

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
        //gameover
        if(!mainCastle) return;
    }

    private IEnumerator GameRoutine()
    {
        //~2:30
        while(Singleton.Get<GameTimeManager>().PlayTime < 150)
        {
            Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.3f, 3.5f));
        }
        GameObject boss = Instantiate(this.boss, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        yield return new WaitUntil(() => !boss);
    }
}
