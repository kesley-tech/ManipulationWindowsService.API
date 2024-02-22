using System.ServiceProcess;

namespace ManipulationWindowsService.API.Domain.Entities
{
    public class WinService
    {
        public WinService() 
        {
            Id = Guid.NewGuid();
            Status = ServiceControllerStatus.Stopped;
            StartupType = ServiceStartMode.Disabled;
            IsDeleted = false;
        }

        public Guid Id { get; set; }
        public string? Name { get; private set; }
        public string? Description { get; private set; }
        public ServiceControllerStatus Status { get; private set; }
        public ServiceStartMode StartupType { get; private set; }
        public string? LogOnAs { get; private set; }
        public bool IsDeleted { get; set; }

        public void Update(string name, string description, ServiceControllerStatus status, ServiceStartMode startMode, string logOnAs)
        {
            this.Name = name;
            this.Description = description;
            this.Status = status;
            this.StartupType = startMode;
            this.LogOnAs = logOnAs;
        }

        public void Delete()
        {
            IsDeleted = true;
        }
    }
}
