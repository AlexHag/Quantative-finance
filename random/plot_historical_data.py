import matplotlib.pyplot as plt

dates = []
close_prices = []

with open('HistoricalPrices.csv', 'r') as file:
    text = file.read()
    data_array = text.split('\n')

i = 1
for i in range(1, len(data_array)):
    element = data_array[i]
    values = element.split(',')
    
    close = values[4]
    date = values[0]
    dates.append(date)
    close_prices.append(float(close)) 

plt.plot(dates, close_prices, marker='o')
plt.xlabel('Date')
plt.ylabel('Close Price')
plt.title('Closing Prices over Time')
plt.xticks(rotation=45)  # Rotates x-axis labels for better readability
plt.grid(True)
plt.show()