using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
        var adjustedAngle = transform.parent.eulerAngles.y > 0 ? transform.parent.eulerAngles.y : 360 + transform.parent.eulerAngles.y;
        adjustedAngle %= 360;

        var relative = Vector3.zero;
        if (adjustedAngle > 180) relative = Camera.main.transform.position - transform.parent.position;
        else relative = transform.parent.position - Camera.main.transform.position;

        relative.y = 0;
        relative = relative.normalized;
        relative = Quaternion.Euler(0, adjustedAngle, 0) * relative;

        //anim.SetFloat("relativeX", relative.x);
        //anim.SetFloat("relativeZ", relative.z);
    }
}
