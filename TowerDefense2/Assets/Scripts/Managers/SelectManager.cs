using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManager : MonoBehaviour
{
    [SerializeField] private LayerMask layer;

    public ISelectable SelectedObject { get; private set; }

    public void Unselect()
    {
        if(SelectedObject != null) SelectedObject.Unselect();
        SelectedObject = null;
    }

    private void Awake()
    {
        Singleton.Register(this);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Singleton.Get<InGameCameraManager>().CurrentMousePosRay;
            if(Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, layer) && hitInfo.transform.TryGetComponent<ISelectable>(out var comp))
            {
                if(SelectedObject != null) SelectedObject.Unselect();
                comp.Select();
                SelectedObject = comp;
                Singleton.Get<DownMenuUI>().State = DownMenuState.inspect;
            }
        }
    }
}
