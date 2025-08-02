using CryStar.Core;
using iCON.System;
using UnityEngine;

namespace CryStar.Field.Event
{
    /// <summary>
    /// ストーリー再生を行うフィールドイベント
    /// </summary>
    public class FieldEvent_Story : FieldEventBase
    {
        [SerializeField] private int _playStoryId;
        
        protected override void OnPlayerEnter(Collider2D playerCollider)
        {
            var storyManager = ServiceLocator.GetLocal<InGameManager>();
            storyManager.PlayStory(_playStoryId);
        }
    }
}