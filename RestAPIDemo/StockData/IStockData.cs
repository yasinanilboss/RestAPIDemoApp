using RestAPIDemo.Models;
using System.Collections.Generic;

namespace RestAPIDemo.StockDatas
{
    public interface IStockData
    {
        List<Stock> GetAll();
        Stock Get(string stockCode);
        Stock Post(Stock stock);
        Stock Put(Stock stock);
        void Delete(Stock stock);
        

    }
}
