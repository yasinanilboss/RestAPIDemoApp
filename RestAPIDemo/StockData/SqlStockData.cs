using RestAPIDemo.Models;
using RestAPIDemo.StockDatas;
using System.Collections.Generic;
using System.Linq;

namespace RestAPIDemo.StockData
{
    public class SqlStockData : IStockData
    {
        private StockContext _stockContext;

        public SqlStockData(StockContext stockContext)
        {
            _stockContext = stockContext;
        }

        public List<Stock> GetAll()
        {
            return _stockContext.Stocks.ToList();
        }

        public Stock Get(string stockCode)
        {
            var data = _stockContext.Stocks.Find(stockCode);
            return data;
        }

        public Stock Post(Stock stock)
        {
            _stockContext.Stocks.Add(stock);
            _stockContext.SaveChanges();
            return stock;
        }

        public Stock Put(Stock stock)
        {
            var existingData = _stockContext.Stocks.Find(stock.StockCode);
            if (existingData != null)
            {
                existingData.StockCode = stock.StockCode;
                existingData.StockName = stock.StockName;
                existingData.KDV = stock.KDV;
                _stockContext.Stocks.Update(existingData);
                _stockContext.SaveChanges();
            }

            return stock;
        }

        public void Delete(Stock stock)
        {
           _stockContext.Stocks.Remove(stock);
           _stockContext.SaveChanges();
        }
    }
}
