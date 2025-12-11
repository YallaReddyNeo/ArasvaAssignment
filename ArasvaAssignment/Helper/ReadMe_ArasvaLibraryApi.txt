******************** Arasva Library API **********************

A simple REST API for managing Books, Members, and Borrowing operations in a library system.

---

## Base URLs

- **Books**: [/api/v1/Book]
- **Members**: [/api/v1/Member]
- **Borrowing**: [/api/v1/Borrowing]

All responses (except some error cases) are wrapped in a GlobalResponse<T>:

json
{
  "success": true,
  "message": "Action completed successfully.",
  "error": null,
  "data": { ... },
  "totalCount": 1 // only for list endpoints
}

1. Book Endpoints

Controller: BookController
Route prefix: /api/v1/Book/getall

1.1 Get all books (with optional filters)

GET /api/v1/Book

Query parameters (optional):
isAvailable – true or false
true: only books that are not currently borrowed (no active borrow entry)
false: only books that are currently borrowed
null / omitted: ignore availability filter (return all books)
author – optional string, filters books where Author contains this value (case-insensitive)

Examples:
GET /api/v1/Book/getall
→ returns all books

GET /api/v1/Book/getall?isAvailable=true
→ returns only available books

GET /api/v1/Book?isAvailable=false&author=rowling
→ returns borrowed books whose author contains rowling

Response (GlobalResponse<IEnumerable<BookFullResponseDTO>>):

{
  "success": true,
  "message": "Action completed successfully.",
  "error": null,
  "data": [
    {
      "id": 1,
      "name": "Book Title",
      "author": "Author Name",
      "pages": 200,
      "category": "Fiction",
      "isActive": true,
      "createdBy": 1,
      "createdDate": "2025-01-01T10:00:00Z",
      "modifiedBy": null,
      "modifiedDate": null
    }
  ],
  "totalCount": 1
}

1.2 Get a single book by ID

GET /api/v1/Book/get/{id}

Path parameter:
id – Book ID (int)

Use:
Returns details of a single book.

Responses:
200 OK – if book exists (GlobalResponse<BookFullResponseDTO>)
400 Bad Request – if book not found (with error message)

1.3 Create a new book

POST /api/v1/Book/create

Body (BookCreateDTO):

{
  "name": "New Book Title",
  "author": "Author Name",
  "pages": 250,
  "category": "Fiction",
  "isActive": true,
  "createdBy": 1
}

Use:
Creates a new book record.

Response:
GlobalResponse<BookCreateResponseDTO> containing the created book with generated Id.

1.4 Update an existing book

PUT /api/v1/Book/update/{id}

Path parameter:
id – Book ID to update

Body (BookUpdateDTO):

{
  "name": "Updated Title",
  "author": "Updated Author",
  "pages": 300,
  "category": "Non-Fiction",
  "isActive": true,
  "modifiedBy": 2
}

Use:
Updates an existing book.

Responses:
200 OK – on success (GlobalResponse<BookUpdateResponseDTO>)
400 Bad Request – if book not found (with error message)

2. Member Endpoints

Controller: MemberController
Route prefix: /api/v1/Member

2.1 Get all members

GET /api/v1/Member/getall

Use:
Returns list of all members.

Response:
GlobalResponse<IEnumerable<MemberFullResponseDTO>>

2.2 Get a single member by ID

GET /api/v1/Member/get/{id}

Path parameter:
id – Member ID (int)

Use:
Returns details of a single member.

Responses:
200 OK – if member exists (GlobalResponse<MemberFullResponseDTO>)
400 Bad Request – if member not found

2.3 Create a new member

POST /api/v1/Member/create

Body (MemberCreateDTO):

{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "isActive": true,
  "createdBy": 1
}


Use:
Creates a new member.

Response:
GlobalResponse<MemberCreateResponseDTO> with new member details.

2.4 Update an existing member

PUT /api/v1/Member/update/{id}

Path parameter:
id – Member ID to update

Body (MemberUpdateDTO):

{
  "name": "John Doe Updated",
  "email": "john.updated@example.com",
  "isActive": true,
  "modifiedBy": 2
}


Use:
Updates member details.

Responses:
200 OK – on success (GlobalResponse<MemberUpdateResponseDTO>)
400 Bad Request – if member not found (with error message)

3. Borrowing Endpoints

Controller: BorrowingController
Route prefix: /api/v1/Borrowing

Business rules enforced:
A member can borrow a book only if it is available.
A book becomes unavailable while borrowed (there is an active borrow record).
A member can return a borrowed book.
The system records both borrow date and return date.

Prevent:
Multiple active borrows of the same book.
Returning a book that is not currently borrowed by that member.

3.1 Borrow a book

POST /api/v1/Borrowing/borrowbook

Body (BorrowRequestDTO):

{
  "bookId": 1,
  "memberId": 10,
  "borrowFromDate": "2025-01-01T09:00:00Z", // optional; if omitted, server uses current time
  "createdBy": 1
}


Use:
Checks that:
Book exists
Member exists
Book is not currently borrowed (no active borrow with BorrowToDate == null)
Creates a new BorrowingHistory record.

Responses:
200 OK – on success (GlobalResponse<BorrowResponseDTO>)
400 Bad Request – if book is already borrowed or validation fails
404 Not Found – if book or member does not exist

3.2 Return a borrowed book

POST /api/v1/Borrowing/returnbook

Body (ReturnRequestDTO):

{
  "bookId": 1,
  "memberId": 10,
  "borrowToDate": "2025-01-10T10:00:00Z", // optional; if omitted, server uses current time
  "modifiedBy": 2
}


Use:
Checks that:
Book exists
Member exists
There is an active borrow for this member & book (BorrowToDate == null)
Updates BorrowToDate, ModifiedBy, ModifiedDate.

Responses:
200 OK – on success (GlobalResponse<BorrowResponseDTO>)
400 Bad Request – if there is no active borrow for this member/book
404 Not Found – if book or member does not exist

3.3 Get full borrowing history of a member

GET /api/v1/Borrowing/borrowhistory/{memberId}

Path parameter:
memberId – Member ID (int)

Use:
Returns the complete borrowing history for a given member.
Includes both active and returned borrows.

Responses:
200 OK – GlobalResponse<IEnumerable<MemberBorrowHistoryDTO>>
404 Not Found – if member does not exist

Example response:

{
  "success": true,
  "message": "Action completed successfully.",
  "error": null,
  "data": [
    {
      "id": 5,
      "bookId": 1,
      "bookName": "Some Book",
      "borrowFromDate": "2025-01-01T09:00:00Z",
      "borrowToDate": "2025-01-10T10:00:00Z",
      "isActiveBorrow": false
    },
    {
      "id": 6,
      "bookId": 2,
      "bookName": "Another Book",
      "borrowFromDate": "2025-02-01T09:00:00Z",
      "borrowToDate": null,
      "isActiveBorrow": true
    }
  ],
  "totalCount": 2
}

Notes
=====
Availability Logic:
A book is unavailable if there exists a BorrowingHistory row with that BookId and BorrowToDate == null.
Otherwise, it is considered available.
All create/update operations assume some basic audit fields (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) are maintained in the database model.