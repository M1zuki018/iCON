using System;
using CryStar.Attribute;
using CryStar.Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using iCON.System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace iCON.Performance
{
    /// <summary>
    /// タイトルスプラッシュの演出を管理するクラス
    /// </summary>
    public class TitleSplashManager : CustomBehaviour
    {
        /// <summary>
        ///  タイトルスプラッシュのアニメーション終了
        /// </summary>
        private event Action _onFinishedTitleSplash;

        /// <summary>
        /// 注意書きが表示された状態で選択ボタンが押されるのを待機している状態
        /// </summary>
        private bool _isWaiting = false;
        
        [FormerlySerializedAs("_prefabCanvasGroup")] [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField, Comment("演出終了時のフェードアウトにかける秒数")] private float _endFadeDuration = 3f;
        [SerializeField, Comment("演出終了時の画面が黒い状態の秒数")] private float _waitDuration = 2f;
        
        [Header("ロゴ関連の設定")]
        [SerializeField] private Image _logo;
        [SerializeField, Comment("表示にかける秒数")] private float _logoFadeDuration = 1f;
        [SerializeField, Comment("表示時間")] private float _logoDisplayTime = 3f;
        
        [Header("グリッチアニメーションの設定")]
        [SerializeField] private RawImage _glitchImage;
        [SerializeField, Comment("表示にかける時間")] private float _glitchDisplayTime = 0.5f;
        [SerializeField, Comment("表示時間")] private float _glitchDisplayDuration = 1f;
        [SerializeField, Comment("ノイズSEのパス")] private string _noizeSePath;
        
        [Header("背景の設定")]
        [SerializeField] private Image _background;
        [SerializeField, Comment("デフォルト色")] private Color _defaultColor = Color.white;
        [SerializeField, Comment("ブルースクリーン時に使用する色")] private Color _blueScreenColor = Color.blue;
        [SerializeField, Comment("ブルースクリーン状態の待機時間")] private float _blueScreenDisplayTime = 2f;
        [SerializeField, Comment("規制音SEのパス")] private string _regulatoryNoiseSePath;

        [Header("注意書きの設定")]
        [SerializeField] private CanvasGroup _cautionaryNote;
        [SerializeField, Comment("表示/非表示にかける秒数")] private float _cautionaFadeDuration = 1f;
        [SerializeField, Comment("電源を切るSEのパス")] private string _powerOffSePath;
        
        /// <summary>
        /// 現在再生中のシーケンス
        /// </summary>
        private Sequence _sequence;

        #region Life cycle

        /// <summary>
        /// Awake
        /// </summary>
        public override UniTask OnAwake()
        {
            // 非表示/デフォルト色にセット
            _logo.DOFade(0f, 0f);
            _cautionaryNote.DOFade(0f, 0f);
            _glitchImage.DOFade(0f, 0f);
            _background.color = _defaultColor;
            
            return base.OnAwake();
        }

        private void Update()
        {
            if (_isWaiting && UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                EndAnimation();
            }
        }
        
        /// <summary>
        /// OnDestroy
        /// </summary>
        private void OnDestroy()
        {
            _sequence?.Kill();
            _onFinishedTitleSplash = null;
        }

        #endregion

        /// <summary>
        /// 終了時の処理を登録する
        /// </summary>
        public void SetupEndAction(Action onFinished)
        {
            _onFinishedTitleSplash = onFinished;
        }
        
        /// <summary>
        /// 演出を開始する
        /// </summary>
        public void Play()
        {
            LogoAnimation().Forget();
        }

        /// <summary>
        /// 演出をスキップする
        /// </summary>
        public void Skip()
        {
            _sequence?.Kill(true);
        }

        #region 演出

        /// <summary>
        /// 1. ロゴの演出
        /// </summary>
        private async UniTask LogoAnimation()
        {
            var seq = DOTween.Sequence()

                // フェードイン
                .Append(_logo.DOFade(1f, _logoFadeDuration))

                // 指定秒数待つ
                .AppendInterval(_logoDisplayTime);
            
            _sequence = seq;

            // キルされたらグリッチアニメーションへ遷移
            seq.OnKill(async () => await GlitchAnimation());
        }

        /// <summary>
        /// 2.グリッチアニメーションの演出
        /// </summary>
        private async UniTask GlitchAnimation()
        {
            await AudioManager.Instance.PlaySE(_noizeSePath, 1f);
            
            var seq = DOTween.Sequence()

                // グリッジアニメーションを再生しつつ、ロゴを消す
                .Append(_glitchImage.DOFade(1f, _glitchDisplayDuration))
                
                .AppendInterval(_glitchDisplayTime)
                .Join(_logo.DOFade(0f, 0.001f));
            
            _sequence = seq;
            
            seq.OnKill(async () => await BlueScreenAnimation());
        }
        
        /// <summary>
        /// 3. ブルースクリーンの演出
        /// </summary>
        private async UniTask BlueScreenAnimation()
        {
            // 規制音SEを鳴らす
            await AudioManager.Instance.PlaySE(_regulatoryNoiseSePath, 1f);
            
            var seq = DOTween.Sequence()
                
                // 即座に背景色を変更して驚かせる
                .Append(_background.DOColor(_blueScreenColor, 0.1f))
                .Join(_glitchImage.DOFade(0f, 0.1f))
                
                // 指定秒数待つ
                .AppendInterval(_blueScreenDisplayTime);
            
            _sequence = seq;
            
            // 注意書きアニメーションへ遷移
            seq.OnKill(ShowCautionaryNoteAnimation);
        }

        /// <summary>
        /// 4. 注意書きのアニメーション
        /// </summary>
        private void ShowCautionaryNoteAnimation()
        {
            var seq = DOTween.Sequence()

                // フェードイン
                .Append(_cautionaryNote.DOFade(1f, _cautionaFadeDuration));
            
            _sequence = seq;
            
            // 待機状態にする
            _sequence.OnKill(() => _isWaiting = true);
                
                // 指定秒数待つ NOTE: 手動で進めることになったので、一旦コメントアウト
                // .AppendInterval(_cautionDisplayTime)
                // フェードアウト
                //.Append(_cautionaryNote.DOFade(0f, _cautionaFadeDuration));
                // _sequence = seq;
                // seq.OnKill(EndAnimation);
        }

        /// <summary>
        /// 5. 演出終了
        /// </summary>
        private async UniTask EndAnimation()
        {
            // 電源を落とすSE
            await AudioManager.Instance.PlaySE(_powerOffSePath, 1f);
            
            var seq = DOTween.Sequence()
         
                .AppendInterval(0.1f)
                // 画面を黒くして待機
                .AppendCallback(() =>
                {
                    _background.color = Color.black;
                    _cautionaryNote.alpha = 0f;
                })
                .AppendInterval(_waitDuration)
                
                // フェードアウト
                .Append(_canvasGroup.DOFade(0f, _endFadeDuration));
            
            _sequence = seq;
            
            seq.OnKill(FinishedAnimation);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// アニメーション終了後の処理
        /// </summary>
        private void FinishedAnimation()
        {
            _onFinishedTitleSplash?.Invoke();
            gameObject.SetActive(false);
        }

        #endregion
    }
}
