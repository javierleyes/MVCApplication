
Preguntas MVC

* Relación entre vista y controller.

* Que contiene la carpeta App_Data y Model con una arquitectura en capas ?

* What's the difference between controller and action method ?

* Is there a route defined and other Action Method ?

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

## Add error in ModelState
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

