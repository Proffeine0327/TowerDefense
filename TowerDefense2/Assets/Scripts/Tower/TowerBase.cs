using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public abstract class TowerBase : MonoBehaviour, ISelectable
{
    public static HashSet<TowerBase> towers = new();

    [SerializeField] protected float range;
    [SerializeField] protected TowerData data;
    [SerializeField] protected Image hpbar;

    protected int curhp;
    protected int exp;
    protected int level;
    protected bool isGhost;

    public void Select()
    {
        Debug.Log("Select!");
    }

    public void Unselect()
    {
        
    }

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
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    protected virtual void Start()
    {
        towers.Add(this);
    }

    protected virtual void Update()
    {
        hpbar.fillAmount = curhp / (float)data.maxhp;

        if (curhp <= 0)
        {
            towers.Remove(this);
            Destroy(gameObject);
            return;
        }
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
            if (!CheckGrid(Vector3.one, out _))
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            if (!CheckGrid(Vector3.one, out _))
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
        GetComponent<SphereCollider>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Tower");
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

    public abstract void Damage(int damageCount);
}
