using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lairinus.UI
{
    public class ActionSlotUI : MonoBehaviour
    {
        [SerializeField] private bool _showOnAwake = true;
        [SerializeField] private bool _isEnabled = true;
        public bool isEnabled { get { return _isEnabled; } }
        private GameObject _thisGameObject = null;

        /// <summary>
        /// Configuration for when the Action is currently being used and is alive
        /// </summary>
        [SerializeField] private Configuration _actionActiveConfiguration = new Configuration();

        /// <summary>
        /// Configuration for when you cannot use the action for a certain period of time
        /// </summary>
        [SerializeField] private Configuration _actionCooldownConfiguration = new Configuration();

        /// <summary>
        /// Configuration for the base UI element
        /// </summary>
        [SerializeField] private StateConfiguration _normalConfiguration = new StateConfiguration();

        /// <summary>
        /// Configuration to handle disabled and active states
        /// </summary>
        [SerializeField] private StateConfiguration _disabledConfiguration = new StateConfiguration();

        /// <summary>
        /// Sets the action slot's main icon image
        /// </summary>
        /// <param name="icon"></param>
        public void SetActionIcon(Sprite icon)
        {
            _normalConfiguration.SetSpriteInternal(icon);
        }

        /// <summary>
        /// Sets the "Disabled" state sprite dynamically
        /// </summary>
        /// <param name="sprite"></param>
        public void SetDisabledSprite(Sprite sprite)
        {
            _disabledConfiguration.SetSpriteInternal(sprite);
        }

        /// <summary>
        /// Shows or hides this Action Slot
        /// </summary>
        /// <param name="show"></param>
        public void ShowActionSlot(bool show)
        {
            if (_thisGameObject == null)
                _thisGameObject = gameObject;

            _thisGameObject.SetActive(show);
            _disabledConfiguration.ShowInternal(!_isEnabled);
        }

        /// <summary>
        /// Updates the ActionSlot to reflect the Action's cooldowns and durations.
        /// </summary>
        public void UpdateActionSlot(float remainingCooldown, float totalCooldown, float remainingDuration, float totalDuration)
        {
            _actionActiveConfiguration.UpdateInternal(remainingDuration, totalDuration);
            _actionCooldownConfiguration.UpdateInternal(remainingCooldown, totalCooldown);
            _disabledConfiguration.ShowInternal(!_isEnabled);
        }

        /// <summary>
        /// Sets this slot's disabled flag. A disabled slot typically means that players cannot interact with it.
        /// </summary>
        public void EnableActionSlot(bool isEnabled)
        {
            _isEnabled = isEnabled;
            _disabledConfiguration.ShowInternal(!isEnabled);
        }

        private void Awake()
        {
            _thisGameObject = gameObject;
            ShowActionSlot(_showOnAwake);
        }

        [System.Serializable]
        public class StateConfiguration
        {
            [SerializeField] private Image _imageObjectUI = null;
            [SerializeField] private Text _textObjectUI = null;
            [SerializeField] private GameObject _rootObject = null;

            /// <summary>
            /// Sets the sprite for the ActionSlot's disabled state
            /// </summary>
            /// <param name="sprite"></param>
            public void SetSpriteInternal(Sprite sprite)
            {
                if (_imageObjectUI == null)
                {
                    Debug.LogError("Error! You're trying to set the sprite inside of \"ActionSlot.DisabledConfiguration.SetDisabledSprite.\" You cannot complete this action while the \"_disabledImage object is null!\" ");
                    return;
                }

                _imageObjectUI.sprite = sprite;
            }

            /// <summary>
            /// Sets the text for this configuration's "_textObjectUI" field
            /// </summary>
            /// <param name="text"></param>
            public void SetTextInternal(string text)
            {
                if (_textObjectUI == null)
                {
                    Debug.LogError("Error! You're trying to set the text inside of \"ActionSlot.DisabledConfiguration.SetDisabledSprite.\" The Text object, '_textObjectUI' is null!");
                    return;
                }

                _textObjectUI.text = text;
            }

            /// <summary>
            /// Shows the UI object
            /// </summary>
            public void ShowInternal(bool isShown)
            {
                if (_rootObject != null)
                {
                    _rootObject.SetActive(isShown);
                }
            }
        }

        [System.Serializable]
        public class Configuration
        {
            public TextFormattingOption textFormattingOption = TextFormattingOption.SecondsOnly;

            [SerializeField] private bool _showText = false;

            private GameObject _textCachedGameObject = null;

            [SerializeField] private bool _useConfiguration = true;

            [SerializeField] private Image filledImage = null;

            [SerializeField] private GameObject rootObject = null;

            [SerializeField] private Text textObject = null;

            public enum TextFormattingOption
            {
                SecondsOnly,
                MinutesOnly,
                HoursOnly,
                HoursThenMinutesThenSeconds
            }

            public bool showConfiguration { get { return _useConfiguration; } set { _useConfiguration = true; } }
            public bool showText { get { return _showText; } set { _showText = true; } }

            /// <summary>
            /// Updates the values in the configuration to show/hide appropriately
            /// </summary>
            /// <param name="remainingTime"></param>
            /// <param name="totalTime"></param>
            public void UpdateInternal(float remainingTime, float totalTime)
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

            private void SetText(float remainingSeconds)
            {
                // Hide and cache the text GameObject to get better performance (provided this object needs to be hidden)
                if (textObject != null)
                {
                    if (_textCachedGameObject == null)
                        _textCachedGameObject = textObject.gameObject;

                    _textCachedGameObject.SetActive(_showText && remainingSeconds > 0);
                }

                if (_showText)
                {
                    // If the text object is null, we can't show the text
                    if (textObject == null)
                    {
                        Debug.LogError("Error! The text object is null inside of this ActionSlotUI configuration object, so no text can be shown!");
                        return;
                    }

                    // Calculate the text value to display on the textObject UI
                    int hourConversion = 3600;
                    int minuteConversion = 60;
                    int fixedAddition = 1;
                    float totalHours = Mathf.Floor(remainingSeconds / hourConversion);
                    float totalMinutes = Mathf.Floor(remainingSeconds / minuteConversion);
                    float totalSeconds = Mathf.Floor(remainingSeconds);
                    double totalRoundedSeconds = System.Math.Round(remainingSeconds, 2);
                    switch (textFormattingOption)
                    {
                        case TextFormattingOption.SecondsOnly:
                            float ceilingSeconds = Mathf.Ceil(remainingSeconds);
                            textObject.text = ceilingSeconds.ToString() + "s";
                            break;

                        case TextFormattingOption.MinutesOnly:
                            textObject.text = (totalMinutes + fixedAddition).ToString() + "m";
                            break;

                        case TextFormattingOption.HoursOnly:
                            textObject.text = (totalHours + fixedAddition).ToString() + "h";
                            break;

                        case TextFormattingOption.HoursThenMinutesThenSeconds:
                            {
                                // Check if we can show hours / minutes / seconds
                                if (totalHours > 0)
                                {
                                    textObject.text = totalHours.ToString() + "h";
                                }

                                // Check if we can show minutes instead
                                else if (totalMinutes > 0)
                                {
                                    textObject.text = totalMinutes.ToString() + "m";
                                }

                                // Check if we can show seconds instead
                                else if (remainingSeconds > 1)
                                {
                                    textObject.text = totalSeconds.ToString() + "s";
                                }

                                // We're showing partial seconds lower than 1
                                else
                                {
                                    textObject.text = totalRoundedSeconds.ToString() + "s";
                                }
                            }
                            break;
                    }
                }
            }

            private void Show(bool show)
            {
                if (rootObject == null)
                {
                    Debug.LogError("Error! The root object attached to this Configuration script is null! You must assign this value for the 'ShowConfiguration' script to work!");
                    return;
                }

                rootObject.SetActive(show);
            }
        }
    }
}