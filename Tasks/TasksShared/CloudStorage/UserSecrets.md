# Adding User Secrets to ASP.NET Core Web App

<i>Note: To read this file as rendered markdown, open in Visual Studio Code, the press Ctrl-Shift-V to view.</i>


Don't add connection strings with passwords to files that will be checked into git.

Instead use User Secrets. Right Click on the ASP.NET Core Visual Studio Project, and select "Manage User Secrets".


Add the following:

```json
{
  "AppConfig": {
    "AzureConnectionString": ""
  }
}
```

Then find the correct value for `AzureConnectionString` from the Azure Storage Explorer.

# Finding Azure Storage Connection Strings from Azure Storage Explorer

To find the Azure Storage Connection string from Azure Storage Explorer, 

1. Open Azure Storage Explorer and navigate to the correct Storage Account. 
2. Choose the Storage Account container to reveal the Properties for that container.
3. Scoll down the Properties of the Storage Account container to "Primary Connection String", and unhide it's value field.
4. Copy the complete "Primary Connection String" value and paste it into the AzureConnectionString value in your secrets file.

Your final `secrets.json` file should look like this:

```json
{
  "AppConfig": {
    "AzureConnectionString": "DefaultEndpointsProtocol=https;AccountName=**REPLACE**;AccountKey=**REPLACE**==;BlobEndpoint=https://**REPLACE**.blob.core.windows.net/;QueueEndpoint=https://**REPLACE**.queue.core.windows.net/;TableEndpoint=https://**REPLACE**.table.core.windows.net/;FileEndpoint=https://**REPLACE**.file.core.windows.net/;"
  }
}
```

<i>Note: The `secrets.json` file is stored in %AppData%\UserSecrets\\\<UserSecretsId>\secrets.json which is outside of the folder structure that is checked into git.  The `UserSecretsId`  value is stored in the `csproj` file like this: `<UserSecretsId>a62eec96-80cc-4403-b2ce-7f0ab8caefb3</UserSecretsId>`
</i>

<i>Note: You  can remove the endpoints you are not using, so in this example, I am only using BlobEndpoint, 
so I can remove QueueEndpoint, TableEndpoint, FileEndpoint. If you are unsure, don't remove any.
</i>



