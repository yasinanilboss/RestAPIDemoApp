using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPIDemo.Models;
using RestAPIDemo.StockDatas;

namespace RestAPIDemo.Controllers
{

    [ApiController]    
    public class StocksController : ControllerBase
    {
        private IStockData _stockData;  
        public StocksController(IStockData stockData)
        {
            _stockData = stockData;
        }
       
        
        [HttpGet]
        [Route("api/[controller]")]
        [Authorize]
        public IActionResult GetAll()
        {
            var data = _stockData.GetAll();
            return Ok(data);
        }

        [HttpGet]
        [Route("api/[controller]/{stockCode}")]
        public IActionResult Get(string stockCode)
        {
            var data = _stockData.Get(stockCode);

            if (data != null) 
            { 
                return Ok(data);
            }   

            return NotFound("Veri Bulunamadı!");
        }

        [HttpPost]
        [Route("api/[controller]/post")]
        [Produces("application/json", "application/xml", "text/plain")]
        [Consumes("application/json", "application/xml", "text/plain")]
        public IActionResult Post([FromBody]Stock stock)
        {
            var data = _stockData.Post(stock);
            return Created(HttpContext.Request.Scheme + "://"
                + HttpContext.Request.Host 
                + HttpContext.Request.Path
                + "/" + stock.StockCode,
                stock);
        }

        [HttpPut]
        [Route("api/[controller]/{stockCode}")]
        [Produces("application/json", "application/xml", "text/plain")]
        [Consumes("application/json", "application/xml", "text/plain")]
        public IActionResult Put(string stockCode, [FromBody]Stock stock)
        {

            var existingData = _stockData.Get(stockCode);

            if (existingData != null)
            {
                stock.StockCode = existingData.StockCode;
                _stockData.Put(stock);
            }

            return Ok(stock);
        }

        [HttpDelete]
        [Route("api/[controller]/{stockCode}")]
        public IActionResult Delete(string stockCode)
        {
            var data = _stockData.Get(stockCode);
            if (data != null)
            {
                _stockData.Delete(data);
                return Ok();
            }
            return NotFound("Silinecek Veri Bulunmadı!");
        }

    }
}
