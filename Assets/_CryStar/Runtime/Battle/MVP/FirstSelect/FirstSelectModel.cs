using CryStar.Core;
using Cysharp.Threading.Tasks;
using iCON.Battle;
using iCON.Enums;

public class FirstSelectModel
{
    private BattleManager _battleManager;
    
    public void Setup()
    {
        _battleManager = ServiceLocator.GetLocal<BattleManager>();
    }
    
    /// <summary>
    /// バトルを開始してコマンド選択に移る
    /// </summary>
    public void StartBattle()
    {
        _battleManager.PlaySelectedSe(true).Forget();
        _battleManager.SetState(BattleSystemState.CommandSelect);
    }

    /// <summary>
    /// 逃走チェック
    /// </summary>
    public void TryEscape()
    {
        _battleManager.PlaySelectedSe(false).Forget();
        _battleManager.SetState(BattleSystemState.TryEscape);
    }
}
