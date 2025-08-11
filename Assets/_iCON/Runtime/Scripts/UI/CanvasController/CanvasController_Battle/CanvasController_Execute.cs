using CryStar.Attribute;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// CanvasController_Execute
    /// </summary>
    public partial class CanvasController_Execute : WindowBase
    {
        [SerializeField, HighlightIfNull] private CustomText _textBox;

        /// <summary>
        /// テキストを設定
        /// </summary>
        public void SetText(string text)
        {
            _textBox.SetText(text);
        }
        
        public override void Hide()
        {
            base.Hide();
            // テキストはリセットして空にしておく
            _textBox.SetText("");
        }
    }
}