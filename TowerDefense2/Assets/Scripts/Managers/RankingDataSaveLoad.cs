using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class RankingDataSaveLoad : MonoBehaviour
{
    public static string Path => Directory.GetParent(Application.dataPath).FullName + "/rank.text";

    public static void Save(List<(int index, string name, int score)> datas)
    {
        var sb = new StringBuilder();
        foreach(var data in datas) sb.Append(data.index).Append(',').Append(data.name).Append(',').Append(data.score).Append('\n');
        File.WriteAllText(Path, sb.ToString());
    }

    public static List<(int index, string name, int score)> Load()
    {
        var content = File.ReadAllText(Path);
        var rows = content.Split('\n');

        var result = new List<(int index, string name, int score)>();
        foreach(var row in rows)
        {
            if(string.IsNullOrWhiteSpace(row)) continue;
            var items = row.Split('\n');
            var index = int.Parse(items[0]);
            var name = items[1];
            var score = int.Parse(items[2]);

            result.Add((index, name, score));
        }
        return result;
    }
}
