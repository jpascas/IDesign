using IDesign.Access;
using IDesign.Access.Entities;
using IDesign.Manager;
using IDesign.Manager.Models;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IDesign.Host.Controllers;

[Authorize]
[Route("api/[controller]")]
public class CountriesController : ApiController
{
    private readonly ICountryManager _manager;
    private readonly IMapper _mapper;
    public CountriesController(ICountryManager manager, IMapper mapper)
    {
        _manager = manager;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var countries = await _manager.GetAllAsync();
        return Ok(countries.Adapt<List<CountryDto>>());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var country = await _manager.GetByIdAsync(id);
        return country == null ? NotFound() : Ok(country.Adapt<CountryDto>());
    }

    [HttpPost]
    public async Task<IActionResult> Post(CountryDto country)
    {
        var countryEntity = country.Adapt<Country>();
        await _manager.AddAsync(countryEntity);
        return CreatedAtAction(nameof(Get), new { id = countryEntity.Id }, countryEntity.Adapt<CountryDto>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, CountryDto country)
    {
        if (id != country.Id) return BadRequest();
        await _manager.UpdateAsync(country.Adapt<Country>());
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _manager.DeleteAsync(id);
        return NoContent();
    }
}
