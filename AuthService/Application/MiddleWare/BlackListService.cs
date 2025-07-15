using System.Collections.Concurrent;

public class BlackListService : IBlackListService
{
    private readonly ConcurrentDictionary<string, DateTime> blacklist = new ();

    public void AddTokenToBlacklist(string token)
    {
        blacklist[token] = DateTime.UtcNow;
    }

    public bool IsTokenBlacklisted(string token)
    {
        return blacklist.ContainsKey(token);
    }

    public void CleanupExpiredTokens(TimeSpan tokenLifetime)
    {
        var now = DateTime.UtcNow;
        foreach (var token in blacklist.Keys)
        {
            if (blacklist.TryGetValue(token, out var addedTime) && now - addedTime > tokenLifetime)
            {
                blacklist.TryRemove(token, out _);
            }
        }
    }
}