using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

[CustomEditor(typeof(TileSetManager))]
public class TileSetManagerEditor : Editor
{
    private TileSetManager Target => target as TileSetManager;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Create"))
        {
            Target.Create();
            EditorUtility.SetDirty(Target);
        }
    }
}

#endif

public class TileSetManager : MonoBehaviour
{
    [Tooltip("Setting Amount Max 16")]
    public GameObject waterPrefab;
    public GameObject[] tileset;
    public TextAsset setting;

#if UNITY_EDITOR
    public void Create()
    {
        for(var i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);

        var content = setting.text[..^1];
        var rows = content.Split('\n');
        var position = Vector3.zero;
        foreach(var row in rows)
        {
            foreach(var item in row.Split(','))
            {
                var itemTrimmed = item.Trim();

                if(itemTrimmed.Length > 0)
                {
                    var block = (GameObject)PrefabUtility.InstantiatePrefab(tileset[System.Convert.ToInt32("0x" + itemTrimmed[0], 16)], transform);
                    block.transform.position = position;

                    var sizeratio = block.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
                    block.transform.localScale /= sizeratio;
                }

                var waterBlock = (GameObject)PrefabUtility.InstantiatePrefab(waterPrefab, transform);
                var waterSizeRatio = waterBlock.GetComponent<MeshFilter>().sharedMesh.bounds.size.x;
                waterBlock.transform.position = position - Vector3.up;
                waterBlock.transform.localScale /= waterSizeRatio;
                position.x += 1;
            }

            position.x = 0;
            position.z -= 1;
        }
    }
#endif
}
