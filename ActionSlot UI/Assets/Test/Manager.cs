using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] private List<Action> actions = new List<Action>();
    [SerializeField] private List<Lairinus.UI.ActionSlotUI> actionSlots = new List<Lairinus.UI.ActionSlotUI>();
    [SerializeField] private List<Button> actionButtons = new List<Button>();

    private void Update()
    {
        UpdateActionInput();
        UpdateActionSlots();
    }

    private void Awake()
    {
        InitializeActionSlots();
        InitializeActionButtons();
    }

    private void InitializeActionSlots()
    {
        for (var a = 0; a < actionSlots.Count; a++)
        {
            if (a < actions.Count && actions[a] != null)
            {
                actionSlots[a].SetActionIcon(actions[a].icon);
            }
        }
    }

    private void InitializeActionButtons()
    {
        for (var a = 0; a < actionButtons.Count; a++)
        {
            int capture = a;
            if (capture < actions.Count)
            {
                Action act = actions[capture];
                actionButtons[a].onClick.AddListener(() => act.Use());
            }
        }
    }

    private void UpdateActionInput()
    {
        for (var a = 0; a < actions.Count; a++)
        {
            if (actions[a] != null)
            {
                if (Input.GetKeyDown(actions[a].keycode))
                {
                    actions[a].Use();
                    break;
                }
            }
        }
    }

    private void UpdateActionSlots()
    {
        for (var a = 0; a < actions.Count; a++)
        {
            Action action = actions[a];
            if (a < actionSlots.Count)
            {
                if (actionSlots[a] != null)
                {
                    actionSlots[a].UpdateActionSlot(action.remainingCooldown, action.totalCooldown, action.remainingDuration, action.totalDuration);
                }
            }
        }
    }
}