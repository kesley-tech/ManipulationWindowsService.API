using System.ServiceProcess;

namespace ManipulationWindowsService.Domain.Entities
{
    public class WinService
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ServiceControllerStatus Status { get; set; }
        public ServiceStartMode StartupType { get; set; }
        public string? LogOnAs { get; set; }
        public bool IsDeleted { get; set; }
    }
}
