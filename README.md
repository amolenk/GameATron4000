ngrok http -host-header=rewrite 5000

TODO

- Run locally against emulator
- Run locally against cloud
- Run production in cloud

- Dev
  - Create dev.bot file
  - install/run ngrok 
  - Create azure bot service
  - update ngrok url when necessary
  - debug
- Prod
  - create .bot file
  - build/push container
  - deploy container to azure
  - create azure bot service
  - run

# Deploy to Azure

## Create a Resource Group
To create a Linux App Service Plan, you can use the following command:

```
az group create --name <resource-group-name> --location westus
```

az group create --name PlayGameATron4000 --location westeurope

Use this newly created Resource Group as the default group in any subsequent
commands so we don't have to type it in each time. Tell the CLI that we want
everything stored in the West US data center too.

```
az configure --defaults group=<resource-group-name> location=westus
```
az configure --defaults group=PlayGameATron4000 location=westeurope

## Create an Linux App Service Plan
az appservice plan create -n GameATron4000Plan --is-linux --sku S1 --number-of-workers 1

# Create a custom Docker container Web App
az webapp create --name PlayGameATron4000 --plan GameATron4000Plan --deployment-container-image-name amolenk/gameatron4000:latest

change:

az webapp config container set -n PlayGameATron4000 -c amolenk/gameatron4000:latest


# Activate the Docker container logging
az webapp log config --name PlayGameATron4000 --web-server-logging filesystem


## env variables:
az webapp config appsettings set \
    --name PlayGameATron4000 \
    --settings GAMEATRON4000__BOT__FILESECRET='1VT7SDK0Qn1u1L8qhF6i84/PSZ5wrjz8Qzd23mKhZOg=' 

# Create Bot registration

See also: https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tools-az-cli?view=azure-bot-service-3.0

az bot create \
    --kind registration \
    --name GameATron4000Prod \
    --description "Game-a-Tron 4000" \
    --endpoint https://playgameatron4000.azurewebsites.net/api/messages \
    --sku S1

az bot directline create --name GameATron4000Prod

https://apps.dev.microsoft.com/#/appList


# Bot file

## Dev

TO-DO

## Production

Steps for prod (not in order yet):
- Create a bot registration
- Deploy to web app
- Setup CI
- Change .bot file
- Build/push container

TODO CI

TODO Remove current file

```
msbot init \
    --name GameATron4000 \
    --endpoint http://localhost/api/messages \
    --secret
```

```
msbot connect bot \
    --serviceName <botRegistrationName> \
    --name Production \
    --tenantId <tenantId> \
    --subscriptionId <subscriptionId> \
    --resourceGroup <resourceGroup> \
    --appId <appId> \
    --appPassword <appPassword> \
    --endpoint https://<botWebAppName>.azurewebsites.net/api/messages \
    --bot GameATron4000.bot \
    --secret <prodBotFileSecret>
```

```
msbot connect generic \
    --name DirectLine \
    --url "no-url" \
    --keys "{\"secret\":\"<directLineSecret>\"}" \
    --bot GameATron4000.bot \
    --secret <prodBotFileSecret>
```




msbot init \
    --name GameATron4000 \
    --endpoint http://localhost/api/messages \
    --secret

msbot connect bot \
    --serviceName GameATron4000Prod \
    --name Production \
    --tenantId amolenkampgmail \
    --subscriptionId 3cabbfa5-126e-4dc3-b68d-1d7fd2ad4583 \
    --resourceGroup PlayGameATron4000 \
    --appId "983aa9a9-188a-4561-88ae-a5f934c06f95" \
    --appPassword "yqegFCBTMF84340$^:{uliR" \
    --endpoint https://playgameatron4000.azurewebsites.net/api/messages \
    --bot GameATron4000.bot \
    --secret 1VT7SDK0Qn1u1L8qhF6i84/PSZ5wrjz8Qzd23mKhZOg=

msbot connect generic \
    --name DirectLine \
    --url "no-url" \
    --keys "{\"secret\":\"touOGSsfn8g.cwA.-Ds.hBgQ1r3uXyBCpQ8pYDL1nIxFxtnwhuLZZDeba9vCyqs\"}" \
    --bot GameATron4000.bot \
    --secret 1VT7SDK0Qn1u1L8qhF6i84/PSZ5wrjz8Qzd23mKhZOg=






