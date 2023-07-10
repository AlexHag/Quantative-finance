import matplotlib.pyplot as plt
import pandas as pd
import matplotlib.dates as mdates

df_report = pd.read_csv('Data/q1.csv')
df_report.set_index('Nyckeltal', inplace=True)

estate_marketvalue_mapping = df_report.loc['Marknadsvärde fastigheter, mkr'].to_dict()
estate_marketvalue_mapping = {period: (float(value) / 5000) for period, value in estate_marketvalue_mapping.items()}
estate_marketvalue_report_periods = list(estate_marketvalue_mapping.keys())
estate_marketvalue_report_values = list(estate_marketvalue_mapping.values())
estate_marketvalue_report_periods.reverse()
estate_marketvalue_report_values.reverse()
estate_marketvalue_report_periods = pd.to_datetime(estate_marketvalue_report_periods)


result_mapping = df_report.loc['Periodens resultat, mkr'].to_dict()
result_mapping = {period: (float(value) / 100) for period, value in result_mapping.items()}
result_report_periods = list(result_mapping.keys())
result_report_values = list(result_mapping.values())
result_report_periods.reverse()
result_report_values.reverse()
result_report_periods = pd.to_datetime(result_report_periods)


rent_revenue_mapping = df_report.loc['Hyresintäkter, mkr'].to_dict()
rent_revenue_mapping = {period: (float(value) / 100) for period, value in rent_revenue_mapping.items()}
rent_revenue_periods = list(rent_revenue_mapping.keys())
rent_revenue_values = list(rent_revenue_mapping.values())
rent_revenue_periods.reverse()
rent_revenue_values.reverse()
rent_revenue_periods = pd.to_datetime(rent_revenue_periods)


kassa_flöde_mapping = df_report.loc['Kassaflöde från den löpande verksamheten, mkr '].to_dict()
kassa_flöde_mapping = {period: (float(value) / 10) for period, value in kassa_flöde_mapping.items()}
kassa_flöde_periods = list(kassa_flöde_mapping.keys())
kassa_flöde_values = list(kassa_flöde_mapping.values())
kassa_flöde_periods.reverse()
kassa_flöde_values.reverse()
kassa_flöde_periods = pd.to_datetime(kassa_flöde_periods)


df = pd.read_csv('Data/sbb_2017_history.csv')

# Convert 'Date' column to datetime
df['Date'] = pd.to_datetime(df['Date'])

# Set 'Date' column as the index
df.set_index('Date', inplace=True)

# Extract the 'Close' column
close_prices = df['Close']

# Create the plot
fig, ax = plt.subplots()
ax.plot(close_prices, color='green')

# Set x-axis ticker to show only a subset of dates
ax.xaxis.set_major_locator(mdates.YearLocator())
ax.xaxis.set_major_formatter(mdates.DateFormatter('%Y'))

# Set labels and title
ax.set_xlabel('Year')
ax.set_ylabel('Close Price')
ax.set_title('Close Price Over Time')


ax.plot(estate_marketvalue_report_periods, estate_marketvalue_report_values, color='red', label='Marknadsvärde fastigheter, mkr')
ax.scatter(estate_marketvalue_report_periods, estate_marketvalue_report_values, color='red')

ax.plot(result_report_periods, result_report_values, color='blue', label='Periodens resultat, tkr')
ax.scatter(result_report_periods, result_report_values, color='blue')

# ax.plot(rent_revenue_periods, rent_revenue_values, color='purple', label='Hyresintäkter, mkr')
# ax.scatter(rent_revenue_periods, rent_revenue_values, color='purple')

ax.plot(kassa_flöde_periods, kassa_flöde_values, color='brown', label='Kassaflöde från den löpande verksamheten, mkr')
ax.scatter(kassa_flöde_periods, kassa_flöde_values, color='brown')

# ax.legend()

# Display the plot
plt.show()

