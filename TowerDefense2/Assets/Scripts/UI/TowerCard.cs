using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerCard : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TowerData data;
    private GameObject instance;
    private bool isDrag;

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDrag) return;
        
        var ray = Camera.main.ScreenPointToRay(eventData.position);
        var plane = new Plane(Vector3.down, Vector3.up);

        if (plane.Raycast(ray, out var f))
        {
            var point = ray.GetPoint(f);

            if (data.isDoubleGrid)
            {
                instance.transform.position = new Vector3(Mathf.RoundToInt(point.x), 2f, Mathf.RoundToInt(point.z));
            }
            else
            {
                instance.transform.position = new Vector3(Mathf.RoundToInt(point.x + 0.5f) - 0.5f, 2f, Mathf.RoundToInt(point.z + 0.5f) - 0.5f);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(Singleton.Get<GameManager>().Money < data.cost) return;

        isDrag = true;

        instance = Instantiate(data.prefeb);
        Singleton.Get<GridSetManager>().Active(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(!isDrag) return;

        isDrag = false;

        instance.GetComponent<TowerBase>().Set();
        instance = null;
        Singleton.Get<GridSetManager>().Active(false);
    }
}
