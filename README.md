# CT-Studio

Code for Demos on the [CT Studio YouTube Channel](https://www.youtube.com/channel/UC-mHR47cULEfJHvk49t1zQA/)

## Tasks 


  ### Tasks WebApp -  The Most Basic ASP.NET Core WebApp in the World  - Written in C# 10 and .NET 6


This WebApp does everything you need except save the tasks.

Open Visual Studio 2022, and create a new Project called using the **ASP.NET Core Web App** Template. 

Choose .NET 6.0 Framework, No Authentication, No support for HTTPS and Don't enable Docker.

To summarise:

  * No Authentication
  * No HTTPS support
  * No Docker Supporet
  * .NET 6.0 Support

Click [Create] and we get a Basic Web App up and running.

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

###  5 minute Bootstrap Crash Course

Get rid of the following code:
```html
<div class="text-center">
    <h1 class="display-4">Welcome</h1>
    <p>Learn about <a href="https://docs.microsoft.com/aspnet/core">building Web apps with ASP.NET Core</a>.</p>
</div>
```

And all the following markdown.
```html
<div class="container-fluid">
    <div class="row">
      <!-- Add items here -->


      <!-- End: Add items here -->
    </div>
  </div>

```
Now, we can add a couple of item to our todo list between   `<!-- Add items here -->` and   `<!-- End: Add items here -->`

Add the following code 4 times, but replace Task Item 1 with 2, 3,4 respectively, and change first, to second, third and fourth respectively.


```html
<div class="col-lg-6 py-2">
            <div class="card active-box flex-row flex-wrap" id="contacts-nav-box" style="min-width:22rem">
                <div class="mx-auto align-self-center p-3"><i class="fa fa-solid fa-list-alt fa-4x home-icon"></i></div>
                <div class="card-block flex-fill mx-auto p-2">
                    <h3 class="card-title">Task Item 2</h3>
                    <p class="card-text" style="min-width:22rem"> This is the second thing on my todo list</p>
                </div>
            </div>
        </div>
```        

Hit the Hot Reload button. (Looks like a red blaze) and you should now have 4 items on your todo list. 

It's nice, but needs some style. Let's add some.

Open wwwroot/css/site.css and replace everything with the following:

```css


body {
    margin: 0;
    font-size: 0.9rem;
    font-weight: 300;
    line-height: 1.5;
    color: #333333;
    text-align: left;
    font-family: 'Open Sans', 'Helvetica Neue', Helvetica, Arial, sans-serif;
    background-color: ghostwhite;
    padding-top: 5rem;
}

body-content {
    padding: 3rem 1.5rem;
    text-align: center;
    background-color: white;
}

h1, h2, h3, h4, h5, h6,
.h1, .h2, .h3, .h4, .h5, .h6
.a, .b, .c, .d, .e, .f {
    margin-bottom: 0.5rem;
    font-family: inherit;
    font-weight: 300;
    line-height: 1.2;
    color: rgb(23, 162, 184);
}

h1, .a {
    font-size: 2.5rem;
}

h2, .b {
    font-size: 2rem;
}

h3, .c {
    font-size: 1.75rem;
}

h4, .d {
    font-size: 1.5rem;
}

h5, .e {
    font-size: 1.25rem;
}

h6, .f {
    font-size: 1rem;
}

.text-value {
    font-size: 2.0rem;
    line-height: 2.5;
    font-weight: 600;
}

footer {
    text-align: center;
    font-size: xx-small;
    height: 20px;
}


.big-icon {
    color: coral;
    padding: 5px;
}

.home-icon {
    color: #17a2b8;
    padding: 5px;
}


.active-box {
    cursor: pointer;
}


.tag {
    padding-left: 2px;
    padding-right: 2px;
    font-size: 8px;
}

.big-tag {
    padding: 2px;
}

input[type=checkbox] {
    /* Double-sized Checkboxes */
    -ms-transform: scale(2); /* IE */
    -moz-transform: scale(2); /* FF */
    -webkit-transform: scale(2); /* Safari and Chrome */
    -o-transform: scale(2); /* Opera */
}

.dotted {
    border: dotted 1px;
}

```
This looks a bit better but some of the awesome fonts are missing. 

Right click on wwwroot and select Add > Client Side Library. 

Using the default Provider of cdjs, search for `font awesome` in the Library: field and change the version to 4.7.0 
and includ all library files. The target location should be wwwroot. 

You will see the font-awesome libraries added to the wwwroot/lib folder.

Check your libman.json file in the root folder of your project. It should look like this:

```
{
  "version": "1.0",
  "defaultProvider": "cdnjs",
  "libraries": [
    {
      "library": "font-awesome@4.7.0",
      "destination": "wwwroot/lib/font-awesome/"
    }
  ]
}
```

Now open  Shared/_Layout.cshtml and just below the bootstrap.min.css, add the following line:

```
    <link rel="stylesheet" href="~/lib/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="//fonts.googleapis.com/css?family=Open+Sans" />
```

The top and <head> section of your _Layout.cshtml file should now look like this:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - TasksWebApp</title>
    <link rel="stylesheet" href="~/lib/font-awesome/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="//fonts.googleapis.com/css?family=Open+Sans" />
    <link rel="stylesheet" href="~/lib/font-awesome/css/fontawesome.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/TasksWebApp.styles.css" asp-append-version="true" />
</head>

```
Let's run again. That looks better. We have a nice icon, and nice title font and a nice description.

### Razor Iterators

Let's start making this a little more programable.

Create a new Folder called ViewModels and add a new class file called `TodoItems.cs`

add the following code:
```c#
namespace TasksWebApp.ViewModels
{
    public record TodoItem(string Title, string Description);

}
```
We will use this to store records of our Todo Tasks

Now, open up the Index.cshtml and add the following lines to the start of the file, just after the ViewData["Title"] line, like this:
```
@{
    ViewData["Title"] = "Home page";

    var todoItems = new List<TodoItem>();
    todoItems.Add(new TodoItem("Task Item 1", "This is the first thing on my todo list"));
    todoItems.Add(new TodoItem("Task Item 2", "This is the second thing on my todo list"));
    todoItems.Add(new TodoItem("Task Item 3", "This is the third thing on my todo list"));
    todoItems.Add(new TodoItem("Task Item 4", "This is the fourth thing on my todo list"));

}
```
Now replace the title and description in the bootstrap code to use the new records. 
For Task Item 1

Replace `Task Item 1` with `@todoItems[0].Title`.

Replace `This is the first thing on my todo list` with `@todoItems[0].Description`.

Repeate this for Task Item 2,3 and 4 but change the relevant index  to [1], [2] or [3] in each of the  bootstrap `<div>`  blocks


Each Task Item should now look like the folliow


```html
<div class="col-lg-6 py-2">
    <div class="card active-box flex-row flex-wrap" id="contacts-nav-box" style="min-width:22rem">
        <div class="mx-auto align-self-center p-3"><i class="fa fa-solid fa-list-alt fa-4x home-icon"></i></div>
        <div class="card-block flex-fill mx-auto p-2">
            <h3 class="card-title">@todoItems[0].Title</h3>
            <p class="card-text" style="min-width:22rem"> @todoItems[0].Description</p>
        </div>
    </div>
</div>
```

Run or Hot Reload the Web Application now and we should see the same result, but with a little bit of code added

Now, let's optomise this a bit and remove the repeating `<div>` block.

An important principle when learning how to code is the DRY principle. Do not Repeate Yourself. DRY.

We are repeating the same `<div>` 4 times, so we are going to replace this with an iterator that iteratrates over the complete list of ToDoItem records

For this we are going to wrap the `<div>` block in a @foreach (var item in todoItems) {   } and then replace the indexed collection `todoItems[0]` with the `item` object

```
@foreach (var item in todoItems)
{
    // .. removed some line for clarity

    <h3 class="card-title">@item.Title</h3>
    <p class="card-text" style="min-width:22rem"> @item.Description</p>

    // .. removed some line for clarity
}
```
This is great, but we have another issue we need to address.

I don't like building up the data we show via todoItem.Add() at the top of the razor page. The Razor page is really supposed to be declarative, in that it declares what the html web page looks like, but it's not the place to setup data. 

So, we are going to move our data setup to the .cshtml.cs code behind page, where it is supposed to be and tidy this file a little more.

We are also going to use a feature of the Razor page that already exists. Do you see the `@model IndexModel` at the top of the Razor Page. This is where the Razor Page is supposed to be getting it's data, so we are going to attach our `ToDoItems` to this `IndexModel`, but we set this up from the `Index.cshtml.cs` code behind page and not the 
`Index.cshtml` razor page.



```

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






