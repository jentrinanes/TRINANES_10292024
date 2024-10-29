**File Processing Service**

**Overview**
A RESTful service for processing CSV and JSON files with API Key-based authentication.

**Prerequisites**

 - .NET 8 SDK
 - Docker

**Setup**
 - **Build and Run with Docker:**
```bash
docker build -t fileprocessorapi .
docker run -d -p 5:000:80 -e ApiKey=YOUR_API_KEY fileprocessorapi
```
**Using the API**

**POST: /api/FileProcessor/upload**
 - Headers: `X-API-KEY: YOUR_API_KEY`
 - Body: `multipart/form-data` with `file` field
 
 **GET: /api/FileProcessor/filecount**
 - Headers: `X-API-KEY: YOUR_API_KEY`
 - Response: `filesProcessed: <count>` 

**Error Handling and Logging**
The service logs each file processing event and provides error responses for unsupported formats or missing API keys.
