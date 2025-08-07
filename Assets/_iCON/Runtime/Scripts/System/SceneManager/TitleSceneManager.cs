using CryStar.Attribute;
using CryStar.Core;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.Constants;
using iCON.Performance;
using iCON.UI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace iCON.System
{
    /// <summary>
    /// タイトルシーンのマネージャー
    /// </summary>
    public class TitleSceneManager : CustomBehaviour
    {
        /// <summary>
        /// タイトルスプラッシュの演出マネージャー
        /// </summary>
        [SerializeField, HighlightIfNull]
        private TitleSplashManager _titleSplashManager;

        /// <summary>
        /// CanvasController
        /// </summary>
        [SerializeField]
        private CanvasController_Title _canvasController;
        
        /// <summary>
        /// Title用BGMのパス
        /// </summary>
        [SerializeField] 
        private string _bgmPath;
        
        /// <summary>
        /// BGMのフェードアウト
        /// </summary>
        [SerializeField]
        private float _bgmFadeDuration = 2f;
        
        // TODO: 仮。AudioListenerとEventSystemをシーン変更前に一度削除して、警告を出さないようにしている
        [SerializeField] private AudioListener _audioListener;
        [SerializeField] private EventSystem _eventSystem;

        /// <summary>
        /// AudioManager
        /// </summary>
        private AudioManager _audioManager;

        /// <summary>
        /// タイトルスプラッシュ完了済み
        /// </summary>
        private bool _isSplashCompleted;

        #region Life cycle
        
        /// <summary>
        /// Start
        /// </summary>
        public override UniTask OnStart()
        {
            if (ServiceLocator.GetGlobal<SceneLoader>().IsLoading)
            {
                // ロード中であれば不正な処理なのでreturn
                return base.OnStart();
            }
            
            if (ValidateComponents())
            {
                // コンポーネントの検証を行う
                return base.OnStart();
            }
            
            _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            
            // スタートボタン押下処理を追加
            _canvasController.OnStartButtonClicked += OnStartButtonClicked;
            
            // タイトルスプラッシュのゲームオブジェクトを確実にアクティブにしておく
            _titleSplashManager.gameObject.SetActive(true);
            _titleSplashManager.SetupEndAction(OnSplashCompleted);
            
            // タイトルスプラッシュの演出を開始
            _titleSplashManager.Play();
            
            return base.OnStart();
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
            {
                _titleSplashManager.Skip();
            }
        }

        private void OnDestroy()
        {
            if (_canvasController != null)
            {
                _canvasController.OnStartButtonClicked -= OnStartButtonClicked;
            }
        }
        
        #endregion

        /// <summary>
        /// スプラッシュ演出完了時の処理
        /// </summary>
        private void OnSplashCompleted() => PlayTitleBGMAsync().Forget();

        /// <summary>
        /// スタートボタンクリック時の処理
        /// </summary>
        private void OnStartButtonClicked() => TransitionToInGameAsync().Forget();
        
        /// <summary>
        /// コンポーネントの検証
        /// </summary>
        private bool ValidateComponents()
        {
            if (_titleSplashManager == null)
            {
                LogUtility.Error("TitleSplashManagerがアサインされていません", LogCategory.System);
                return true;
            }

            if (_canvasController == null)
            {
                LogUtility.Error("Canvas Controllerがアサインされていません", LogCategory.System);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// TitleBGMを再生する
        /// </summary>
        private async UniTask PlayTitleBGMAsync()
        {
            if (_isSplashCompleted)
            {
                // 既にスプラッシュを終えてBGMを流していたら以降の処理は行わない
                return;
            }
            
            if (_bgmPath == null || string.IsNullOrEmpty(_bgmPath))
            {
                return;
            }
            
            // 二度処理が行われないようにフラグを立てる
            _isSplashCompleted = true;
            await _audioManager.PlayBGMWithFadeIn(_bgmPath, _bgmFadeDuration);
        }

        /// <summary>
        /// インゲームシーンへ遷移する
        /// </summary>
        private async UniTask TransitionToInGameAsync()
        {
            await _audioManager.PlaySE(KSEPath.Select, 1);
            
            // BGMのフェードアウト後にシーン遷移
            await _audioManager.FadeOutBGM(_bgmFadeDuration);

            // TODO: 仮。警告が出ないように
            _audioListener.enabled = false;
            _eventSystem.enabled = false;
            
            await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.InGame, true));
        }
    }
}