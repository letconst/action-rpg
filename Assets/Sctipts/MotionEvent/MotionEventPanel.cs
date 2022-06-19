using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotionEventPanel : MonoBehaviour
{
    [SerializeField]
    private Button saveButton;

    [SerializeField]
    private Button resetButton;

    [SerializeField]
    private Button createNewMotionEventButton;

    [SerializeField]
    private GameObject motionListItemScrollView;

    [SerializeField]
    private Transform motionEventItemParent;

    [SerializeField]
    private MotionEventListItem listItemTemplate;

    private AnimationClip _currentClip;

    private List<AnimationEvent> _clipEvents = new List<AnimationEvent>();

    private MotionEventTool _motionEventTool;

    private void Awake()
    {
        createNewMotionEventButton.onClick.AddListener(OnCreateNewEventButtonClick);
        saveButton.onClick.AddListener(OnSaveMotionEvent);
        resetButton.onClick.AddListener(OnResetMotionEvent);
    }
    
    #region ボタンクリック時のイベントメソッド

    private void OnCreateNewEventButtonClick()
    {
        foreach (AnimationEvent checkEvent in _clipEvents)
        {
            if (checkEvent.time == _motionEventTool.CurrentFrame)
            {
                Debug.LogError("NewEventAdditionError: 同フレームにイベントがすでに設定されています");

                return;
            }
        }

        // AnimationEvent作成
        var newAnimEvent = new AnimationEvent
        {
            functionName    = "OnMotionEvent",
            time            = _motionEventTool.CurrentFrame,
            stringParameter = string.Empty
        };

        // 作成したイベントを追加・反映
        _clipEvents.Add(newAnimEvent);
        SetupMotionEventPanel(_clipEvents);
    }

    private void OnSaveMotionEvent()
    {
#if UNITY_EDITOR
        UnityEditor.AnimationUtility.SetAnimationEvents(_currentClip, _clipEvents.ToArray());
        UnityEditor.EditorUtility.DisplayDialog("保存完了", "モーションイベントを保存しました", "閉じる");
#endif
    }

    private void OnResetMotionEvent()
    {
        Setup(_currentClip, _motionEventTool);
    }
    
    #endregion

    public void Setup(AnimationClip clip, MotionEventTool tool)
    {
        _currentClip     = clip;
        _motionEventTool = tool;

        if (_currentClip == null) return;

        AnimationEvent[] eventData = _currentClip.events;
        _clipEvents.Clear();
        _clipEvents.AddRange(eventData);

        if (eventData == null) return;

        if (eventData.Length == 0)
        {
            motionListItemScrollView.SetActive(false);

            return;
        }

        SetupMotionEventPanel(eventData);
    }

    private void SetupMotionEventPanel(IEnumerable<AnimationEvent> events)
    {
        if (motionEventItemParent != null)
        {
            foreach (Transform child in motionEventItemParent)
            {
                if (child.GetComponent<MotionEventListItem>().isCloned)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        listItemTemplate.gameObject.SetActive(true);

        foreach (AnimationEvent eventData in events)
        {
            MotionEventListItem newListItem = Instantiate(listItemTemplate, motionEventItemParent);
            var                 meli        = newListItem.GetComponent<MotionEventListItem>();
            meli.isCloned = true;
            meli.Setup(eventData, OnMotionEventDeleted);
        }

        listItemTemplate.gameObject.SetActive(false);
        motionListItemScrollView.SetActive(true);
    }

    private void OnMotionEventDeleted(AnimationEvent animationEvent)
    {
        _clipEvents.Remove(animationEvent);
        SetupMotionEventPanel(_clipEvents);

        if (_clipEvents.Count == 0)
        {
            motionListItemScrollView.SetActive(false);
        }
    }
}
