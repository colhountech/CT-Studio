# ASP.NET Core 6.0 Web REST API Patterns (not minimal api)

## Dependency Injection (DI)

Using DI to inject resources into the controllers

## GET returns Task<ActionResult<IEnumerable<T>>>

Example: GET: api/TodoItems

* GET without parameter returns a list of objects.
* Mark method with `[HttpGet]`
* Produce a Status 404 if items are not found, by using the `NotFound()` method

## GET with ID param returns Task<ActionResult<T>>

Example:  GET: api/TodoItems/5

* Mark method with `[HttpGet("{id}")]` to constrain route to match `id`
* Produce a Status 404 if item is not found, by using the `NotFound()` method

## PUT with ID returns No Content as Task<IActionResult>

Example: PUT: api/TodoItems/5

* Mark Method with `[HttpPut("{id}")]` to constrain route to match `id`
* Produce a Status 400 if Attribute doesn't match parameter, by using the `BadRequest()` method
* Produce a Status 404 if item is not found, by using the `NotFound()` method
* Produce a Status 204 if successful, and nothing is returned, by using `NoContent()` method

## POST with ID return ActionName as Task<ActionResult<T>>

Example: POST: api/TodoItems

* Mark Method with `[[HttpPost]` the HTTP body will be parsed and mapped to parameter
* Produce a Status 400 Bad Request, on empty or error, by using the `Problem('why message')` method
* Produce a Status 201, redirecting to where it's created. e.g. location: http://localhost:23015/api/T/1 
* Example: `CreatedAtAction(nameof (GetT), new { id = T.Id }, t);`
* Use `nameof(<<GetT>>) instead of hardcoded string of GETT Method

## DELETE with ID returns No Content as Task<IActionResult>

Example: DELETE: api/TodoItems/5

* Mark Method with `[HttpDelete("{id}")]` to constrain route to match `id`
* Produce a Status 404 if item is not found, by using the `NotFound()` method
* Produce a Status 204 if successful, and nothing is returned, by using `NoContent()` method
