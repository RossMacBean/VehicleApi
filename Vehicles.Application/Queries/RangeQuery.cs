namespace Vehicles.Application.Queries;

internal class RangeQuery<T>(T min, T max)
    where T : IComparable<T>
{
    public T Min { get; private set; } = min;
    public T Max { get; private set; } = max;

    public RangeQuery<T> WithMin(T value)
    {
        Min = value;
        return this;
    }

    public RangeQuery<T> WithMax(T value)
    {
        Max = value;
        return this;
    }

    public bool IsInRange(T value)
    {
        return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
    }
}