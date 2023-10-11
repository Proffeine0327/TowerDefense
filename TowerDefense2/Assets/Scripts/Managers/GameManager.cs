using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Variable")]
    [SerializeField] private int endStageIndex;
    [SerializeField] private int startMoney;
    [SerializeField] private int spawnAmount;
    [Header("Reference")]
    [Header("slim, but, kingslim, knightslim, speedslim")]
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
    public GameObject MainCastle => mainCastle;
    public GameObject[] SubCastles => subCastles;

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
        if (!mainCastle) return;
    }

    private IEnumerator GameRoutine()
    {
        var spawnCount = spawnAmount;
        while (spawnCount > 0)
        {
            Instantiate(
                enemies[Random.Range(0, enemies.Length)],
                spawnPoints[Random.Range(0, spawnPoints.Length)].position,
                Quaternion.identity);
            
            spawnCount--;
            yield return new WaitForSeconds(Random.Range(0.3f, 3.5f));
        }
        GameObject boss = Instantiate(this.boss, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);

        yield return new WaitUntil(() => !boss);
    }
}
