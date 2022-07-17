# webapi-msa-audit-template
ASP.NET Core Web API Audit Microservice Template using ASP Net Core 6.0, PostgreSQL, MassTranist with AmazonSQS, JWT Auth

## Install
Clone repo
```console
git clone https://github.com/dmamulashvili/webapi-msa-audit-template.git
```

Install template
```console
cd webapi-msa-audit-template
dotnet new --install .
```

Create Solution
```console
cd /<DIRECTORY_TO_CREATE_SOLUTION_AT>
dotnet new webapi-msa-audit -o "MyCompany.MyProject.Audit"
```
## Configure:
Update PostgreSQL connection string in case you're not using local one.
```json
  "ConnectionStrings": {
    "AuditDbContext": "Server=localhost;Port=5432;Database=MyCompany.MyProject.AuditDb;User Id=postgres;password=postgres"
  },
```
Create aws user with Programmatic access & read/write permissions to SNS/SQS.
>**Warning**
>The following characters are accepted in QueueName: alphanumeric characters, hyphens (-), and underscores (_).
```json
"AmazonSqsConfiguration": {
    "AccessKey": "",
    "SecretKey": "",
    "RegionEndpointSystemName": "eu-central-1",
    "QueueName" : "Audit_API"
  },
```
Configure JWT
>**Warning**
>ValidateAudience is disabled by default in `Program.cs`, you can leave it empty.
```json
"JWT": {
    "ValidAudience": "",
    "ValidIssuer": "",
    "Secret": ""
  }
```

> **Note** 
>
> ASP.NET Core Web API Identity Microservice template: <https://github.com/dmamulashvili/webapi-msa-identity-template.git>
>
> ASP.NET Core Web API Microservice template using Clean Architecture & CQRS: <https://github.com/dmamulashvili/webapi-msa-template.git>
