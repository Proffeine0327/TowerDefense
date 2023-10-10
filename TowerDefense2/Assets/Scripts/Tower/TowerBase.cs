using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TowerBase : PlayerUnit
{
    [SerializeField] protected float range;
    [SerializeField] protected TowerData data;
    [SerializeField] protected Image hpbar;
    
    protected int exp;
    protected int level;
    protected bool isGhost;

    protected virtual void Awake()
    {
        exp = 0;
        level = 0;
        curhp = data.maxhp;

        if (data.type == Define.BuildType.MainCastle || data.type == Define.BuildType.SubCastle)
        {
            return;
        }

        isGhost = true;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    protected override void Update()
    {
        base.Update();
        hpbar.fillAmount = curhp / (float)data.maxhp;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Set()
    {
        if (data.isDoubleGrid)
        {
            if (!CheckGrid(Vector3.one * 0.95f, out _))
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            if (!CheckGrid(Vector3.one * 0.45f, out _))
            {
                Destroy(gameObject);
                return;
            }
        }

        if (Singleton.Get<GameManager>().Money < data.cost)
        {
            Destroy(gameObject);
            return;
        }
        
        Singleton.Get<GameManager>().Money -= data.cost;
        isGhost = false;
        GetComponent<Collider>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Tower");
        units.Add(this);
    }

    private bool CheckGrid(Vector3 boxSize, out RaycastHit hitInfo)
    {
        return Physics.BoxCast(
            transform.position + Vector3.up * 5,
            boxSize,
            Vector3.down,
            out hitInfo,
            Quaternion.identity,
            Mathf.Infinity,
            LayerMask.GetMask("Grid", "Tower")) && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Grid");
    }
}
