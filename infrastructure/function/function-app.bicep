param functionAppName string
param appServicePlanName string

param azureTenantId string
param tableName string
param connectionString string
param graphQlUrl string


resource functionApp 'Microsoft.Web/sites@2021-02-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    enabled: true
    serverFarmId: /subscriptions/d0aca4c0-2eb5-4a9b-a593-1ff443c743b4/resourceGroups/LoopyStatsRG/providers/Microsoft.Web/serverfarms/ASP-LoopyStatsRG-9938
    reserved: true
    httpsOnly: true
    redundancyMode: 'None'
    siteConfig: {
      alwaysOn: true
      ftpsState: 'FtpsOnly'
      minTlsVersion: '1.2'
      http20Enabled: true
      appSettings: [
        {
          name: 'tableName'
          value: 'tableName'
        }
        {
          name: 'connectionString'
          value: 'connectionString'
        }
        {
          name: 'graphQlUrl'
          value: 'graphQlUrl'
        }
      ]
    }
  }
}