import math
import matplotlib.pyplot as plt
import numpy as np

def calculate_call_option_price(S0, X, r, T, sigma):
    d1 = (math.log(S0 / X) + (r + (sigma**2 / 2)) * T) / (sigma * math.sqrt(T))
    d2 = d1 - (sigma * math.sqrt(T))
    
    C = S0 * cumulative_normal_distribution(d1) - X * math.e**(-r*T) * cumulative_normal_distribution(d2)
    
    return C

def cumulative_normal_distribution(x):
    return (1 + math.erf(x / math.sqrt(2))) / 2

# Define the range of original asset prices
original_prices = np.linspace(80, 120, 100)

# Define the initial values
S0 = 100
initial_time = 0.5
sigma = 0.2
initial_strike_price = 105
initial_interest_rate = 0.05

# Create a separate figure for the sliders
fig, ax = plt.subplots()
plt.subplots_adjust(left=0.25, bottom=0.35)

# Set the initial plot with the initial values
call_option_prices = [calculate_call_option_price(S0, initial_strike_price, initial_interest_rate, initial_time, sigma) for S0 in original_prices]
line, = plt.plot(original_prices, call_option_prices)
plt.xlabel('Original Asset Price')
plt.ylabel('Call Option Price')
plt.title('Call Option Price vs Original Asset Price\n(Strike Price: {}, Interest Rate: {}Time in years: {})'.format(initial_strike_price, initial_interest_rate, initial_time))
plt.grid(True)

# Create the slider axes for strike price and interest rate
strike_slider_ax = plt.axes([0.25, 0.20, 0.65, 0.03])
interest_rate_slider_ax = plt.axes([0.25, 0.15, 0.65, 0.03])
time_slider_ax = plt.axes([0.25, 0.10, 0.65, 0.03])

# Create the strike price and interest rate sliders
strike_price_slider = plt.Slider(strike_slider_ax, 'Strike Price', 80, 120, valinit=initial_strike_price)
interest_rate_slider = plt.Slider(interest_rate_slider_ax, 'Interest Rate', 0, 0.1, valinit=initial_interest_rate)
time_slider = plt.Slider(time_slider_ax, 'Time in years', 0.1, 3, valinit=initial_time)

# Function to update the plot based on the slider values
def update_plot(val):
    strike_price = strike_price_slider.val
    interest_rate = interest_rate_slider.val
    time = time_slider.val

    # Calculate the call option prices for each original asset price
    call_option_prices = [calculate_call_option_price(S0, strike_price, interest_rate, time, sigma) for S0 in original_prices]

    # Update the plot
    line.set_ydata(call_option_prices)
    ax.set_title('Call Option Price vs Original Asset Price\n(Strike Price: {}, Interest Rate: {}Time in years: {})'.format(strike_price, interest_rate, time))
    fig.canvas.draw_idle()

# Link the sliders to the update_plot function
strike_price_slider.on_changed(update_plot)
interest_rate_slider.on_changed(update_plot)
time_slider.on_changed(update_plot)

# Display the plot
plt.show()
