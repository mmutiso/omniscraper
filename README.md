# Omni Scraper
A Twitter bot that attempts to determine the intent of a Tweep and delivers the content they need. 

Try for yourself by tagging [@omniscraper](https://twitter.com/omniscraper) on Twitter.

The bot will reply with either:
- A link to thread.
- A link to a video/gif in the replied tweet.



## Contribution
This is a .NET project build on the .NET 5 framework and fully in the open. The driver program is `Omniscraper.Daemon` which is a console application hosting a backgroud service. Development has been done using Visual studio 2019 but should work just fine using VS code(untested). 

Database migration can be performed from the `Omniscraper.Core` project using the usual dotnet entity framework commands defined in the link [here](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli).
 
Every merge(push or PR) into the `main` branch will trigger a build configured in the Github actions. 

Current build status is as below. 

![build status](https://github.com/mmutiso/omniscraper/actions/workflows/main-merge-build.yml/badge.svg)


The web part to all this is developed from [this](https://github.com/frankiemutiso/omniscraper-web) repository by [Francis](https://github.com/frankiemutiso). 

## Support 

As at this point, support by following and tagging [@omniscraper](https://twitter.com/omniscraper) on Twitter to capture your videos/GIFS and soon threads.