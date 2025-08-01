using CryStar.Core;
using iCON.System;
using UnityEngine;

namespace iCON.Field.System
{
    /// <summary>
    /// ストーリー再生を行うアクションイベント
    /// </summary>
    public class ActionEvent_Story : ActionEventBase
    {
        [SerializeField] private int _playStoryId;
        
        protected override void OnPlayerEnter(Collider2D playerCollider)
        {
            var storyManager = ServiceLocator.GetLocal<InGameManager>();
            storyManager.PlayStory(_playStoryId);
        }
    }
}