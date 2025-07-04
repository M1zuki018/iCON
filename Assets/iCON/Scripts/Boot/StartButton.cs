using Cysharp.Threading.Tasks;
using iCON.System;
using UnityEngine;
using UnityEngine.UI;

namespace iCON.Boot
{
    /// <summary>
    /// スタートボタン
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class StartButton : MonoBehaviour
    {
        [SerializeField] private SceneSelector _sceneSelector;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleStart);
        }

        private void HandleStart()
        {
            ServiceLocator.Get<SceneLoader>().LoadSceneAsync(
                new SceneTransitionData(SceneType.Title, true, true)).Forget();
        }
    }

}
