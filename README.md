# Deploy to Azure

## Prerequisites

To use these tools from the command line, you will need Node.js installed to your machine:

- [Node.js (v8.5 or greater)](https://nodejs.org/)
- [.NET Core SDK version 2.1.403 or higher](https://www.microsoft.com/net/download)

## 1. Install tools

- [Install latest version of the Azure CLI.](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
- [Install Bot Builder tools.](https://aka.ms/botbuilder-tools-readme)

You can now manage bots using Azure CLI like any other Azure resource.

Login to Azure CLI by running the following command:

```
az login
```

## 2. Create a new bot from Azure CLI

You can use Azure CLI to create new bots entirely from the command line.

First create a resource group using the following command:

```
az group create \
    --name TestGameATron4000 \
    --location westus
```

Use this newly created Resource Group as the default group in any subsequent
commands so we don't have to type it in each time. Tell the CLI that we want
everything stored in the West US data center too.

```
az configure --defaults \
    group=TestGameATron4000 \
    location=westus
```

The bot itself will be hosted on a Linux App Service Plan as that makes it possible to host the bot code in a container. To connect the bot to various channels, register the bot with the Bot Service by creating a Bot Channels Registration.

To secure the connection between the Bot Service and the bot, register an application with Azure AD to get a Microsoft App Id and password:

```
az ad app create --display-name GameATron4000 \
    --identifier-uris uri:gameatron4000
    --password <ChooseAnAppPassword>
```

After the command has completed, the output JSON will contain an ```appId``` element with the Microsoft App Id.

TODO

