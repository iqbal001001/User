using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.Domain;
using Microsoft.Extensions.Logging;
using User.ServiceInterface;
using User.Web.Dto;
using User.Web.Extensions;
using System.Threading;
using Newtonsoft.Json;

namespace User.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserService _UserService;

        public UserController(ILogger<UserController> logger, IUserService UserService,
            IUrlHelper urlHelper) : base(logger, urlHelper)
        {
            _UserService = UserService;
        }

        [HttpGet()]
        [Route("", Name = "UserList")]
        public async Task<ActionResult<List<UserDto>>> Get(
            string jsonSearchFilters = "{}",
            string sort = "id",
            int page = 1, int pageSize = 5,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var filters = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonSearchFilters);
            
                var (result, totalCount) = await _UserService.GetAllAsync(sort, page, pageSize, filters, cancellationToken);

                ApplyPagination(sort, page, pageSize, totalCount);

                return Ok(result.Select(p => p.ToDto()));
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex.Message);
                 return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _UserService.GetByIdAsync(id, cancellationToken);

                if (result == null)
                {
                    return NotFound();
                }

                var dto = result.ToDto();

                return Ok(dto);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserInfo>> Post(UserInsertDto UserInsertDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var User = UserInsertDto.ToDomain();
                if (!_UserService.Validate(User))
                {
                    return BadRequest();
                }
                await _UserService.AddAsync(User, cancellationToken);

                return CreatedAtAction(nameof(Post), new { id = User.Id }, User);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserInfo>> Put(int id, UserDto UserDto, CancellationToken cancellationToken = default)
        {
            if (id != UserDto.Id)
            {
                return BadRequest();
            }
            try
            {

                var found = await _UserService.AnyByIdAsync(id, cancellationToken);
                if (!found)
                {
                    return NotFound();
                }
                var p = await _UserService.GetByIdAsync(id, cancellationToken);
                var User = UserDto.ToDomain(p);
                if (!_UserService.Validate(User))
                {
                    return BadRequest();
                }
                await _UserService.UpdateAsync(User, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var found = await _UserService.AnyByIdAsync(id, cancellationToken);
                if (!found)
                {
                    return NotFound();
                }
                await _UserService.DeleteAsync(id, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            return NoContent();
        }
    }
}
