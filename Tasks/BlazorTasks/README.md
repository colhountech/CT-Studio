# What's the difference between Blazor Server and Blazor Web Assembly (WASM)

In this Visual Studio Solution, I have created 3 projects:

* A Blazor Server Project
* A Blazor WebAssembly Project
* A Blazor Hosted Solution where the Client App and the Server API are in
different projects

What I want to do in this demo is first investigate the difference in
behaviour of 2 projects created from the default `blazorserver`
template and the `blazorwasm` template respectively.

Next I want to look at what's different between the build and
configuration sequence of the 2 difference projects

Finally, I want to look at what my Solution would look like if I was
to separate out the Client App from a typical API Controller REST
endpoint and what components we can share in common.

This should be really interesting.

# Behaviour.

First Let's run each and look at the behaviour.

## Blazor Server

I'll start with the blazor server projects. I'll set as default
startup project and spin up.

I'm going to set the Default Browser to Edge and open up the inspector.

We get 12 requests, 336kb transferred. Top 5 files by size are:

| Resource             | Size   |
|----------------------|--------|
| 1. bootstrap.min.cs  | 163 kB |
| 2. blazor.server.js  | 132 kB |
| 3. <font>            | 15 kB  |
| 4. <css>             | 9.6 kB |
| 5. <home page>       | 3.9 kB |

We also have a couple of resource begining with _blazor such as `_blazor?id=-GphP43cN2yeT5hzs4XYRA` 
 `_blazor/negotiate?negotiateVersion=1` and
`_blazor/initializers`.

I think it's safe to assume this is the signalR wiring between the rendered client and the server.

Let's navigate to the Counter page.

Click a couple of times, we can see the number incrementing.

Notice in the Network tab that no new files are loaded from the server.

What magic is this?

Blazor keeps the client DOM synced with the server by making tiny
binary message calls to the server over SignalR which is a websocket
type infrastructure that has fall back to polling and a few other
features.


To see these binary messages, reload the page and then navigate to the
_blazor?id and click across from the headers tab and the payload tab
to the Messages tab. You will see binary messages flowing up and down
between the server as you click.

Put a breakpoint inside the OnInitializedAsync() of the
FetchData.razor page and you will see that the code is hit every time
you navigate from the counter to the fetch data component.

What does this mean?

It means that the @code in the OnInitializedAsync() method to
GetForecastAsync() is called by the client which sends the request
over the wire to the Server than then runs the Methods and returns the
data over SignalR.

Show the Elements of the page. Watch what happens as you nagivate
between counter and fetch-data.

You can see parts of the page flashing as they change but the page
doesn't reload or load new pages.

Blazor Apps are SPAs or Single Page Application. A single page is
loaded and then the DOM is manipulated to load up new compoents and
elements depending on the what routes you choose.

our new terminology for SPA is to use routes rather than paths because
technically, the paths don't change, only the route that changes the
DOM is updated.

Notice also that when we navigate from Counter to Fetech Data that no
new pages are loaded via the network tab. The only thing that changes
in the network tab are these binary messages that flow to and from the
server.

Every client has to sync it's DOM state with the server, Microsoft
have told us that we can easily support 20,000 simultaneous
connections on a decent sized web server (D3_V2), after that we are
going to get memory pressure.

Stop the App. Let's now look at Blazor Wasm

# Blazor Web Assembly

I've set the startup project to the Blazor Wasm App and let's start it
again.

The first thing to notice is that you get a blank "Loading..." screen
for 10-20 seconds before anything starts.

This is because the .NET WebAssumbly runtime has to be download
alongside the client app 

On my 2015 MacBook it takes about 15 seconds - your milage may vary.
Currently for this demo I'm using .net 6.0.400

The magic behind Blazor WASM is the `blazor.webassembly.js` script
which downloads the .net wasm runtime and the app and it's
dependencies and start the whole show running. 

Once up and running, Blazor WebAssembly apps are really fast, because
of a very special architecture: In all SPAs that I am aware of, the
elemetns and components are updated by traversig the DOM tree
hierarachy and updating elements. Blazor works different because it
matains a flat array of reference to every elemtn and so traversal time of
the array is linear. This shoud make client UI updates really fast
especially for rich client app like Data Grids.


Some other great advantages of WASM include the potential for offline
work, using local storage in the browser (e.g. utilizing a local and
native sqlite wasm instance for caching data), hosting the client app
as a static web reources or CDN, utilizing client resources and Ahead
of Time Compilation (AOT) performance increase.

The bigest disadvantage is a longer load time, although assembly
trimming and optimisation in .NET 7 should see load performance get
better.

Apart from that, the apps are almost identical. Later I'll talk about
the boot sequence for each and the Entry Points to the app, but for
now, it's only important to know that the code and technology that is
run for both WASM and Blazor Server is almost identical.

Here are the difference from the perspective of your Application Code.

# Difference between Blazor Server and Blazor WASM

The key different you have to think about when buildig for Blazor
Server or Blazor WASM is in how you interract with services.


Fetch Data is a great example.

Notice the difference in the text heading for each example:

For Blazor Server we have :

```

<h1>Weather forecast</h1>

<p>This component demonstrates fetching data from a service.</p>

```

In this example, we are injecting a service directly into a Razor
component, which magically sets up the wiring for the client page to
communicate with the service running on the server.

This is a very subtle point and worth emphasising.

This page is rendered as client page, run on the client browser, but
the GetForcaseAsync still runs on the server. Every time the component
is activated, the OnInitializedAsync is run and the client page takes
to the service running on the server using SignalR to do the wiring.

This is the `_blazor?id` Binary Messages you see going over the network.


```
@code {
    private WeatherForecast[]? forecasts;
   
    protected override async Task OnInitializedAsync()
    {
	            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
    }
}
```    
    
Set a breakpoint in visual studio in the `OnIniitializedAsync` method
and you will see the code hit the breakpoint every time you navigate
to the page/route.


For Blazor Web Assembly we have:

```

<h1>Weather forecast</h1>

<p>This component demonstrates fetching data from the server.</p>

```

Again, there is a subtle difference here. In this case, the client is
running is WASM and it may not even have a .NET Core Server backend -
it could be deployed serverless to a CDN or as a static resource.

In this case, we are not injecting the service, we are injecting the
HttpClient and making a http call to a server somewhere else.

The `Http.GetFromJsonAsync` is literally downloading a static json
file called `sample-data/weather.json` from the server, but this could
just as easily hit a REST API endpoint and consume data this way too.


What is important to remember is that:

* Blazor Server app can inject Services directly into the App and the
wiring is taken care of by Blazor Server SignalR Hub.

* Blazor Wasm apps have to manage to the server calls by injecting
Http client into the client and making calls directly to the server -
if you are accessing static json resources from a static endpoint or
CDN, then this will also be blisteringly fast.


If you are coming from an existing SPA framework, then WASM may be a
more intuitive transition for you.

If you are coming from a Razor Pages or MVC framework, then Blazor
Server is probably right for you to get started.

There is a third model and that's the Hosted Model.

In this case, we seperate out the WASM Client and the API Controllers
on the server - there is also a dotnet template available by passing
the `--hosted` argument when creating a `blazorwasm` project/solution.

Let's have a quick Look

# Blazor Wasm Hosted Model

In this example, the common Domain Model is moved to a shared project
because both the wasm client and api contoller on the server need a
definition to the same model.


This is probably the closest of how most people will use Blazor App.
You have a standalone Client Blazor App that uses well defined API
endpoint and maps these json based endpoints directly to POCO's or
Model objects.

The Server only has a small change to a standard MVC or API Web App.

```
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

```

Is that it? Well - almost, we still need to reference the WASM client
project and load the Web Assembly Server.

If you edit the Server project, you will see the Web Assembly server.
Currently mine looks like this.

```
    <PackageReference Include="Microsoft.AspNetCore.Components.WebAssembly.Server" Version="6.0.8" />

```

This will setup everything to configure the SignalR wiring and setup
the blazor server files.


Now let's look at the Client App.

`Program.cs` reference the RootComponent as "#app" which is pretty
standard for SPAs.


`wwwroot/index.html` contain the '#app' div that contains the client
app. the "Loading ..." will get replaced by the running app once
blazor is up and running.

```
 <div id="app">Loading...</div>
 
```
Notice that the index.html file contains a script that loads the
blazor server.

```

    <script src="_framework/blazor.webassembly.js"></script>

```

Next the Razor App is loaded - `App.razor` into this div. This sets up
the routing and fallback if routes are not found, we also setup the
MainLayout here. The most imporant line is below:


```

    <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />

```

Notice the `MainLayout` reference, this will give us our <NavMenu/>
and you will see the @Body Property of the LayoutComponentBase. I've
simplfied it below:


```

@inherits LayoutComponentBase

    <NavMenu />

<main>

     @Body

</main>

```

As we navigate through our app, the routes are loaded into this @Body
property.


**Tip: here is a useful tip. Blazor and Razor directives are lower
case - like @code and @if and @bind. On the other hand - Properties
are uppercase like @Body and @RenderBody


And Finally, we can see each of our Razor components in the Pages
folder of the Client App.

* Pages/Index.razor
* Pages/Counter.razor
* Pages/FetchData.razor

Noties that all the razor components are upper case, and it is this
naming convention that allows us to embed an existing Razor Component
in a different page. For example if we want to embed the Counter.razor
component in the Index.razor page, we just add the component tag
<Counter>.

And that's really all there is.  Everything else is just an extension
of what you have learned here like adding new components.

I do really hope you found this useful.

Let me know.

P.S. For more Hosting and Deployment Advanced issues see
https://docs.microsoft.com/en-us/aspnet/core/blazor/host-and-deploy/webassembly?view=aspnetcore-6.0

