using System;
using UnityEngine;

public class TimeTickSystem : MonoBehaviour
{
    public class OnTickEventArgs : EventArgs
    {
        public int tick;
    }

    public static event EventHandler<OnTickEventArgs> OnTick;

    private const float TICK_TIMER_MAX = 0.15f;

    private int tick;
    private float tickTimer;

    private void Awake()
    {
        tick = 0;
    }

    /// <summary>
    /// Oyun iki karesi aras�ndan ge�en s�reyi ekleyerek zamanlay�c� olu�turur ve hedefine ula�t���nda bir 'Olay' �a��r�r
    /// </summary>
    private void Update()
    {
        tickTimer += Time.deltaTime;

        if (tickTimer >= TICK_TIMER_MAX)
        {
            tickTimer -= TICK_TIMER_MAX;
            tick++;

            OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });
        }
    }
}