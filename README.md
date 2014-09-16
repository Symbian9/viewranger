ViewRanger BuddyBeacon .NET Client
==========

An integration library for .NET applications to access ViewRanger's BuddyBeacon API. The library allows users of the ViewRanger app to download the last seen locations of users.

With only a few lines of C# you can query the API to find geographical information about your users.


```C#
var username = "user1";
var pin = "1234";
var client = new ViewRangerClient();
var lastLocation = client.GetLastPosition()
    .ForUser(username, pin)
    .Request();	
```

Or perhaps you want to find out how far your user had walked on a particular day?

```C#
Console.WriteLine("You've walked {0} miles today",
    client.GetTrack().ForUser(username, pin).ForToday().Request().TotalDistance / 1609);
```

The ViewRanger BuddyBeacon API Client is quick and easy to use. To find out more view the [wiki](https://github.com/ardliath/viewranger/wiki) or download the [NuGet Package](https://www.nuget.org/packages/ViewRanger.BuddyBeacon.API).
