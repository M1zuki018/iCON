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
        /// イベント終了アクション
        /// </summary>
        private event Action<int> _onEventEnd;
        
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
            await base.OnAwake();
            
            ServiceLocator.Register(this, ServiceType.Local);
            
            // ストーリー再生時以外はゲームオブジェクトを非アクティブにしておく
            _storyOrchestrator.gameObject.SetActive(false);
            
            // NOTE: ロード画面が表示されている間に事前ロードまで進めておき、スムーズにゲームを進める
            await _storyOrchestrator.LoadSceneDataAsync(1);
            
            _onEventEnd += index => EndEvent(index).Forget();
        }

        private void Start()
        {
            // イベント開始処理
            // NOTE: ロードに被らないようにMonoBehaviorのStartで行う
            _currentEventIndex.Subscribe(x => PlayEvent(x).Forget());
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
        public void PlayStory(int storyId)
        {
            // 状態をストーリー中に変更する
            _currentStateProp.Value = InGameStateType.Story;
            
            _storyOrchestrator.gameObject.SetActive(true);
            _storyOrchestrator.PlayStoryAsync(storyId,
                () =>
                {
                    _storyOrchestrator.gameObject.SetActive(false);
                    
                    // イベント終了を通知
                    _onEventEnd?.Invoke(storyId);
                }).Forget();
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
        
        // TODO: 動くものは作ったのであとで設計の手直しを行う
        private async UniTask PlayEvent(int index)
        {
            switch (index)
            {
                case 1:
                    PlayStory(1);
                    break;
                case 2:
                    PlayStory(2);
                    break;
                case 3:
                    await ShowObjective("衣装スタッフに声をかける");
                    await PreloadStoryAsync(new int[3]{3,4,5});
                    break;
                case 8:
                    PlayStory(6);
                    break;
                case 9:
                    PlayStory(7);
                    break;
            }
        }

        private async UniTask EndEvent(int index)
        {
            switch (index)
            {
                case 1:
                case 2:
                    break;
                case 5:
                    _mapInstanceManager.RemoveAndShowMap(3);
                    _currentEventIndex.Value = 7;
                    break;
                case 6:
                    await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.Battle, false, true));
                    break;
                case 7:
                    // ゲームクリア
                    await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.Title, true, true));
                    _currentEventIndex.Value = 1;
                    return;
            }
            
            _currentEventIndex.Value += 1;
            
            // 状態をFieldに変更する
            _currentStateProp.Value = InGameStateType.Field;
        }
    }
}