using AutoMapper;
using CoreCodeCamp.Data.Entities;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampController : ControllerBase
    {
        private readonly ICampRepository repository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public CampController(ICampRepository repository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }


        [HttpGet]
        public async Task<ActionResult<CampModel[]>> GetCamps()
        {
            try
            {
                var results = await repository.GetAllCampsAsync();
                CampModel[] models = mapper.Map<CampModel[]>(results);
                return models;
            }
            catch (Exception)
            {
                return BadRequest("Data Failure");
            }   
        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> GetCamp(string moniker)
        {
            try
            {
                var result = await repository.GetCampAsync(moniker);
                CampModel model = mapper.Map<CampModel>(result);
                return model;
            }
            catch (Exception)
            {
                return BadRequest("Data Failure");
            }
        }

        //[HttpGet("{search}")]
        //public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate)
        //{
        //    try
        //    {
        //        var results = await repository.GetAllCampsByEventDate(theDate);
        //        if (!results.Any()) { return NotFound(); }
        //        return mapper.Map<CampModel[]>(results);
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest("Data Failure");
        //    }
        //}

        //////////////////////////////////////POST//////////////////////////////////

        //public async Task<ActionResult<CampModel>> Post(CampModel campModel)
        //{
        //    try
        //    {
        //        var location = linkGenerator.GetPathByAction("Get", "Camp", new {moniker = campModel.Moniker });
        //        if (string.IsNullOrWhiteSpace(location))
        //        {
        //            return BadRequest("cannot get the moniker");
        //        }

        //        //Create a new CampModel
        //        var camp = mapper.Map<Camp>(campModel);
        //        repository.Add(camp);
        //        if (await repository.SaveChangesAsync())
        //        { 
        //            return Created($"api/Camp/{camp.Moniker}", mapper.Map<CampModel>(camp));
        //        }
        //        return BadRequest("Cannot be save Camp in Data");
        //    }
        //    catch (Exception)
        //    {

        //        return BadRequest("Data Failure");
        //    }
        //}

        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker,CampModel campModel)
        {
            try
            {
                var oldCampModel = await repository.GetCampAsync(moniker);
                if(oldCampModel == null)
                {
                    return NotFound("We ca not Found a campModel with such as moniker");
                }
                mapper.Map(campModel, oldCampModel);
                if (await repository.SaveChangesAsync())
                {
                    return mapper.Map<CampModel>(oldCampModel);
                }
                return BadRequest("Data Failure");
            }
            catch (Exception)
            {

                return BadRequest("Data Failure");
            }
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete ( string moniker)
        {
            try
            {
                var oldCamp = await repository.GetCampAsync(moniker);
                if(oldCamp == null)
                {
                    return NotFound("Not Found camp");
                }

                repository.Delete(oldCamp);
                if(await repository.SaveChangesAsync())
                {
                    return Ok();
                }
                return BadRequest("Data Failure");

            }
            catch (Exception)
            {

                return BadRequest("Data Failure");
            }
        }
    }
}
