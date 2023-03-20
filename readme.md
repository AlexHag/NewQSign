# QSign Refactored

Moving and changing controller logic to services and updating the authentication system.

## API First

| Resource | Method | Description | Input | Output |
| -------- | ------ | ----------- |------ |------- |
| api/login | post | Logs in user | Email; Passowrd; | JWT |
| api/register | post | Registers the user | FirstName; LastName; Email; Password; | JWT |
| api/document | get [Authorize] | Returns the users documents |  | List<DocumentData\> |
| api/document | post [Authorize] | Uploads a document | IFormFile | Created At api/document/{id} |
| api/document/{id} | get | Get one document data and the signatures assosiated with it | DocumentId | <DocumentData\> <SignatureInfo\> |
| api/document/{id}/download | get | Get one document file | DocumentId | File |
| api/document/{id}/sign | post [Authorize] | Signs a document | DocumentId | <SignatureInfo\> |

## API Details

### [post] api/register
Request Body
```
{
  "firstName": "Alex",
  "lastName": "Hag",
  "email": "alex@email.com",
  "password": "password"
}
```
Response
```
{
  "authorization": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjY5ZjY3MmVjLTIzMzItNGRmMi04MzAwLTMxNzcwNThkNWRjMiIsIm5iZiI6MTY3NzI0MzI3NywiZXhwIjoxNjc3ODQ4MDc3LCJpYXQiOjE2NzcyNDMyNzcsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6MzAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTA4OSJ9.giJWr8yjktv8T1ycpZkceWUKfXncSmHp8XT89uXXTuI",
  "userInfo": {
    "firstName": "Alex",
    "lastName": "Hag",
    "email": "alex@email.com",
    "publicKeyPem": "04b4ec37e729c527263d72e996b737e0dbc7dd1e1e1bdbd1da92cde9c5b7e6390f0697154c4c5a1cda82eacb92509689f13f45faaba26c9b13b07bccb25ff616e0"
  }
}
```
### [post] api/login
Request Body
```
{
  "email": "alex@email.com",
  "password": "password"
}
```
Response
```
{
  "authorization": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjY5ZjY3MmVjLTIzMzItNGRmMi04MzAwLTMxNzcwNThkNWRjMiIsIm5iZiI6MTY3NzI0MzMwMywiZXhwIjoxNjc3ODQ4MTAzLCJpYXQiOjE2NzcyNDMzMDMsImlzcyI6Imh0dHA6Ly9sb2NhbGhvc3Q6MzAwMCIsImF1ZCI6Imh0dHA6Ly9sb2NhbGhvc3Q6NTA4OSJ9.DRTXvyvllVMyZgdVN2AdZjRPiDERQGBKgt84Zm-hfPA",
  "userInfo": {
    "firstName": "Alex",
    "lastName": "Hag",
    "email": "alex@email.com",
    "publicKeyPem": "04b4ec37e729c527263d72e996b737e0dbc7dd1e1e1bdbd1da92cde9c5b7e6390f0697154c4c5a1cda82eacb92509689f13f45faaba26c9b13b07bccb25ff616e0"
  }
}
```
### [get] api/document
Response
```
[
  {
    "id": "d774a077-622a-4f66-bd2a-3a9b2c76238c",
    "subjectUserId": "69f672ec-2332-4df2-8300-3177058d5dc2",
    "subjectFirstName": "Alex",
    "subjectLastName": "Hag",
    "subjectEmail": "alex@email.com",
    "filename": "contract1.txt",
    "hash": "5042929b3a7f1214403bcd253227d4554eaf3e53066744c14c723482f6d671f1"
  },
  {
    "id": "436886d5-bc3f-4a30-b764-85f8aeed0656",
    "subjectUserId": "69f672ec-2332-4df2-8300-3177058d5dc2",
    "subjectFirstName": "Alex",
    "subjectLastName": "Hag",
    "subjectEmail": "alex@email.com",
    "filename": "contract2.txt",
    "hash": "8c96dc343d2ab8a1e8cad2037ce143a967e756d56700970edbf880c7caaee2df"
  }
]
```
### [post] api/document
Request Body
```
File
```
Response
```
{
  "id": "d774a077-622a-4f66-bd2a-3a9b2c76238c",
  "subjectUserId": "69f672ec-2332-4df2-8300-3177058d5dc2",
  "subjectFirstName": "Alex",
  "subjectLastName": "Hag",
  "subjectEmail": "alex@email.com",
  "filename": "contract1.txt",
  "hash": "5042929b3a7f1214403bcd253227d4554eaf3e53066744c14c723482f6d671f1"
}
```
### [get] api/document/{id}
Response
```
{
  "documentInfo": {
    "id": "436886d5-bc3f-4a30-b764-85f8aeed0656",
    "subjectUserId": "69f672ec-2332-4df2-8300-3177058d5dc2",
    "subjectFirstName": "Alex",
    "subjectLastName": "Hag",
    "subjectEmail": "alex@email.com",
    "filename": "contract2.txt",
    "hash": "8c96dc343d2ab8a1e8cad2037ce143a967e756d56700970edbf880c7caaee2df"
  },
  "documentSignatures": [
    {
      "id": "847f6ba8-cf71-4517-ba9c-fdf9045f7116",
      "subjectId": "69f672ec-2332-4df2-8300-3177058d5dc2",
      "subjectFirstName": "Alex",
      "subjectLastName": "Hag",
      "subjectEmail": "alex@email.com",
      "documentId": "436886d5-bc3f-4a30-b764-85f8aeed0656",
      "documentHash": "8c96dc343d2ab8a1e8cad2037ce143a967e756d56700970edbf880c7caaee2df",
      "signature": "fd69cd975292667020cd43215e193a46dcbd12e00dbe0512cf48c0e91e559a4c2c6828332b159774182015653fc31de85382839afddbe74e3cfdfd6472431d6b",
      "subjectPublicKey": "04b4ec37e729c527263d72e996b737e0dbc7dd1e1e1bdbd1da92cde9c5b7e6390f0697154c4c5a1cda82eacb92509689f13f45faaba26c9b13b07bccb25ff616e0",
      "issuedAt": "2023-02-24T12:57:59.1716284"
    },
    {
      "id": "07663cde-d1a3-49cf-87c1-198d45e53acd",
      "subjectId": "1d01f86c-13ef-44a8-8d5c-1fd0531af2f9",
      "subjectFirstName": "Axel",
      "subjectLastName": "D",
      "subjectEmail": "axel@email.com",
      "documentId": "436886d5-bc3f-4a30-b764-85f8aeed0656",
      "documentHash": "8c96dc343d2ab8a1e8cad2037ce143a967e756d56700970edbf880c7caaee2df",
      "signature": "64ab2b07dcae9f7921bc28e745863ab0f23404e61b2920632320a7bfb72ccaa531fa8cb5d668ce06bef3a42765a18357df1bf6433a5059b0521b65f93fcf374e",
      "subjectPublicKey": "04643368e29b1599f4175743696cb0a1d159d7b0b6c8a12c593440d81771c6f5aec24515c9b3657d5be8c2d18a03e0a923862124f3fa9123f203495d989327bebe",
      "issuedAt": "2023-02-24T13:02:42.6230714"
    }
  ]
}
```

### [get] api/document/{id}/download
Response
```
File
```

### [post] api/document/{id}/sign
Response
```
{
  "id": "07663cde-d1a3-49cf-87c1-198d45e53acd",
  "subjectId": "1d01f86c-13ef-44a8-8d5c-1fd0531af2f9",
  "subjectFirstName": "Axel",
  "subjectLastName": "D",
  "subjectEmail": "axel@email.com",
  "documentId": "436886d5-bc3f-4a30-b764-85f8aeed0656",
  "documentHash": "8c96dc343d2ab8a1e8cad2037ce143a967e756d56700970edbf880c7caaee2df",
  "signature": "64ab2b07dcae9f7921bc28e745863ab0f23404e61b2920632320a7bfb72ccaa531fa8cb5d668ce06bef3a42765a18357df1bf6433a5059b0521b65f93fcf374e",
  "subjectPublicKey": "04643368e29b1599f4175743696cb0a1d159d7b0b6c8a12c593440d81771c6f5aec24515c9b3657d5be8c2d18a03e0a923862124f3fa9123f203495d989327bebe",
  "issuedAt": "2023-02-24T13:02:42.6230714Z"
}
```