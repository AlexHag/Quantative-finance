#include <iostream>
#include <fstream>
#include <vector>
#include <sstream>
#include <istream>

using namespace std;

class Price
{
    public:
        string Date;
        double Open;
        double High;
        double Low;
        double Close;
        double AdjClose;
        double Volume;
};

class Stock
{
    public:
        string Ticker;
        vector<Price> Prices;
        Stock(string path)
        {
            size_t lastSlashPos = path.find_last_of('/');
            size_t lastDotPos = path.find_last_of('.');
            Ticker = path.substr(lastSlashPos + 1, lastDotPos - lastSlashPos - 1);
            ifstream file(path);
            if (!file.is_open())
            {
                cerr << "Failed to open the file." << endl;
                throw;
            }

            string header;
            getline(file, header);
            string line;
            while (getline(file, line))
            {
                Price price;
                istringstream lineStream(line);
                string cell;
                getline(lineStream, price.Date, ',');

                getline(lineStream, cell, ',');
                price.Open = stod(cell);

                getline(lineStream, cell, ',');
                price.High = stod(cell);

                getline(lineStream, cell, ',');
                price.Low = stod(cell);

                getline(lineStream, cell, ',');
                price.Close = stod(cell);

                getline(lineStream, cell, ',');
                price.AdjClose = stod(cell);

                getline(lineStream, cell, ',');
                price.Volume = stod(cell);

                Prices.push_back(price);
            }
            file.close();
        }

        void PrintStockData()
        {
            for (const Price &price : Prices)
            {
                cout << "Date: " << price.Date << 
                        ", Open: " << price.Open << 
                        ", High: " << price.Close << 
                        ", Low: " << price.Low << 
                        ", Close: " << price.Close << 
                        ", Adj Close: " << price.AdjClose << 
                        ", Volume: " << price.Volume << endl;
            }
        }

        Price GetPriceFromDate(string Date)
        {
            for (const Price &price : Prices)
            {
                if (price.Date == Date)
                {
                    return price;
                }
            }
            Price notFoundPrice;;
            notFoundPrice.Date = "Date not found";
            return notFoundPrice;
        }
};

double optimalStrategy(double balance, Stock stock)
{
    int quantity = balance / stock.Prices[0].Open; 
    double price = quantity * stock.Prices[0].Open;
    balance -= price;

    bool hasPosition = true;
    for(int day = 1; day < stock.Prices.size(); day++)
    {
        // cout << "Balance: " << balance << ", Stock price: "<< stock.Prices[day].Open << ", Quantity: " << quantity << ", Value: "<< quantity * stock.Prices[day].Open << endl;
        // sell
        if (hasPosition && stock.Prices[day + 1].Open < stock.Prices[day].Open)
        {
            price = quantity * stock.Prices[day].Open;
            balance += price;
            quantity = 0;
            hasPosition = false;
            // cout << "Sell" << endl;
        }
        // buy
        else if (!hasPosition && stock.Prices[day + 1].Open > stock.Prices[day].Open)
        {
            quantity = balance / stock.Prices[day].Open;
            price = quantity * stock.Prices[day].Open;
            balance -= price;
            hasPosition = true;
            // cout << "Buy" << endl;
        }
    }
    // cout << hasPosition << endl;
    // cout << "Balance: " << balance << ", Stock price: "<< stock.Prices[stock.Prices.size() - 1].Open << ", Quantity: " << quantity << endl;
    return balance;
}

int main()
{
    string stocks[6]
      = { "AAPL", "AMZN", "INTC", "MSFT", "NVDA", "TSLA" };


    for (int i = 0; i < 6; i++)
    {
        string path = "./stocks/" + stocks[i] + ".csv";
        
        Stock stock(path);
        int balance = optimalStrategy(10000, stock);
        cout << stocks[i] << ": " << balance << endl;
    }
    
    return 0;
}
