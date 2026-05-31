using JobBoard.Application.Abstractions;
using JobBoard.Application.DTOs.Companies;
using JobBoard.Application.DTOs.Jobs;
using JobBoard.Application.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<JobDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<JobDto>>> GetAll(CancellationToken cancellationToken)
        {
            var jobs = await _jobService.GetAllAsync(cancellationToken);
            return Ok(jobs);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var job = await _jobService.GetByIdAsync(id, cancellationToken);
            if (job is null) return NotFound();

            return Ok(job);
        }

        [HttpPost]
        [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobDto>> Create([FromBody] CreateJobDto dto, CancellationToken cancellationToken)
        {
            var Created = await _jobService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = Created.Id }, Created);
        }

        [HttpPut]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateJobDto dto, CancellationToken cancellationToken)
        {
            var Updated = await _jobService.UpdateAsync(id, dto, cancellationToken);
            if(!Updated) return NotFound();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            var Deleted = await _jobService.DeleteAsync(id, cancellationToken);
            if(!Deleted) return NotFound();

            return NoContent();
        }
    }
}
