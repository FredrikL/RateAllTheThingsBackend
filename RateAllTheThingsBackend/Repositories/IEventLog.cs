using RateAllTheThingsBackend.Models;

namespace RateAllTheThingsBackend.Repositories
{
    public interface IEventLog
    {
        void LogEvent(Event e);
    }
}