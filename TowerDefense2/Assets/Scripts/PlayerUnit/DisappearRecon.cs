using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DisappearRecon : Recon, ISelectable
{
    private int remainTime = 60;

    public override string ExplainContent =>
        $"Remain. {remainTime / 60:0}:{remainTime:00}\n" +
        $"DPS. {1 / stats[0].attackDelay * stats[0].damage:0.##}\n" +
        $"Speed. {stats[0].speed}\n\n" +
        explain;

    public override int RequireCost => stats[0].nextRequireCost;

    public void Init()
    {
        var gm = Singleton.Get<GameManager>();
        var rand50p = Random.Range(0, 2) == 0;
        if (gm.ExistSubCastles && rand50p)
        {
            var randangle = Random.Range(0, 360);
            var pos = 
                new Vector3(Mathf.Cos(randangle * Mathf.Deg2Rad), 0, Mathf.Sin(randangle * Mathf.Deg2Rad)) * 1.75f + 
                gm.SubCastles[Random.Range(0, gm.SubCastles.Length)].transform.position;
            pos.y = 10;
            transform.position = pos;
            Debug.Log(pos);
        }
        else
        {
            var randangle = Random.Range(0, 360);
            var pos = 
                new Vector3(Mathf.Cos(randangle * Mathf.Deg2Rad), 0, Mathf.Sin(randangle * Mathf.Deg2Rad)) * 1.75f + 
                gm.MainCastle.transform.position;
            pos.y = 10;
            transform.position = pos;
            Debug.Log(pos);
        }

        StartCoroutine(MoveRoutine());
        StartCoroutine(DisappearRoutine());
    }

    protected override void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        recons.Add(this);
    }

    private IEnumerator DisappearRoutine()
    {
        while(remainTime-- > 0) yield return new WaitForSeconds(1f);
        recons.Remove(this);
        Destroy(gameObject);
    }
}
