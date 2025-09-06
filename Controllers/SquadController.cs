using Microsoft.AspNetCore.Mvc;

namespace SquadService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SquadController : ControllerBase
{
    private static readonly List<Squad> _squads = new()
    {
        new Squad(1, "Alpha Squad", "Elite combat unit"),
        new Squad(2, "Beta Squad", "Support and logistics"),
        new Squad(3, "Gamma Squad", "Reconnaissance team")
    };

    /// <summary>
    /// Gets all squads
    /// </summary>
    /// <returns>List of squads</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Squad>> GetSquads()
    {
        return Ok(_squads);
    }

    /// <summary>
    /// Gets a specific squad by ID
    /// </summary>
    /// <param name="id">The squad ID</param>
    /// <returns>The squad</returns>
    [HttpGet("{id}")]
    public ActionResult<Squad> GetSquad(int id)
    {
        var squad = _squads.FirstOrDefault(s => s.Id == id);
        if (squad == null)
        {
            return NotFound($"Squad with ID {id} not found");
        }
        return Ok(squad);
    }

    /// <summary>
    /// Creates a new squad
    /// </summary>
    /// <param name="squad">The squad to create</param>
    /// <returns>The created squad</returns>
    [HttpPost]
    public ActionResult<Squad> CreateSquad([FromBody] Squad squad)
    {
        if (squad == null)
        {
            return BadRequest("Squad data is required");
        }

        // In a real app, you'd generate a new ID and save to database
        var newSquad = squad with { Id = _squads.Max(s => s.Id) + 1 };
        _squads.Add(newSquad);

        return CreatedAtAction(nameof(GetSquad), new { id = newSquad.Id }, newSquad);
    }

    /// <summary>
    /// Updates an existing squad
    /// </summary>
    /// <param name="id">The squad ID</param>
    /// <param name="squad">The updated squad data</param>
    /// <returns>The updated squad</returns>
    [HttpPut("{id}")]
    public ActionResult<Squad> UpdateSquad(int id, [FromBody] Squad squad)
    {
        if (squad == null)
        {
            return BadRequest("Squad data is required");
        }

        var existingSquad = _squads.FirstOrDefault(s => s.Id == id);
        if (existingSquad == null)
        {
            return NotFound($"Squad with ID {id} not found");
        }

        // In a real app, you'd update in database
        var index = _squads.FindIndex(s => s.Id == id);
        _squads[index] = squad with { Id = id };

        return Ok(_squads[index]);
    }

    /// <summary>
    /// Deletes a squad
    /// </summary>
    /// <param name="id">The squad ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    public IActionResult DeleteSquad(int id)
    {
        var squad = _squads.FirstOrDefault(s => s.Id == id);
        if (squad == null)
        {
            return NotFound($"Squad with ID {id} not found");
        }

        _squads.Remove(squad);
        return NoContent();
    }
}

public record Squad(int Id, string Name, string Description);
