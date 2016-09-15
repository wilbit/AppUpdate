namespace Wilbit.AppUpdate.Configuration
{
    public interface IFeedSource
    {
        Feed GetNextFeed();
    }
}