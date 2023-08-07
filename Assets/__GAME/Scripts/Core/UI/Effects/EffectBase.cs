using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

enum Timer
{
    Fixed,
    RandomBetween
}

enum PlayMode
{
    Loop,
    Once,
    Custom
}

public class EffectBase : MonoBehaviour
{
    [Header("MAIN PROPS")]

    [SerializeField] bool startAuto = true;

    [SerializeField] private PlayMode playMode;

    [ConditionalField(nameof(playMode), false, PlayMode.Custom)]
    [SerializeField] int playCount;

    Coroutine activeRoutine;

    [Header("DELAY")]

    [SerializeField] bool setDelay = false;
    [ConditionalField(nameof(setDelay), false, true)]
    [SerializeField] float delay = 1f;

    [Header("TIMER")]

    [SerializeField] Timer timerType;
    [ConditionalField(nameof(timerType), false, Timer.Fixed)]
    [SerializeField] float repeatInterval;
    [ConditionalField(nameof(timerType), false, Timer.RandomBetween)]
    [SerializeField] float minRepeatInterval, maxRepeatInterval;

    void Start()
    {
        if(startAuto)
            StartEffect();
    }

    public void StartEffect()
    {
        if (playMode == PlayMode.Once)
            playCount = 1;

        if(playMode == PlayMode.Loop)
            activeRoutine = StartCoroutine(PlayLoopRoutine());
        else
            activeRoutine = StartCoroutine(PlayWithCountRoutine(playCount));

        OnEffectStarted();
    }
   

    IEnumerator PlayWithCountRoutine(int count)
    {
        int turn = 0;

        if (setDelay)
            yield return new WaitForSeconds(delay);

        while(turn < count)
        {
            float _waitFor = GetTime();

            Effect();

            yield return new WaitForSeconds(_waitFor);

            turn++;
        }

        StopEffect();
    }

    IEnumerator PlayLoopRoutine()
    {
        if (setDelay)
            yield return new WaitForSeconds(delay);

        while (true)
        {
            float _waitFor = GetTime();

            Effect();

            yield return new WaitForSeconds(_waitFor);

        }
    }

    public float GetTime()
    {
        switch (timerType)
        {
            case Timer.Fixed:
                return repeatInterval;
            case Timer.RandomBetween:
                return Random.Range(minRepeatInterval, maxRepeatInterval);
        }

        return 1f;
    }

    public void StopEffect()
    {
      
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        OnEffectStopped();
    }

    private void OnDisable()
    {
        StopEffect();
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopEffect();
        StopAllCoroutines();
    }

    public void ResetTimer()
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        activeRoutine = StartCoroutine(PlayLoopRoutine());

    }

    public virtual void Effect(){}
    public virtual void OnEffectStopped() { }
    public virtual void OnEffectStarted() { }

}
