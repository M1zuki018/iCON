using System.Collections.Generic;
using CryStar.CommandBattle.Data;
using Cysharp.Threading.Tasks;
using iCON.Enums;
using UnityEngine;

namespace CryStar.CommandBattle.UI
{
    /// <summary>
    /// バトルシーンのCanvasManager
    /// </summary>
    public class BattleCanvasManager : CoordinatorManagerBase
    {
        /// <summary>
        /// キャンバスを切り替える
        /// </summary>
        public void ShowCanvas(BattleCanvasType canvasType)
        {
            base.ShowCanvas((int)canvasType);
        }

        /// <summary>
        /// キャンバスを開き直す
        /// NOTE: 通常のShowCanvasメソッドだと同じCanvasを開こうとしたときにreturnされてしまうので
        /// こちらのメソッドを使う
        /// </summary>
        public void ShowCanvasReopen(BattleCanvasType canvasType)
        {
            base.ShowCanvasReopen((int)canvasType);
        }
    }
}
