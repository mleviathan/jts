using Jts.Models;

public interface IJiraManager
{
    Task<List<Issue>?> GetIssues();
}