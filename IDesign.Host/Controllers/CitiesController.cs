using IDesign.Access;
using IDesign.Access.Entities;
using IDesign.Manager;
using IDesign.Manager.Models;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace IDesign.Host.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CitiesController : ApiController
{
    private readonly ICityManager _manager;
    private readonly IMapper _mapper;
    public CitiesController(ICityManager manager, IMapper mapper)
    {
        _manager = manager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var cities = await _manager.GetAllAsync();
        return Ok(cities.Adapt<List<CityDto>>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var city = await _manager.GetByIdAsync(id);
        return city == null ? NotFound() : Ok(city.Adapt<CityDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Post(CityDto city)
    {
        var cityEntity = city.Adapt<City>();
        await _manager.AddAsync(cityEntity);
        return CreatedAtAction(nameof(Get), new { id = city.Id }, cityEntity.Adapt<CountryDto>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CityDto city)
    {
        if (id != city.Id) return BadRequest();
        await _manager.UpdateAsync(city.Adapt<City>());
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _manager.DeleteAsync(id);
        return NoContent();
    }
}
