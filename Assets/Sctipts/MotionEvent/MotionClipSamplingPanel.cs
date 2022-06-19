using UnityEngine;
using UnityEngine.UI;

public class MotionClipSamplingPanel : MonoBehaviour
{
    [SerializeField]
    private Text currentClipName;

    [SerializeField]
    private Text clipTotalLength;

    [SerializeField]
    private Text currentFrame;

    [SerializeField]
    private Slider samplingSeekbar;

    private AnimationClip _currentClip;

    private System.Action<float> _onChangeSamplingValue;

    /// <summary>
    /// シークバーの値が変更された際のイベントメソッドを設定する
    /// </summary>
    /// <param name="callback"></param>
    public void SetOnChangeSamplingValue(System.Action<float> callback)
    {
        _onChangeSamplingValue = callback;

        samplingSeekbar.onValueChanged.RemoveAllListeners();
        samplingSeekbar.onValueChanged.AddListener(v =>
        {
            currentFrame.text = samplingSeekbar.value.ToString();
            _onChangeSamplingValue?.Invoke(samplingSeekbar.value);
        });
    }

    /// <summary>
    /// 指定のAnimationClip情報をUIに反映する
    /// </summary>
    /// <param name="clip"></param>
    public void SetSamplingClip(AnimationClip clip)
    {
        _currentClip         = clip;
        currentClipName.text = clip.name;
        clipTotalLength.text = clip.length.ToString();
        currentFrame.text    = "0";

        samplingSeekbar.minValue = 0;
        samplingSeekbar.maxValue = clip.length;
        samplingSeekbar.value    = 0;
    }
}
