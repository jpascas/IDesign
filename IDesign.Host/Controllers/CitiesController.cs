using IDesign.Manager;
using IDesign.Access;
using Microsoft.AspNetCore.Mvc;

namespace IDesign.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CitiesController : ControllerBase
{
    private readonly ICityManager _manager;
    public CitiesController(ICityManager manager) => _manager = manager;

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _manager.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var city = await _manager.GetByIdAsync(id);
        return city == null ? NotFound() : Ok(city);
    }

    [HttpPost]
    public async Task<IActionResult> Post(City city)
    {
        await _manager.AddAsync(city);
        return CreatedAtAction(nameof(Get), new { id = city.Id }, city);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, City city)
    {
        if (id != city.Id) return BadRequest();
        await _manager.UpdateAsync(city);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _manager.DeleteAsync(id);
        return NoContent();
    }
}
