using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Management;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ManipulationWindowsService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WindowsServiceController : ControllerBase
    {
        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            ManagementClass management = new ManagementClass("Win32_Process");
            ManagementObjectCollection mCollection = management.GetInstances();

            foreach (ManagementObject process in mCollection)
            {
                //ListViewItem novoListViewItem = new ListViewItem();

                //novoListViewItem.Text = (process["ProcessId"].ToString());
                //novoListViewItem.SubItems.Add((string)process["Name"]);
                //novoListViewItem.SubItems.Add((string)process["ExecutablePath"]);

                //try
                //{
                //    FileVersionInfo info = FileVersionInfo.GetVersionInfo((string)process["ExecutablePath"]);
                //    novoListViewItem.SubItems.Add(info.FileDescription);
                //}
                //catch
                //{
                //    novoListViewItem.SubItems.Add("Não Disponível");
                //}

                //lvServicosAtivos.Items.Add(novoListViewItem);
            }

            return Ok(mCollection);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
