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


License
------
MIT License

Copyright (c) 2020 Gillingoz

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
