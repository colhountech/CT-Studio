# Web API and CORS

PUT https://localhost:7011/SyncOrder net::ERR_FAILED

Access to fetch at 'https://localhost:7011/SyncOrder' from origin 'https://localhost:61549' has been blocked by 
CORS policy: Response to preflight request doesn't pass access control check: No 'Access-Control-Allow-Origin' 
header is present on the requested resource. If an opaque response serves your needs, set the request's mode to
'no-cors' to fetch the resource with CORS disabled.

Web API can be tested with swaggerUI but when calling from another origin, we need to allow access.

Note: Never turn off CORS. It is a shure set way to get hacked or DDOSed



