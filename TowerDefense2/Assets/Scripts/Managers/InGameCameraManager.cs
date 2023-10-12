using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public enum CameraState { normal, focus, free }

public class InGameCameraManager : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float normalDist;
    [SerializeField] private float zoomDist;
    [SerializeField] private float thickness;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Camera minimapCam;
    [SerializeField] private Transform camPivot;
    [SerializeField] private Transform normalPivot;
    [Header("Key")]
    [SerializeField] private Transform mainCastlePos;
    [SerializeField] private Transform[] subCastlePos;

    private int subCastlePosIndex;
    private float curDist;
    private Vector3 storePosition;

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
    public CameraState CameraState { get; set; }

    public void MinimapClick(Vector3 ratio)
    {
        var worldPos = minimapCam.ViewportToWorldPoint(ratio);
        worldPos.y = normalPivot.position.y;

        camPivot.position = worldPos;
        CameraState = CameraState.free;
        Singleton.Get<SelectManager>().Unselect();
        Singleton.Get<DownMenuUI>().State = DownMenuState.none;
    }

    private void Awake()
    {
        Singleton.Register(this);

        curDist = normalDist;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if (Input.mousePosition.x <= thickness || Input.GetKey(KeyCode.A))
            camPivot.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0), Space.World);
        if (Input.mousePosition.x >= Screen.width - thickness || Input.GetKey(KeyCode.D))
            camPivot.Rotate(new Vector3(0, -rotateSpeed * Time.deltaTime, 0), Space.World);

        switch (CameraState)
        {
            case CameraState.normal:
                camPivot.position = Vector3.Lerp(camPivot.position, normalPivot.position, Time.unscaledDeltaTime * lerpSpeed);
                curDist = Mathf.Lerp(curDist, normalDist, Time.unscaledDeltaTime * lerpSpeed);
                break;
            case CameraState.focus:
                try
                {
                    var curSelect = Singleton.Get<SelectManager>().SelectedObject;
                    camPivot.position = Vector3.Lerp(camPivot.position, curSelect.transform.position, Time.unscaledDeltaTime * lerpSpeed);
                    curDist = Mathf.Lerp(curDist, zoomDist, Time.unscaledDeltaTime * lerpSpeed);
                }
                catch (MissingReferenceException)
                {
                    CameraState = CameraState.normal;
                    Singleton.Get<DownMenuUI>().State = DownMenuState.none;
                    break;
                }
                break;
            case CameraState.free:
                curDist = Mathf.Lerp(curDist, normalDist, Time.unscaledDeltaTime * lerpSpeed);
                break;
        }
        mainCamera.transform.position = camPivot.position + -mainCamera.transform.forward * curDist;
        Key();
    }

    private void Key()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            camPivot.position = mainCastlePos.position;
            CameraState = CameraState.free;
            Singleton.Get<DownMenuUI>().State = DownMenuState.none;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (subCastlePos.Length == 0) return;

            subCastlePosIndex++;
            if (subCastlePosIndex == subCastlePos.Length) subCastlePosIndex = 0;

            camPivot.position = subCastlePos[subCastlePosIndex].position;
            CameraState = CameraState.free;
            Singleton.Get<DownMenuUI>().State = DownMenuState.none;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (Singleton.Get<SelectManager>().SelectedObject is not Recon)
            {
                foreach (var r in Recon.recons)
                {
                    CameraState = CameraState.focus;
                    Singleton.Get<SelectManager>().Select(r);
                    Singleton.Get<DownMenuUI>().State = DownMenuState.inspect;
                    return;
                }
                return;
            }

            CameraState = CameraState.focus;
            var next = false;
            foreach (var r in Recon.recons)
            {
                if (ReferenceEquals(r, Singleton.Get<SelectManager>().SelectedObject as Recon))
                {
                    next = true;
                    continue;
                }

                if (next)
                {
                    Singleton.Get<SelectManager>().Select(r);
                    Singleton.Get<DownMenuUI>().State = DownMenuState.inspect;
                    return;
                }
            }
            if (next)
            {
                foreach (var r in Recon.recons)
                {
                    Singleton.Get<SelectManager>().Select(r);
                    Singleton.Get<DownMenuUI>().State = DownMenuState.inspect;
                    return;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (storePosition == Vector3.zero)
            {
                camPivot.position = mainCastlePos.position;
                CameraState = CameraState.free;
            }
            else
            {
                camPivot.position = storePosition;
                CameraState = CameraState.free;
            }
            Singleton.Get<DownMenuUI>().State = DownMenuState.none;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            storePosition = camPivot.position;
            Singleton.Get<DownMenuUI>().State = DownMenuState.none;
        }
    }
}