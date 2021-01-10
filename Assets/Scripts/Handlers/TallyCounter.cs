public class TallyCounter
{
    public int TickCount { get; private set; }
    public int MaxTickCount { get; private set; }
    public bool IsOver { get => TickCount >= MaxTickCount; }

    public TallyCounter(int maxTickCount)
    {
        MaxTickCount = maxTickCount;
    }

    public void Tick()
    {
        TickCount++;
    }

    public bool TickAndIsOver()
    {
        TickCount++;

        return IsOver;
    }
}
