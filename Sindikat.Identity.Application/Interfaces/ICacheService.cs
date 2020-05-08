using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Interfaces
{
    public interface ICacheService
    {
        Task<byte[]> GetAsync(string key);

        Task SetAsync(string key, string value, double minutesTTL);
    }
}
