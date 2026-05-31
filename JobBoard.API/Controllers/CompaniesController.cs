using JobBoard.Application.Abstractions;
using JobBoard.Application.DTOs.Companies;
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
        [ProducesResponseType(typeof(IReadOnlyList<CompanyDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IReadOnlyList<CompanyDto>>> GetAll(CancellationToken cancellationToken)
        {
            var companies = await _companyService.GetAllAsync(cancellationToken);
            return Ok(companies);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ProducesResponseType(typeof(CompanyDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CompanyDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var company = await _companyService.GetByIdAsync(id, cancellationToken);

            if (company is null)
                return NotFound();

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

        [HttpPut]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCompanyDto dto, CancellationToken cancellationToken)
        {
            var updated = await _companyService.UpdateAsync(id, dto, cancellationToken);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var deleted = await _companyService.DeleteAsync(id, cancellationToken);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
