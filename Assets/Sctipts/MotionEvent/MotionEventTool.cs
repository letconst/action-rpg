using System;
using UnityEngine;

public class MotionEventTool : MonoBehaviour
{
    [SerializeField]
    private Animator targetActorAnimator;

    [SerializeField]
    private MotionListItem listItemTemplate;

    [SerializeField]
    private Transform motionListParent;

    [SerializeField]
    private MotionClipSamplingPanel samplingPanel;

    [SerializeField]
    private MotionEventPanel eventPanel;

    private AnimationClip[] _motionClips;

    private GameObject _samplingActor;

    private AnimationClip _currentClip;

    public float CurrentFrame { get; private set; } = 0;

    private void Start()
    {
        if (targetActorAnimator)
        {
            _motionClips   = targetActorAnimator.runtimeAnimatorController.animationClips;
            _samplingActor = targetActorAnimator.gameObject;
        }

        samplingPanel.SetOnChangeSamplingValue(PlaySamplingAnimation);

        LoadAllMotion();
    }

    /// <summary>
    /// Actorに存在するモーションを読み込み、リストに反映する
    /// </summary>
    private void LoadAllMotion()
    {
        if (_motionClips == null)
        {
            listItemTemplate.gameObject.SetActive(false);

            return;
        }

        foreach (AnimationClip clip in _motionClips)
        {
            GameObject newListItem = Instantiate(listItemTemplate.gameObject, motionListParent);
            var        mli         = newListItem.GetComponent<MotionListItem>();
            string     clipName    = clip.name;

            if (clipName.Equals("Idle"))
            {
                _currentClip = clip;

                PlaySamplingAnimation(0);
                samplingPanel.SetSamplingClip(_currentClip);
                eventPanel.Setup(clip, this);
            }

            mli.Setup(clipName, () =>
            {
                _currentClip = clip;

                PlaySamplingAnimation(0);
                samplingPanel.SetSamplingClip(_currentClip);
                eventPanel.Setup(clip, this);
            });
        }

        listItemTemplate.gameObject.SetActive(false);
    }

    /// <summary>
    /// Actorに現在選択してるモーションを、指定フレーム目で再生する
    /// </summary>
    /// <param name="targetFrame">何フレーム目を再生するか</param>
    private void PlaySamplingAnimation(float targetFrame)
    {
        if (_currentClip == null) return;

        if (targetActorAnimator != null && targetActorAnimator.enabled)
        {
            targetActorAnimator.enabled = false;
        }

        _currentClip.SampleAnimation(_samplingActor, targetFrame);
    }
}
