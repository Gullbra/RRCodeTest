# Azure App Service Deployment Guide for ASP.NET 8 + Angular 18

## Prerequisites

- Azure account (free tier available)
- Visual Studio 2022 or VS Code with C# extension
- .NET 8 SDK installed
- Node.js 18+ installed
- Angular CLI installed globally (`npm install -g @angular/cli`)
- Git installed

## Project Structure Setup

Your project should be structured like this:
```
MyProject/
├── MyProject.Server/          # ASP.NET 8 Web API
│   ├── Controllers/
│   ├── Program.cs
│   └── MyProject.Server.csproj
├── myproject.client/          # Angular 18 app
│   ├── src/
│   ├── angular.json
│   └── package.json
└── MyProject.sln
```

## Step 1: Configure Your ASP.NET 8 Backend

### 1.1 Update Program.cs

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://your-app-name.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthorization();
app.MapControllers();

// Serve Angular static files
app.UseDefaultFiles();
app.UseStaticFiles();

// Fallback to index.html for Angular routing
app.MapFallbackToFile("index.html");

app.Run();
```

### 1.2 Update .csproj File

Add these properties to your ASP.NET project file:

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <SpaRoot>../myproject.client</SpaRoot>
    <SpaProxyLaunchCommand>npm start</SpaProxyLaunchCommand>
    <SpaProxyServerUrl>http://localhost:4200</SpaProxyServerUrl>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.AspNetCore.SpaProxy" Version="8.0.0" />
  </ItemGroup>

  <Target Name="DebugEnsureNodeEnv" BeforeTargets="Build" Condition=" '$(Configuration)' == 'Debug' And !Exists('$(SpaRoot)\node_modules') ">
    <Exec Command="node --version" ContinueOnError="true">
      <Output TaskParameter="ExitCode" PropertyName="ErrorCode" />
    </Exec>
    <Exec WorkingDirectory="$(SpaRoot)" Command="npm install" />
  </Target>

  <Target Name="PublishRunWebpack" AfterTargets="ComputeFilesToPublish">
    <Exec WorkingDirectory="$(SpaRoot)" Command="npm install" />
    <Exec WorkingDirectory="$(SpaRoot)" Command="npm run build" />

    <ItemGroup>
      <DistFiles Include="$(SpaRoot)\dist\**" />
      <ResolvedFileToPublish Include="@(DistFiles->'%(FullPath)')" Exclude="@(ResolvedFileToPublish)">
        <RelativePath>%(DistFiles.Identity)</RelativePath>
        <CopyToPublishDirectory>PreserveNewest</CopyToPublishDirectory>
        <ExcludeFromSingleFile>true</ExcludeFromSingleFile>
      </ResolvedFileToPublish>
    </ItemGroup>
  </Target>

</Project>
```

## Step 2: Configure Angular 18 Frontend

### 2.1 Update angular.json

Ensure your build configuration outputs to `dist/`:

```json
{
  "projects": {
    "myproject-client": {
      "architect": {
        "build": {
          "builder": "@angular-devkit/build-angular:browser",
          "options": {
            "outputPath": "dist",
            "index": "src/index.html",
            "main": "src/main.ts",
            "polyfills": "src/polyfills.ts",
            "tsConfig": "tsconfig.app.json"
          }
        }
      }
    }
  }
}
```

### 2.2 Update Base Href

In your Angular `index.html`, set the base href:

```html
<base href="/" />
```

### 2.3 Configure API Calls

Update your Angular services to use relative URLs:

```typescript
// environment.ts
export const environment = {
  production: false,
  apiUrl: '/api'  // Relative URL
};

// environment.prod.ts
export const environment = {
  production: true,
  apiUrl: '/api'  // Relative URL
};
```

## Step 3: Deploy to Azure App Service

### Method 1: Using Azure Portal (Recommended for beginners)

1. **Create Azure App Service**
   - Go to [Azure Portal](https://portal.azure.com)
   - Click "Create a resource" → "Web App"
   - Fill in:
     - **Subscription**: Your subscription
     - **Resource Group**: Create new or use existing
     - **Name**: Your app name (must be unique)
     - **Publish**: Code
     - **Runtime stack**: .NET 8 (LTS)
     - **Operating System**: Windows or Linux
     - **Region**: Choose closest to your users
     - **Pricing**: Free F1 (for testing)

2. **Configure Deployment**
   - After creation, go to your App Service
   - Navigate to "Deployment Center"
   - Choose "GitHub" or "Azure DevOps" as source
   - Authorize and select your repository
   - Choose branch (usually `main`)
   - Azure will auto-detect your .NET project

### Method 2: Using Azure CLI

```bash
# Install Azure CLI if not installed
# Login to Azure
az login

# Create resource group
az group create --name myResourceGroup --location "East US"

# Create App Service plan (Free tier)
az appservice plan create --name myAppServicePlan --resource-group myResourceGroup --sku F1

# Create web app
az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name myUniqueAppName --runtime "DOTNET:8.0"

# Deploy from local Git
az webapp deployment source config-local-git --name myUniqueAppName --resource-group myResourceGroup
```

### Method 3: Using Visual Studio

1. Right-click on your ASP.NET project
2. Select "Publish"
3. Choose "Azure"
4. Select "Azure App Service (Windows)" or "Azure App Service (Linux)"
5. Sign in to Azure
6. Create new App Service or select existing
7. Click "Publish"

## Step 4: Configure Application Settings

In Azure Portal, go to your App Service → Configuration → Application Settings:

```
ASPNETCORE_ENVIRONMENT = Production
WEBSITE_NODE_DEFAULT_VERSION = 18.17.0
SCM_DO_BUILD_DURING_DEPLOYMENT = true
```

## Step 5: Set up Custom Domain (Optional)

1. Go to App Service → Custom domains
2. Add your domain
3. Configure DNS records as shown
4. Enable SSL certificate (free with App Service)

## Step 6: Monitor and Troubleshoot

### Enable Application Insights
1. Go to App Service → Application Insights
2. Click "Turn on Application Insights"
3. Configure settings and create

### Check Logs
1. Go to App Service → Log stream
2. Or use Azure CLI: `az webapp log tail --name myAppName --resource-group myResourceGroup`

### Common Issues and Solutions

**Issue**: 404 errors on Angular routes
**Solution**: Ensure `MapFallbackToFile("index.html")` is in Program.cs

**Issue**: CORS errors
**Solution**: Update CORS policy with your Azure domain

**Issue**: Build failures
**Solution**: Check that Node.js version is compatible in Application Settings

**Issue**: Static files not loading
**Solution**: Verify `UseStaticFiles()` and `UseDefaultFiles()` are configured

## Step 7: Set up CI/CD (Optional)

### Using GitHub Actions

Create `.github/workflows/azure-webapps-dotnet-core.yml`:

```yaml
name: Build and deploy ASP.Net Core app to Azure Web App

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build:
    runs-on: windows-latest

    steps:
    - uses: actions/checkout@v3

    - name: Set up .NET Core
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'

    - name: Set up Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'

    - name: Build with dotnet
      run: dotnet build --configuration Release

    - name: dotnet publish
      run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp

    - name: Upload artifact for deployment job
      uses: actions/upload-artifact@v3
      with:
        name: .net-app
        path: ${{env.DOTNET_ROOT}}/myapp

  deploy:
    runs-on: windows-latest
    needs: build
    environment:
      name: 'Production'
      url: ${{ steps.deploy-to-webapp.outputs.webapp-url }}

    steps:
    - name: Download artifact from build job
      uses: actions/download-artifact@v3
      with:
        name: .net-app

    - name: Deploy to Azure Web App
      id: deploy-to-webapp
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'your-app-name'
        slot-name: 'Production'
        publish-profile: ${{ secrets.AZUREAPPSERVICE_PUBLISHPROFILE }}
        package: .
```

## Costs and Limitations

**Free Tier (F1) includes:**
- 1 GB storage
- 165 compute hours/month
- Custom domains supported
- 10 apps maximum
- No auto-scaling
- Apps sleep after 20 minutes of inactivity

**Next tier (B1) - $13.14/month:**
- 10 GB storage
- No sleep mode
- Custom SSL certificates
- Auto-scaling up to 3 instances

## Security Best Practices

1. **Use HTTPS**: Always enabled by default
2. **Environment Variables**: Store secrets in Application Settings
3. **Authentication**: Consider Azure AD integration
4. **CORS**: Restrict to your domain only
5. **API Keys**: Use Azure Key Vault for sensitive data

## Conclusion

Your ASP.NET 8 + Angular 18 app should now be running on Azure App Service. The free tier is perfect for development and small projects, with easy scaling options when you need more resources.

Remember to monitor your usage through the Azure Portal to stay within free tier limits!