using Jasmine.Abs.Api.Dto;
using Jasmine.Abs.Api.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Jasmine.Abs.Api.Repositories;
using System.Linq;
using System;
using Jasmine.Abs.Api.Dto.AccountReceivables;
using Jasmine.Abs.Api.Repositories.Exceptions;

namespace Jasmine.Abs.Api.Controllers.AccountReceivables
{
    [Route("api/lc-documents")]
    [ApiController]
    public class LcDocumentController : ControllerBase
    {
        readonly ILcDocumentRepository _repository;
        readonly ILogger<LcDocumentController> _logger;
        public LcDocumentController(ILcDocumentRepository repository, ILogger<LcDocumentController> logger)
        {
            _logger = logger;
            _repository = repository;
        }



        [HttpGet(Name = "GetLcDocumentList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<LcDocumentListDto>>> GetLcDocumentListAsync()
        {
            var items = await _repository.GetAllAsync();
            return Ok(items);

        }
        
        /// <summary>
        /// Gets the LcDocument.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;LcDocumentDto&gt;&gt;.</returns>
        [HttpGet("{id}", Name = "GetLcDocumentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LcDocumentDto>> GetLcDocumentById(int id)
        {
            var entity = await _repository.GetAsync(id).ConfigureAwait(false);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        /// <summary>
        /// Gets the LcDocumentDetail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;ActionResult&lt;LcDocumentDetail&gt;&gt;.</returns>
        [HttpGet("{id}/detail", Name = "GetLcDocumentDetailById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LcDocumentDetailDto>> GetLcDocumentDetailById(int id)
        {
            var entity = await _repository.GetLcDocumentDetailById(id).ConfigureAwait(false);
            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }


        /// <summary>
        /// Creates the LcDocument.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;ActionResult&lt;LcDocumentDto&gt;&gt;.</returns>
        [HttpPost(Name = "CreateLcDocument")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LcDocumentDto>> CreateLcDocument([FromBody] LcDocumentDto entity)
        {

            foreach (var invoice in entity.CommercialInvoices)
            {
                invoice.CreatedUser = User.Identity.Name;
                invoice.CreatedDate=DateTime.Now;
            }
            var createdEntity = await _repository.SaveAsync(entity).ConfigureAwait(false);

            if (createdEntity == null)
            {
                return NotFound();
            }
            return CreatedAtRoute(nameof(GetLcDocumentById), new { createdEntity.Id }, createdEntity);
        }


        /// <summary>
        /// Updates the LcDocument.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task&lt;ActionResult&lt;LcDocumentDto&gt;&gt;.</returns>
        [HttpPut(Name = "UpdateLcDocument")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LcDocumentDto>> UpdateLcDocument([FromBody] LcDocumentDto entity)
        {
            var updatedEntity = await _repository.UpdateAsync(entity);
            if (updatedEntity == null)
            {
                return NotFound();
            }
            return Ok(updatedEntity);
        }

        /// <summary>
        /// Updates the LcDocument.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDoc"></param>
        /// <returns>Task&lt;ActionResult&lt;LcDocumentDto&gt;&gt;.</returns>
        [HttpPatch("{id}", Name = "UpdateLcDocumentPartially")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LcDocumentDto>> UpdateLcDocumentPartially(int id, [FromBody] Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<LcDocumentDto> patchDoc)
        {

            var group = await _repository.GetLcDocumentForUpdateAsync(id);
            if (group == null)
            {
                return NotFound();
            }

            patchDoc.ApplyTo(group, ModelState);


            if (group.CommercialInvoices.Any())
            {
                foreach (var invoice in group.CommercialInvoices)
                {
                    invoice.CreatedUser=User.Identity.Name;
                    invoice.CreatedDate=DateTime.Now;
                }
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }
            var updatedEntity = await _repository.UpdateAsync(group);
            if (updatedEntity == null)
            {
                return NotFound();
            }
            return Ok(updatedEntity);
        }


        /// <summary>
        /// Deletes the LcDocument.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpDelete("{id}", Name = "DeleteLcDocument")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteLcDocument(int id)
        {
            try
            {
                var entity = await _repository.GetLcDocumentForUpdateAsync(id);

                if (entity == null)
                {
                    return NotFound();
                }

                await _repository.DeleteAsync(entity);
                return NoContent();
            }
            catch (EntityAlreadyInUseException exception)
            {
                _logger.Log(LogLevel.Error, exception, exception.Message);
                return new ObjectResult(exception.Message) { StatusCode = 403 };
            }
        }


        /// <summary>
        /// get quotation for print.
        /// </summary>
        /// <param name="id">The quotation identifier.</param>
        /// <returns>Task&lt;IActionResult&gt;.</returns>
        [HttpGet("{id}/print", Name = "GetLcDocumentForPrint")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<LcDocumentForPrintDto>> GetQuotationForPrintAsync(int id)
        {
            var entity = await _repository.GetLcDocumentForPrintAsync(id).ConfigureAwait(false);

            entity.CommercialInvoices = await _repository.GetInvoicesAsync(id);

            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

    }
}
