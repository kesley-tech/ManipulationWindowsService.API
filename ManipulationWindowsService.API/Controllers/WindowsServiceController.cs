using ManipulationWindowsService.API.Domain.Persistence;
using ManipulationWindowsService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ServiceProcess;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManipulationWindowsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindowsServiceController : ControllerBase
    {
        private WinServiceDbContext _dbContext;

        public WindowsServiceController(WinServiceDbContext dbContext) 
        {
            this._dbContext = dbContext;
        }

        [HttpGet("GetLocal")]
        public IActionResult GetLocal()
        {
            try
            {
                var userRequested = string.Concat(HttpContext.Connection.RemoteIpAddress,":",HttpContext.Connection.RemotePort);

                ServiceController[] dataList = ServiceController.GetServices();

                var serviceListDTO = new List<WinService>();
                foreach (var data in dataList)
                {
                    var currentService = new WinService();
                    //currentService.Update(data.ServiceName,
                    //                      data.DisplayName,
                    //                      data.Status,
                    //                      data.StartType,
                    //                      "");

                    serviceListDTO.Add(currentService);
                }

                return serviceListDTO.Any() ? Ok(serviceListDTO) : NotFound();
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message,"\n",ex.InnerException));
            }
        }

        [HttpGet("GetLocalByName/{name}")]
        public IActionResult GetLocal(string name)
        {
            try
            {
                WinService serviceDTO = _dbContext.WinServiceList.SingleOrDefault(service => service.Name == name);
                return serviceDTO != null ? Ok(serviceDTO) : NotFound();
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message, "\n", ex.InnerException));
            }
        }

        [HttpGet("GetBase")]
        public IActionResult GetBase()
        {
            try
            {
                List<WinService> serviceListDTO = _dbContext.WinServiceList.Where(service => !service.IsDeleted).ToList();
                return serviceListDTO.Any() ? Ok(serviceListDTO) : NotFound();
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message, "\n", ex.InnerException));
            }
        }

        [HttpGet("GetBaseById/{id}")]
        public IActionResult GetBase(Guid id)
        {
            try
            {
                WinService serviceDTO = _dbContext.WinServiceList.SingleOrDefault(service => service.Id == id);
                return serviceDTO != null ? Ok(serviceDTO) : NotFound();
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message, "\n", ex.InnerException));
            }
        }

        [HttpPost("Insert")]
        public IActionResult Insert(WinService inputModel)
        {
            try
            {
                var userRequested = string.Concat(HttpContext.Connection.RemoteIpAddress,":",HttpContext.Connection.RemotePort);

                //_dbContext.WinServiceList.Add(inputModel);

                return CreatedAtAction(nameof(GetBase), new { id = inputModel.Id }, inputModel);
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message, "\n", ex.InnerException));
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(Guid id, WinService inputModel)
        {
            try
            {
                var userRequested = string.Concat(HttpContext.Connection.RemoteIpAddress, ":", HttpContext.Connection.RemotePort);
                
                WinService serviceDTO = _dbContext.WinServiceList.SingleOrDefault(service => service.Id == id);

                if (serviceDTO is null)
                    return NotFound();

                //serviceDTO.Update(inputModel.Name, inputModel.Description, inputModel.Status, inputModel.StartupType, inputModel.LogOnAs);

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message, "\n", ex.InnerException));
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var userRequested = string.Concat(HttpContext.Connection.RemoteIpAddress, ":", HttpContext.Connection.RemotePort);

                WinService serviceDTO = _dbContext.WinServiceList.SingleOrDefault(service => service.Id == id);

                if (serviceDTO is null)
                    return NotFound();

                //serviceDTO.Delete();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message, "\n", ex.InnerException));
            }
        }

        [HttpPost("SaveLocalToBase")]
        public IActionResult SaveLocalToBase()
        {
            try
            {
                OkObjectResult objectResult = (OkObjectResult)GetLocal();
                List<WinService>? serviceLocalList = (List<WinService>)objectResult.Value;
                InsertExceptWinService(serviceLocalList);
                UpdateDiffWinService(serviceLocalList);
                DeleteDiffWinService(serviceLocalList);

                return Ok("Dados registrados com sucesso!");
            }
            catch (Exception ex)
            {
                return Problem(string.Concat(ex.Message, "\n", ex.InnerException));
            }
        }

        private void DeleteDiffWinService(List<WinService>? serviceLocalList)
        {
            List<WinService> oldData = (from serviceBase in _dbContext.WinServiceList
                                        where !serviceLocalList.Any(x => x.Name.Contains(serviceBase.Name))
                                        select serviceBase).ToList();

            foreach (WinService old in oldData)
                Delete(old.Id);
        }

        private void UpdateDiffWinService(List<WinService>? serviceLocalList)
        {
            List<WinService> diffData = (from localService in serviceLocalList
                                        join baseService in _dbContext.WinServiceList on localService.Name equals baseService.Name
                                        where baseService.Name != localService.Name ||
                                              baseService.Description != localService.Description ||
                                              baseService.Status != localService.Status ||
                                              baseService.StartupType != localService.StartupType ||
                                              baseService.LogOnAs != localService.LogOnAs ||
                                              baseService.IsDeleted != localService.IsDeleted 
                                         select localService).ToList();
             
            foreach (WinService diff in diffData)
                Update(diff.Id, diff);
        }

        private void InsertExceptWinService(List<WinService>? serviceLocalList)
        {
            List<WinService> newData = (from serviceLocal in serviceLocalList
                                        where !_dbContext.WinServiceList.Any(x => x.Name == serviceLocal.Name)
                                        select serviceLocal).ToList();

            foreach (WinService data in newData)
                Insert(data);
        }
    }
}
