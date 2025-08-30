using System.Collections.Generic;
using CryStar.CommandBattle.Data;
using Cysharp.Threading.Tasks;
using iCON.Enums;
using UnityEngine;

namespace iCON.UI
{
    /// <summary>
    /// バトルシーンのCanvasManager
    /// </summary>
    public class BattleCanvasManager : SceneCanvasManagerBase
    {
        /// <summary>
        /// キャラクターアイコンのPrefab
        /// TODO: 管理方法は検討中
        /// </summary>
        [SerializeField]
        private CharacterIconContents _unitIconPrefab;
        private List<CharacterIconContents> _icons;
        
        [SerializeField]
        private Transform _unitIconParent;

        /// <summary>
        /// エネミーアイコンのPrefab
        /// </summary>
        [SerializeField] 
        private EnemyIconContents _enemyIconPrefab;
        private List<EnemyIconContents> _enemyIcons;

        [SerializeField] 
        private Transform _enemyIconParent;
         
        /// <summary>
        /// ダメージテキストのオブジェクトプールを管理するクラス
        /// </summary>
        [SerializeField]
        private DamageTextPool _damageTextPool;

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
            var index = (int)canvasType;
            
            // インデックスの範囲チェック
            if (index < 0 || index >= _canvasObjects.Count)
            {
                Debug.LogError($"キャンバスインデックスが範囲外です: {index}");
                return;
            }
        
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
        /// キャラクターアイコンを生成する
        /// </summary>
        public async UniTask SetupIcons(IReadOnlyList<BattleUnitData> unitData, IReadOnlyList<BattleUnitData> enemyData)
        {
            // リストを新規作成
            _icons = new List<CharacterIconContents>(unitData.Count);
            
            for (int i = 0; i < unitData.Count; i++)
            {
                var icon = Instantiate(_unitIconPrefab, _unitIconParent);
                _icons.Add(icon);
                icon.Setup(_damageTextPool);
                await icon.SetIcon(unitData[i].UserData.IconPath);
            }
            
            SubscribeToUnitEvents(unitData);

            _enemyIcons = new List<EnemyIconContents>(enemyData.Count);
            
            for (int i = 0; i < enemyData.Count; i++)
            {
                var icon = Instantiate(_enemyIconPrefab, _enemyIconParent);
                _enemyIcons.Add(icon);
                icon.Setup(_damageTextPool);
                await icon.SetIcon(enemyData[i].UserData.IconPath);
            }
            
            SubscribeToEnemyEvents(enemyData);
        }

        /// <summary>
        /// キャラクターのHP・SP変動アクションを購読する
        /// </summary>
        private void SubscribeToUnitEvents(IReadOnlyList<BattleUnitData> unitData)
        {
            // 味方ユニットのイベント購読
            for (int i = 0; i < unitData.Count; i++)
            {
                var unit = unitData[i];
                var index = i; // ローカル変数でキャプチャ
                
                unit.OnHpChanged += (currentHp, maxHp, damage) => UpdatePlayer(index, currentHp, maxHp, damage);
                //unit.OnSpChanged += (newSp) => UpdatePlayer(index);
                unit.OnDeath += () => DisEnableIcon(index, true);
            }
        }
        
        /// <summary>
        /// エネミーのHP変動アクションを購読する
        /// </summary>
        private void SubscribeToEnemyEvents(IReadOnlyList<BattleUnitData> enemyData)
        {
            // エネミーのイベント購読
            for (int i = 0; i < enemyData.Count; i++)
            {
                var unit = enemyData[i];
                var index = i; // ローカル変数でキャプチャ
                
                unit.OnHpChanged += (currentHp, maxHp, damage) => UpdateEnemy(index, damage);
                unit.OnDeath += () => DisEnableIcon(index, false);
            }
        }

        /// <summary>
        /// ダメージを受けた時のUI更新
        /// </summary>
        private void UpdatePlayer(int index, int value, int maxValue, int damage)
        {
            // HPバーを更新
            _icons[index].SetHpSlider(value, maxValue);
            
            // ダメージ量の表記を行う
            _icons[index].SetDamageText(damage).Forget();
        }

        /// <summary>
        /// 敵がダメージを受けた時のUI更新
        /// </summary>
        private void UpdateEnemy(int index, int damage)
        {
            _enemyIcons[index].SetDamageText(damage).Forget();
        }

        /// <summary>
        /// 死亡時にアイコンを非表示にする
        /// </summary>
        private void DisEnableIcon(int index, bool isPlayer)
        {
            if (isPlayer)
            {
                _icons[index].Hide();
            }
            else
            {
                _enemyIcons[index].Hide();
            }
        }
    }

}
