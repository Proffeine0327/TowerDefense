using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    private static Singleton instance;

    public static void Register<T>(T singleton) where T : MonoBehaviour
    {
        if(instance == null) instance = new GameObject("<<< Singleton >>>").AddComponent<Singleton>();

        if(instance.singletons.ContainsKey(typeof(T).FullName))
            instance.singletons[typeof(T).FullName] = singleton;
        else
            instance.singletons.Add(typeof(T).FullName, singleton);
    }

    public static T Get<T>() where T : MonoBehaviour => instance.singletons[typeof(T).FullName] as T;

    private readonly Dictionary<string, MonoBehaviour> singletons = new();
}
