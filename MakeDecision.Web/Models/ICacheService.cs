using System;

namespace MakeDecision.Web.Models
{
    public interface ICacheService
    {
        T Get<T>(string cacheID, string dependFilePath, Func<T> getItemCallback) where T : class;
    }
}