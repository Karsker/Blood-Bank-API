# Blood Bank Management REST API
A REST API to manage blood bank entries, developed with ASP.NET Core framework.

## Models
The API consists of a single model for a blood bank entry: `BloodBankEntry`
It includes the following fields:

| Field | Desription | Type |
|---|---|---|
|Id|Unique identifier of the entry| int
| donorName | Name of the donor |string|
| donorAge |Age of the donor | int |
| donorBloodType | Type of blood | string |
| contactInfo | Email address of the donor | string |
| quantity | Quantity of blood donated (ml) | double |
| status | Status of the blood sample | DateTime |
| collectionDate | Date of collection | DateTime |
| expirationDate | Date of expiration | DateTime |

The `donorBloodType` can take one of the following values with the corresponding blood group each represents:
- `APOS`: A+
- `BPOS`: B+
- `ABPOS`: AB+
- `ABNEG`: AB-
- `OPOS`: O+
- `ONEG`: O-

The `status` can take one of the following values:
- `AVAILABLE`: Blood sample is available for use
- `REQUESTED`: Blood sample has been requested
- `EXPIRED`: Blood sample has expired and cnnot be used

The values for `donorBloodType` and `status` fields are validated in the controller.

## Data Storage
The data is stored in an in-memory list. Hence, the data does not persist between application runs.

## Endpoints
### Retrieve All Entries
To retrieve all the entries in the in the in-memory list, send a `GET` request to the endpoint: `/api/BloodBank`.

Sample response in the JSON format:
```
[
    {
        "id": 1,
        "donorName": "John Doe",
        "donorAge": 35,
        "donorBloodType": "APOS",
        "contactInfo": "johndoe@email.com",
        "quantity": 450.04,
        "collectionDate": "2024-10-10T00:00:00",
        "expirationDate": "2024-11-10T00:00:00",
        "status": "AVAILABLE"
    },
    {
        "id": 2,
        "donorName": "Jane Smith",
        "donorAge": 28,
        "donorBloodType": "BNEG",
        "contactInfo": "janesmith@email.com",
        "quantity": 422.77,
        "collectionDate": "2024-10-19T00:00:00",
        "expirationDate": "2024-11-19T00:00:00",
        "status": "EXPIRED"
    },
]
```

The data can be paginated by providing the `pages` and `size` params with the request. For example, to get only 3 entries per page, and get the data of the 2nd page, the request would look like: `https://localhost:7042/api/BloodBank?pages=2&size=3`.

#### Sorting
The response can be sorted by using the `sortBy` parameter in the request. It accepts the following values (case insensitive):

| Value | Description |
| --- | --- |
| donorname | Name of the donor |
| age | Age of the donor |
| donorbloodtype | Blood type |
| status | Blood status |
| collectiondate | Date of collection |
| expirationdate | Date of expiration |
| quantity | Quantity |

For example, to sort by collection date, use the following query: `/api/BloodBank?SortBy=collectionDate`






### Retrieve Data For A Specific Entry
To retrieve data for a specific entry by its ID, send a get request to the following dynamic endpoint with the ID of the entry: `/api/BloodBank/{id}`.

### Adding Entries
To add an entry for a blood sample to the database (in-memory), send a POST request to the endpoint: `/api/BloodBank`. The request body (JSON) must contain each of the following fields:
- `donorName`: A string value
- `donorAge`: Number greater than or equal to 18
- `donorBloodType`: Any one of the valid values for blood type as defined in the `BloodBankEntry` model as a string
- `contactInfo`: A string representing an email address (should be in valid format)
- `quantity`: A number
- `collectionDate`: A string in valid date format (YYYY-MM-DD)
- `expirationDate`: A string in valid date format (YYYY-MM-DD)
- `status`: Any one of the valid values for blood status as defined in the `BloodBankEntry` model as a string

The following is a sample POST request body
```
{
  "donorName": "Peter",
  "donorAge": 18,
  "donorBloodType": "ANEG",
  "contactInfo": "peter@gmail.com",
  "quantity": 350,
  "collectionDate": "2024-11-20",
  "expirationDate": "2024-11-22",
  "status": "AVAILABLE"
}
```

The values for `status` and `donorBloodType` may be in any case as long as they are valid. They are automatically converted to uppercase before adding to the database. While `id` may be provided in the body, it is overridden (auto generated) to the next available value in the database, and hence is not necessary to be provided.

If any of the above values are missing or are not in the valid format, the request fails with 400 (Bad Request) status code.

On success, the status code 200 (OK) is returned along with the added entry.


### Updating An Entry
To update an entry, send a PUT request to the endpoint: `/api/BloodBank` with the body containing the update blood bank entry JSON object. It is not required to include all the fields in the request. The fields that are included and valid are updated.
For example, the current entry for id 1 is:
```
 {
        "id": 1,
        "donorName": "John Doe",
        "donorAge": 35,
        "donorBloodType": "APOS",
        "contactInfo": "johndoe@email.com",
        "quantity": 450.04,
        "collectionDate": "2024-10-10T00:00:00",
        "expirationDate": "2024-11-10T00:00:00",
        "status": "AVAILABLE"
    },
```

To update the status to `REQUESTED`, send a PUT request with the body:
```
{
  "id": 1,
  "status": "Requested"
}
```

Only the `status` field gets updated. For updating, providing the `id` field is necessary. If the `id` field is not provided in the request, 404 status code (Not Found) is returned.
On seccessful updation, the updated entry is returned.


### Deleting An Entry
To delte an entry, send a DELETE request to the endpoint `/api/BloodBank/{id}`, replacing `{id}` with the `id` of the entry to delete. If an entry with the specified `id` does not exist, a 404 status code (Not Found) is returned. On successful deletion, 204 (No Content) status is returned.

### Filtering/Searching Entries
The endpoint `/api/BloodBank/search/` allows filtering the entries by the following parameters:
- Blood Type
- Status
- Donor Name

If multiple parameters are specified, the priority for the parameters are followed in the order specified above. That is, the status filter is applied after the blood type filter, followed by donor name.

For example, the following URL fetches entries that have `BPOS` blood group: `/api/BloodBank/search?bloodType=BPOS`.
The request returns the following response for the data at the time of writing:
```
[
    {
        "id": 5,
        "donorName": "David Lee",
        "donorAge": 39,
        "donorBloodType": "BPOS",
        "contactInfo": "dlee@email.com",
        "quantity": 399.87,
        "collectionDate": "2024-09-16T00:00:00",
        "expirationDate": "2024-10-16T00:00:00",
        "status": "AVAILABLE"
    },
    {
        "id": 10,
        "donorName": "Jennifer Garcia",
        "donorAge": 41,
        "donorBloodType": "BPOS",
        "contactInfo": "jgarcia@email.com",
        "quantity": 421.18,
        "collectionDate": "2024-10-17T00:00:00",
        "expirationDate": "2024-11-17T00:00:00",
        "status": "REQUESTED"
    }
]
```

The `status` parameter can be used to retrieve only `AVAILABLE` blood sample entries: `/api/BloodBank/search?bloodType=BPOS&status=AVAILABLE`. The response then is:
```
[
    {
        "id": 5,
        "donorName": "David Lee",
        "donorAge": 39,
        "donorBloodType": "BPOS",
        "contactInfo": "dlee@email.com",
        "quantity": 399.87,
        "collectionDate": "2024-09-16T00:00:00",
        "expirationDate": "2024-10-16T00:00:00",
        "status": "AVAILABLE"
    }
]
```

## Screenshots
The screenshots of the request and response for each endpoint can be found in the Screenshots directory. The screenshots for both Swagger and Postman are provided in their respective sub directories.

## Running Locally
To run the API locally, clone this repository or download the zip file and extract to a folder. 
1. Open Visual Studio
2. If not already installed, install   the **ASP.NET and web development** workload from Visual Studio Installer
3. In Visual Studio, click on **Open a project or solution** and select the `BloodBankMgmt.sln` file
4. After the solution loads, click on the **Start** button to run the program.