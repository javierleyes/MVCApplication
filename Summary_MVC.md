
# Controller

## Action method

All the public methods in the Controlle class are called Action methods.
Action method cannot be overloaded and cannot be a static method.

## ActionResult

There result classes represent different types of responses such as html, file, string, json, javascript etc.

## Action Selectors

* ActionName
[ActionName("find")]

ActionName attribute allows us to specify a different action name than the method name. 
El nombre viejo no se puede usar.

* NonAction 
[NonAction]

Selector attribute indicates that a public method of a Controller is not an action method. 

* ActionVerbs

The ActionVerbs selector is used when you want to control the selection of an action method based on a Http request method.
HttpGet, HttpPost, HttpPut, HttpDelete, HttpOptions & HttpPatch.
If you do not apply any attribute then it considers it a GET request by default.

## Http method	Usage

GET:	To retrieve the information from the server. Parameters will be appended in the query string.

POST:	To create a new resource.
PUT:	To update an existing resource.

HEAD:	Identical to GET except that server do not return message body.

OPTIONS:	OPTIONS method represents a request for information about the communication options supported by web server.

DELETE:	To delete an existing resource.

PATCH:	To full or partial update the resource.


#### You can also apply multiple http verbs using AcceptVerbs attribute.  [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]

#### Multiple action methods can have same name with different action verbs. Method overloading rules are applicable.

## Model Binding

```
[HttpPost]
public ActionResult Edit([Bind(Include = "StudentId, StudentName")] Student std)
{
    var name = std.StudentName;
           
    //write code to update student 
            
    return RedirectToAction("Index");
}

[HttpPost]
public ActionResult Edit([Bind(Exclude = "Age")] Student std)
{
    var name = std.StudentName;
           
    //write code to update student 
            
    return RedirectToAction("Index");
}
```

#### The Bind attribute will improve the performance by only bind properties which you needed.

## Razor Syntax

* @model allows you to use model object anywhere in the view.

* Inline expression
    
    Start with @ symbol to write server side C# or VB code with Html code.

* Multi-statement Code block

    You can write multiple line of server side code enclosed in braces @{ ... }

if-else condition
```
@if(DateTime.IsLeapYear(DateTime.Now.Year) )
{
    @DateTime.Now.Year @:is a leap year.
}
else 
{ 
    @DateTime.Now.Year @:is not a leap year.
}
```

for loop
```
@for (int i = 0; i < 5; i++) { 
    @i.ToString() <br />
}
```

Html.TextBox() in Razor View
```
@model Student

@Html.TextBox("StudentName", null, new { @class = "form-control" })  
```

TextBoxFor() in Razor View
```
@model Student

@Html.TextBoxFor(m => m.StudentName, new { @class = "form-control" }) 
```

TextAreaFor() in Razor View
```
@model Student

@Html.TextAreaFor(m => m.Description, new { @class = "form-control" })  
```


Html.CheckBoxFor() in Razor View
```
@model Student

@Html.CheckBoxFor(m => m.isNewlyEnrolled)
```

Html.RadioButtonFor() in Razor View
```
@model Student

@Html.RadioButtonFor(m => m.Gender,"Male")
@Html.RadioButtonFor(m => m.Gender,"Female")
```

Html.DropDownListFor() in Razor View
```
@using MyMVCApp.Models

@model Student

@Html.DropDownListFor(m => m.StudentGender, new SelectList(Enum.GetValues(typeof(Gender))), "Select Gender")
```

HiddenFor() in Razor View
```
@model Student

@Html.HiddenFor(m => m.StudentId)
```

PasswordFor() in Razor View`
```
@model Student

@Html.PasswordFor(m => m.Password)
```

DisplayFor() in Razor View
```
@model Student

@Html.DisplayFor(m => m.StudentName)
```

LabelFor() in Razor View
```
@model Student

@Html.LabelFor(m => m.StudentName)
```

EditorFor() in Razor view
```
@Html.EditorFor(m => m.StudentId)
```

## Example: Custom error message
```
Property:
[Required(ErrorMessage="Please enter student name.")]

@model Student  
    
@Html.Editor("StudentName") <br />
@Html.ValidationMessage("StudentName", "Please enter student name.", new { @class = "text-danger" })
```

## Validation 

* Add error in ModelState
```
if (ModelState.IsValid) { 
              
    //check whether name is already exists in the database or not
    bool nameAlreadyExists = * check database *       
        
    if(nameAlreadyExists)
    {
        ModelState.AddModelError(string.Empty, "Student Name already exists.");
    
        return View(std);
    }
}
```

## Layout View

* The layout view is same as the master page of the ASP.NET webform application.

* _ViewStart.cshtml is included in the Views folder by default. It sets up the default layout page for all the views in the folder and its subfolders using the Layout property. You can assign a valid path of any Layout page to the Layout property.

* _ViewStart.cshtml can also be included in sub folder of View folder to set the default layout page for all the views included in that particular subfolder only.

* Create: MVC 5 Layout Page

* Setting Layout property in individual view

```
@{
    ViewBag.Title = "Home Page";
    Layout = "~/Views/Shared/_myLayoutPage.cshtml";
}
```

* Specify Layout Page in ActionResult Method
```
public ActionResult Index()
{
    return View("Index", "_myLayoutPage");
}
```

## Partial View

* To create a partial view, right click on Shared folder -> select Add -> click on View.

    Check -> Create as a partial view 

* Render Partial View

    You can render the partial view in the parent view using html helper methods: Partial() or RenderPartial() or RenderAction().

Html.RenderPartial()

Helper method renders the specified partial view. It accept partial view name as a string parameter and returns MvcHtmlString. It returns html string so you have a chance of modifing the html before rendering.

```  
@{
    Html.RenderPartial("_HeaderNavBar");   
}

```  

Html.Partial()

The RenderPartial helper method is same as the Partial method except that it returns void and writes resulted html of a specified partial view into a http response stream directly.


```
@Html.Partial("_HeaderNavBar")
```    

## ViewBag

Example:

```  
public ActionResult Index()
{
    ViewBag.TotalStudents = studentList.Count();

    return View();
}

//Example: Acess ViewBag in a View

<label>Total Students:</label>  @ViewBag.TotalStudents
```  

Internally, ViewBag is a wrapper around ViewData. It will throw a runtime exception, if the ViewBag property name matches with the key of ViewData.

ViewBag transfers data from the controller to the view, ideally temporary data which in not included in a model.

You can assign any number of propertes and values to ViewBag

The ViewBag's life only lasts during the current http request. 

ViewBag values will be null if redirection occurs.

ViewBag is actually a wrapper around ViewData.

## ViewData

ViewData is similar to ViewBag.

ViewData is a dictionary which can contain key-value pairs where each key must be string.

Example: 
```
public ActionResult Index()
{
    IList<Student> studentList = new List<Student>();
    studentList.Add(new Student(){ StudentName = "Bill" });
    studentList.Add(new Student(){ StudentName = "Steve" });
    studentList.Add(new Student(){ StudentName = "Ram" });

    ViewData["students"] = studentList;
  
    return View();
}

//Example: Access ViewData in a Razor View

<ul>
@foreach (var std in ViewData["students"] as IList<Student>)
{
    <li>
        @std.StudentName
    </li>
}
</ul>

```

## Important !

ViewData and ViewBag both use the same dictionary internally. So you cannot have ViewData Key matches with the property name of ViewBag, otherwise it will throw a runtime exception.

## TempData

It is useful when you want to transfer non-sensitive data from one action method to another action method of the same or a different controller as well as redirects. 

Example

```
public ActionResult Index()
{
    TempData["name"] = "Test data";
    TempData["age"] = 30;

    return View();
}

public ActionResult About()
{
    string userName;
    int userAge;
        
    if(TempData.ContainsKey("name"))
        userName = TempData["name"].ToString();
    
    if(TempData.ContainsKey("age"))
        userAge = int.Parse(TempData["age"].ToString());
    
    return View();
}
```

Call TempData.Keep() to retain TempData values in a third consecutive request.

```
public ActionResult Index()
{
    TempData["myData"] = "Test data";
    return View();
}

public ActionResult About()
{
    string data;
        
    if(TempData["myData"] != null)
        data = TempData["myData"] as string;
        
    TempData.Keep();
        
    return View();
}

public ActionResult Contact()
{
    string data;
        
    if(TempData["myData"] != null)
        data = TempData["myData"] as string;
            
    return View();
}
```

TempData can be used to store data between two consecutive requests. 

TempData values will be retained during redirection.

TempData value must be type cast before use. Check for null values to avoid runtime error.

TempData can be used to store only one time messages like error messages, validation messages.

Call TempData.Keep() to keep all the values of TempData in a third request.

## Filters

Filter is a custom class where you can write custom logic to execute before or after an action method executes. Filters can be applied to an action method or controller in a declarative or programmatic way. Declarative means by applying a filter attribute to an action method or controller class and programmatic means by implementing a corresponding interface.

Types:

* Exception filters ([HandleError])
* Result filters ([OutputCache])
* Action filters
* Authorization filters ([Authorize], [RequireHttps])

Please make sure that CustomError mode is on in System.web section of web.config, in order for HandleErrorAttribute work properly.


Example: SetCustomError in web.config
```
<customErrors mode="On" /> 
```
Filters can be applied at three levels.

1. Global Level

You can apply filters at global level in the Application_Start event of Global.asax.cs file by using default FilterConfig.RegisterGlobalFilters() mehtod. Global filters will be applied to all the controller and action methods of an application.

```
// MvcApplication class contains in Global.asax.cs file 
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
    }
}

// FilterConfig.cs located in App_Start folder 
public class FilterConfig
{
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
        filters.Add(new HandleErrorAttribute());
    }
}
```

2. Controller level

```
[HandleError]
public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

}
```

3. Action method level

```
public class HomeController : Controller
{
    [HandleError]
    public ActionResult Index()
    {
        return View();
    }

}
```

The same way, you can apply multiple built-in or custom filters globally or at controller or action method level for different purpose such as [Authorize],[RequireHttps], [ChildActionOnly],[OutputCache],[HandleError].

So, filters run in the following order.

1. Authorization filters

2. Action filters

3. Response filters

4. Exception filters

Create Custom Filter

```
class MyErrorHandler : FilterAttribute, IExceptionFilter
{
    public override void IExceptionFilter.OnException(ExceptionContext filterContext)
    {
        Log(filterContext.Exception);

        base.OnException(filterContext);
    }

    private void Log(Exception exception)
    {
        //log exception here..
 
    }
}
```

```
Custom HandleErrorAttribute 
class MyErrorHandler : HandleErrorAttribute
{
    public override void OnException(ExceptionContext filterContext)
    {
        Log(filterContext.Exception);

        base.OnException(filterContext);
    }

    private void Log(Exception exception)
    {
        //log exception here..
 
    }
}   
```

# Bundling and minification

Bundling allow us to load the bunch of static files from the server into one http request.
Bundling technique in MVC 4 allows us to load more than one JavaScript file.

Minification technique optimizes script or css file size by removing unnecessary white space and comments and shortening variable names to one character.

Bundle Types

MVC 5 includes following bundle classes in System.web.Optimization namespace:

ScriptBundle: ScriptBundle is responsible for JavaScript minification of single or multiple script files and bundling.

StyleBundle: StyleBundle is responsible for CSS minification of single or multiple style sheet files.

DynamicFolderBundle: Represents a Bundle object that ASP.NET creates from a folder that contains files of the same type.

## ScriptBundle 

Example: BundleConfig.RegisterBundle()

```
using System.Web;
using System.Web.Optimization;

public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {   
        // create an object of ScriptBundle and 
        // specify bundle name (as virtual path) as constructor parameter 
        ScriptBundle scriptBndl = new ScriptBundle("~/bundles/bootstrap");

        
        //use Include() method to add all the script files with their paths 
        scriptBndl.Include(
                            "~/Scripts/bootstrap.js",
                            "~/Scripts/respond.js"
                          );

        
        //Add the bundle into BundleCollection
        bundles.Add(scriptBndl);

        BundleTable.EnableOptimizations = true;
    }
}
```

1. First of all create an instance of ScriptBundle class by specifing the bundle name as a constructor parameter. This bundle name is a virtual path starting with ~/. You can give anything in virtual path but it's recommended to give a path that will be easy to identify as a bundle. Here, we have given "~/bundles/bootstrap" path, so that we can easily identify that this bundle includes bootstrap related files.

2. Use Include method to add one or more JS files into a bundle with its relative path after root path using ~ sign.

3. Final, add the bundle into BundleCollection instance, which is specified as a parameter in RegisterBundle() method.

4. Last, BundleTable.EnableOptimizations = true enables bundling and minification in debug mode. If you set it to false then it will not do bundling and minification.


You can also use IncludeDirectory method of bundle class to add all the files under particular directory as shown below.

```
public static void RegisterBundles(BundleCollection bundles)
{            
    bundles.Add(new ScriptBundle("~/bundles/scripts").IncludeDirectory("~/Scripts/","*.js",true));
}
```

* Using Wildcards

Sometime third party script files includes versions in a name of script file. So it is not advisable to changes the code whenever you upgrade the version of script file. With the use of wildcards, you don't have to specify a version of a script file. It automatically includes files with the version available.

Example: Wildcard with bundle
```
public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {            
        bundles.Add(new ScriptBundle("~/bundles/jquery")
               .Include( "~/Scripts/jquery-{version}.js"));
    }
}
```

* Using CDN

You can also use Content Delivery Network to load script files. For example, you can load jquery library from CDN as shown below.

Example: Load files from CDN
```
public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {            
        var cdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js";

        bundles.Add(new ScriptBundle("~/bundles/jquery", cdnPath)
               .Include( "~/Scripts/jquery-{version}.js"));
    }
}
```

* Include ScriptBundle in Razor View 

@Scripts.Render("~/bundles/bootstrap")

```
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title</title>
    @Scripts.Render("~/bundles/bootstrap")
</head>
<body>
    @*html code removed for clarity *@
</body>
</html>
```

```
Please make sure that you have set debug = false in web.config <compilation debug="false" targetFramework="4.5"/>
```

## StyleBundle

Example: StyleBundle
```
public class BundleConfig
{
    public static void RegisterBundles(BundleCollection bundles)
    {            
        bundles.Add(new StyleBundle("~/bundles/css").Include(
                                                    "~/Content/bootstrap.css",
                                                    "~/Content/site.css"
                                                ));
        // add ScriptBundle here..  
        
    }
}
```

* Include Style Bundle in Razor View

@Styles.Render("~/bundles/css")


```
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - My ASP.NET Application</title>
    @Styles.Render("~/bundles/css")
</head>
<body>
    @*html code removed for clarity *@
</body>
</html>
```

# Area

Area allows us to partition large application into smaller units where each unit contains separate MVC folder structure, same as default MVC folder structure. For example, large enterprise application may have different modules like admin, finance, HR, marketing etc. 

```
public class adminAreaRegistration : AreaRegistration 
{
    public override string AreaName 
    {
        get 
        {
            return "admin";
        }
    }

    public override void RegisterArea(AreaRegistrationContext context) 
    {
        context.MapRoute(
            "admin_default",
            "admin/{controller}/{action}/{id}",
            new { action = "Index", id = UrlParameter.Optional }
        );
    }
}
```

Usar readonly en vez de disabled

# Resources

https://www.tutorialsteacher.com/mvc/asp.net-mvc-tutorials

https://www.tutorialspoint.com/asp.net_mvc/

https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/recommended-resources-for-mvc

http://techfunda.com/howto/asp-net-mvc


