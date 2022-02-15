# Quickstart in Couchbase Lite Query in C# with dotnet and Xamarin 
#### Build an cross platform iOS, Android, and UWP App in Xamarin Forms with Couchbase Lite 

> This repo is designed to show you an app that allows users to log in and make changes to their user profile information. User profile information is persisted as a Document in the local Couchbase Lite Database. When the user logs out and logs back in again, the profile information is loaded from the Database. This app also demostrates how you can bundle, load, and use a prebuilt instance of Couchbase Lite and introduces you to the basics of the QueryBuilder interface.
> 
Full documentation can be found on the [Couchbase Developer Portal](https://developer.couchbase.com/tutorial-quickstart-xamarin-forms-query).


## Prerequisites
To run this prebuilt project, you will need:

- For iOS development a Mac running MacOS 11 or 12 
- For iOS Development [Xcode 12/13](https://apps.apple.com/us/app/xcode/id497799835?mt=12)
- For Android development SDK version 22 or higher
- For UWP development, a Windows computer running Windows 10 1903 or higher
- Visual Studio for [Mac](https://visualstudio.microsoft.com/vs/mac/) or [PC](https://visualstudio.microsoft.com/vs/)

### Installing Couchbase Lite Framework

The [Couchbase Documentation](https://docs.couchbase.com/couchbase-lite/3.0/csharp/gs-install.html) has examples on how to add Couchbase Lite via nuget package.

## App Architecture

The sample app follows the [MVP pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter), separating the internal data model, from a passive view through a presenter that handles the logic of our application and acts as the conduit between the model and the view

## Try it out

* Open `src/UserProfileDemo.sln` using Visual Studio
* Build and run the project.
* Verify that you see the login screen.

## Conclusion

This tutorial walked you through an example of how to use a pre-built Couchbase Lite database and has a simple Query example to show you how to use the `QueryBuilder` API in dotnet with C# and Xamarin Forms.
