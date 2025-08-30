using System.Collections.Generic;
using CryStar.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// UIマネージャーの基底クラス
/// </summary>
public abstract class SceneCanvasManagerBase : CustomBehaviour
{
    [SerializeField] protected List<WindowBase> _canvasObjects = new List<WindowBase>();
    [SerializeField] protected int _defaultCanvasIndex = 0;
    
    protected Stack<int> _canvasStack = new Stack<int>(); // UIパネルのスタック管理用
    protected int _currentCanvasIndex = -1; // 現在表示中のキャンバスインデックス
    
    /// <summary>
    /// 現在表示中のウィンドウを取得する
    /// </summary>
    public WindowBase CurrentCanvas => _canvasObjects[_currentCanvasIndex];
    
    public override UniTask OnStart()
    {
        ShowCanvas(_defaultCanvasIndex);
        return base.OnStart();
    }

    /// <summary>
    /// 指定したキャンバスを表示し、それ以外を非表示にする
    /// </summary>
    public virtual void ShowCanvas(int index)
    {
        // インデックスの範囲チェック
        if (index < 0 || index >= _canvasObjects.Count)
        {
            Debug.LogError($"キャンバスインデックスが範囲外です: {index}");
            return;
        }
        
        // 同じキャンバスを表示しようとしている場合は何もしない
        if (_currentCanvasIndex == index  && _canvasStack.Count == 1 && _canvasStack.Peek() == index)
            return;

        if (_currentCanvasIndex >= 0)
        {
            // 現在のCanvasの切り替わり処理を実行
            // NOTE: 初期値を-1にしているためエラーにならないように条件文を書いている
            _canvasObjects[_currentCanvasIndex]?.Exit();
        }
        
        // 全てのキャンバスを非表示にする
        foreach (var canvas in _canvasObjects)
        {
            canvas?.Exit();
            canvas?.Hide();
        }
        
        // スタックをクリアして新しいインデックスをプッシュ
        _canvasStack.Clear();
        _canvasStack.Push(index);
        
        _currentCanvasIndex = index; // 現在のインデックスを更新
        
        // 新しいCanvasの切り替わり処理を実行
        _canvasObjects[index]?.Enter(); 
        _canvasObjects[index]?.Show();
    }
    
    /// <summary>
    /// 現在の画面の上に新しい画面をオーバーレイとして表示する
    /// </summary>
    public virtual void PushCanvas(int index)
    {
        // インデックスの範囲チェック
        if (index < 0 || index >= _canvasObjects.Count)
        {
            Debug.LogError($"キャンバスインデックスが範囲外です: {index}");
            return;
        }
        
        // 同じキャンバスが最上位に既に表示されている場合は何もしない
        if (_currentCanvasIndex == index)
            return;
        
        // 現在のCanvasの切り替わり処理を実行
        _canvasObjects[_currentCanvasIndex]?.Exit();
        
        // キャンバス切り替え
        for (int i = 0; i < _canvasObjects.Count; i++)
        {
            if (i == index)
            {
                _canvasObjects[i]?.Show();
            }
            else
            {
                _canvasObjects[i]?.Block();
            }
        }
        
        _canvasStack.Push(index); // スタックに新しいインデックスをプッシュ
        _currentCanvasIndex = index; // 現在のインデックスを更新
        _canvasObjects[index].Enter(); // 新しいCanvasの切り替わり処理を実行
    }

    /// <summary>
    /// 最上位の画面を閉じて、一つ前の画面に戻る
    /// </summary>
    public virtual void PopCanvas()
    {
        // スタックが空または1つしかない場合は何もしない
        if (_canvasStack.Count <= 1)
        {
            Debug.Log("これ以上前の画面に戻れません");
            return;
        }
        
        int currentIndex = _canvasStack.Pop(); // 現在の画面をポップ
        int previousIndex = _canvasStack.Peek(); // 一つ前の画面を取得
        
        // 現在のCanvasの切り替わり処理を実行
        _canvasObjects[currentIndex]?.Exit();
        
        _canvasObjects[currentIndex]?.Hide();　// 現在の画面を非表示にする
        _canvasObjects[previousIndex]?.Unblock();　// 一つ前の画面のブロックを解除
        
        _currentCanvasIndex = previousIndex; // 現在のインデックスを更新
        _canvasObjects[previousIndex].Enter(); // 新しいCanvasの切り替わり処理を実行
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
        if (_currentCanvasIndex == targetIndex)
            return;
            
        // 現在のCanvasの切り替わり処理を実行
        _canvasObjects[_currentCanvasIndex]?.Exit();
            
        // 目的のインデックスが出てくるまでポップして非表示にする
        while (_canvasStack.Count > 0 && _canvasStack.Peek() != targetIndex)
        {
            int index = _canvasStack.Pop();
            _canvasObjects[index]?.Exit();
            _canvasObjects[index]?.Hide();
        }
        
        // 目的のキャンバスをUnblockする
        _canvasObjects[targetIndex]?.Unblock();
        
        _currentCanvasIndex = targetIndex; // 現在のインデックスを更新
        _canvasObjects[targetIndex].Enter(); // 新しいCanvasの切り替わり処理を実行
    }
    
    /// <summary>
    /// スタック内に特定のキャンバスが存在するかチェック
    /// </summary>
    protected bool IsCanvasInStack(int index)
    {
        return _canvasStack.Contains(index);
    }
    
    /// <summary>
    /// 現在のキャンバスインデックスを取得
    /// </summary>
    public int GetCurrentCanvasIndex()
    {
        return _currentCanvasIndex;
    }

    /// <summary>
    /// キャンバスの参照を取得する
    /// </summary>
    public WindowBase GetCanvas(int index)
    {
        return _canvasObjects[index];
    }
}