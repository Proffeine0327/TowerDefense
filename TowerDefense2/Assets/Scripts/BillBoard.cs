using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] private bool justLookCamera;
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if(justLookCamera) return;
        var relative = Camera.main.transform.position - transform.position;
        relative.y = 0;
        relative = relative.normalized;

        var dir = Quaternion.Euler(0, -transform.parent.eulerAngles.y, 0) * relative;

        anim.SetFloat("camDirX", dir.x);
        anim.SetFloat("camDirZ", dir.z);
    }
}
