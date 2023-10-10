using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLine : Particle
{
    private float curDuration;
    private LineRenderer lr;

    protected override void Start()
    {
        base.Start();
        curDuration = duration;
    }

    public void Init(Vector3 pos1, Vector3 pos2)
    {
        lr = GetComponent<LineRenderer>();
        lr.SetPosition(0, pos1);
        lr.SetPosition(1, pos2);
    }

    private void Update()
    {
        curDuration -= Time.deltaTime;
        lr.widthMultiplier = curDuration / duration;
    }
}
