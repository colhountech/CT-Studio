# Obsolete

I've moved the WebApi Functionality into the WebApp Project.
This project is no long being used.
I've keep the README files because it gives some interesting insights.

# Eventual Consistency

This ASP.NET REST Api implementation is a great example of how eventual consistency can cause
aparent bugs.

While the Web Api works as intended, there are 2 clients accessing azure storage at the same time: WebApp and WebApi

* The WebApp is responsible for displaying the data
* The WebApi (Currently) is resposible for reordering the data

When the WebApi re-orders the data, then WebApp does not retrieve the most update version, and
after going into edit mode and  reordering and the pressing [save] if you were to hit F5 Refresh
to reload the page, the order will reset and look like the data didn't save.

However, if you restart the program (or wait a few minutes), the correct saved order is retrieved and
the order is correct.

Great time to look at change to look at eTAGS and [Managing Concurrency in Blob Storage](https://docs.microsoft.com/en-us/azure/storage/blobs/concurrency-manage?tabs=dotnet)
