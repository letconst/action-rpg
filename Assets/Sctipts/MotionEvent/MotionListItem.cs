using UnityEngine;
using UnityEngine.UI;

public class MotionListItem : MonoBehaviour
{
    [SerializeField]
    private Button listItemButton;

    [SerializeField]
    private Text motionNameLabel;

    /// <summary>
    /// モーションボタンを初期化する
    /// </summary>
    /// <param name="motionName">表示名</param>
    /// <param name="onClick">クリック時の処理</param>
    public void Setup(string motionName, System.Action onClick)
    {
        listItemButton.onClick.AddListener(() => onClick?.Invoke());
        motionNameLabel.text = motionName;
    }
}
