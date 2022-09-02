# New Solution File
1059  dotnet new sln

# Add Projects
 1063  dotnet new blazorserver --output  BlazorServerTasksApp
 1065  dotnet new blazorwasm --output BlazorWasmTasksApp 
 1067  dotnet sln add BlazorTasks.sln BlazorServerTasksApp/BlazorServerTasksApp.csproj
 1068  dotnet sln add BlazorTasks.sln BlazorWasmTasksApp/BlazorWasmTasksApp.csproj

# open in VS
1070  open BlazorTasks.sln

# Add Shared
1072  dotnet new classlib --output BlazorSharedTasks
1073  dotnet sln BlazorTasks.sln add BlazorSharedTasks/BlazorSharedTasks.csproj

# Add pwa sample
 1080  dotnet new blazorwasm --output BlazorWasmPwaApp --pwa
 1081  dotnet sln BlazorTasks.sln add BlazorWasmPwaApp/BlazorWasmPwaApp.csproj
 
# Create a separate Solution for a Hosted App ( WebAPI Server + Client App)
 1091  dotnet new blazorwasm --hosted --output BlazorWasmHostedApp 
 
 
