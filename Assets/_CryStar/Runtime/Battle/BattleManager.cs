using System;
using System.Collections.Generic;
using System.Linq;
using CryStar.Attribute;
using CryStar.Audio;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.Enums;
using iCON.UI;
using UnityEngine;

namespace iCON.Battle
{
    /// <summary>
    /// バトルを管理するクラス
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        /// <summary>
        /// View
        /// </summary>
        [SerializeField, HighlightIfNull]
        private BattleCanvasManager _view;
        
        /// <summary>
        /// バトルの進行状態
        /// </summary>
        [SerializeField]
        private BattleSystemState _currentState;

        /// <summary>
        /// 戦闘BGMのPath　TODO: 仮
        /// </summary>
        [SerializeField] 
        private string _bgmPath;

        /// <summary>
        /// ダメージを受けたときのSEのPath TODO: 仮
        /// </summary>
        [SerializeField] 
        private string _damagedSePath;
        
        /// <summary>
        /// 行動選択をしたときのSEのPath TODO: 仮
        /// </summary>
        [SerializeField]
        private string _selectSePath;
        
        /// <summary>
        /// 重要な行動選択をしたときのSEのPath TODO: 仮
        /// </summary>
        [SerializeField]
        private string _selectSePath2;
        
        /// <summary>
        /// 操作をキャンセルしたときのSEのPath TODO: 仮
        /// </summary>
        [SerializeField]
        private string _cancelSePath;
        
        /// <summary>
        /// 現在のステートのステートマシン用クラスの参照
        /// </summary>
        private BattleStateBase _currentStateHandler;
        
        /// <summary>
        /// Enumとステートマシン用クラスのkvpの辞書
        /// </summary>
        private Dictionary<BattleSystemState, BattleStateBase> _states;

        /// <summary>
        /// バトルで使用する変数をまとめたクラス
        /// </summary>
        private BattleData _data;

        /// <summary>
        /// 実行待ちのコマンドを記録しておくリスト
        /// </summary>
        private List<BattleCommandEntry> _commandList = new List<BattleCommandEntry>();
        
        /// <summary>
        /// 現在コマンドを選んでいるキャラクターのIndex
        /// </summary>
        private int _currentCommandSelectIndex = 0;
        
        /// <summary>
        /// AudioManager
        /// </summary>
        private AudioManager _audioManager;
        
        /// <summary>
        /// バトルで使用する変数をまとめたクラス
        /// </summary>
        public BattleData Data => _data;
        
        /// <summary>
        /// 現在コマンドを選んでいるキャラクターのデータ
        /// </summary>
        public BattleUnit CurrentSelectingUnit => _currentCommandSelectIndex < _data.UnitCount ? 
            _data.UnitData[_currentCommandSelectIndex] : null;
        
        /// <summary>
        /// AudioManager
        /// </summary>
        public AudioManager AudioManager => _audioManager;

        #region Life cycle

        private void Awake()
        {
            // サービスロケーターに登録（特にGrobalで使用する必要はないのでLocalで登録する）
            ServiceLocator.Register(this, ServiceType.Local);
            // ステートマシンの初期化
            InitializeStates();
            SetState(_currentState);
        }

        private void Start()
        {
            // バトルデータ作成
            _data = new BattleData(new List<int>{1}, new List<int>{2}, _bgmPath);
            
            // アイコンを用意する
            _view.SetupIcons(_data.UnitData, _data.EnemyData).Forget();

            if (_audioManager == null)
            {
                // 参照が無ければServiceLocatorから取得
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }
            
            // 戦闘BGMを再生する
            _audioManager.PlayBGM(_bgmPath).Forget();
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                _currentStateHandler.Cancel();
            }
        }

        #endregion

        /// <summary>
        /// バトルの状態を変更する
        /// </summary>
        public void SetState(BattleSystemState state)
        {
            try
            {
                _currentStateHandler?.Exit();
                _currentState = state;
                _currentStateHandler = _states[state];
                _currentStateHandler.Enter(this, _view);
            }
            catch(Exception e)
            {
                LogUtility.Error($"バトル進行中に例外が発生しました {e.Message}", LogCategory.Gameplay);
            }
        }

        /// <summary>
        /// コマンドをリストに追加
        /// </summary>
        public void AddCommandList(CommandType commandType, BattleUnit[] targets = null)
        {
            // コマンドを取得
            var command = BattleCommandFactory.GetCommand(commandType);
            if (command == null)
            {
                LogUtility.Error($"未知のコマンドです: {commandType}", LogCategory.Gameplay);
                return;
            }
            
            // コマンドの実行者は現在コマンドを選択中のキャラクターとする
            var executor = CurrentSelectingUnit;
            if (executor == null)
            {
                LogUtility.Error("実行者が設定されていません", LogCategory.Gameplay);
                return;
            }
            
            // デフォルトターゲットの設定 TODO: 今後対象を選択式にする
            if (targets == null || targets.Length == 0)
            {
                targets = GetDefaultTargets(commandType);
            }
            
            // 実行待ちコマンドを登録
            var entry = new BattleCommandEntry(executor, command, targets);
            _commandList.Add(entry);
            
            LogUtility.Info($"{executor.Name}のコマンド登録: {commandType}");
        }
        
        /// <summary>
        /// 次のキャラクターのコマンド選択に移る
        /// </summary>
        /// <returns>次に移れる場合はtrue</returns>
        public bool CheckNextCommandSelect()
        {
            _currentCommandSelectIndex++;
            return _currentCommandSelectIndex < _data.UnitCount;
        }

        /// <summary>
        /// コマンドリストを作成する
        /// </summary>
        /// <returns></returns>
        public List<BattleCommandEntry> CreateCommandList()
        {
            // 敵のAI行動を追加
            AddEnemyCommands();
            
            // コマンドを優先度順にソート（コマンドの優先順->攻撃速度）
            _commandList = _commandList
                .OrderByDescending(entry => entry.Priority)
                .ThenByDescending(entry => entry.Executor.Speed)
                .ToList();
            
            return _commandList;
        }

        /// <summary>
        /// コマンドを実行する
        /// </summary>
        public async UniTask<string> ExecuteCommandAsync(BattleCommandEntry entry)
        {
            // コマンドの実行終了を待機
            var result = await entry.Command.ExecuteAsync(entry.Executor, entry.Targets);
            
            await PlayDamageSound();
                
            if (result.IsSuccess)
            {
                LogUtility.Info(result.Message);
                    
                // エフェクト処理
                foreach (var effect in result.Effects)
                {
                    // TODO: エフェクトの表示処理
                    await UniTask.Delay(100); // エフェクトの表示時間
                }
                    
            }
            else
            {
                LogUtility.Warning($"コマンド実行失敗: {result.Message}");
            }
            
            return result.Message;
        }
        
        /// <summary>
        /// バトル実行
        /// </summary>
        public async UniTask<(bool isFinish, bool isWin)> CheckBattleEndAsync()
        {
            // コマンドリストをクリア
            _commandList.Clear();
            ResetCommandSelectIndex();
            
            return CheckBattleEnd();
        }
        
        /// <summary>
        /// バトル結果のデータを取得する
        /// </summary>
        public (string name, int experience) GetResultData()
        {
            // バトルに参加しているユニットの数が1人以上であればreturnする名前に「たち」をつける
            var add = _data.UnitCount != 1 ? "たち" : "";
            var name = $"{_data.UnitData[0].Name}{add}";
            
            // TODO: 経験値取得処理
            return (name, 300);
        }

        #region サウンド関連

        /// <summary>
        /// ダメージを受けたときのSEを再生する
        /// </summary>
        public async UniTask PlayDamageSound()
        {
            if (_audioManager == null)
            {
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }

            if (!string.IsNullOrEmpty(_damagedSePath))
            {
                await _audioManager.PlaySE(_damagedSePath, 1f);
            }
        }
        
        /// <summary>
        /// ダメージを受けたときのSEを再生する
        /// </summary>
        public async UniTask PlayCancelSound()
        {
            if (_audioManager == null)
            {
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }

            if (!string.IsNullOrEmpty(_cancelSePath))
            {
                await _audioManager.PlaySE(_cancelSePath, 1f);
            }
        }
        
        /// <summary>
        /// BGM再生を止める
        /// </summary>
        public void FinishBGM()
        {
            if (_audioManager == null)
            {
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }
            
            _audioManager.FadeOutBGM(0.5f).Forget();
        }

        /// <summary>
        /// 選択したときのSEを再生する
        /// </summary>
        /// <param name="isImportant">重要な選択のSEを使用するか</param>
        public async UniTask PlaySelectedSe(bool isImportant)
        {
            if (_audioManager == null)
            {
                _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            }

            if (!string.IsNullOrEmpty(_selectSePath) && !string.IsNullOrEmpty(_selectSePath2))
            {
                await _audioManager.PlaySE(isImportant ? _selectSePath2 : _selectSePath, 1f);
            }
        }

        #endregion
        
        
        /// <summary>
        /// StateMachineの初期化
        /// </summary>
        private void InitializeStates()
        {
            _states = new Dictionary<BattleSystemState, BattleStateBase>();
    
            try
            {
                _states.Add(BattleSystemState.FirstSelect, new FirstSelectState());
                _states.Add(BattleSystemState.TryEscape, new TryEscapeState());
                _states.Add(BattleSystemState.CommandSelect, new CommandSelectState());
                _states.Add(BattleSystemState.Execute, new ExecuteState());
                _states.Add(BattleSystemState.Win, new WinState());
                _states.Add(BattleSystemState.Lose, new LoseState());
                _states.Add(BattleSystemState.Idea, new IdeaState());
            }
            catch(Exception e)
            {
                LogUtility.Error($"State初期化中に例外が発生: {e.Message}", LogCategory.Gameplay);
            }
        }

        #region Battle Private Methods

        /// <summary>
        /// デフォルトターゲットを取得
        /// </summary>
        private BattleUnit[] GetDefaultTargets(CommandType commandType)
        {
            return commandType switch
            {
                // TODO: 仮実装。攻撃の場合はターゲットに生存している敵を1体設定
                CommandType.Attack => _data.EnemyData.Where(u => u.IsAlive).Take(1).ToArray(),
                CommandType.Idea => _data.EnemyData.Where(u => u.IsAlive).Take(1).ToArray(),
                
                // ガードは自身をターゲットに設定
                CommandType.Guard => new BattleUnit[] { CurrentSelectingUnit },
                _ => Array.Empty<BattleUnit>()
            };
        }
        
        /// <summary>
        /// 敵のAI行動を追加
        /// </summary>
        private void AddEnemyCommands()
        {
            foreach (var enemy in _data.EnemyData.Where(u => u.IsAlive))
            {
                // TODO: 仮実装として常に攻撃としている
                var command = BattleCommandFactory.GetCommand(CommandType.Attack);
                var targets = _data.UnitData.Where(u => u.IsAlive).Take(1).ToArray();
                
                if (command != null && targets.Length > 0)
                {
                    var entry = new BattleCommandEntry(enemy, command, targets);
                    _commandList.Add(entry);
                }
            }
        }
        
        /// <summary>
        /// バトル終了条件をチェック
        /// </summary>
        private (bool isFinish, bool isWin) CheckBattleEnd()
        {
            // 戦闘に参加しているすべてのUnitの生存状態を調べる
            bool allPlayersDead = _data.UnitData.All(u => !u.IsAlive);
            bool allEnemiesDead = _data.EnemyData.All(u => !u.IsAlive);
            
            if (allPlayersDead)
            {
                LogUtility.Info("プレイヤーの敗北");
                return (true, false);
            }
            
            if (allEnemiesDead)
            {
                LogUtility.Info("プレイヤーの勝利");
                return (true, true);
            }
            
            return (false, false);
        }
        
        /// <summary>
        /// コマンド選択インデックスをリセット
        /// </summary>
        private void ResetCommandSelectIndex()
        {
            _currentCommandSelectIndex = 0;
        }

        #endregion
    }
}
