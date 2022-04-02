using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.TaskManager.Hangfire
{
    public interface IHangfireService
    {
        Task SyncMovies();
    }
}
