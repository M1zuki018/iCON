using CryStar.Attribute;
using CryStar.Core;
using CryStar.Utility;
using CryStar.Utility.Enum;
using Cysharp.Threading.Tasks;
using iCON.Constants;
using iCON.Enums;
using iCON.Performance;
using iCON.UI;
using UnityEngine;

namespace iCON.System
{
    /// <summary>
    /// タイトルシーンのマネージャー
    /// </summary>
    public class TitleSceneManager : MonoBehaviour
    {
        /// <summary>
        /// タイトルスプラッシュの演出マネージャー
        /// </summary>
        [SerializeField, HighlightIfNull]
        private TitleSplashManager _titleSplashManager;

        /// <summary>
        /// CanvasManager
        /// </summary>
        [SerializeField]
        private TitleCanvasManager _canvasManager;
        
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

        /// <summary>
        /// AudioManager
        /// </summary>
        private AudioManager _audioManager;
        
        /// <summary>
        /// Title
        /// </summary>
        private CanvasController_Title _titleCC;
        
        /// <summary>
        /// コンフィグ
        /// </summary>
        private CanvasController_Config _configCC;

        /// <summary>
        /// タイトルスプラッシュ完了済み
        /// </summary>
        private bool _isSplashCompleted;

        #region Life cycle

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            if (ValidateComponents())
            {
                // コンポーネントの検証を行う
                return;
            }
            
            _audioManager = ServiceLocator.GetGlobal<AudioManager>();
            
            BindTitleCanvas();
            
            // タイトルスプラッシュのゲームオブジェクトを確実にアクティブにしておく
            _titleSplashManager.gameObject.SetActive(true);
            _titleSplashManager.SetupEndAction(OnSplashCompleted);
            
            // タイトルスプラッシュの演出を開始
            _titleSplashManager.Play();
        }

        /// <summary>
        /// Update
        /// </summary>
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.Space) || UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                _titleSplashManager.Skip();
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (_canvasManager.CurrentCanvas == _configCC)
                {
                    // コンフィグ画面が開かれていたら、画面を閉じる
                    _canvasManager.PopCanvas();
                }
            }
        }

        private void OnDestroy()
        {
            if (_titleCC != null)
            {
                _titleCC.OnNewGameButtonClicked -= OnNewGameButtonClicked;
                _titleCC.OnLoadGameButtonClicked -= OnLoadGameButtonClicked;
                _titleCC.OnConfigButtonClicked -= OnConfigButtonClicked;
            }
        }
        
        #endregion
        
        /// <summary>
        /// スプラッシュ演出完了時の処理
        /// </summary>
        private void OnSplashCompleted() => PlayTitleBGMAsync().Forget();

        /// <summary>
        /// ゲームを最初から始めるボタンクリック時の処理
        /// </summary>
        private void OnNewGameButtonClicked() => TransitionToInGameAsync().Forget();
        
        /// <summary>
        /// ゲームを続きから始めるボタンクリック時の処理
        /// TODO: 現状1から再生されるようになっている
        /// </summary>
        private void OnLoadGameButtonClicked() => TransitionToInGameAsync().Forget();
        
        /// <summary>
        /// 設定画面を開くボタンクリック時の処理
        /// </summary>
        private void OnConfigButtonClicked() => ShowConfigCanvas();
        
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

            if (_canvasManager == null)
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
            await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.InGame, true));
        }
        
        /// <summary>
        /// Canvas
        /// </summary>
        private void BindTitleCanvas()
        {
            if (_titleCC == null)
            {
                _titleCC = _canvasManager.GetCanvas(TitleCanvasType.Title) as CanvasController_Title;
            }
            
            // CanvasControllerのボタン押下処理を追加
            if (_titleCC != null)
            {
                _titleCC.OnNewGameButtonClicked += OnNewGameButtonClicked;
                _titleCC.OnLoadGameButtonClicked += OnLoadGameButtonClicked;
                _titleCC.OnConfigButtonClicked += OnConfigButtonClicked;
            }
        }

        private void ShowConfigCanvas()
        {
            if (_configCC == null)
            {
                _configCC = _canvasManager.GetCanvas(TitleCanvasType.Config) as CanvasController_Config;
            }
            
            _canvasManager.PushCanvas(TitleCanvasType.Config);
        }
    }
}