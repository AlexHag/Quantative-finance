export interface Portfolio {
  id: number | undefined
  cashTransactions: Array<CashTransaction> | undefined
  stockTransactions: Array<StockTransaction>  | undefined

  portfolioPositions: Array<PortfolioPosition> | undefined

  portfolioReturns: Array<PortfolioReturn> | undefined

  cashBalance: number | undefined
}

export interface CashTransaction {
  id: number | undefined
  amount: number | undefined
  type: string | undefined
  createdAt: Date | undefined
}

export interface StockTransaction {
  id: number | undefined
  stock: Stock | undefined
  quantity: number | undefined
  orderPrice: number | undefined
  type: string | undefined
  createdAt: Date | undefined
}

export interface PortfolioPosition {
  id: number | undefined
  stock: Stock | undefined
  quantity: number | undefined
  cashIn: number | undefined
  cashOut: number | undefined
}

export interface Stock {
  id: number | undefined
  name: string | undefined
  ticker: string | undefined
  // StockPrices
}

export interface PortfolioReturn {
  id: number | undefined
  date: Date | undefined
  value: number | undefined
}


// export interface Portfolio {
//   Id: number
//   CashTransactions: Array<CashTransaction>
//   StockTransactions: Array<StockTransaction>

//   PortfolioPositions: Array<PortfolioPosition>

//   CashBalance: number
// }

// export interface CashTransaction {
//   Id: number
//   Amount: number
//   Type: string
//   CreatedAt: Date
// }

// export interface StockTransaction {
//   Id: number
//   Stock: Stock
//   Quantity: number
//   OrderPrice: number
//   Type: string
//   CreatedAt: Date
// }

// export interface PortfolioPosition {
//   Id: number
//   Stock: Stock
//   Quantity: number
//   CashIn: number
//   CashOut: number
// }

// export interface Stock {
//   Id: number
//   Name: string
//   Ticker: string
//   // StockPrices
// }
