# QSign Refactored

Moving and changing controller logic to services and updating the authentication system.

Api First

| Resource | Method | Description | Input | Output |
| -------- | ------ | ----------- |------ |------- |
| api/login | post | Logs in user | Email; Passowrd; | JWT |
| api/register | post | Registers the user | FirstName; LastName; Email; Password; | JWT |
| api/document | get [Authorize] | Returns the users documents |  | List<DocumentData\> |
| api/document | post [Authorize] | Uploads a document | IFormFile | Created At api/document/{id} |
| api/document/{id} | get | Get one document data | DocumentId | <DocumentData\> |
| api/document/{id}/download | get | Get one document file | DocumentId | File |
| api/signature | post [Authorize] | Signs a document | DocumentId | <SignatureInfo\> |
| api/signature/{id} | get | Get one signature | SidnatureId | <SignatureInfo\> |
| api/signature/document/{id} | get | Get all documents signatures | DocumentId | <SignatureInfo\> |

API Change in commit 7

| Resource | Method | Description | Input | Output |
| -------- | ------ | ----------- |------ |------- |
| api/login | post | Logs in user | Email; Passowrd; | JWT |
| api/register | post | Registers the user | FirstName; LastName; Email; Password; | JWT |
| api/document | get [Authorize] | Returns the users documents |  | List<DocumentData\> |
| api/document | post [Authorize] | Uploads a document | IFormFile | Created At api/document/{id} |
| api/document/{id} | get | Get one document data and the signatures assosiated with it | DocumentId | <DocumentData\> <SignatureInfo\> |
| api/document/{id}/download | get | Get one document file | DocumentId | File |
| api/document/{id}/sign | post [Authorize] | Signs a document | DocumentId | <SignatureInfo\> |
| The api's | bellow | can | be | deleted |
| api/signature | post [Authorize] | Signs a document | DocumentId | <SignatureInfo\> |
| api/signature/{id} | get | Get one signature | SidnatureId | <SignatureInfo\> |
| api/signature/document/{id} | get | Get all documents signatures | DocumentId | <SignatureInfo\> |