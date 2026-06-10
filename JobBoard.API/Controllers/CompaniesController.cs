using JobBoard.Application.Abstractions;
using JobBoard.Application.Common.Pagination;
using JobBoard.Application.Common.Queries;
using JobBoard.Application.DTOs.Companies;
using JobBoard.Application.DTOs.Jobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<CompanyDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<CompanyDto>>> GetPaged([FromQuery]CompanyQueryParameters parameters, CancellationToken cancellationToken)
        {
            var companies = await _companyService.GetPagedAsync(parameters, cancellationToken);

            return Ok(companies);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompanyDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetByIdAsync(id, cancellationToken);

            return Ok(company);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CompanyDto>> Create([FromBody] CreateCompanyDto dto, CancellationToken cancellationToken)
        {
            var createdCompany = await _companyService.CreateAsync(dto, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = createdCompany.Id }, createdCompany);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyDto dto, CancellationToken cancellationToken)
        {
            await _companyService.UpdateAsync(id, dto, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            await _companyService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
