using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TelegramBotBARS_WebAPI.JsonConverters;
using TelegramBotBARS_WebAPI.Services;

namespace TelegramBotBARS_WebAPI.Controllers
{
    [Route("api/data")]
    [ApiController]
    public class DataController : ControllerBase, IDisposable
    {
        private readonly DbDataProvider _dataProvider;
        private readonly JsonSerializerSettings _serializerOptions;

        public DataController(DbDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            _serializerOptions = new JsonSerializerSettings
            {
                
            };
            _serializerOptions.Converters.Add(new DateOnlyConverter());
        }

        [HttpGet]
        [Route("statements")]
        public IActionResult GetStatements(string? semester, string? type)
        {
            return new JsonResult(
                _dataProvider.GetStatements(semester, type), 
                _serializerOptions);
        }
        [HttpGet]
        [Route("statements/{statementId:guid}")]
        public IActionResult GetStatement(Guid statementId)
        {
            return new JsonResult(
                _dataProvider.GetStatement(statementId),
                _serializerOptions);
        }
        [HttpGet]
        [Route("records/{statementId:guid}")]
        public IActionResult GetMLRecords(Guid statementId)
        {
            return new JsonResult(
                _dataProvider.GetMLRecords(statementId), 
                _serializerOptions);
        }
        [HttpGet]
        [Route("records")]
        public IActionResult GetMLRecords(string semester)
        {
            return new JsonResult(
                _dataProvider.GetMLRecords(semester),
                _serializerOptions);
        }
        [HttpGet]
        [Route("events")]
        public IActionResult GetControlEvents(Guid statementId)
        {
            return new JsonResult(
                _dataProvider.GetControlEvents(statementId), 
                _serializerOptions);
        }

        public void Dispose()
        {
            _dataProvider.Dispose();
        }
    }
}
