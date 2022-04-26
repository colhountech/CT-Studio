# CT-Studio

Code for Demos on the [CT Studio YouTube Channel](https://www.youtube.com/channel/UC-mHR47cULEfJHvk49t1zQA/)

## Tasks 


  ### Tasks WebApp -  The Most Basic ASP.NET Core WebApp in the World  - Written in C# 10 and .NET 6


This WebApp does everything you need except save the tasks. That's for another day

  * No Authentication
  * No HTTPS support
  * No Docker Supporet
  * .NET 6.0 Support
  * Minimal APIs


Our basic template `Program.cs` looks like this.

```c#

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

```
There is a lot happening here, so let's break this down.

ASP.NET 6.0 is a modular framework where we build up all the services that we need and hook these services into our Web Application URL requests as needed. This makes the framework very fast and light and secure for specific purposes, but extensible so that we can have more functionality as needed.

First we use the sealed class `WebApplication` to configure the web app and setup pipelines and routes. This hides a lot of complexity and gives us reasonable defaults to start. We can always exposes these defaults and change the startup settings in many different ways later.

We first get access to an instance of the web application builder object, that allows us to build a web application with reasonable defaults

```
var builder = WebApplication.CreateBuilder(args);

```
Next, we add support for the RazorPages Service thorugh an extenion method. Extensions methods are great way of keeping libraries small and added extensions to libraries without having to recompile libraries. This is useful for adding just the services that our web application needs to support. 

In this case, we  add RazorPage Service which is a framework provided service that contains the RazorPages engine and understands the Razor Pages markup to help build HTML that renders dynamic strings and objects. We could also build our own services and hook into the Web Application pipeline. Anything is possible.

While we have added the ability to understand Razor Pages markup and processing, we haven't attached this service to a route yet, so it won't work until we map the RazorPages service to the request pipeline, see later.

```
// Add services to the container.
builder.Services.AddRazorPages();

```
Next we build the web application

```
var app = builder.Build();
```
That's pretty much all that's required but let's add a few basic things to make it better.

We wil add a few features to the pipeline to hook into certain "middleware" features.

First, we hook  into errors if we are using the development environment. This gives us a nice frieldly error message if something goes wrong. 

Obviously on a real production server, we don't want to expose this information to the user, because it might expose sensitive information about our error such as passwords.

```
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
```
Next, we hook in some middleware to read files off the disk and serve them unaltered to the user. This is for files like text files, image files, css files and javascript files that don't require the web application server to process do anything with.

```
app.UseStaticFiles();
```

We will add middleware to hook into certain URL routes, and if any URL routes matches our defined code pages, then our request will get routed to the code pages we have created.

```
app.UseRouting();
```
We will add middelwarwe for authorization, although we won't use this yet.

```
app.UseAuthorization();
```

We now enable Razor Pages by added the abiliy to detect RazorPages in the incoming requests and map these to our Razor code pages that we have written. 

```
app.MapRazorPages();
```

Fine. All ready to run. Let's start the Web Application.

```
app.Run();

```

There are a few more pages we need to be aware of before we run the code. 

#### Pages

Pages is where our basic web pages are stored. By default we will route to Index.cshtml which is the Razor page dynamic markup , and Index.cshtml.cs which is the code that is behind this markup page. You will see a few pages here including:

* Index.cshtml - Default Home PAge
* Error.cshtml - The Page that's loaded if we error out
* Privact.cshtml - a default razor page where you can drop in your privacy terms and conditions

Some lesser known files and folders include:

* _ViewStart.cshtml - a default setting for supporting different themes. The default theme is in the Shared folder as _Layout.cshtml
* _ViewImports.cstml - This is like a global bucket that is used to give hints on searching for other libraries and code pages
* Shared\ - Folder of shared resource such as Themes and shared Controls
* Shared\_ValidationSCriptPartial - This is used for forms that need client side validation before we send form data to the server. Mostly this just works and we won't need to worry about this ( for a while )

OK. Let's run this. Our request is reouted to the default Razor Page `Index.cshtml` and is rendered to our browser. 



  ### Tasks WebApp - Configure for HTTPS 
  ### Tasks WebApp Authentication
  


  ### Tasks WebApi 

  ### Tasks DB is JSON File

  ### Tasks Database First

  ### Tasks DB is DbUp

  ### Tasks DB is SqlSchemaComparison

  ### Tasks EF Code First

  ### Tasks CosmoDB

  ### Tasks Docker

  ### Tasks Kubernetes

  ### Tasks Azure K8s






