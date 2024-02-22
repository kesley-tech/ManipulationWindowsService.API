using ManipulationWindowsService.API.Domain.Entities;

namespace ManipulationWindowsService.API.Domain.Persistence
{
    public class WinServiceDbContext
    {
        public List<WinService> WinServiceList { get; set; }
        public WinServiceDbContext() 
        { 
            WinServiceList = new List<WinService>();
        }
    }
}
