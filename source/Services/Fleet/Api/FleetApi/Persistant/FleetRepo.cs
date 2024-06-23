using System.Collections.Concurrent;

namespace FleetApi;

public class FleetRepo: IFleetRepo
{
    private ConcurrentDictionary<string, string> _connections;
    private static ulong _indexer;
    
    public FleetRepo()
    {
        _connections = new ConcurrentDictionary<string, string>();
        // _indexer = 0;
        
    }

    public bool AddConnection(string name, string connId)
    {
        bool addedOrExists = false;
        if (!_connections.ContainsKey(connId))
        {
            addedOrExists = _connections.TryAdd(connId, name);
        }
        return addedOrExists;
    }

    public bool IsExists(string connId)
    {
        return _connections.ContainsKey(connId);
    }

    public IEnumerable<string> GetConnectionsList()
    {
        return _connections.Keys;
    }

    public ulong GetNextId()
    {
        var nextId = Interlocked.Increment(ref _indexer);
        return nextId;
    }
}

public interface IFleetRepo
{
    public bool AddConnection(string name, string connId);

    public bool IsExists(string connId);
    public IEnumerable<string> GetConnectionsList();

    public ulong GetNextId();
}