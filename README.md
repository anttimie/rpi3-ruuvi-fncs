# Azure function app for RPI3 and RuuviTag combo

Extract Event Grid messages from RPI3, that listens to RuuviTag over Bluetooth.

## Why
---

- to extract data from Azure Event Grid messages
- save to Azure Storage
- "process" the data
- build web app or whatever you like to display the data :)

## Steps
---

- git clone
- open
- build
- run to test @localhost
- create a free function app with AI to [Azure](https://portal.azure.com)
- publish to the previously created function app