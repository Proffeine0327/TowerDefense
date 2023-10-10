using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    [SerializeField] protected float duration;

    protected virtual void Start()
    {
        Destroy(gameObject, duration);
    }
}
