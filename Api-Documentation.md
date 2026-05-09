# LMS Platform — API Reference

> **Base URL (via Gateway):** `http://localhost:5000`  
> All requests go through the YARP API Gateway. Auth endpoints are public; everything else requires a valid JWT unless noted.

---

## Authentication Header

All protected endpoints expect:

```
Authorization: Bearer <access_token>
```

The gateway validates the JWT and forwards user identity as `X-User-Id` and `X-User-Role` headers to downstream services. Services read from those headers — **not** from the JWT directly.

---

## Standard Response Envelope

Every endpoint wraps its response in:

```json
{
  "success": true,
  "data": <payload>,
  "message": "Human-readable message.",
  "statusCode": 200
}
```

Paginated endpoints return `data` as:

```json
{
  "data": [...],
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5,
  "totalRecords": 47
}
```

---

## 1. Auth Service — `/api/auth`

### POST `/api/auth/register`

Creates a new user account. Sends a verification email before the account is active.

**Auth required:** No

**Request body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!",
  "role": 0
}
```

| Field | Type | Required | Notes |
|---|---|---|---|
| `email` | string | ✅ | |
| `password` | string | ✅ | |
| `role` | int | ❌ | `0` = Student (default), `1` = Instructor, `2` = Admin |

**Response `200`:**
```json
{
  "success": true,
  "data": {
    "userId": "guid",
    "emailVerficationToken": "string"
  },
  "message": "Registration successful. Please check your email to verify your account."
}
```

---

### POST `/api/auth/login`

**Auth required:** No

**Request body:**
```json
{
  "email": "user@example.com",
  "password": "SecurePass123!"
}
```

**Response `200`:**
```json
{
  "success": true,
  "data": {
    "accessToken": "eyJhbGci...",
    "refreshToken": "string",
    "userId": "guid"
  },
  "message": "Login successful."
}
```

---

### POST `/api/auth/refresh-token`

Exchanges a refresh token for a new access/refresh token pair.

**Auth required:** No

**Request body:**
```json
{
  "refreshToken": "string"
}
```

**Response `200`:** Same shape as login — `{ accessToken, refreshToken, userId }`.

---

### POST `/api/auth/change-password`

**Auth required:** Yes (any role)

**Request body:**
```json
{
  "currentPassword": "OldPass123!",
  "newPassword": "NewPass456!"
}
```

**Response `200`:** `data: null`, message confirms success.

---

### POST `/api/auth/forgot-password`

Initiates a password reset flow. Always returns 200 regardless of whether the email exists (prevents enumeration).

**Auth required:** No

**Request body:**
```json
{
  "email": "user@example.com"
}
```

---

### POST `/api/auth/reset-password`

Completes the reset flow using the token sent to the user's email.

**Auth required:** No

**Request body:**
```json
{
  "token": "reset-token-from-email",
  "newPassword": "NewPass456!"
}
```

---

### GET `/api/auth/verify-email?token=<token>`

Confirms the user's email address using the token from the registration email.

**Auth required:** No

**Query params:**

| Param | Type | Required |
|---|---|---|
| `token` | string | ✅ |

---

### POST `/api/auth/resend-verification`

Re-sends the verification email.

**Auth required:** No

**Request body:**
```json
{
  "email": "user@example.com"
}
```

---

## 2. User Service — `/api/users`

### GET `/api/users`

Returns all users (paginated). Admin only.

**Auth required:** Yes — `Admin`

**Query params:**

| Param | Type | Default |
|---|---|---|
| `pageNumber` | int | `1` |
| `pageSize` | int | `10` |

**Response `200`:**
```json
{
  "success": true,
  "data": {
    "data": [
      {
        "id": "guid",
        "authUserId": "guid",
        "email": "string",
        "fullName": "string",
        "role": "Student | Instructor | Admin",
        "status": "Active | Inactive"
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 3,
    "totalRecords": 25
  }
}
```

---

### GET `/api/users/me`

Returns the authenticated user's full profile.

**Auth required:** Yes (any role)

**Response `200`:**
```json
{
  "success": true,
  "data": {
    "id": "guid",
    "authUserId": "guid",
    "email": "string",
    "firstName": "string",
    "lastName": "string",
    "fullName": "string",
    "bio": "string | null",
    "avatarUrl": "string | null",
    "phoneNumber": "string | null",
    "role": "Student | Instructor | Admin",
    "status": "Active | Inactive",
    "createdAt": "ISO8601",
    "lastModifiedAt": "ISO8601 | null"
  }
}
```

---

### GET `/api/users/{id}`

Returns a specific user's profile by their User Service ID (GUID).

**Auth required:** Yes (any role)

---

### PUT `/api/users/me`

Updates the authenticated user's profile.

**Auth required:** Yes (any role)

**Request body:**
```json
{
  "firstName": "string",
  "lastName": "string",
  "bio": "string | null",
  "avatarUrl": "string | null",
  "phoneNumber": "string | null"
}
```

**Response `200`:** Full `UserProfileDto` (same shape as `GET /api/users/me`).

---

### PUT `/api/users/me/avatar`

Updates only the avatar URL (after uploading the file via the Upload service first).

**Auth required:** Yes (any role)

**Request body:**
```json
{
  "avatarUrl": "https://cdn.example.com/avatars/user.jpg"
}
```

---

### PUT `/api/users/{authUserId}/deactivate`

Soft-deactivates a user account.

**Auth required:** Yes — `Admin`

No request body needed.

---

### DELETE `/api/users/{authUserId}`

Permanently deletes a user.

**Auth required:** Yes — `Admin`

---

## 3. Course Service — `/api/courses`

### GET `/api/courses`

Fetches a paginated list of course summaries. Public — no auth needed.

**Auth required:** No

**Query params:**

| Param | Type | Default | Notes |
|---|---|---|---|
| `page` | int | `1` | |
| `pageSize` | int | `20` | |
| `category` | string | — | Filter by category name |
| `level` | int | — | `0` = Beginner, `1` = Intermediate, `2` = Advanced |
| `search` | string | — | Full-text search on title |

**Response `200`:**
```json
{
  "data": [
    {
      "id": "guid",
      "title": "string",
      "thumbnailUrl": "string | null",
      "instructorId": "guid",
      "price": 49.99,
      "level": 0,
      "status": 0,
      "category": "string | null",
      "sectionCount": 5,
      "totalLessonCount": 24,
      "totalDurationInSeconds": 18000
    }
  ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 3,
  "totalRecords": 58
}
```

> **Note:** `status` — `0` = Draft, `1` = Published, `2` = Archived. The public listing only returns Published courses in practice.

---

### GET `/api/courses/{id}`

Returns full course detail, including all sections and lessons.

**Auth required:** No

**Response `200`:**
```json
{
  "id": "guid",
  "title": "string",
  "description": "string",
  "thumbnailUrl": "string | null",
  "instructorId": "guid",
  "price": 49.99,
  "level": 1,
  "status": 1,
  "category": "string | null",
  "createdAt": "ISO8601",
  "lastModifiedAt": "ISO8601 | null",
  "sections": [
    {
      "id": "guid",
      "title": "string",
      "order": 1,
      "lessons": []
    }
  ]
}
```

**Response `404`:** `{ "code": "...", "description": "..." }`

---

### POST `/api/courses`

Creates a new course in Draft status.

**Auth required:** Yes — `Instructor` or `Admin`

**Request body:**
```json
{
  "title": "string",
  "description": "string",
  "price": 49.99,
  "level": 0,
  "category": "string | null",
  "thumbnailUrl": "string | null"
}
```

**Response `201`:** `{ "id": "guid" }` with `Location` header pointing to the created course.

---

### PUT `/api/courses/{id}`

Updates course metadata. Does not affect sections or lessons.

**Auth required:** Yes — `Instructor` or `Admin` (must be the course owner)

**Request body:** Same shape as `POST /api/courses`.

**Response `204`:** No content.

---

### DELETE `/api/courses/{id}`

Deletes the course entirely.

**Auth required:** Yes — `Instructor` or `Admin` (must be the course owner)

**Response `204`:** No content.

---

### POST `/api/courses/{id}/publish`

Transitions a course from Draft → Published and fires a `CoursePublishedEvent`.

**Auth required:** Yes — `Instructor` or `Admin` (must be the course owner)

No request body.

**Response `204`:** No content.

---

### POST `/api/courses/{id}/sections`

Adds a new section to the course.

**Auth required:** Yes — `Instructor` or `Admin`

**Request body:**
```json
{
  "title": "string",
  "order": 1
}
```

**Response `204`:** No content.

---

### DELETE `/api/courses/{id}/sections/{sectionId}`

Removes a section (and presumably its lessons).

**Auth required:** Yes — `Instructor` or `Admin`

**Response `204`:** No content.

---

### POST `/api/courses/{id}/sections/{sectionId}/lessons`

Adds a lesson to an existing section.

**Auth required:** Yes — `Instructor` or `Admin`

**Request body:**
```json
{
  "sectionId": "guid",
  "title": "string",
  "description": "string | null",
  "type": 0,
  "contentUrl": "string | null",
  "durationInSeconds": 600,
  "order": 1,
  "isFreePreview": false
}
```

| `type` value | Meaning |
|---|---|
| `0` | Video |
| `1` | Article |
| `2` | PDF |
| `3` | External Link |
| `4` | Quiz |

> ⚠️ The `sectionId` in the body **must match** the `sectionId` in the URL path or the request returns `400`.

**Response `204`:** No content.

---

### PUT `/api/courses/{id}/sections/{sectionId}/lessons/{lessonId}`

Updates an existing lesson.

**Auth required:** Yes — `Instructor` or `Admin`

**Request body:**
```json
{
  "title": "string",
  "description": "string | null",
  "type": 0,
  "contentUrl": "string | null",
  "durationInSeconds": 600,
  "isFreePreview": false
}
```

**Response `204`:** No content.

---

### DELETE `/api/courses/{id}/sections/{sectionId}/lessons/{lessonId}`

Removes a lesson.

**Auth required:** Yes — `Instructor` or `Admin`

**Response `204`:** No content.

---

### POST `/api/courses/submissions`

Submits a lesson assignment (students only). The `fileUrl` should be obtained from the Upload service first.

**Auth required:** Yes — `Student`

**Request body:**
```json
{
  "lessonId": "guid",
  "fileUrl": "https://cdn.example.com/submissions/file.pdf"
}
```

**Response `200`:** `data: "Submission successful."`

---

## 4. Enrollment Service — `/api/enrollments`

All enrollment endpoints require authentication. Students can only manage their own enrollments.

### POST `/api/enrollments`

Enrolls the authenticated student in a course.

**Auth required:** Yes — `Student`

**Request body:**
```json
{
  "courseId": "guid"
}
```

**Response `201`:**
```json
{
  "success": true,
  "data": {
    "id": "guid",
    "studentId": "guid",
    "courseId": "guid",
    "enrollmentDate": "ISO8601",
    "status": "Active"
  },
  "message": "Student enrolled successfully."
}
```

---

### GET `/api/enrollments/my-enrollments`

Returns all enrollments belonging to the authenticated student.

**Auth required:** Yes — `Student`

**Response `200`:**
```json
{
  "success": true,
  "data": [
    {
      "id": "guid",
      "studentId": "guid",
      "courseId": "guid",
      "enrollmentDate": "ISO8601",
      "status": "Active"
    }
  ]
}
```

---

### GET `/api/enrollments`

Returns all enrollments across all students, grouped by student. Paginated.

**Auth required:** Yes — `Admin` (implied by service design)

**Query params:**

| Param | Type | Default |
|---|---|---|
| `pageNumber` | int | `1` |
| `pageSize` | int | `10` |

**Response `200`:**
```json
{
  "success": true,
  "data": {
    "data": [
      {
        "studentId": "guid",
        "courseIds": ["guid", "guid"]
      }
    ],
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 2,
    "totalRecords": 15
  }
}
```

---

### DELETE `/api/enrollments/course/{courseId}`

Unenrolls the authenticated student from a specific course.

**Auth required:** Yes — `Student`

**Response `200`:** `data: "Student unenrolled from the course successfully."`

---

## 5. Upload Service — `/api/upload`

### POST `/api/upload`

Uploads a file. Use this **before** setting `thumbnailUrl`, `contentUrl`, `fileUrl`, or `avatarUrl` in any other endpoint — get the URL here first, then use it there.

**Auth required:** Yes (any role)

**Content-Type:** `multipart/form-data`

**Form fields:**

| Field | Type | Notes |
|---|---|---|
| `file` | file | The binary file |
| `context` | query param (int) | `0` = CourseMaterial, `1` = AssignmentSubmission, `2` = UserProfilePicture |

**Example request:**
```
POST /api/upload?context=2
Content-Type: multipart/form-data

file: <binary>
```

**Response `200`:**
```json
{
  "success": true,
  "data": "https://cdn.example.com/uploads/uuid-filename.jpg",
  "message": "File uploaded successfully."
}
```

---

## Error Responses

| HTTP Status | When |
|---|---|
| `400` | Validation failure, malformed body, business rule violation |
| `401` | Missing or invalid JWT |
| `403` | Valid JWT but insufficient role |
| `404` | Resource not found |

Error shape (courses service):
```json
{
  "code": "Course.NotFound",
  "description": "The course with the given ID was not found."
}
```

---

## Roles & Permissions Summary

| Endpoint group | Student | Instructor | Admin |
|---|---|---|---|
| Auth (all) | ✅ | ✅ | ✅ |
| GET courses | ✅ | ✅ | ✅ |
| Create/edit/delete course | ❌ | ✅ (own only) | ✅ |
| Publish course | ❌ | ✅ (own only) | ✅ |
| Submit lesson | ✅ | ❌ | ❌ |
| Enroll / unenroll | ✅ | ❌ | ❌ |
| View own enrollments | ✅ | ❌ | ❌ |
| View all enrollments | ❌ | ❌ | ✅ |
| View own profile | ✅ | ✅ | ✅ |
| Update own profile | ✅ | ✅ | ✅ |
| View any user | ✅ | ✅ | ✅ |
| List all users | ❌ | ❌ | ✅ |
| Deactivate / delete user | ❌ | ❌ | ✅ |
| Upload files | ✅ | ✅ | ✅ |