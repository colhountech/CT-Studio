# Do Not Repeat Yourself - Pragmatic Refactoring Strategies

The next step in this evolving project is to bring in a different
serialisation type from our default File Storage serialisation, namely
Azure Blob Storage

Currently the database is stored as a json file on
local disk so we are doing a 2 step migration.

1. We will replicate the existing TodoItemService to a new
TodoItemAzureBlobService.  We will swap over to the new implementation
in the Program constructor by replacing..
`builder.Services.AddSingleton<ITodoItemService, TodoItemService>();`
with `builder.Services.AddSingleton<ITodoItemService, TodoItemAzureBlobService>();` 
and of course make sure everthing is working as
expected.

You will then notice that there is a lot of overlap between the
TodoItemService.cs and TodoItemAzureBlobService.cs and this violated
the DRY Principle (Do not Repeate Yourself)

2. We will then refactor the commonal code between both of these and
move the serialisation into a new Dependency called IRepository. One
implementation will be the IFileRepository and the other being the 
IAzureStorageBlob implementation. 

The main benefit is that when we add more feature to the ITodoService
in the future, we do not need to replicate the code in each service
implementation


