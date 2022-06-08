using RestAPIDemo.Models;
using System.Collections.Generic;
using System.Linq;

namespace RestAPIDemo.StockDatas
{
    public class StockData : IStockData
    {
        private List<Stock> stocks = new List<Stock>
        {
            new Stock() 
            {
                StockCode = "1",
                StockName = "A",
                KDV = 3
            },

            new Stock()
            {
                StockCode = "2",
                StockName = "B",
                KDV = 3
            },

            new Stock()
            {
                StockCode = "3",
                StockName = "C",
                KDV = 3
            },

        };

        public List<Stock> GetAll()
        {
            return stocks;
        }

        public Stock Get(string stockCode)
        {
            return stocks.SingleOrDefault(x => x.StockCode == stockCode);
        }

        public Stock Post(Stock stock)
        {
            stocks.Add(stock);
            return stock;
        }

        public Stock Put(Stock stock)
        {
            var existingData = Get(stock.StockCode);
            existingData.StockCode = stock.StockCode;
            existingData.StockName = stock.StockName;
            existingData.KDV = stock.KDV;
            return existingData;
        }

        public void Delete(Stock stock)
        {
            stocks.Remove(stock);
        }
        
    }
}
