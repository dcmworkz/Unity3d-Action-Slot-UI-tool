using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : MonoBehaviour
{
    public Sprite icon = null;
    public KeyCode keycode = KeyCode.Alpha1;
    [SerializeField] private float _totalDuration = 5;
    [SerializeField] private float _totalCooldown = 10;
    public float totalDuration { get { return _totalDuration; } }
    public float totalCooldown { get { return _totalCooldown; } }
    public float remainingDuration { get; private set; }
    public float remainingCooldown { get; private set; }
    private bool _onCooldown = false;

    /// <summary>
    /// Uses the Action and starts the appropriate Coroutine
    /// </summary>
    public void Use()
    {
        if (!_onCooldown)
        {
            _onCooldown = true;
            StartCoroutine(ProcessDurationAndCooldownRoutine());
        }
    }

    /// <summary>
    /// Emulates running an action and waiting for its' cooldown to finish.
    /// </summary>
    /// <returns></returns>
    IEnumerator ProcessDurationAndCooldownRoutine()
    {
        remainingDuration = _totalDuration;

        while (remainingDuration > 0)
        {
            remainingDuration -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        remainingCooldown = _totalCooldown;

        while (remainingCooldown > 0)
        {
            remainingCooldown -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        remainingDuration = 0;
        remainingCooldown = 0;

        _onCooldown = false;
    }
}