using CryStar.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace iCON.Story.UI
{
    public class UIContents_Character : UIContentsCanvasGroupBase
    {
        [Header("Custom Image")]
        [SerializeField] private CustomImage _rearImage;
        [SerializeField] private CustomImage _bodyImage;
        [SerializeField] private CustomImage _faceImage;
        [SerializeField] private CustomImage _hairImage;

        public async UniTask SetBaseImage(string rearPath, string bodyPath, string hairPath)
        {
            await UniTask.WhenAll(
                _hairImage.ChangeSpriteAsync(hairPath),
                _rearImage.ChangeSpriteAsync(rearPath),
                _bodyImage.ChangeSpriteAsync(bodyPath)
            );
        }

        public async UniTask SetFaceImage(string facePath)
        {
            await _faceImage.ChangeSpriteAsync(facePath);
        }
    }
}