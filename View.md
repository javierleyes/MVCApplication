# Grid

How to pagination ?

Pre-requisites:

Install PagedList.Mvc from nuget

## Controller

```
[HttpGet]
public ActionResult Index(int? page)
{
    var varillas = this.VarillaService.GetAllDTO();

    // resultados por pagina
    int pageSize = 3;

    // numero de pagina
    int pageNumber = (page ?? 1);

    return View(varillas.ToPagedList(pageNumber, pageSize));
}
```

## View 

```
@using PagedList;
@using PagedList.Mvc;

@model IPagedList<Cadres.Dto.VarillaDTO>

.....

Page @(Model.PageCount < Model.PageNumber ? 0 : Model.PageNumber) of @Model.PageCount

@Html.PagedListPager(Model, page => Url.Action("Index", new { page }))
```

# Resources

https://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/sorting-filtering-and-paging-with-the-entity-framework-in-an-asp-net-mvc-application

