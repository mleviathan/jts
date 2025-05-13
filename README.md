# jts

A library to simplify fetch and clone of issue from Jira to ServiceDesk.

## Easily sync tasks from Jira to ServiceDesk

You can easily manage tasks from Jira to ServiceDesk.

## Library usage  

The entrypoint is Jts.JiraManager, with which you can fetch your Jira issues via `GetIssues()` or clone them via `CloneIssue(string issueKey, string projectKey);`. 
Please give a look at Jts.Console to see a simple implementation.

## How to run

### Jts.Console

Add your personal Jira ApiKey, BaseUrl and email to the application [secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-9.0&tabs=windows)  
An example of a valid secrets.json is:
```
{
    "apiKey": "{you_api_key}",
    "baseUrl": {base_url},
    "email": {email}
}
```

The application will check if everything is setup correctly right after starting.
