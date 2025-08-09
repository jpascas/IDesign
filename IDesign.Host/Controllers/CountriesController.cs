using IDesign.Manager;
using IDesign.Access;
using Microsoft.AspNetCore.Mvc;

namespace IDesign.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountriesController : ControllerBase
{
    private readonly ICountryManager _manager;
    public CountriesController(ICountryManager manager) => _manager = manager;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _manager.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var country = await _manager.GetByIdAsync(id);
        return country == null ? NotFound() : Ok(country);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Country country)
    {
        await _manager.AddAsync(country);
        return CreatedAtAction(nameof(Get), new { id = country.Id }, country);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Country country)
    {
        if (id != country.Id) return BadRequest();
        await _manager.UpdateAsync(country);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _manager.DeleteAsync(id);
        return NoContent();
    }
}
