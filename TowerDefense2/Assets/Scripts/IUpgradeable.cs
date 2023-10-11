using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpgradeable
{
    public abstract int RequireCost { get; }
    public abstract void Upgrade();
}
