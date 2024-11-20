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
### Retrie All Entries
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

### Retrieve Data For A Specific Entry
To retrieve data for a specific entry by its ID, send a get request to the following dynamic endpoint with the ID of the entry: `/api/BloodBank/{id}`.






