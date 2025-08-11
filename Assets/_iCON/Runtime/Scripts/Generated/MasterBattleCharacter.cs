using System.Collections.Generic;

public static class MasterBattleCharacter
{
    private static readonly Dictionary<int, string> _iconPath = new Dictionary<int, string>()
    {
        { 1, "Assets/AssetStoreTools/Images/Battle/Character/Yuki.png" },
        { 2, "Assets/AssetStoreTools/Images/Battle/Character/Enemy.png" }
    };
    
    public static string IconPath(int id) => _iconPath[id];
}
