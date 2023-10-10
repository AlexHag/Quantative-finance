import math
from datetime import datetime, timedelta
import numpy as np
from scipy.stats import zscore

def calculate_call_option_price(S0, X, r, T, sigma):
    d1 = (math.log(S0 / X) + (r + (sigma**2 / 2)) * T) / (sigma * math.sqrt(T))
    d2 = d1 - (sigma * math.sqrt(T))
    
    C = S0 * cumulative_normal_distribution(d1) - X * math.e**(-r*T) * cumulative_normal_distribution(d2)
    
    return C

def cumulative_normal_distribution(x):
    return (1 + math.erf(x / math.sqrt(2))) / 2

def average(array):
    sum = 0
    for i in range(len(array)):
        sum += array[i]
    mean = sum / len(array)
    return mean

def standard_deviation(array):
    mean = average(array)
    numerator = 0
    for i in range(len(array)):
        numerator += (array[i] - mean) * (array[i] - mean)

    sigma = math.sqrt(numerator / len(array) )

    return sigma

def z_score(array):
    sigma = standard_deviation(array)
    mean = average(array)
    new_array = []
    for element in array:
        new_array.append((element - mean) / sigma)
    return z_scores

dates = []
close_prices = []

with open('HistoricalPrices.csv', 'r') as file:
    text = file.read()
    data_array = text.split('\n')

i = 1
for i in range(1, len(data_array)):
    element = data_array[i]
    values = element.split(',')
    
    dates.append(values[0])
    close_prices.append(float(values[4])) 

end_date = datetime.strptime(dates[0], "%m/%d/%y")
start_date = datetime.strptime(dates[-1], "%m/%d/%y")
days_passed = (end_date - start_date).days


z_scores = zscore(close_prices)
normalized_scores = (z_scores - np.min(z_scores)) / (np.max(z_scores) - np.min(z_scores))
normalized_sigma = standard_deviation(normalized_scores)

# C is the call options price
# S0 is the current price of the underlying asset
# X is the strike price of the option
# r is the risk free interest rate
# T is the time to expiration of the option (in years)
# sigma is the volatility

S0 = float(close_prices[-1])
X = float(close_prices[-1])
r = 0.04
T = days_passed / 365
sigma = standard_deviation(normalized_scores)

call_price = calculate_call_option_price(S0, X, r, T, sigma)
print("Call Option Price:", call_price)