using UnityEngine;

public enum TimingAddType
{
    /// <summary>
    /// Calculate next ElapseAtTime based on last ElapseAtTime
    /// </summary>
    AddOnElapsedTime,

    /// <summary>
    /// Calculate next ElapseAtTime based on current time
    /// </summary>
    AddOnCurrentTime
}

/// <summary>
/// Timing in seconds, that are not in fixed update.
/// So it might be more.
/// </summary>
public class Timing
{
    /// <summary>
    /// IsElapsed will return true is now will pass this time
    /// </summary>
    public float ElapseAtTime { get; protected set; }

    /// <summary>
    /// Previous ElapseAtTime. Not used in this class logic
    /// </summary>
    public float? LastElapseTime { get; protected set; }

    /// <summary>
    /// Used to calculate next ElapseAtTime
    /// </summary>
    public float NextElapseTime { get; set; }

    /// <summary>
    /// If used random next time
    /// </summary>
    public float? NextMinElapseTime { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public TimingAddType NextAddType { get; set; }

    private System.Action _callback;

    public float TimeLeftUntilBeginTimeRate {
        get
        {
            var start = LastElapseTime.GetValueOrDefault();
            var end = ElapseAtTime - start;
            var current = GetCurrentTime() - start;
            return 1f / end * current;
        }
    }

    public static Timing StartTiming(float nextWaitTime, TimingAddType type)
    {
        var timing = new Timing(nextWaitTime, type);
        timing.Start();
        return timing;
    }

    public Timing(float nextWaitTime, TimingAddType type)
    {
        NextElapseTime = nextWaitTime;
        NextAddType = type;
    }

    public Timing(float nextMaxWaitTime, float nextMinWaitTime, TimingAddType type)
    {
        NextElapseTime = nextMaxWaitTime;
        NextMinElapseTime = nextMinWaitTime;
        NextAddType = type;
    }

    public virtual void Start()
    {
        if (NextMinElapseTime == null) StrictNextElapseTime();
        else RandomizeNextElapseTime();
    }

    public virtual void Start(System.Action callback)
    {
        _callback = callback;
        Start();
    }

    protected virtual void RandomizeNextElapseTime()
    {
        LastElapseTime = ElapseAtTime;
        var timeFrom = GetAddFromTime(NextAddType);
        ElapseAtTime = timeFrom + Random.Range(NextMinElapseTime.Value, NextElapseTime);
    }

    protected virtual void StrictNextElapseTime()
    {
        LastElapseTime = ElapseAtTime;
        var timeFrom = GetAddFromTime(NextAddType);
        ElapseAtTime = timeFrom + NextElapseTime;
    }

    public bool IsElapsed
    {
        get { return GetCurrentTime() > ElapseAtTime; }
    }

    /// <summary>
    /// Will callback only once. This timer whould need to be started with new callback method
    /// </summary>
    /// <returns></returns>
    public bool CallbackIfElapsed()
    {
        if (_callback == null) return false;
        if(IsElapsed)
        {
            _callback();
            _callback = null;
            return true;
        }
        return false;
    }

    protected float GetAddFromTime(TimingAddType addType)
    {
        switch (addType)
        {
            case TimingAddType.AddOnElapsedTime:
                return ElapseAtTime;
            case TimingAddType.AddOnCurrentTime:
                return GetCurrentTime();
        }
        return 0;
    }

    protected float GetCurrentTime()
    {
        return Singles.World.CurrentSecond;
    }
}

