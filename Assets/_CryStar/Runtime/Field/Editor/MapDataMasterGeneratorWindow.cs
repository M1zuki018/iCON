using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CryStar.Field.Editor
{
    /// <summary>
    /// MapDataMasterの辞書を生成するウィンドウ
    /// </summary>
    public class MapDataMasterGeneratorWindow : BaseMasterGeneratorWindow
    {
        [MenuItem("CryStar/Field/Map Data Master Generator")]
        public static void ShowWindow()
        {
            GetWindow<MapDataMasterGeneratorWindow>("Map Data Master Generator");
        }

        protected override string GetWindowTitle() => "Map Data Master Generator";

        protected override void GenerateClass(IList<IList<object>> data)
        {
            // TODO: プレハブを生成するメソッド
            var prefab = GameObject.Find(name);
            
            var sb = new StringBuilder();
        
        // クラス定義の開始
        sb.AppendLine("// ============================================================================");
        sb.AppendLine("// AUTO GENERATED - DO NOT MODIFY");
        sb.AppendLine($"// Generated at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        sb.AppendLine("// ============================================================================");
        sb.AppendLine();
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using CryStar.Field.Data;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine("/// マップデータの定数クラス");
        sb.AppendLine("/// </summary>");
        sb.AppendLine($"public static class {_className}");
        sb.AppendLine("{");
        
        // データ辞書の生成
        sb.AppendLine("    private static readonly Dictionary<int, MapData> _mapData = new Dictionary<int, MapData>");
        sb.AppendLine("    {");

        foreach (var row in data)
        {
            if (row.Count < 3) continue; // 最低限の列数チェック
            
            var id = int.Parse(row[0].ToString());
            var name = row[1].ToString();
            var displayName = row[2].ToString();
            
            sb.AppendLine($"        {{");
            sb.AppendLine($"            {id}, new MapData({id}, \"{name}\", \"{displayName}\", {prefab}) ");
            sb.AppendLine($"        }},");
        }
        
        sb.AppendLine("    };");
        sb.AppendLine();
        
        // メソッド群の追加
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// IDからマップデータを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static MapData GetMapData(int id)");
        sb.AppendLine("    {");
        sb.AppendLine("        return _mapData.GetValueOrDefault(id, null);");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 管理名からマップデータを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static MapData GetMapDataByName(string name)");
        sb.AppendLine("    {");
        sb.AppendLine("        foreach (var kvp in _mapData)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (kvp.Value.Name == name)");
        sb.AppendLine("                return kvp.Value;");
        sb.AppendLine("        }");
        sb.AppendLine("        return null;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 表示名からマップデータを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static MapData GetMapDataByDisplayName(string name)");
        sb.AppendLine("    {");
        sb.AppendLine("        foreach (var kvp in _mapData)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (kvp.Value.DisplayName == name)");
        sb.AppendLine("                return kvp.Value;");
        sb.AppendLine("        }");
        sb.AppendLine("        return null;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// IDからマップのプレハブを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static GameObject GetMapPrefab(int id)");
        sb.AppendLine("    {");
        sb.AppendLine("        return _mapData.GetValueOrDefault(id, null).Prefab;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 管理名からマップのプレハブを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static GameObject GetMapPrefabByName(string name)");
        sb.AppendLine("    {");
        sb.AppendLine("        foreach (var kvp in _mapData)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (kvp.Value.Name == name)");
        sb.AppendLine("                return kvp.Value.Prefab;");
        sb.AppendLine("        }");
        sb.AppendLine("        return null;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 表示名からマップのプレハブを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static GameObject GetMapPrefabByDisplayName(string name)");
        sb.AppendLine("    {");
        sb.AppendLine("        foreach (var kvp in _mapData)");
        sb.AppendLine("        {");
        sb.AppendLine("            if (kvp.Value.DisplayName == name)");
        sb.AppendLine("                return kvp.Value.Prefab;");
        sb.AppendLine("        }");
        sb.AppendLine("        return null;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 全マップのIDリストを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static IEnumerable<int> GetAllMapIds()");
        sb.AppendLine("    {");
        sb.AppendLine("        return _mapData.Keys;");
        sb.AppendLine("    }");
        sb.AppendLine();
        
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// 全マップデータを取得");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static IEnumerable<MapData> GetAllMapDatas()");
        sb.AppendLine("    {");
        sb.AppendLine("        return _mapData.Values;");
        sb.AppendLine("    }");
        
        sb.AppendLine("}");
        
        // ファイル出力
        SaveToFile(sb.ToString());
        }
    }

}

