Chronos 
=========
#### Simple, Fast, Versatile and full-featured reccurence integation service for Dynamics 365

## Overview

Incredibly easy way to perform **recurring jobs** to import on premise files into Dynamics 365 for Finance and Operations.

Chronos uses Hangfire to handle background tasks in a **reliable way** and run them on shared hosting, dedicated hosting or in cloud. You can start with a simple setup and grow computational power for background jobs with time for these scenarios:

- creation of archives compatible with Dynamics 365's DMF
- upload archives to Dynamics 365 through Azure API management

Chronos configuration supports multiple environments.

Installation
-------------
Chronos is available as a windows service archieve file. Download the file from Github release section, extract the .zip file and use command line interface to install Chronos:

```
C:\Chronos\Gillingoz.Chronos.Service.exe Install
```

After installation update jobs.json file to configure Chronos service.

Usage
------
