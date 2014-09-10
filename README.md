ViewRanger BuddyBeacon .NET client
==========

An integration library for .NET applications to access ViewRanger's BuddyBeacon API. The library allows users of the ViewRanger app to download the last seen locations of users.

With only a few lines of C# you can find the last time a user's BuddyBeacon was sent.


```C#
var appKey = @"123456789";
var username = "user1";
var pin = "1234";
var client = new ViewRangerClient(appKey);
var lastLocation = client.GetLastPosition()
    .ForUser(username, pin)
    .Request();
	


var track = client.GetTrack()
	.ForUser(username, pin)
    .From(DateTime.Now.AddHours(-5))
    .To(DateTime.Now)
    .Limit(50)
    .Request();	
```
