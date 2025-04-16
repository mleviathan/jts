# Curls collection for testing purposes


https://developer.atlassian.com/server/jira/platform/jira-rest-api-examples/#searching-for-issues-assigned-to-a-particular-user

```
curl --location 'https://jira.aruba.it/rest/api/2/search?startAt=0&maxResults=50&expand=names,schema,parent' --header 'Authorization {PAT}' --header 'Content-Type: application/json'
```

In this response we are interested in key, description, summary and parent.key properties.
Example of a response:

```
{
  "expand": "schema,names",
  "startAt": 0,
  "maxResults": 50,
  "total": 6,
  "issues": [
    {
      "expand": "operations,versionedRepresentations,editmeta,changelog,renderedFields",
      "id": "10005",
      "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10005",
      "key": "SMS-6",
      "fields": {
        "statuscategorychangedate": "2025-04-14T18:43:55.467+0200",
        "issuetype": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10001",
          "id": "10001",
          "description": "Tasks track small, distinct pieces of work.",
          "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium",
          "name": "Task",
          "subtask": false,
          "avatarId": 10318,
          "entityId": "7960515e-d27f-43fe-aa90-12d4bc43cb4d",
          "hierarchyLevel": 0
        },
        "parent": {
          "id": "10001",
          "key": "SMS-2",
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10001",
          "fields": {
            "summary": "(Sample) Billing and Invoicing",
            "status": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/status/10000",
              "description": "",
              "iconUrl": "https://leviathancode.atlassian.net/",
              "name": "To Do",
              "id": "10000",
              "statusCategory": {
                "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
                "id": 2,
                "key": "new",
                "colorName": "blue-gray",
                "name": "To Do"
              }
            },
            "priority": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
              "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
              "name": "Medium",
              "id": "3"
            },
            "issuetype": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10002",
              "id": "10002",
              "description": "Epics track collections of related bugs, stories, and tasks.",
              "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium",
              "name": "Epic",
              "subtask": false,
              "avatarId": 10307,
              "entityId": "7c604a2d-2431-4d24-9bc6-dd601705f3b7",
              "hierarchyLevel": 1
            }
          }
        },
        "components": [],
        "timespent": null,
        "timeoriginalestimate": null,
        "description": "As an admin, I want to generate monthly invoices for all active subscriptions so that users can see their billing history.",
        "project": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/project/10000",
          "id": "10000",
          "key": "SMS",
          "name": "jts",
          "projectTypeKey": "software",
          "simplified": true,
          "avatarUrls": {
            "48x48": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412",
            "24x24": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=small",
            "16x16": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=xsmall",
            "32x32": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=medium"
          }
        },
        "fixVersions": [],
        "statusCategory": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/4",
          "id": 4,
          "key": "indeterminate",
          "colorName": "yellow",
          "name": "In Progress"
        },
        "customfield_10034": null,
        "aggregatetimespent": null,
        "resolution": null,
        "customfield_10015": null,
        "security": null,
        "aggregatetimeestimate": null,
        "resolutiondate": null,
        "workratio": -1,
        "summary": "(Sample) Generate Monthly Invoices",
        "lastViewed": "2025-04-14T20:53:00.760+0200",
        "watches": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-6/watchers",
          "watchCount": 1,
          "isWatching": true
        },
        "creator": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "subtasks": [],
        "created": "2025-04-14T18:43:54.729+0200",
        "reporter": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "customfield_10021": null,
        "aggregateprogress": {
          "progress": 0,
          "total": 0
        },
        "priority": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
          "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
          "name": "Medium",
          "id": "3"
        },
        "customfield_10001": null,
        "labels": [],
        "environment": null,
        "customfield_10019": "0|i0000v:",
        "timeestimate": null,
        "aggregatetimeoriginalestimate": null,
        "versions": [],
        "duedate": null,
        "progress": {
          "progress": 0,
          "total": 0
        },
        "votes": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-6/votes",
          "votes": 0,
          "hasVoted": false
        },
        "issuelinks": [],
        "assignee": null,
        "updated": "2025-04-14T18:43:55.467+0200",
        "status": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/status/10001",
          "description": "",
          "iconUrl": "https://leviathancode.atlassian.net/",
          "name": "In Progress",
          "id": "10001",
          "statusCategory": {
            "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/4",
            "id": 4,
            "key": "indeterminate",
            "colorName": "yellow",
            "name": "In Progress"
          }
        }
      }
    },
    {
      "expand": "operations,versionedRepresentations,editmeta,changelog,renderedFields",
      "id": "10003",
      "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10003",
      "key": "SMS-5",
      "fields": {
        "statuscategorychangedate": "2025-04-14T18:43:55.417+0200",
        "issuetype": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10001",
          "id": "10001",
          "description": "Tasks track small, distinct pieces of work.",
          "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium",
          "name": "Task",
          "subtask": false,
          "avatarId": 10318,
          "entityId": "7960515e-d27f-43fe-aa90-12d4bc43cb4d",
          "hierarchyLevel": 0
        },
        "parent": {
          "id": "10000",
          "key": "SMS-1",
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10000",
          "fields": {
            "summary": "(Sample) User Subscription Management",
            "status": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/status/10000",
              "description": "",
              "iconUrl": "https://leviathancode.atlassian.net/",
              "name": "To Do",
              "id": "10000",
              "statusCategory": {
                "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
                "id": 2,
                "key": "new",
                "colorName": "blue-gray",
                "name": "To Do"
              }
            },
            "priority": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
              "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
              "name": "Medium",
              "id": "3"
            },
            "issuetype": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10002",
              "id": "10002",
              "description": "Epics track collections of related bugs, stories, and tasks.",
              "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium",
              "name": "Epic",
              "subtask": false,
              "avatarId": 10307,
              "entityId": "7c604a2d-2431-4d24-9bc6-dd601705f3b7",
              "hierarchyLevel": 1
            }
          }
        },
        "components": [],
        "timespent": null,
        "timeoriginalestimate": null,
        "description": "As a user, I want to create a new subscription so that I can access premium features.",
        "project": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/project/10000",
          "id": "10000",
          "key": "SMS",
          "name": "jts",
          "projectTypeKey": "software",
          "simplified": true,
          "avatarUrls": {
            "48x48": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412",
            "24x24": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=small",
            "16x16": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=xsmall",
            "32x32": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=medium"
          }
        },
        "fixVersions": [],
        "statusCategory": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/3",
          "id": 3,
          "key": "done",
          "colorName": "green",
          "name": "Done"
        },
        "customfield_10034": null,
        "aggregatetimespent": null,
        "resolution": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/resolution/10000",
          "id": "10000",
          "description": "Work has been completed on this issue.",
          "name": "Done"
        },
        "customfield_10015": null,
        "security": null,
        "aggregatetimeestimate": null,
        "resolutiondate": "2025-04-14T18:43:55.406+0200",
        "workratio": -1,
        "summary": "(Sample) Create User Subscription",
        "lastViewed": null,
        "watches": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-5/watchers",
          "watchCount": 1,
          "isWatching": true
        },
        "creator": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "subtasks": [],
        "created": "2025-04-14T18:43:54.719+0200",
        "reporter": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "customfield_10021": null,
        "aggregateprogress": {
          "progress": 0,
          "total": 0
        },
        "priority": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
          "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
          "name": "Medium",
          "id": "3"
        },
        "customfield_10001": null,
        "labels": [],
        "environment": null,
        "customfield_10019": "0|i0000n:",
        "timeestimate": null,
        "aggregatetimeoriginalestimate": null,
        "versions": [],
        "duedate": null,
        "progress": {
          "progress": 0,
          "total": 0
        },
        "issuelinks": [],
        "votes": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-5/votes",
          "votes": 0,
          "hasVoted": false
        },
        "assignee": null,
        "updated": "2025-04-14T18:43:55.417+0200",
        "status": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/status/10002",
          "description": "",
          "iconUrl": "https://leviathancode.atlassian.net/",
          "name": "Done",
          "id": "10002",
          "statusCategory": {
            "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/3",
            "id": 3,
            "key": "done",
            "colorName": "green",
            "name": "Done"
          }
        }
      }
    },
    {
      "expand": "operations,versionedRepresentations,editmeta,changelog,renderedFields",
      "id": "10002",
      "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10002",
      "key": "SMS-4",
      "fields": {
        "statuscategorychangedate": "2025-04-14T18:43:55.261+0200",
        "issuetype": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10001",
          "id": "10001",
          "description": "Tasks track small, distinct pieces of work.",
          "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium",
          "name": "Task",
          "subtask": false,
          "avatarId": 10318,
          "entityId": "7960515e-d27f-43fe-aa90-12d4bc43cb4d",
          "hierarchyLevel": 0
        },
        "parent": {
          "id": "10000",
          "key": "SMS-1",
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10000",
          "fields": {
            "summary": "(Sample) User Subscription Management",
            "status": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/status/10000",
              "description": "",
              "iconUrl": "https://leviathancode.atlassian.net/",
              "name": "To Do",
              "id": "10000",
              "statusCategory": {
                "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
                "id": 2,
                "key": "new",
                "colorName": "blue-gray",
                "name": "To Do"
              }
            },
            "priority": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
              "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
              "name": "Medium",
              "id": "3"
            },
            "issuetype": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10002",
              "id": "10002",
              "description": "Epics track collections of related bugs, stories, and tasks.",
              "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium",
              "name": "Epic",
              "subtask": false,
              "avatarId": 10307,
              "entityId": "7c604a2d-2431-4d24-9bc6-dd601705f3b7",
              "hierarchyLevel": 1
            }
          }
        },
        "components": [],
        "timespent": null,
        "timeoriginalestimate": null,
        "description": "As a user, I want to update my subscription details so that I can change my plan or payment method.",
        "project": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/project/10000",
          "id": "10000",
          "key": "SMS",
          "name": "jts",
          "projectTypeKey": "software",
          "simplified": true,
          "avatarUrls": {
            "48x48": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412",
            "24x24": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=small",
            "16x16": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=xsmall",
            "32x32": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=medium"
          }
        },
        "fixVersions": [],
        "statusCategory": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/4",
          "id": 4,
          "key": "indeterminate",
          "colorName": "yellow",
          "name": "In Progress"
        },
        "customfield_10034": null,
        "aggregatetimespent": null,
        "resolution": null,
        "customfield_10015": null,
        "security": null,
        "aggregatetimeestimate": null,
        "resolutiondate": null,
        "workratio": -1,
        "summary": "(Sample) Update User Subscription",
        "watches": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-4/watchers",
          "watchCount": 1,
          "isWatching": true
        },
        "lastViewed": "2025-04-16T19:42:39.634+0200",
        "creator": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "subtasks": [],
        "created": "2025-04-14T18:43:54.701+0200",
        "reporter": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "customfield_10021": null,
        "aggregateprogress": {
          "progress": 0,
          "total": 0
        },
        "priority": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
          "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
          "name": "Medium",
          "id": "3"
        },
        "customfield_10001": null,
        "labels": [],
        "environment": null,
        "customfield_10019": "0|i00007:",
        "timeestimate": null,
        "aggregatetimeoriginalestimate": null,
        "versions": [],
        "duedate": null,
        "progress": {
          "progress": 0,
          "total": 0
        },
        "votes": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-4/votes",
          "votes": 0,
          "hasVoted": false
        },
        "issuelinks": [],
        "assignee": null,
        "updated": "2025-04-14T18:43:55.260+0200",
        "status": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/status/10001",
          "description": "",
          "iconUrl": "https://leviathancode.atlassian.net/",
          "name": "In Progress",
          "id": "10001",
          "statusCategory": {
            "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/4",
            "id": 4,
            "key": "indeterminate",
            "colorName": "yellow",
            "name": "In Progress"
          }
        }
      }
    },
    {
      "expand": "operations,versionedRepresentations,editmeta,changelog,renderedFields",
      "id": "10004",
      "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10004",
      "key": "SMS-3",
      "fields": {
        "statuscategorychangedate": "2025-04-14T18:43:55.195+0200",
        "issuetype": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10001",
          "id": "10001",
          "description": "Tasks track small, distinct pieces of work.",
          "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10318?size=medium",
          "name": "Task",
          "subtask": false,
          "avatarId": 10318,
          "entityId": "7960515e-d27f-43fe-aa90-12d4bc43cb4d",
          "hierarchyLevel": 0
        },
        "parent": {
          "id": "10001",
          "key": "SMS-2",
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10001",
          "fields": {
            "summary": "(Sample) Billing and Invoicing",
            "status": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/status/10000",
              "description": "",
              "iconUrl": "https://leviathancode.atlassian.net/",
              "name": "To Do",
              "id": "10000",
              "statusCategory": {
                "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
                "id": 2,
                "key": "new",
                "colorName": "blue-gray",
                "name": "To Do"
              }
            },
            "priority": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
              "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
              "name": "Medium",
              "id": "3"
            },
            "issuetype": {
              "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10002",
              "id": "10002",
              "description": "Epics track collections of related bugs, stories, and tasks.",
              "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium",
              "name": "Epic",
              "subtask": false,
              "avatarId": 10307,
              "entityId": "7c604a2d-2431-4d24-9bc6-dd601705f3b7",
              "hierarchyLevel": 1
            }
          }
        },
        "components": [],
        "timespent": null,
        "timeoriginalestimate": null,
        "description": "As a user, I want to ensure my payments are processed securely so that I can trust the subscription service.",
        "project": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/project/10000",
          "id": "10000",
          "key": "SMS",
          "name": "jts",
          "projectTypeKey": "software",
          "simplified": true,
          "avatarUrls": {
            "48x48": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412",
            "24x24": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=small",
            "16x16": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=xsmall",
            "32x32": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=medium"
          }
        },
        "fixVersions": [],
        "customfield_10034": null,
        "statusCategory": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
          "id": 2,
          "key": "new",
          "colorName": "blue-gray",
          "name": "To Do"
        },
        "aggregatetimespent": null,
        "resolution": null,
        "customfield_10015": null,
        "security": null,
        "aggregatetimeestimate": null,
        "resolutiondate": null,
        "workratio": -1,
        "summary": "(Sample) Payment Processing Integration",
        "watches": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-3/watchers",
          "watchCount": 1,
          "isWatching": true
        },
        "lastViewed": "2025-04-14T20:23:00.880+0200",
        "creator": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "subtasks": [],
        "created": "2025-04-14T18:43:54.701+0200",
        "reporter": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "customfield_10021": null,
        "aggregateprogress": {
          "progress": 0,
          "total": 0
        },
        "priority": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
          "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
          "name": "Medium",
          "id": "3"
        },
        "customfield_10001": null,
        "labels": [],
        "environment": null,
        "customfield_10019": "0|i0000f:",
        "timeestimate": null,
        "aggregatetimeoriginalestimate": null,
        "versions": [],
        "duedate": null,
        "progress": {
          "progress": 0,
          "total": 0
        },
        "issuelinks": [],
        "votes": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-3/votes",
          "votes": 0,
          "hasVoted": false
        },
        "assignee": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "updated": "2025-04-14T20:50:05.561+0200",
        "status": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/status/10000",
          "description": "",
          "iconUrl": "https://leviathancode.atlassian.net/",
          "name": "To Do",
          "id": "10000",
          "statusCategory": {
            "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
            "id": 2,
            "key": "new",
            "colorName": "blue-gray",
            "name": "To Do"
          }
        }
      }
    },
    {
      "expand": "operations,versionedRepresentations,editmeta,changelog,renderedFields",
      "id": "10001",
      "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10001",
      "key": "SMS-2",
      "fields": {
        "statuscategorychangedate": "2025-04-14T18:43:54.558+0200",
        "issuetype": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10002",
          "id": "10002",
          "description": "Epics track collections of related bugs, stories, and tasks.",
          "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium",
          "name": "Epic",
          "subtask": false,
          "avatarId": 10307,
          "entityId": "7c604a2d-2431-4d24-9bc6-dd601705f3b7",
          "hierarchyLevel": 1
        },
        "components": [],
        "timespent": null,
        "timeoriginalestimate": null,
        "description": "Handle billing processes and generate invoices for user subscriptions.",
        "project": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/project/10000",
          "id": "10000",
          "key": "SMS",
          "name": "jts",
          "projectTypeKey": "software",
          "simplified": true,
          "avatarUrls": {
            "48x48": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412",
            "24x24": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=small",
            "16x16": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=xsmall",
            "32x32": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=medium"
          }
        },
        "fixVersions": [],
        "customfield_10034": null,
        "statusCategory": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
          "id": 2,
          "key": "new",
          "colorName": "blue-gray",
          "name": "To Do"
        },
        "aggregatetimespent": null,
        "resolution": null,
        "customfield_10015": null,
        "security": null,
        "aggregatetimeestimate": null,
        "resolutiondate": null,
        "workratio": -1,
        "summary": "(Sample) Billing and Invoicing",
        "watches": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-2/watchers",
          "watchCount": 1,
          "isWatching": true
        },
        "lastViewed": null,
        "creator": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "subtasks": [],
        "created": "2025-04-14T18:43:53.630+0200",
        "reporter": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "customfield_10021": null,
        "aggregateprogress": {
          "progress": 0,
          "total": 0
        },
        "priority": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
          "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
          "name": "Medium",
          "id": "3"
        },
        "customfield_10001": null,
        "labels": [],
        "customfield_10017": null,
        "environment": null,
        "customfield_10019": "0|hzzzzz:",
        "timeestimate": null,
        "aggregatetimeoriginalestimate": null,
        "versions": [],
        "duedate": "2025-04-28",
        "progress": {
          "progress": 0,
          "total": 0
        },
        "votes": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-2/votes",
          "votes": 0,
          "hasVoted": false
        },
        "issuelinks": [],
        "assignee": null,
        "updated": "2025-04-14T18:43:54.238+0200",
        "status": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/status/10000",
          "description": "",
          "iconUrl": "https://leviathancode.atlassian.net/",
          "name": "To Do",
          "id": "10000",
          "statusCategory": {
            "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
            "id": 2,
            "key": "new",
            "colorName": "blue-gray",
            "name": "To Do"
          }
        }
      }
    },
    {
      "expand": "operations,versionedRepresentations,editmeta,changelog,renderedFields",
      "id": "10000",
      "self": "https://leviathancode.atlassian.net/rest/api/2/issue/10000",
      "key": "SMS-1",
      "fields": {
        "statuscategorychangedate": "2025-04-14T18:43:54.549+0200",
        "issuetype": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issuetype/10002",
          "id": "10002",
          "description": "Epics track collections of related bugs, stories, and tasks.",
          "iconUrl": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/issuetype/avatar/10307?size=medium",
          "name": "Epic",
          "subtask": false,
          "avatarId": 10307,
          "entityId": "7c604a2d-2431-4d24-9bc6-dd601705f3b7",
          "hierarchyLevel": 1
        },
        "components": [],
        "timespent": null,
        "timeoriginalestimate": null,
        "description": "Manage user subscriptions including creation, updates, and cancellations.",
        "project": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/project/10000",
          "id": "10000",
          "key": "SMS",
          "name": "jts",
          "projectTypeKey": "software",
          "simplified": true,
          "avatarUrls": {
            "48x48": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412",
            "24x24": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=small",
            "16x16": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=xsmall",
            "32x32": "https://leviathancode.atlassian.net/rest/api/2/universal_avatar/view/type/project/avatar/10412?size=medium"
          }
        },
        "fixVersions": [],
        "customfield_10034": null,
        "statusCategory": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
          "id": 2,
          "key": "new",
          "colorName": "blue-gray",
          "name": "To Do"
        },
        "aggregatetimespent": null,
        "resolution": null,
        "customfield_10015": null,
        "security": null,
        "aggregatetimeestimate": null,
        "resolutiondate": null,
        "workratio": -1,
        "summary": "(Sample) User Subscription Management",
        "watches": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-1/watchers",
          "watchCount": 1,
          "isWatching": true
        },
        "lastViewed": null,
        "creator": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "subtasks": [],
        "created": "2025-04-14T18:43:53.630+0200",
        "reporter": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/user?accountId=712020%3Aa57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "accountId": "712020:a57b456f-c13f-4efc-a100-cfaf9e8b13e5",
          "emailAddress": "michelecafagna@proton.me",
          "avatarUrls": {
            "48x48": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "24x24": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "16x16": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png",
            "32x32": "https://secure.gravatar.com/avatar/77c5528503cd6ec07b73d23037d86819?d=https%3A%2F%2Favatar-management--avatars.us-west-2.prod.public.atl-paas.net%2Finitials%2FMC-2.png"
          },
          "displayName": "Michele Cafagna",
          "active": true,
          "timeZone": "Europe/Rome",
          "accountType": "atlassian"
        },
        "customfield_10021": null,
        "aggregateprogress": {
          "progress": 0,
          "total": 0
        },
        "priority": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/priority/3",
          "iconUrl": "https://leviathancode.atlassian.net/images/icons/priorities/medium_new.svg",
          "name": "Medium",
          "id": "3"
        },
        "customfield_10001": null,
        "labels": [],
        "customfield_10017": null,
        "environment": null,
        "customfield_10019": "0|hzzzzz:",
        "timeestimate": null,
        "aggregatetimeoriginalestimate": null,
        "versions": [],
        "duedate": "2025-04-21",
        "progress": {
          "progress": 0,
          "total": 0
        },
        "votes": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/issue/SMS-1/votes",
          "votes": 0,
          "hasVoted": false
        },
        "issuelinks": [],
        "assignee": null,
        "updated": "2025-04-14T18:43:54.228+0200",
        "status": {
          "self": "https://leviathancode.atlassian.net/rest/api/2/status/10000",
          "description": "",
          "iconUrl": "https://leviathancode.atlassian.net/",
          "name": "To Do",
          "id": "10000",
          "statusCategory": {
            "self": "https://leviathancode.atlassian.net/rest/api/2/statuscategory/2",
            "id": 2,
            "key": "new",
            "colorName": "blue-gray",
            "name": "To Do"
          }
        }
      }
    }
  ]
}
```