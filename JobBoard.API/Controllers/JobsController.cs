using JobBoard.Application.Abstractions;
using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
using JobBoard.Application.DTOs.Jobs;
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
        [ProducesResponseType(typeof(PagedResult<JobDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<JobDto>>> GetPaged([FromQuery] JobQueryParameters parameters,CancellationToken cancellationToken)
        {
            var jobs = await _jobService.GetPagedAsync(parameters,cancellationToken);

            return Ok(jobs);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JobDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JobDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var job = await _jobService.GetByIdAsync(id, cancellationToken);

            return Ok(job);
        }

        [HttpPost]
        [ProducesResponseType(typeof(JobDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JobDto>> Create([FromBody] CreateJobDto dto, CancellationToken cancellationToken)
        {
            var created = await _jobService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateJobDto dto, CancellationToken cancellationToken)
        {
            await _jobService.UpdateAsync(id, dto, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            await _jobService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
