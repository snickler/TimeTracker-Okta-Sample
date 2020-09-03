# TimeTracker
This sample repository is a modification of the WorkItemStopWatch project demoed for UnoConf. The goal of this sample is to demonstrate how to integrate the Okta .NET SDKs into both a Universal Windows Platform applicaton and an ASP.NET Core API. 

## What Does This Thing Do?

The core of this applicaton revolves around the `TimeTracker.UWP` and `TimeTracker.WebAPI` projects. 

#### TimeTracker UWP

The TimeTracker UWP application allows you to visually track time against individual work items in the style of a stop watch. Native authentication is required, and is implemented through the use of Authorization Code Flow with PKCE from the Okta Xamarin SDK library and the resulting access token is used to send requests to the TimeTracker API.

#### TimeTracker WebAPI

The TimeTracker WebAPI project runs as an ASP.NET Core 3.1 API Server which communicates with the current autenticated user's Okta profile. It reads and writes a JSON serialized string to a custom `workItems` attibute.



## Prequisites


Before cloning the repository, the following prequisites must be met:

- Windows 10 Version 1903 or greater
- Latest [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) with the Universal Windows Platform and .NET Core Cross-platform development workload installed.
- An [Okta Developer account](https://www.okta.com/developer/signup/)

## Getting Started

An Okta Application, API token, and User Profile attribute addition are needed to properly execute the application.

#### Create a new Okta Application
 
After your Okta Developer account is complete, create a new Okta Application from within your Okta Developer Console by accessing `https://{yourOktaDomain}/dev/console/apps/new`, replacing {yourOktaDomain} with the first part of the URL that is similar to  `dev-12345.okta.com`. It is important to make sure the `-admin` is removed from the url. 

Since this is for the UWP application, you will need to select `Native Client`. Select `Refresh Token` under the Grant type allowed section on the next page, then press Done. 

#### Create a new API Token

Hover over `API` and click on `Tokens` from the Okta Developer Console. Enter a name for the token and press Create Token. 

Copy the token to a place you will be able to retrieve.


#### Add `workItems` attribute to User

Hover over `Users` then click on `Profile Editor`. Edit the profile of the default Okta user: `User (default)`.

Click the `Add Atribute` button and create a `string` attribute with the following values:


 | 
--- | --- 
**Display name** | Work Items 
**Variable name** | workItems

Save when finished.


#### Clone repository

Clone the repository by using the command below.

``` 
git clone git://github.com/snickler/TimeTracker 
```



### Configure the solution

Open `TimeTracker.sln` in Visual Studio 2019 from your local copy of the repository. 

The solution contains 5 projects:

Project | Purpose
--- | ---
Okta.Xamarin | .NET Standard 2.0 Okta OIDC library for Xamarin
Okta.Xamarin.UWP | Provides UWP support for the Okta OIDC library, included from https://github.com/phenixdotnet/okta-oidc-xamarin/tree/uwp-support
TimeTracker.Core | Minor support library for interacting between the UWP app and WebAPI
TimeTracker.UWP | Main Universal Windows Platform application. Uses the Okta.Xamarin.UWP library
TimeTracker.WebApi | ASP.NET Core 3.1 API server. Uses the Okta.Sdk library



#### Add Client Details to TimeTracker.UWP

The `ConfigureContainer` method inside of `App.xaml.cs` reigsters an instance of the OIDC Client is registered within the Unity container. 

Enter the `ClientId` and the `PostLogoutRedirectUri` from the Native Client you first created. UWP applications require an extra Login Redirect Uri to function properly. Add the `RedirectUri` value below, to the Login redirect URIs list from the Okta Developer Console. 

```csharp
Container.RegisterInstance<IOidcClient>(new OidcClient(new OktaConfig
{
	ClientId = "{clientId}",
	OktaDomain = "http://{yourOktaDomain}",
	RedirectUri = "ms-app://s-1-15-2-2232705493-413311223-3232844436-3813861752-4134362570-1499584371-116746020/",
	PostLogoutRedirectUri = "{yourOktaDomain}:/",
	Scope = "openid profile offline_access"
}));
```

#### Add API Token Details to TimeTracker.WebApi

The Okta client information is stored and read from `appsettings.json`. 

Enter `https://{yourOktaDomain}` into the `Domain` property. 

To add the `Token`, right-click on the TimeTracker.WebApi project and click `Manage User-Secrets`. 

Create the `Okta:Token` configuration value in the `secrets.json` file and enter the API Token you saved earlier.

For development purposes, you wouldn't want this API token stored with the other common configuration. The User Secrets are stored in your local user profile.


```json
{
  "Okta": {
    "Token": "{yourApiToken}"
  }
}
``` 


#### Add `dotnet-devcerts`

Communicating with the WebApi will fail if the IIS Development Certificate isn't trusted.

Run the following command in command prompt: 

`dotnet dev-certs https --trust`


### Running the App and WebApi Projects

The solution is configured to run both projects. Press `F5` to begin debugging. 

The UWP Application will load and should present and authentication dialog. The WebApi will open a browser that lists the OpenAPI spec.




## Frameworks and Libraries Used

### TimeTracker.UWP
- Okta OIDC Xamarin SDK
- Windows Template Studio
- WinUI 2
- Prism
- Newtonsoft.Json

### TimeTracker.WebAPI
- ASP.NET Core
- Okta .NET Management SDK
- Okta OIDC ASP.NET Core
- Swashbuckle.ASPNetCore