using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public abstract GameObject gameObject { get; }
    public abstract Transform transform { get; }
    public abstract void Select();
    public abstract void Unselect();
}
