using System.Collections.Concurrent;

namespace FleetApi;

public class FleetRepo
{
    private ConcurrentDictionary<string, string> _clientsNameToConnectionId;
    private static ulong _indexer;
    
    public FleetRepo()
    {
        _clientsNameToConnectionId = new ConcurrentDictionary<string, string>();
        _indexer = 0;
    }

    public bool AddNewConnection(string name, string connId)
    {
        return _clientsNameToConnectionId.TryAdd(name, connId);
    }

    public ulong GetNextId()
    {
        var nextId = Interlocked.Increment(ref _indexer);
        return nextId;
    }
}