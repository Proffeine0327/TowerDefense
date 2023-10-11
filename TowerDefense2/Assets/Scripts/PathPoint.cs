using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    private readonly List<Transform> points = new();

    public Vector3 GetRandomPoint()
        => points[Random.Range(0, points.Count)].position;

    private void Awake()
    {
        Singleton.Register(this);

        foreach(Transform t in transform)
            points.Add(t);
    }
}
