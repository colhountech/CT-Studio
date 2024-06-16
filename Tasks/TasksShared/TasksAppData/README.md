# Class Library

This is a good time to introduce Class Libraries.

At this stage we have stored the TodoItemData.cs and MessageData.cs in the Web App project. 

Now we are introducing some new functionality, namely the `CloudStorage` project.

This new addition could cause circulare references.

The Web App Depends on the CloudStorage App, so adds CloudStorage as a Project Reference.

However, the CloudStorage needs to know about the App Data types to store, which would need a reference to Web App.

But we can't add a reference back to the Web App, or we get a circular reference and it's like falling through the looking glass in the film Inception.

Instead, we introduce a new Class Library call TasksAppData and both CloudStoage and WebApp can both reference this project
