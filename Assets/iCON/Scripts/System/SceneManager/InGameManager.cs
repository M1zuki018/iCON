using System.Linq;
using CryStar.Attribute;
using CryStar.Core;
using CryStar.Core.Enums;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.System
{
    /// <summary>
    /// インゲームのGameManager
    /// </summary>
    public class InGameManager : CustomBehaviour
    {
        /// <summary>
        /// ストーリーマネージャー
        /// </summary>
        [SerializeField, HighlightIfNull]
        private StoryManager _storyManager;

        /// <summary>
        /// 再生したいストーリーの名前
        /// </summary>
        [SerializeField]
        private string _playStoryName;
        
        /// <summary>
        /// データを読み込む際に必要なデータ
        /// </summary>
        [SerializeField]
        private StoryLine[] _storyLine;

        public override async UniTask OnStart()
        {
            await base.OnStart();
            
            ServiceLocator.Register(this, ServiceType.Local);
            
            // ストーリー再生時以外はゲームオブジェクトを非アクティブにしておく
            // _storyManager.gameObject.SetActive(false);
        }

        private async void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F8))
            {
                await ServiceLocator.GetGlobal<SceneLoader>().LoadSceneAsync(new SceneTransitionData(SceneType.Title, true, true));
            }
        }
    }
}