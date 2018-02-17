using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionSlotUI : MonoBehaviour
{
    private GameObject _thisGameObject = null;
    [SerializeField] private bool _showOnAwake = true;

    /// <summary>
    /// Configuration for the base UI element
    /// </summary>
    [SerializeField] private BaseUIConfiguration baseConfiguration = new BaseUIConfiguration();

    /// <summary>
    /// Configuration for when you cannot use the action for a certain period of time
    /// </summary>
    [SerializeField] private Configuration actionCooldownConfiguration = new Configuration();

    /// <summary>
    /// Configuration for when the Action is currently being used and is alive
    /// </summary>
    [SerializeField] private Configuration actionActiveConfiguration = new Configuration();

    [System.Serializable]
    public class BaseUIConfiguration
    {
        [SerializeField] private Image _slotBackgroundImage = null;
        [SerializeField] private Image _actionIconImage = null;

        public void SetActionIcon(Sprite actionIconImage)
        {
            if (_actionIconImage == null)
            {
                Debug.LogError("Error! Cannot set the Action Icon sprite because the ActionIconImage object is null!");
                return;
            }

            _actionIconImage.sprite = actionIconImage;
        }
    }

    [System.Serializable]
    public class Configuration
    {
        public enum TextFormattingOption
        {
            ShowSeconds,
            ShowMinutes,
            ShowSecondsAndMinutes,
        }

        public bool showText { get { return _showText; } set { _showText = true; } }
        [SerializeField] private bool _showText = false;
        [SerializeField] private GameObject rootObject = null;
        [SerializeField] private Image filledImage = null;
        [SerializeField] private Text textObject = null;
        [SerializeField] private bool _useConfiguration = true;
        public bool showConfiguration { get { return _useConfiguration; } set { _useConfiguration = true; } }
        public TextFormattingOption textFormattingOption = TextFormattingOption.ShowSeconds;

        private void Show(bool show)
        {
            if (rootObject == null)
            {
                Debug.LogError("Error! The root object attached to this Configuration script is null! You must assign this value for the 'ShowConfiguration' script to work!");
                return;
            }

            rootObject.SetActive(show);
        }

        private void SetText(float remainingTime)
        {
            if (_showText)
            {
                // If the text object is null, we can't show the text
                if (textObject == null)
                {
                    Debug.LogError("Error! The text object is null inside of this ActionSlotUI configuration object, so no text can be shown!");
                    return;
                }

                switch (textFormattingOption)
                {
                    case TextFormattingOption.ShowSeconds:
                        textObject.text = ((int)remainingTime + 1).ToString() + "s";
                        break;
                }
            }
        }

        private void SetFillAmount(float remainingTime, float totalTime)
        {
            if (filledImage == null)
            {
                Debug.LogError("Error! Cannot set the value of the image because it doesn't exist!");
                return;
            }

            filledImage.type = Image.Type.Filled;
            filledImage.fillAmount = remainingTime / totalTime;
        }

        /// <summary>
        /// Updates the values in the configuration to show/hide appropriately
        /// </summary>
        /// <param name="remainingTime"></param>
        /// <param name="totalTime"></param>
        public void Update(float remainingTime, float totalTime)
        {
            if (remainingTime == 0)
                Show(false);
            else
            {
                if (showConfiguration && remainingTime > 0)
                {
                    Show(true);
                    SetText(remainingTime);
                    SetFillAmount(remainingTime, totalTime);
                }
                else
                    Show(false);
            }
        }
    }

    /// <summary>
    /// Updates the ActionSlot to reflect the Action's cooldowns and durations.
    /// </summary>
    public void UpdateActionSlot(float remainingCooldown, float totalCooldown, float remainingDuration, float totalDuration)
    {
        actionActiveConfiguration.Update(remainingDuration, totalDuration);
        actionCooldownConfiguration.Update(remainingCooldown, totalCooldown);
    }

    private void Awake()
    {
        _thisGameObject = gameObject;
        Show(_showOnAwake);
    }

    /// <summary>
    /// Shows or hides this Action Slot
    /// </summary>
    /// <param name="show"></param>
    public void Show(bool show)
    {
        if (_thisGameObject == null)
            _thisGameObject = gameObject;

        _thisGameObject.SetActive(show);
    }
}