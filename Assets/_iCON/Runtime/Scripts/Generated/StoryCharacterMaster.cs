// ============================================================================
// AUTO GENERATED - DO NOT MODIFY
// Generated at: 2025-07-24 12:43:20
// ============================================================================

using System.Collections.Generic;
using CryStar.Story.Data;
using CryStar.Story.Enums;
using UnityEngine;
using iCON.System;

/// <summary>
/// ストーリーキャラクター情報の定数クラス
/// </summary>
public static class StoryCharacterMaster
{
    private static readonly Dictionary<int, CharacterData> _characterData = new Dictionary<int, CharacterData>
    {
    };

    /// <summary>
    /// IDからキャラクターデータを取得
    /// </summary>
    public static CharacterData GetCharacter(int id)
    {
        return _characterData.GetValueOrDefault(id, null);
    }

    /// <summary>
    /// フルネームからキャラクターデータを取得
    /// </summary>
    public static CharacterData GetCharacterByName(string fullName)
    {
        foreach (var kvp in _characterData)
        {
            if (kvp.Value.FullName == fullName)
                return kvp.Value;
        }
        return null;
    }

    /// <summary>
    /// キャラクターの表情パスを取得
    /// </summary>
    public static string GetExpressionPath(int characterId, FacialExpressionType expression)
    {
        var character = GetCharacter(characterId);
        if (character?.ExpressionPaths?.ContainsKey(expression) == true)
        {
            return character.ExpressionPaths[expression];
        }
        return null;
    }

    /// <summary>
    /// 全キャラクターのIDリストを取得
    /// </summary>
    public static IEnumerable<int> GetAllCharacterIds()
    {
        return _characterData.Keys;
    }

    /// <summary>
    /// 全キャラクターデータを取得
    /// </summary>
    public static IEnumerable<CharacterData> GetAllCharacters()
    {
        return _characterData.Values;
    }
}
