# LoopStats
Loopstats is a way for getting statistics on the [Loopring Protocol](https://www.loopring.io). I use [Juan Mardefagos graph](https://api.thegraph.com/subgraphs/name/juanmardefago/loopring36/graphql) to fetch statistics every 15 minutes and also every day at 00:30. This can be used as you want, but since this is a personal account, please don't go nuts calling the API. 

## What data can you get
* blockCount
* transactionCount
* transferCount
* transferNFTCount
* tradeNFTCount
* nftMintCount
* userCount
* nftCount

### Latest
GetLatest will as the name states, get you the latest data. 

### Last day count
The last day count gets you how many blocks, transactions, transfers, transfer NFT, trades, mints, users and NFTs there have been the last 24 hours (from when you queried)

### All stats
Gets you all rows of stats (up until 1000 rows). This is from the partition every 15 minutes

### All daily
Gets you all the rows of stats (up until 1000). This is from the partition every day. 

### Count daily
This gets you the count from the latest daily (collected at 00:30) to the latest block collected. 

## Data
Since this is a new repository the data is not that big yet. I will try to add historic data later, but as for now it is only from 16th May and onwards. 

**NOW INCLUDES DATA (except user count) from block 20000. Will add more data at a later point**
