namespace CryStar.Game.Events
{
    /// <summary>
    /// GameEventのパラメーター用のデータクラス
    /// </summary>
    public class GameEventParameters
    {
        /// <summary>
        /// Int型パラメーター
        /// </summary>
        public int IntParam { get; set; }
        
        /// <summary>
        /// String型パラメーター
        /// </summary>
        public string StringParam { get; set; }
        
        /// <summary>
        /// Int配列型パラメーター
        /// </summary>
        public int[] IntArrayParam { get; set; }
    }
}
