namespace CryStar.Game.Enums
{
    /// <summary>
    /// ゲームイベントの列挙型
    /// </summary>
    public enum GameEventType
    {
        GameClear = 0, // ゲームクリア
        Objective = 1, // 目標表示
        StoryPreload = 2, // ストーリーデータの事前ロード
        Story = 3,
        Map = 4,
        
        Custom = 99,
    }
}