using CryStar.Attribute;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_Lose
    /// </summary>
    public partial class CanvasController_Lose : WindowBase
    {
        [SerializeField, HighlightIfNull] private CustomText _textBox;

        public override void Show()
        {
            base.Show();
            // メッセージを表示
            _textBox.SetText($"負けてしまった...");
        }

        public override void Hide()
        {
            base.Hide();
            // テキストはリセットして空にしておく
            _textBox.SetText("");
        }
    }
}