using Com.DanLiris.Service.Core.WebApi.Helpers;
using Com.Moonlay.NetCore.Lib.Service;
using EWorkplaceCoreService.Lib.Helpers.IdentityService;
using EWorkplaceCoreService.Lib.Helpers.ValidateService;
using EWorkplaceCoreService.Lib.Models;
using EWorkplaceCoreService.Lib.Services.Divisions;
using EWorkplaceCoreService.Lib.Services.JobTitles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorkplaceCoreService.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Authorize]
    [Route("v{version:apiVersion}/job-titles")]
    public class JobTitleController : Controller
    {
        private readonly IJobTitleService _jobTitleService;
        private readonly IDivisionService _divisionService;
        private readonly IIdentityService _identityService;
        private readonly IValidateService _validateService;
        private const string API_VERSION = "1.0";

        public JobTitleController(IServiceProvider serviceProvider)
        {
            _jobTitleService = serviceProvider.GetService<IJobTitleService>();
            _divisionService = serviceProvider.GetService<IDivisionService>();
            _identityService = serviceProvider.GetService<IIdentityService>();
            _validateService = serviceProvider.GetService<IValidateService>();
        }

        private void VerifyUser()
        {
            _identityService.Username = User.Claims.ToArray().SingleOrDefault(p => p.Type.Equals("username")).Value;
            _identityService.Token = Request.Headers["Authorization"].FirstOrDefault().Replace("Bearer ", "");
            _identityService.TimezoneOffset = Convert.ToInt32(Request.Headers["x-timezone-offset"]);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] JobTitle jobTitle)
        {
            try
            {
                VerifyUser();
                _validateService.Validate(jobTitle);

                await _jobTitleService.Create(jobTitle);

                var result = new ResultFormatter(API_VERSION, General.CREATED_STATUS_CODE, General.OK_MESSAGE)
                    .Ok();
                return Created(string.Concat(Request.Path, "/", 0), result);
            }
            catch (ServiceValidationExeption e)
            {
                var result = new ResultFormatter(API_VERSION, General.BAD_REQUEST_STATUS_CODE, General.BAD_REQUEST_MESSAGE)
                    .Fail(e);
                return BadRequest(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(API_VERSION, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] int id)
        {
            try
            {
                VerifyUser();

                var viewModel = await _jobTitleService.GetSingleById(id);

                var result = new ResultFormatter(API_VERSION, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(viewModel);
                return Ok(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(API_VERSION, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int size = 25)
        {
            try
            {
                VerifyUser();

                var jobTitleQuery = _jobTitleService.GetQuery();
                if (!string.IsNullOrWhiteSpace(keyword))
                    jobTitleQuery = jobTitleQuery.Where(entity => entity.Code.Contains(keyword) || entity.Name.Contains(keyword));

                var jobTitleQueryResult = await jobTitleQuery
                    .Skip((page - 1) * size)
                    .Take(size)
                    .OrderByDescending(entity => entity.LastModifiedUtc)
                    .ToListAsync();

                var jobTitles = new List<JobTitleViewModel>();
                if (jobTitleQueryResult.Count > 0)
                {
                    var divisionIds = jobTitleQueryResult.Select(entity => entity.DivisionId).ToList();
                    var divisions = _divisionService.GetQuery().Where(entity => divisionIds.Contains(entity.Id)).ToList();

                    foreach (var jobTitle in jobTitleQueryResult)
                    {
                        var division = divisions.FirstOrDefault(entity => entity.Id == jobTitle.DivisionId);
                        var jobTitleViewModel = new JobTitleViewModel(jobTitle, division);
                        jobTitles.Add(jobTitleViewModel);
                    }
                }

                var result = new ResultFormatter(API_VERSION, General.OK_STATUS_CODE, General.OK_MESSAGE)
                    .Ok(jobTitles);
                return Ok(result);
            }
            catch (Exception e)
            {
                var result = new ResultFormatter(API_VERSION, General.INTERNAL_ERROR_STATUS_CODE, e.Message)
                    .Fail();
                return StatusCode(General.INTERNAL_ERROR_STATUS_CODE, result);
            }
        }
    }
}