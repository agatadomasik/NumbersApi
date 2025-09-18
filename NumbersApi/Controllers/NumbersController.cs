using Microsoft.AspNetCore.Mvc;
using NumbersApi.Models;
using NumbersApi.Services;

namespace NumbersApi.Controllers;

[ApiController]
[Route("[controller]")]
public class NumbersController : ControllerBase
{
    private readonly INumbersService _numbersService;

    public NumbersController(INumbersService numbersService)
    {
        _numbersService = numbersService;
    }

    [HttpPost]
    public IActionResult AddNumbers([FromBody] AddNumbersRequest request)
    {
        if (request?.Numbers == null || !request.Numbers.Any())
        {
            return BadRequest("Numbers array cannot be empty.");
        }

        _numbersService.AddNumbers(request.Numbers);

        return Ok("Numbers added successfully.");
    }

    [HttpGet]
    public IActionResult GetNumbers()
    {
        var numbers = _numbersService.GetAllNumbers();
        return Ok(numbers);
    }

    [HttpGet("sorted")]
    public IActionResult GetSortedNumbers([FromQuery] string sort = "asc")
    {
        if (sort != "asc" && sort != "desc")
        {
            return BadRequest("Sort parameter must be 'asc' or 'desc'.");
        }

        var sortedNumbers = _numbersService.GetSortedNumbers(sort);
        return Ok(sortedNumbers);
    }

    [HttpGet("search")]
    public IActionResult SearchNumber([FromQuery] int value)
    {
        var response = _numbersService.SearchNumber(value);
        return Ok(response);
    }

    [HttpGet("stats")]
    public IActionResult GetStatistics()
    {
        var stats = _numbersService.GetStatistics();
        return Ok(stats);
    }

    [HttpPost("process/parallel")]
    public async Task<IActionResult> ProcessNumbersParallel()
    {
        var result = await _numbersService.ProcessNumbersParallelAsync();
        return Ok(result);
    }
}