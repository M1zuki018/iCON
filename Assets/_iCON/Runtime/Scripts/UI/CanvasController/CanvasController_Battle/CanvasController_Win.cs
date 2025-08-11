using CryStar.Attribute;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_Win
    /// </summary>
    public partial class CanvasController_Win : WindowBase
    {
        [SerializeField, HighlightIfNull] private CustomText _textBox;

        /// <summary>
        /// テキストを設定
        /// </summary>
        public void SetText(string name, int experience)
        {
            _textBox.SetText($"{name}の勝利\n経験値{experience}を手に入れた");
        }
        
        public override void Hide()
        {
            base.Hide();
            // テキストはリセットして空にしておく
            _textBox.SetText("");
        }
    }
}