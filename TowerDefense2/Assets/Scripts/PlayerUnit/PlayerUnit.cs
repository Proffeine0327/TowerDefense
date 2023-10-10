using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour, ISelectable
{
    public readonly static HashSet<PlayerUnit> units = new();

    protected int curhp;

    public virtual void Select() { }
    public virtual void Unselect() { }

    protected virtual void Update()
    {
        if (curhp <= 0)
        {
            units.Remove(this);
            Destroy(gameObject);
            return;
        }
    }

    public virtual void Damage(int amount)
    {
        curhp -= amount;
        PrefabContainer.Instantiate("HitEffect", transform.position, Quaternion.identity);
    }
}
