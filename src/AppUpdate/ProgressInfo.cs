namespace Wilbit.AppUpdate
{
    public sealed class ProgressInfo
    {
        public ProgressInfo(long value, long minimum, long maximum)
        {
            Value = value;
            Minimum = minimum;
            Maximum = maximum;
        }

        public long Maximum { get; }
        public long Minimum { get; }
        public long Value { get; }
    }
}