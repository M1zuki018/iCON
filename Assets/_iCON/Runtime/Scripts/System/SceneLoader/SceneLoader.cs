using System;
using System.Threading;
using CryStar.Core;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.Constants;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace iCON.System
{
    /// <summary>
    /// SceneLoader
    /// </summary>
    // NOTE: ServiceLocatorが-1000番を使用
    [DefaultExecutionOrder(-999)]
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// タイムアウトとするまでの時間
        /// </summary>
        [SerializeField]
        private float _loadingTimeout = 30f;
        
        /// <summary>
        /// シーン初期化のタイムアウト時間
        /// </summary>
        [SerializeField]
        private float _initializationTimeout = 15f;
        
        /// <summary>
        /// 遷移状態
        /// </summary>
        private LoadingStateType _loadingState = LoadingStateType.None;
        
        /// <summary>
        /// 現在のシーン
        /// </summary>
        private SceneType _currentScene;
        
        /// <summary>
        /// ロード操作をキャンセルするためのトークン
        /// </summary>
        private CancellationTokenSource _cts;
        
        /// <summary>
        /// 遷移状態
        /// </summary>
        public LoadingStateType LoadingState => _loadingState;
        
        /// <summary>
        /// 現在のシーン
        /// </summary>
        public SceneType CurrentScene => _currentScene;
        
        /// <summary>
        /// ロード中か
        /// </summary>
        public bool IsLoading => _loadingState == LoadingStateType.Loading;

        #region Lifecycle
        /// <summary>
        /// Awake
        /// </summary>
        private void Awake()
        {
            ServiceLocator.Register(this);
            _currentScene = GetCurrentSceneType();
        }

        private void OnDestroy()
        {
            CancelCurrentOperation();
            _cts?.Dispose();
            _cts = null;
        }
        #endregion
        
        /// <summary>
        /// 画面遷移を行う
        /// </summary>
        public async UniTask<bool> LoadSceneAsync(SceneTransitionData data)
        {
            // シーン読み込み中か確認
            if (IsLoading)
            {
                LogUtility.Error("現在シーン読み込み中です。リクエストを実行しません", LogCategory.System);
                return false;
            }
            
            // 念のため前回のロード操作をキャンセルしておく
            CancelCurrentOperation();
            _cts = new CancellationTokenSource();

            try
            {
                // 状態をロード中に変更
                _loadingState = LoadingStateType.Loading;

                // 遷移処理を実行
                bool result = await ExecuteSceneTransition(data, _cts.Token);

                if (result)
                {
                    // 遷移成功時は現在のシーン情報とロード状態を更新する
                    _currentScene = data.TargetScene;
                    _loadingState = LoadingStateType.Completed;
                    LogUtility.Info($"シーン遷移が完了しました: {data.TargetScene} CurrentScene: {_currentScene}", LogCategory.System);
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                _loadingState = LoadingStateType.None;
                return false;
            }
            catch (Exception e)
            {
                LogUtility.Error($"シーン遷移で予期しないエラー: {e.Message}", LogCategory.System);
                _loadingState = LoadingStateType.Failed;
                return false;
            }
        }
        
        /// <summary>
        /// 進行しているロード操作をキャンセルする
        /// </summary>
        public void CancelCurrentOperation()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts?.Cancel();
                _loadingState = LoadingStateType.None;
                LogUtility.Info("シーン遷移をキャンセルしました", LogCategory.System);
            }
        }

        #region Private Methods

        /// <summary>
        /// シーン遷移の実行
        /// </summary>
        private async UniTask<bool> ExecuteSceneTransition(SceneTransitionData data, CancellationToken token)
        {
            try
            {
                // タイムアウト設定
                var timeoutTask = UniTask.Delay(delayTimeSpan:TimeSpan.FromSeconds(_loadingTimeout), cancellationToken: token);
                
                // シーンの初期化完了を待機
                WaitForSceneInitialization(token).Forget();
                
                // シーンを保存しておく
                // NOTE: LoadSceneModeをAdditiveにしているため、自分でシーンを破棄する必要がある
                Scene currentScene = SceneManager.GetActiveScene();
                Scene loadingScene = default;
                
                // ロードシーンの表示
                if (data.UseLoadingScreen)
                {
                    loadingScene = await LoadLoadingScreenAsync(token);
                }

                // アセットのプリロードを行うか確認
                if (data.PreloadAssets)
                {
                    await PreloadAssetsAsync(token);
                }

                // 次のシーンをロードする処理
                var loadTask = LoadTargetSceneAsync(data.TargetScene, token);
                
                // ロードが早く終わるかタイムアウトが早いか判定
                var completedTask = await UniTask.WhenAny(loadTask, timeoutTask);
                if (completedTask == 1)
                {
                    // タイムアウトした場合
                    throw new TimeoutException($"シーン遷移 タイムアウト: {data.TargetScene}");
                }
                
                // ロード画面を非表示にするなど
                await SwitchToNewSceneAsync(data, loadingScene, currentScene);

                return true;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                LogUtility.Error($"シーン遷移に失敗しました: {ex.Message}", LogCategory.System);
                return false;
            }
        }
        
        /// <summary>
        /// シーンの初期化完了を待機
        /// </summary>
        private async UniTask WaitForSceneInitialization(CancellationToken token)
        {
            try
            {
                // シーン初期化開始状態にする
                SceneLoadingCoordinator.NotifySceneInitializationStarted();
                
                LogUtility.Info("🔄 シーン初期化の完了を待機中...", LogCategory.System);
                
                // 初期化タイムアウト設定
                var initTimeoutTask = UniTask.Delay(
                    delayTimeSpan: TimeSpan.FromSeconds(_initializationTimeout), 
                    cancellationToken: token
                );
                
                // 初期化完了待機
                var initWaitTask = SceneLoadingCoordinator.WaitForSceneInitializationAsync(token);
                
                // どちらか早い方を待機
                var completedTask = await UniTask.WhenAny(initWaitTask, initTimeoutTask);
                
                if (completedTask == 1)
                {
                    // 初期化がタイムアウトした場合
                    LogUtility.Warning($"⚠️ シーン初期化がタイムアウトしました（{_initializationTimeout}秒）", LogCategory.System);
                    // タイムアウトしても処理を続行（エラーにはしない）
                }
                else
                {
                    LogUtility.Info("✅ シーン初期化の完了を確認しました", LogCategory.System);
                }
            }
            catch (OperationCanceledException)
            {
                LogUtility.Info("シーン初期化の待機がキャンセルされました", LogCategory.System);
                throw;
            }
            catch (Exception ex)
            {
                LogUtility.Error($"シーン初期化の待機中にエラーが発生しました: {ex.Message}", LogCategory.System);
                // エラーが発生しても処理を続行（ロード画面を閉じる）
            }
        }

        /// <summary>
        /// ロード画面を表示するタスク
        /// </summary>
        private async UniTask<Scene> LoadLoadingScreenAsync(CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(SceneType.Load.ToString(), LoadSceneMode.Additive);
            Scene loadingScene = SceneManager.GetSceneByName(SceneType.Load.ToString());

            // カメラを有効にするためにロードシーンをアクティブに設定
            SceneManager.SetActiveScene(loadingScene);

            // 少し待機してローディング画面が確実に表示されるようにする
            await UniTask.Delay(100, cancellationToken: token);
            
            return loadingScene;
        }
        
        /// <summary>
        /// アセットのプリロード
        /// </summary>
        private async UniTask PreloadAssetsAsync(CancellationToken token)
        {
            await UniTask.Delay(KSceneManagement.ASSET_PRELOAD_WAIT_TIME_MS, cancellationToken: token);
        }
        
        /// <summary>
        /// 内部的なロード処理
        /// </summary>
        private async UniTask LoadTargetSceneAsync(SceneType targetScene, CancellationToken token)
        {
            await SceneManager.LoadSceneAsync(targetScene.ToString(), LoadSceneMode.Additive);
            token.ThrowIfCancellationRequested();
        }
        
        /// <summary>
        /// 新しいシーンへの切り替え
        /// </summary>
        private async UniTask SwitchToNewSceneAsync(SceneTransitionData data, Scene loadingScene, Scene currentScene)
        {
            // 新しいシーンを取得してアクティブに設定
            Scene newScene = SceneManager.GetSceneByName(data.TargetScene.ToString());
            SceneManager.SetActiveScene(newScene);
            
            if (currentScene.IsValid() && _currentScene.ToString() != data.TargetScene.ToString())
            {
                // 古いメインシーンをアンロード
                await SceneManager.UnloadSceneAsync(currentScene);
            }
            
            if (data.UseLoadingScreen && loadingScene.IsValid())
            {
                // ローディングスクリーンと古いシーンの削除
                await SceneManager.UnloadSceneAsync(loadingScene);
            }
        }

        /// <summary>
        /// 現在のシーンタイプを取得
        /// </summary>
        private SceneType GetCurrentSceneType()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (Enum.TryParse<SceneType>(currentSceneName, out var sceneType))
            {
                return sceneType;
            }
            return SceneType.None;
        }

        #endregion
    }
}