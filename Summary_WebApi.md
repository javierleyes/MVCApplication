
# Web APi

# Attribute Routing

The Route attribute can be applied on any controller or action method.

Example: Attribute Routing
```
public class StudentController : ApiController
{
    [Route("api/student/names")]
    public IEnumerable<string> Get()
    {
        return new string[] { "student1", "student2" };
    }
}
```

# [FromUri] and [FromBody]
You have seen that by default Web API gets the value of a primitive parameter from the query string and complex type parameter from the request body.

# Action Method Return Type

The Web API action method can have following return types.

1. Void
2. Primitive type or Complex type
3. HttpResponseMessage
4. IHttpActionResult



The advantage of sending HttpResponseMessage from an action method is that you can configure a response your way. You can set the status code, content or error message (if any) as per your requirement.

Example: Return HttpResponseMessage

```
public HttpResponseMessage Get(int id)
{
    Student stud = GetStudentFromDB(id); 

    if (stud == null) {
        return Request.CreateResponse(HttpStatusCode.NotFound, id);
    }

    return Request.CreateResponse(HttpStatusCode.OK, stud);
}  
```

Example: Return IHttpActionResult Type using Ok() and NotFound() Methods

```
public IHttpActionResult Get(int id)
{
    Student stud = GetStudentFromDB(id);
            
    if (stud == null)
    {
        return NotFound();
    }

    return Ok(stud);
}
```

* Create Custom Result Type

You can create your own custom class as a result type that implements IHttpActionResult interface.

```
public class TextResult : IHttpActionResult
{
    string _value;
    HttpRequestMessage _request;

    public TextResult(string value, HttpRequestMessage request)
    {
        _value = value;
        _request = request;
    }

    public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage()
        {
            Content = new StringContent(_value),
            RequestMessage = _request
        };
        return Task.FromResult(response);
    }
}

public IHttpActionResult GetName(int id)
{
    string name = GetStudentName(id);
            
    if (String.IsNullOrEmpty(name))
    {
        return NotFound();
    }
            
    return new TextResult(name, Request);
}
```

