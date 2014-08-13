PoshZen
============

Powershell Client for ZenDesk

Installation
----
### The PSGet Way

If you don't already have [psget](http://psget.net/), here's the one-liner:
```powershell
(new-object Net.WebClient).DownloadString("http://psget.net/GetPsGet.ps1") | iex
```
Then install the latest release of PoshZen like so:
```powershell
install-module PoshZen
```

### The Regular Way
Download the [latest release zip](https://github.com/ghostsquad/PoshZen/releases)

Extract the contents to one of the following module folders
  * C:\Users\YourUsernameHere\Documents\WindowsPowerShell\Modules\
  * C:\Program Files\WindowsPowerShell\Modules
  * C:\windows\system32\WindowsPowerShell\v1.0\Modules\

E.G. 
```
C:\Users\wes\Documents\WindowsPowerShell\Modules\PoshZen> gci | %{$_.name}
PoshZen.psd1
...
```

Execute ```Import-Module PoshZen``` (or add this to your profile)


Getting Started
----

```powershell
# set your credentials
Set-ZendeskCredentials -Domain "https://mydomain.zendesk.com" -UserName "user@domain.com" -Password "password123"

# cmdlet discovery
C:\> Get-Command -Module PoshZen

CommandType     Name                                               ModuleName
-----------     ----                                               ----------
Cmdlet          Get-Ticket                                         PoshZen
Cmdlet          Get-Tickets                                        PoshZen
...

```