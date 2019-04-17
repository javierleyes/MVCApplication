
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

# Consume Web API

Web API can be accessed in the server side code in .NET and also on client side using JavaScript frameworks such as jQuery, AnguarJS, KnockoutJS etc.

## Consume Web API in AngularJS

Web API can be accessed directly from the UI at client side using AJAX capabilities of any JavaScript framework such as AngularJS, KnockoutJS, Ext JS etc.

## Consume Web API in ASP.NET MVC

To consume Web API in ASP.NET MVC server side we can use HttpClient in the MVC controller. HttpClient sends a request to the Web API and receives a response. We then need to convert response data that came from Web API to a model and then render it into a view.

* Consume Web API Get method

```
// API
public IHttpActionResult GetAllStudents(bool includeAddress = false)
{
	IList<StudentViewModel> students = null;

	...
	...
	...

	if (students.Count == 0)
	{
		return NotFound();
	}

	return Ok(students);
}
```
```
[HttpGet]
public ActionResult Index()
{
    IEnumerable<StudentViewModel> students = null;

    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:64189/api/");
        //HTTP GET
        var responseTask = client.GetAsync("student");
        responseTask.Wait();

        var result = responseTask.Result;
        if (result.IsSuccessStatusCode)
        {
            var readTask = result.Content.ReadAsAsync<IList<StudentViewModel>>();
            readTask.Wait();

            students = readTask.Result;
        }
        else //web api sent error response 
        {
            //log response status here..

            students = Enumerable.Empty<StudentViewModel>();

            ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
        }
    }
    return View(students);
}
```

* Consume Web API Post method

```
// API
public IHttpActionResult PostNewStudent(StudentViewModel student)
{
	if (!ModelState.IsValid)
		return BadRequest("Not a valid model");

	...
	
	return Ok();
}
```
```
[HttpPost]
public ActionResult create(StudentViewModel student)
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:64189/api/student");

        //HTTP POST
        var postTask = client.PostAsJsonAsync<StudentViewModel>("student", student);
        postTask.Wait();

        var result = postTask.Result;
        if (result.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
    }

    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

    return View(student);
}
```

* Consume Web API Put method

```
//API
public IHttpActionResult Put(StudentViewModel student)
{
	if (!ModelState.IsValid)
		return BadRequest("Not a valid data");

	using (var ctx = new SchoolDBEntities())
	{
		....

		if (existingStudent != null)
		{
			...
		}
		else
		{
			return NotFound();
		}
	}
	
	return Ok();
}
```
```
[HttpGet]
public ActionResult Edit(int id)
{
	StudentViewModel student = null;

	using (var client = new HttpClient())
	{
		client.BaseAddress = new Uri("http://localhost:64189/api/");
		//HTTP GET
		var responseTask = client.GetAsync("student?id=" + id.ToString());
		responseTask.Wait();

		var result = responseTask.Result;
		if (result.IsSuccessStatusCode)
		{
			var readTask = result.Content.ReadAsAsync<StudentViewModel>();
			readTask.Wait();

			student = readTask.Result;
		}
	}

	return View(student);
}

[HttpPost]
public ActionResult Edit(StudentViewModel student)
{
	using (var client = new HttpClient())
	{
		client.BaseAddress = new Uri("http://localhost:64189/api/student");

		//HTTP POST
		var putTask = client.PutAsJsonAsync<StudentViewModel>("student", student);
		putTask.Wait();

		var result = putTask.Result;
		if (result.IsSuccessStatusCode)
		{

			return RedirectToAction("Index");
		}
	}

	return View(student);
}
```

* Consume Web API Delete method

```
//API
public IHttpActionResult Delete(int id)
{
    if (id <= 0)
        return BadRequest("Not a valid student id");

    using (var ctx = new SchoolDBEntities())
    {
        var student = ctx.Students
            .Where(s => s.StudentID == id)
            .FirstOrDefault();

        ctx.Entry(student).State = System.Data.Entity.EntityState.Deleted;
        ctx.SaveChanges();
    }

    return Ok();
}
```
```
public ActionResult Delete(int id)
{
    using (var client = new HttpClient())
    {
        client.BaseAddress = new Uri("http://localhost:64189/api/");

        //HTTP DELETE
        var deleteTask = client.DeleteAsync("student/" + id.ToString());
        deleteTask.Wait();

        var result = deleteTask.Result;
        if (result.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
    }

    return RedirectToAction("Index");
}
```

## The following table lists all the methods of HttpClient to send different HTTP requests.

* GetAsync	

Sends a GET request to the specified Uri as an asynchronous operation.

* GetByteArrayAsync	

Sends a GET request to the specified Uri and returns the response body as a byte array in an asynchronous operation.

* GetStreamAsync	

Sends a GET request to the specified Uri and returns the response body as a stream in an asynchronous operation.

* GetStringAsync	

Sends a GET request to the specified Uri and returns the response body as a string in an asynchronous operation.

* PostAsync	

Sends a POST request to the specified Uri as an asynchronous operation.

* PostAsJsonAsync	

Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as JSON.

* PostAsXmlAsync	

Sends a POST request as an asynchronous operation to the specified Uri with the given value serialized as XML.

* PutAsync	

Sends a PUT request to the specified Uri as an asynchronous operation.

* PutAsJsonAsync	

Sends a PUT request as an asynchronous operation to the specified Uri with the given value serialized as JSON.

* PutAsXmlAsync	

Sends a PUT request as an asynchronous operation to the specified Uri with the given value serialized as XML.

* DeleteAsync	

Sends a DELETE request to the specified Uri as an asynchronous operation.





