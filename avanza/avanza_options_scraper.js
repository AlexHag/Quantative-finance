const fs = require('fs').promises;
const cheerio = require('cheerio');

const headerMap = {
  'Köpoptioner': 'name',
  'K.antal': 'buy_order_quantity',
  'Köp': 'buy_order_price',
  'Sälj': 'sell_order_price',
  'S.antal': 'sell_order_quantity',
  'Säljoptioner': 'name'
}

async function getInstrumentHtml(instrumentId, endMonth) {
  let url = `https://www.avanza.se/borshandlade-produkter/optioner-terminer/lista.html?name=&underlyingInstrumentId=${instrumentId}&optionTypes=STANDARD&selectedEndDates=2024-${endMonth}&page=1&sortField=NAME&sortOrder=ASCENDING&activeTab=matrix`;
  return fetch(url)
    .then(p => p.text());
}

function convertTableToJson(html) {
  const $ = cheerio.load(html);
  const tableRows = $('#contentTable tr');
  const headers = [];
  const data = [];

  $(tableRows).first().find('th').each(function() {
    let baseName = $(this).text().trim();
    let className = $(this).attr('class');

    let suffix = '';
    let key = '';
    if (className.includes('tLeft')) {
      suffix = 'call_option_';
      key = headerMap[baseName];
    } else if (className.includes('tCenter')) {
      key = 'strike_price';
    } else if (className.includes('tRight')) {
      suffix = 'put_option_';
      key = headerMap[baseName];
    }

    let uniqueName = suffix + key;
    headers.push(uniqueName);
  });

  $(tableRows).slice(1).each(function() {
    const rowData = {};
    $(this).find('td').each(function(index) {
      let key = headers[index];
      rowData[key] = $(this).text().trim();
    });
    data.push(rowData);
  });

  let cleanedTable = cleanTable(data);
  return cleanedTable;
}

function cleanTable(table) {
  table.forEach(obj => {
    delete obj.call_option_undefined;
    delete obj.put_option_undefined;

    Object.keys(obj).forEach(key => {
      if (key.includes("name")) return;

      let value = obj[key];
      if (value == "-") {
        obj[key] = null;
      } else {
        value = value.replace()
        value = value.replace(/\s/g, "").replace(",", ".");
        obj[key] = parseFloat(value);
      }
    });
  })
  return table;
}

async function Main() {
  const htmlContent = await getInstrumentHtml("9270", "03");
  const stock_options = convertTableToJson(htmlContent.trim());
  console.log(JSON.stringify(stock_options, null, "\t"));
}

Main();