using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TowerBase : MonoBehaviour, ISelectable
{
    public readonly static HashSet<TowerBase> towers = new();

    [SerializeField] protected float range;
    [SerializeField] protected TowerData data;
    [SerializeField] protected Image hpbar;

    public virtual string ExplainContent => 
        $"Max Hp. {data.levelStats[level].maxhp}\n" +
        $"Hp. {curhp}\n\n" +
        data.explain;

    protected int exp;
    protected int level;
    protected float curhp;
    protected bool isGhost;

    public virtual void Select() { }
    public virtual void Unselect() { }

    public void HealEffect()
    {
        curhp += data.levelStats[level].maxhp * 0.3f;
    }

    public virtual void Damage(int amount)
    {
        curhp -= amount;
        PrefabContainer.Instantiate("HitEffect", transform.position, Quaternion.identity);
    }

    protected virtual void Awake()
    {
        curhp = data.levelStats[level].maxhp;

        if (data.type == Define.BuildType.MainCastle || data.type == Define.BuildType.SubCastle)
        {
            return;
        }

        isGhost = true;
        gameObject.GetComponent<Collider>().enabled = false;
    }

    protected virtual void Update()
    {
        if (curhp <= 0)
        {
            towers.Remove(this);
            Destroy(gameObject);
            return;
        }
        hpbar.fillAmount = curhp / data.levelStats[level].maxhp;
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
            var offsets = new Vector3[]
            {
                new Vector3(0.5f, 0, 0.5f),
                new Vector3(0.5f, 0, -0.5f),
                new Vector3(-0.5f, 0, 0.5f),
                new Vector3(-0.5f, 0, -0.5f),
            };

            foreach (var offset in offsets)
            {
                if (!CheckGrid(offset))
                {
                    Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.TowerCannotSet);
                    Destroy(gameObject);
                    return;
                }
            }
        }
        else
        {
            if (!CheckGrid(Vector3.zero))
            {
                Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.TowerCannotSet);
                Destroy(gameObject);
                return;
            }
        }

        if (Singleton.Get<GameManager>().Money < data.cost)
        {
            Singleton.Get<AnnounceUI>().DisplayText(AnnounceType.LackMoney);
            Destroy(gameObject);
            return;
        }

        Singleton.Get<GameManager>().Money -= data.cost;
        isGhost = false;
        GetComponent<Collider>().enabled = true;
        GetComponent<NavMeshObstacle>().enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Tower");
        towers.Add(this);
    }

    private bool CheckGrid(Vector3 offset)
    {
        return Physics.BoxCast(
            transform.position + Vector3.up * 5 + offset,
            Vector3.one * 0.45f,
            Vector3.down,
            out var hitInfo,
            Quaternion.identity,
            Mathf.Infinity,
            LayerMask.GetMask("Grid", "Tower")) && hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Grid");
    }
}
