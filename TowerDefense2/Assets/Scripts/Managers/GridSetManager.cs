using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(GridSetManager))]
public class GridSetManagerEditor : Editor
{
    private GridSetManager Target => target as GridSetManager;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Create")) Target.CreateGrid();
        if (GUILayout.Button("Compress")) Target.Compress();
    }
}
#endif

public class GridSetManager : MonoBehaviour
{
    public List<GameObject> grids = new();
    public GameObject gridPrefab;
    public Vector2Int size;

    private void Awake()
    {
        Singleton.Register(this);
        Active(false);
    }

    public void Active(bool on)
    {
        foreach (var grid in grids) grid.SetActive(on);
    }

#if UNITY_EDITOR
    public void CreateGrid()
    {
        for (var i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);

        grids.Clear();

        for (var y = 0; y < size.y; y++) for (var x = 0; x < size.x; x++)
            {
                var grid = (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(gridPrefab, transform);
                grid.transform.position = new Vector3(x, 1.2f, -y);
                grid.layer = LayerMask.NameToLayer("Grid");
                var children = grid.GetComponentsInChildren<Transform>(includeInactive: true);
                foreach (var child in children) child.gameObject.layer = LayerMask.NameToLayer("Grid");
                grids.Add(grid);
            }

        EditorUtility.SetDirty(gameObject);
    }

    public void Compress()
    {
        for(var i = grids.Count - 1; i >= 0; i--) if (grids[i] == null) grids.RemoveAt(i);
        EditorUtility.SetDirty(gameObject);
    }
#endif
}
