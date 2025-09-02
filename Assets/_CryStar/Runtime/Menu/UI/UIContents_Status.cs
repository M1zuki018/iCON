using System.Collections.Generic;
using UnityEngine;

namespace CryStar.Menu.UI
{
    /// <summary>
    /// キャラクターステータス画面の大きな枠の単位のコンテンツ
    /// </summary>
    public class UIContents_Status : MonoBehaviour
    {
        /// <summary>
        /// ステータスコンテンツのリスト
        /// </summary>
        [SerializeField]
        private List<UIContents_Parameter> _parameters = new List<UIContents_Parameter>();

        /// <summary>
        /// 引数で指定したインデックスのステータスの値を更新する
        /// </summary>
        public void SetValue(int index, int value)
        {
            _parameters[index].SetValue(value);
        }
    }
}
