﻿using AutoMapper;
using Inlamningsuppgift_Marie.Models.Artist;
using Inlamningsuppgift_Marie.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inlamningsuppgift_Marie.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _service;
        private readonly IMapper _mapper;

        public ArtistsController(IArtistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<ActionResult> CreateArtist(CreateArtistModel request)
        {
            var result = await _service.CreateArtistAsync(request);
            if (result != null)
                return new OkObjectResult(result);

            return BadRequest("The artistname is already exists");
            //return new BadRequestResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllArtists()
        {
            return new OkObjectResult(await _service.GetAllArtistsAsync());
        }

        [HttpGet("{artistId:int}")]
        public async Task<ActionResult> GetArtistById(int artistId)
        {
            //return new OkObjectResult(await _service.GetArtistByIdAsync(id));
            //var artist = await _databaseContext.Artists.FindAsync(album.ArtistId);
            //album.ArtistName = artist.ArtistName;

            var artist = await _service.GetArtistByIdAsync(artistId);
            return Ok(artist);
        }

        [HttpDelete("{artistId:int}")]
        public async Task<ActionResult> DeleteArtistById(int artistId)
        {
            if (await _service.DeleteArtistByIdAsync(artistId))
                return new OkResult();

            return new NotFoundResult();
        }
    }
}
