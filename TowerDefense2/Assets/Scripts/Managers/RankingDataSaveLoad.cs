using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class RankingDataSaveLoad : MonoBehaviour
{
    public static string Path
    {
        get
        {
#if UNITY_EDITOR
            return Application.dataPath + "/rank.txt";
#endif
            return Directory.GetParent(Application.dataPath).FullName + "/rank.txt";
        }
    }

    public static void Save(List<(int index, string name, int score)> datas)
    {
        var sb = new StringBuilder();
        foreach (var data in datas) sb.Append(data.index).Append(',').Append(data.name).Append(',').Append(data.score).Append('\n');
        File.WriteAllText(Path, sb.ToString());
    }

    public static List<(int index, string name, int score)> Load()
    {
        if(!File.Exists(Path)) return new List<(int index, string name, int score)>();

        var content = File.ReadAllText(Path);
        var rows = content.Split('\n')[..^1];

        var result = new List<(int index, string name, int score)>();
        foreach (var row in rows)
        {
            var items = row.Split(',');
            var index = int.Parse(items[0]);
            var name = items[1];
            var score = int.Parse(items[2]);

            result.Add((index, name, score));
        }
        return result;
    }
}
