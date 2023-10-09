using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameCameraManager : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float zoomSize;
    [SerializeField] private float normalDist;
    [SerializeField] private float zoomDist;
    [SerializeField] private Transform camPivot;
    [SerializeField] private Transform normalPivot;

    private bool isDragging;
    private float curDist;

    public Camera mainCamera => Camera.main;
    public bool IsUIClick
    {
        get
        {
            var eventData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };

            var raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            foreach (var result in raycastResults)
            {
                if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                    return true;
            }
            return false;
        }
    }
    public Ray CurrentMousePosRay => mainCamera.ScreenPointToRay(Input.mousePosition);

    private void Awake()
    {
        Singleton.Register(this);

        curDist = normalDist;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsUIClick) isDragging = true;
        if (isDragging) camPivot.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0), Space.World);
        if (Input.GetMouseButtonUp(0)) isDragging = false;

        var curSelect = Singleton.Get<SelectManager>().SelectedObject;

        if (curSelect != null)
        {
            camPivot.position = Vector3.Lerp(camPivot.position, curSelect.transform.position, Time.unscaledDeltaTime * lerpSpeed);
            curDist = Mathf.Lerp(curDist, zoomDist, Time.unscaledDeltaTime * lerpSpeed);
        }
        else
        {
            camPivot.position = Vector3.Lerp(camPivot.position, normalPivot.position, Time.unscaledDeltaTime * lerpSpeed);
            curDist = Mathf.Lerp(curDist, normalDist, Time.unscaledDeltaTime * lerpSpeed);
        }
        mainCamera.transform.position = camPivot.position + -mainCamera.transform.forward * curDist;
    }
}
