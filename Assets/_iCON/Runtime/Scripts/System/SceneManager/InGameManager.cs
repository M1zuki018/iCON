using System;
using CryStar.Attribute;
using CryStar.Core;
using CryStar.Core.Enums;
using CryStar.Core.ReactiveExtensions;
using CryStar.Field.Map;
using CryStar.Field.UI;
using CryStar.Story.Orchestrators;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.Enums;
using UnityEngine;

namespace iCON.System
{
    /// <summary>
    /// インゲームのGameManager
    /// </summary>
    public class InGameManager : CustomBehaviour
    {
        /// <summary>
        /// 現在のイベントのindex
        /// </summary>
        private static ReactiveProperty<int> _currentEventIndex = new ReactiveProperty<int>(1);
        
        /// <summary>
        /// ストーリーオーケストレーター
        /// </summary>
        [SerializeField, HighlightIfNull]
        private StoryOrchestrator _storyOrchestrator;
        
        /// <summary>
        /// Field View
        /// </summary>
        [SerializeField]
        private FieldView _fieldView;
        
        /// <summary>
        /// マップ管理クラス
        /// </summary>
        [SerializeField]
        private MapInstanceManager _mapInstanceManager;

        /// <summary>
        /// 現在のInGameの状態のリアクティブプロパティ
        /// </summary>
        private readonly ReactiveProperty<InGameStateType> _currentStateProp = new ReactiveProperty<InGameStateType>(InGameStateType.Field);
        
        /// <summary>
        /// 現在のInGameの状態のリアクティブプロパティ
        /// </summary>
        public ReadOnlyReactiveProperty<InGameStateType> CurrentStateProp => _currentStateProp;
        
        public override async UniTask OnAwake()
        {
            ServiceLocator.Register(this, ServiceType.Local);
            
            await base.OnAwake();
            
            // ストーリー再生時以外はゲームオブジェクトを非アクティブにしておく
            _storyOrchestrator.gameObject.SetActive(false);
            
            // NOTE: ロード画面が表示されている間に事前ロードまで進めておき、スムーズにゲームを進める
            await _storyOrchestrator.LoadSceneDataAsync(1);
        }

        private async void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F8))
            {
                await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.Title, true, true));
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.F9))
            {
                await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.Battle, false, true));
            }
        }

        #region ストーリー

        /// <summary>
        /// ストーリーを再生する
        /// </summary>
        public async UniTask PlayStory(int storyId)
        {
            // 状態をストーリー中に変更する
            _currentStateProp.Value = InGameStateType.Story;
            
            _storyOrchestrator.gameObject.SetActive(true);
            
            // UniTaskCompletionSourceで完了を待機
            var completionSource = new UniTaskCompletionSource<int>();
            
            _storyOrchestrator.PlayStoryAsync(storyId,
                () =>
                {
                    _storyOrchestrator.gameObject.SetActive(false);
                    
                    // 状態をFieldに変更する
                    _currentStateProp.Value = InGameStateType.Field;
                    
                    // ストーリー読了を記録
                    StoryUserData.AddStoryClearData(storyId);
                    
                    // ストーリー完了を通知
                    completionSource.TrySetResult(storyId);
                }).Forget();
            
            // ストーリー完了まで待機
            await completionSource.Task;
        }

        /// <summary>
        /// ストーリーの事前ロードを行う
        /// </summary>
        public async UniTask PreloadStoryAsync(int[] storyIdArray)
        {
            foreach (var storyId in storyIdArray)
            {
                await _storyOrchestrator.LoadSceneDataAsync(storyId);
            }
            LogUtility.Info($"{storyIdArray.Length}件 ストーリーのプリロードを行いました", LogCategory.System);
        }
        
        #endregion

        #region フィールド

        /// <summary>
        /// マップのInstanceを削除してから新しいマップを生成
        /// </summary>
        public void RemoveAndShowMap(int newMapId)
        {
            _mapInstanceManager.RemoveAndShowMap(newMapId);
        }

        /// <summary>
        /// 目標UIを表示する
        /// </summary>
        public async UniTask ShowObjective(string message)
        {
            await _fieldView.ShowObjectiveText(message);
        }
        
        #endregion

        [MethodButtonInspector]
        public void Reset()
        {
            _currentEventIndex.Value = 1;
        }
    }
}