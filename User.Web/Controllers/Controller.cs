using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseController<T> : ControllerBase, IActionFilter
{
    protected readonly ILogger _logger;
    protected readonly IUrlHelper _urlHelper;

    public BaseController(ILogger<BaseController<T>> logger)
    {
        _logger = logger;
    }
    public BaseController(ILogger<BaseController<T>> logger, IUrlHelper urlHelper)
    {
        _logger = logger;
        _urlHelper = urlHelper;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.Log(LogLevel.Trace, "OnActionExecuted");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.Log(LogLevel.Trace, "OnActionExecuting");
    }


    protected void ApplyPagination(string sort, int page, int pageSize, int totalCount)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        var firstPageLink = _urlHelper.Link("UserList",
           new
           {
               page = 1,
               pageSize = pageSize,
               sort = sort
           });

        var prevLink = page > 1 ? _urlHelper.Link("UserList",
            new
            {
                page = page - 1,
                pageSize = pageSize,
                sort = sort
            }) : "";
        var nextLink = page < totalPages ? _urlHelper.Link("UserList",
            new
            {
                page = page + 1,
                pageSize = pageSize,
                sort = sort
            }) : "";

        var lastPageLink = _urlHelper.Link("UserList",
           new
           {
               page = totalPages,
               pageSize = pageSize,
               sort = sort
           });

        var metadata = new
        {
            currentPage = page,
            pageSize,
            totalCount,
            totalPages,
            hasPrevious = page > 1,
            hasNext = page < totalPages,
            previousPageLink = prevLink,
            nextPageLink = nextLink,
            firstPageLink,
            lastPageLink
        };

        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
    }
}