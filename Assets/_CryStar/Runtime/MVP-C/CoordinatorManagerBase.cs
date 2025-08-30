using System.Collections.Generic;
using CryStar.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// MVP-Cパターンのマネージャーの基底クラス
/// </summary>
public abstract class CoordinatorManagerBase : CustomBehaviour
{
    [SerializeField] protected List<CoordinatorBase> _coordinators = new List<CoordinatorBase>();
    [SerializeField] protected int _defaultIndex = 1;
    
    protected Stack<int> _stack = new Stack<int>(); // スタック管理用
    protected int _currentIndex = -1; // 現在アクティブなコーディネーターのインデックス
    
    /// <summary>
    /// 現在アクティブなコーディネーターを取得する
    /// </summary>
    public CoordinatorBase CurrentCanvas => _coordinators[_currentIndex];
    
    public override UniTask OnStart()
    {
        ShowCanvas(_defaultIndex);
        return base.OnStart();
    }

    /// <summary>
    /// 指定したキャンバスを表示し、それ以外を非表示にする
    /// </summary>
    public virtual void ShowCanvas(int index)
    {
        // 同じキャンバスを表示しようとしている場合は何もしない
        if (_currentIndex == index && _stack.Count == 1 && _stack.Peek() == index)
        {
            Debug.LogWarning($"同じキャンバスを開こうとしています: {index}");
            return;   
        }
        
        Show(index);
    }

    /// <summary>
    /// キャンバスを開き直す
    /// NOTE: 通常のShowCanvasメソッドだと同じCanvasを開こうとしたときにreturnされてしまうので
    /// こちらのメソッドを使う
    /// </summary>
    public virtual void ShowCanvasReopen(int index)
    {
        Show(index);
    }
    
    /// <summary>
    /// 現在の画面の上に新しい画面をオーバーレイとして表示する
    /// </summary>
    public virtual void PushCanvas(int index)
    {
        // インデックスの範囲チェック
        if (index < 0 || index >= _coordinators.Count)
        {
            Debug.LogError($"キャンバスインデックスが範囲外です: {index}");
            return;
        }
        
        // 同じキャンバスが最上位に既に表示されている場合は何もしない
        if (_currentIndex == index)
            return;
        
        // 現在のCanvasの切り替わり処理を実行
        _coordinators[_currentIndex]?.Exit();
        
        // キャンバス切り替え
        for (int i = 0; i < _coordinators.Count; i++)
        {
            if (i != index)
            {
                // 対象以外のコーディネーターは反応しないようにする
                _coordinators[i]?.Block();
            }
        }
        
        _stack.Push(index); // スタックに新しいインデックスをプッシュ
        _currentIndex = index; // 現在のインデックスを更新
        _coordinators[index].Enter(); // 新しいCanvasの切り替わり処理を実行
    }

    /// <summary>
    /// 最上位の画面を閉じて、一つ前の画面に戻る
    /// </summary>
    public virtual void PopCanvas()
    {
        // スタックが空または1つしかない場合は何もしない
        if (_stack.Count <= 1)
        {
            Debug.Log("これ以上前の画面に戻れません");
            return;
        }
        
        int currentIndex = _stack.Pop(); // 現在の画面をポップ
        int previousIndex = _stack.Peek(); // 一つ前の画面を取得
        
        // 現在のCanvasの切り替わり処理を実行
        _coordinators[currentIndex]?.Exit();
        _coordinators[previousIndex]?.Unblock();　// 一つ前の画面のブロックを解除
        
        _currentIndex = previousIndex; // 現在のインデックスを更新
        _coordinators[previousIndex].Enter(); // 新しいCanvasの切り替わり処理を実行
    }
    
    /// <summary>
    /// 特定のインデックスまでスタックをポップする
    /// </summary>
    public virtual void PopToCanvas(int targetIndex)
    {
        // スタックに目的のインデックスがない場合
        if (!IsCanvasInStack(targetIndex))
        {
            Debug.LogError($"指定されたインデックス {targetIndex} はスタック内に存在しません");
            return;
        }
        
        // 目的のインデックスが最上位の場合は何もしない
        if (_currentIndex == targetIndex)
            return;
            
        // 現在のCanvasの切り替わり処理を実行
        _coordinators[_currentIndex]?.Exit();
            
        // 目的のインデックスが出てくるまでポップして非表示にする
        while (_stack.Count > 0 && _stack.Peek() != targetIndex)
        {
            int index = _stack.Pop();
            _coordinators[index]?.Exit();
        }
        
        // 目的のキャンバスをUnblockする
        _coordinators[targetIndex]?.Unblock();
        
        _currentIndex = targetIndex; // 現在のインデックスを更新
        _coordinators[targetIndex].Enter(); // 新しいCanvasの切り替わり処理を実行
    }
    
    /// <summary>
    /// スタック内に特定のキャンバスが存在するかチェック
    /// </summary>
    protected bool IsCanvasInStack(int index)
    {
        return _stack.Contains(index);
    }
    
    /// <summary>
    /// 現在のキャンバスインデックスを取得
    /// </summary>
    public int GetCurrentCanvasIndex()
    {
        return _currentIndex;
    }

    /// <summary>
    /// キャンバスの参照を取得する
    /// </summary>
    public WindowBase GetCanvas(int index)
    {
        return _coordinators[index];
    }

    /// <summary>
    /// Canvasを開く
    /// </summary>
    private void Show(int canvasIndex)
    {
        // インデックスの範囲チェック
        if (canvasIndex < 0 || canvasIndex >= _coordinators.Count)
        {
            Debug.LogError($"キャンバスインデックスが範囲外です: {canvasIndex}");
            return;
        }

        if (_currentIndex >= 0)
        {
            // 現在のCanvasの切り替わり処理を実行
            // NOTE: 初期値を-1にしているためエラーにならないように条件文を書いている
            _coordinators[_currentIndex]?.Exit();
        }
        
        // 全てのキャンバスを非表示にする
        foreach (var canvas in _coordinators)
        {
            canvas?.Exit();
        }
        
        // スタックをクリアして新しいインデックスをプッシュ
        _stack.Clear();
        _stack.Push(canvasIndex);
        
        _currentIndex = canvasIndex; // 現在のインデックスを更新
        
        // 新しいCanvasの切り替わり処理を実行
        _coordinators[canvasIndex]?.Enter();
    }
}
