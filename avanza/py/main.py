import numpy as np
import scipy.stats as si
from options import Option
from instruments import Instruments

def f(v):
    return '-          ' if v is None else f"{v:<15.3f}"

def fs(v):
    return f"{v:<15}"

def d1(S, K, r, sigma, T):
    return (np.log(S/K)+(r+sigma**2/2)*T)/(sigma*np.sqrt(T))

def d2(S, K, r, sigma, T):
    return (np.log(S/K)+(r-sigma**2/2)*T)/(sigma*np.sqrt(T))

# S: current stock price
# K: strike/exercise price of the option
# r: risk free rate
# T: time until option expiration 
# σ (Sigma): Annualised volatility of the asset's returns (historical volatility)
# C0: Current call option price
# P0: Current put option price 

def call_price(S, K, r, sigma, T):
    return S*si.norm.cdf(d1(S, K, r, sigma, T), 0.0, 1.0)-K*np.exp(-r*T)*si.norm.cdf(d2(S, K, r, sigma, T), 0.0, 1.0)

def put_price(S, K, r, sigma, T):
    return K*np.exp(-r*T)*si.norm.cdf(-d2(S, K, r, sigma, T), 0.0, 1.0)-S*si.norm.cdf(-d1(S, K, r, sigma, T), 0.0, 1.0)

def get_put_profit(string, premium, price):
    if premium is None:
        return None
    put_required_return = (string - premium) / price
    return (1 - put_required_return) * 100

def get_call_profit(string, premium, price):
    if premium is None:
        return None
    call_required_return = (string + premium) / price
    return (call_required_return - 1) * 100

def print_calls(option, r, t, vol):
    print(f"{fs('Strike')} {fs('Premium')} {fs('Black price')} {fs('Diff')} {fs('Return to Profit')}")

    for matrix in option.matrix:
        if (matrix['call_sell_price'] is not None):
            black_price = call_price(option.price, matrix['strike_price'], r, vol, t)
            call_profit = get_call_profit(matrix['strike_price'], matrix['call_sell_price'], option.price)
            diff = matrix['call_sell_price'] - black_price

            print(f"{f(matrix['strike_price'])} {f(matrix['call_sell_price'])} {f(black_price)} {f(diff)} {f(call_profit)} %")

def print_puts(option, r, t, vol):
    print(f"{fs('Strike')} {fs('Premium')} {fs('Black price')} {fs('Diff')} {fs('Return to profit')}")

    for matrix in option.matrix:
        if (matrix['put_sell_price'] is not None):
            black_price = put_price(option.price, matrix['strike_price'], r, vol, t)
            put_profit = get_put_profit(matrix['strike_price'], matrix['put_sell_price'], option.price)
            diff = matrix['put_sell_price'] - black_price

            print(f"{f(matrix['strike_price'])} {f(matrix['put_sell_price'])} {f(black_price)} {f(diff)} -{f(put_profit)} %")

def print_put_protection_matrix(option):
    print(f"{fs('Strike')} {fs('Premium')} {fs('Return to Profit')} {fs('Max loss %')} {fs('Diff')} \t {fs('Total Price')} {fs('Max Loss kr')}")
    
    min_diff = 100
    min_strike = 0
    min_premium = 0
    for matrix in option.matrix:
        strike = matrix['strike_price']
        premium = matrix['put_sell_price']
        if premium is not None:
            position_profit_return = ((option.price + premium) / option.price - 1) * 100
            put_protection_return = (1 - (strike - premium) / option.price) * 100
            diff = position_profit_return - put_protection_return
            total_price = option.price * 100 + premium * 100
            max_loss = total_price - strike * 100

            if (abs(diff) < min_diff):
                min_diff = diff
                min_premium = premium
                min_strike = strike

            print(f"{f(strike)} {f(premium)} {f(position_profit_return)} {f(put_protection_return)} {f(diff)} \t {f(total_price)} {f(max_loss)}")
    print("Min:")
    print(f"Strike: {min_strike} \t Premium: {min_premium} \t Diff: {min_diff}")

def main():
    r = 0.0392
    t = 10/12

    for instrument in Instruments().data:
        option = Option(instrument['name'], "12", True)
        vol = instrument['vol']
        print(f"Name: {option.name}, Price: {option.price}")
        print_calls(option, r, t, vol)
        print()
        print_puts(option, r, t, vol)
        print()
        print_put_protection_matrix(option)
        print("\n")

if __name__ == "__main__":
    main()