using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private List<Action> actions = new List<Action>();
    [SerializeField] private List<ActionSlotUI> actionSlots = new List<ActionSlotUI>();

    void Update()
    {
        UpdateActionInput();
        UpdateActionSlots();
    }

    private void Awake()
    {
        InitializeActionSlots();
    }

    void InitializeActionSlots()
    {
    }

    void UpdateActionInput()
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

    void UpdateActionSlots()
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