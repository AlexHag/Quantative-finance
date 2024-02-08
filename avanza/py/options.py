import requests
from bs4 import BeautifulSoup
import json
from instruments import Instruments

class Option:
    def __init__(self, name, end_month, use_file=False):
        self.use_file = use_file
        self.name = name
        self.end_month = end_month
        self.instrument_id = self.get_instrument_id()
        self.price, self.matrix = self.get_option_matrix()

    def get_instrument_id(self):        
        for instrument in Instruments().data:
            if instrument['name'] == self.name:
                return instrument['instrumentId']

    def get_option_matrix_from_file(self):
        try:
            print("Fetching option matrix from file...")
            with open(f'data/{self.name}-{self.end_month}.json') as f:
                data = json.load(f)
            return data['price'], data['matrix']
        except:
            print("Failed to get option matrix from file...")
            return None, None

    def get_option_matrix(self):
        if (self.use_file):
            price, matrix = self.get_option_matrix_from_file()
            if price is not None and matrix is not None:
                return price, matrix

        print("Fetching option matrix from Avanza...")
        url = f"https://www.avanza.se/borshandlade-produkter/optioner-terminer/lista.html?name=&underlyingInstrumentId={self.instrument_id}&optionTypes=STANDARD&selectedEndDates=2024-{self.end_month}&page=1&sortField=NAME&sortOrder=ASCENDING&activeTab=matrix"
        html = requests.get(url).text
        soup = BeautifulSoup(html, 'html.parser')
        price = float(soup.find('td', class_='noSort lastPrice').text.strip().replace(u'\xa0', u'').replace(',','.'))
        table_rows = soup.select('#contentTable tr')
        data = []

        for row in table_rows[1:]:
            row_data = {}
            td = row.find_all('td')
            row_data['call_name'] = td[1].text.strip()
            row_data['call_buy_price'] = float(td[3].text.strip().replace(u'\xa0', u'').replace(',','.')) if td[3].text.strip() != '-' else None
            row_data['call_sell_price'] = float(td[4].text.strip().replace(u'\xa0', u'').replace(',','.')) if td[4].text.strip() != '-' else None
            row_data['strike_price'] = float(td[6].text.strip().replace(u'\xa0', u'').replace(',','.')) if td[6].text.strip() != '-' else None
            row_data['put_buy_price'] = float(td[8].text.strip().replace(u'\xa0', u'').replace(',','.'))  if td[8].text.strip() != '-' else None
            row_data['put_sell_price'] = float(td[9].text.strip().replace(u'\xa0', u'').replace(',','.'))  if td[9].text.strip() != '-' else None
            row_data['put_name'] = td[11].text.strip()
            data.append(row_data)

        file = {'price': price, 'matrix': data}
        with open(f'data/{self.name}-{self.end_month}.json', 'w') as f:
            json.dump(file, f, indent=4)

        return price, data