# Ticker Subscription Web Application

This is a demonstration web application with two features:
- Subscription Background Service
  - Connects to the Deribit service with JSON-RPC over WebSockets
  - Retrieves all instruments for all currencies
  - Subscribes to each instrument's ticker, and persists that data within Azure table storage
- Web API
  - Makes available the persisted data on a per-instrument basis.


## How to run

For the data persistence to work, you need a local Azure storage emulator running ([see Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio)).

Once the storage emulator is running, you can open the solution in Visual Studio and hit build/run.

The Web API will then be available via Swagger. You can "Try It Out", enter any instrument name, and hit execute to retrieve the persisted data for that instrument.
