using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyPrefabFair
{
    public string key;
    public GameObject prefab;
}

public class PrefabContainer : MonoBehaviour
{
    private static PrefabContainer instance;

    [SerializeField] private KeyPrefabFair[] prefabs;
    public readonly Dictionary<string, GameObject> container = new();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach(var prefab in prefabs)
            container.Add(prefab.key, prefab.prefab);
    }

    public static GameObject Instantiate(string key)
    {
        return Instantiate(instance.container[key]);
    }

    public static GameObject Instantiate(string key, Vector3 position, Quaternion quaternion)
    {
        return Instantiate(instance.container[key], position, quaternion);
    }

    public static GameObject Instantiate(string key, Transform parent)
    {
        return Instantiate(instance.container[key], parent);
    }
}
