
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

* HttpResponseMessage

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

* IHttpActionResult

The IHttpActionResult was introduced in Web API 2 (.NET 4.5). An action method in Web API 2 can return an implementation of IHttpActionResult class which is more or less similar to ActionResult class in ASP.NET MVC.

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

# Media Type Formatters

Configure JSON Serialization

JSON formatter can be configured in WebApiConfig class. The JsonMediaTypeFormatter class includes various properties and methods using which you can customize JSON serialization. For example, Web API writes JSON property names with PascalCase by default. To write JSON property names with camelCase, set the CamelCasePropertyNamesContractResolver on the serializer settings as shown below.

Example: Customize JSON Serialization in C#

```
public static class WebApiConfig
{
    public static void Register(HttpConfiguration config)
    {
        config.MapHttpAttributeRoutes();
            
        config.Routes.MapHttpRoute(
            name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }
        );

        // configure json formatter
        JsonMediaTypeFormatter jsonFormatter = config.Formatters.JsonFormatter;

        jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    }
}
```

# Web API Filters

Web API includes filters to add extra logic before or after action method executes. Filters can be used to provide cross-cutting features such as logging, exception handling, performance measurement, authentication and authorization.

1. Override Filter
2. Exception Filter
3. Authorization Filter
4. Authentication Filter
5. Action Filter
6. Simple Filter

Let's create simple LogAttribute class for logging purpose to demonstrate action filter.

```
public class LogAttribute : ActionFilterAttribute 
{
    public LogAttribute()
    {

    }

    public override void OnActionExecuting(HttpActionContext actionContext)
    {
        Trace.WriteLine(string.Format("Action Method {0} executing at {1}", actionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");
    }

    public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
    {
        Trace.WriteLine(string.Format("Action Method {0} executed at {1}", actionExecutedContext.ActionContext.ActionDescriptor.ActionName, DateTime.Now.ToShortDateString()), "Web API Logs");
    }
}
```
