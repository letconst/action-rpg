using UnityEngine;
using UnityEngine.UI;

public class MotionEventListItem : MonoBehaviour
{
    [SerializeField]
    private Text currentFrame;

    [SerializeField]
    private Text eventData;

    [SerializeField]
    private InputField eventDataInput;

    [SerializeField]
    private Button editButton;

    [SerializeField]
    private Button deleteButton;

    private System.Action<AnimationEvent> _onDeleteEvent;

    public bool isCloned;

    private AnimationEvent _animEventData;

    private enum EditState
    {
        Edit,
        Apply
    }

    private EditState _currentEditState = EditState.Edit;

    private void Awake()
    {
        if (editButton)
        {
            editButton.onClick.AddListener(OnEditButtonClick);
        }

        if (deleteButton)
        {
            deleteButton.onClick.AddListener(OnDeleteButtonClick);
        }
    }

    private void OnEditButtonClick()
    {
        switch (_currentEditState)
        {
            case EditState.Edit:
            {
                _currentEditState                              = EditState.Apply;
                editButton.GetComponentInChildren<Text>().text = "完了";
                eventDataInput.gameObject.SetActive(true);

                break;
            }

            case EditState.Apply:
            {
                _currentEditState                              = EditState.Edit;
                editButton.GetComponentInChildren<Text>().text = "編集";
                eventDataInput.gameObject.SetActive(false);

                // 入力されたものを反映
                string newEventData = eventDataInput.text;
                _animEventData.stringParameter = newEventData;
                eventData.text                 = newEventData;

                break;
            }
        }
    }

    private void OnDeleteButtonClick()
    {
        _onDeleteEvent?.Invoke(_animEventData);
    }

    public void Setup(AnimationEvent data, System.Action<AnimationEvent> onDeleteCallback)
    {
        if (data != null)
        {
            _animEventData = data;
        }

        _onDeleteEvent = onDeleteCallback;

        SetupDisplay();
    }

    private void SetupDisplay()
    {
        if (_animEventData == null) return;

        currentFrame.text   = _animEventData.time.ToString("0.000");
        eventData.text      = _animEventData.stringParameter;
        eventDataInput.text = _animEventData.stringParameter;
        eventDataInput.gameObject.SetActive(false);
    }
}
