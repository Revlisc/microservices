using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServies.Http;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepo _repository;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _commandDataClient;

        public PlatformsController(IPlatformRepo repository, IMapper mapper, ICommandDataClient commandDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _commandDataClient = commandDataClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDTO>> GetPlatforms()
        {
            Console.WriteLine("--> Getting Platforms....");

            var platformItems = _repository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDTO>>(platformItems));

        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public ActionResult<PlatformReadDTO> GetPlatformById(int id)
        {
            var platformItem = _repository.GetPlatformById(id);
            if (platformItem != null)
            {
                return Ok(_mapper.Map<PlatformReadDTO>(platformItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDTO>> CreatePlatform(PlatformCreateDTO platform)
        {
            var platformModel = _mapper.Map<Platform>(platform);
            _repository.CreatePlatform(platformModel);
            _repository.SaveChanges();

            var platformReadDTO = _mapper.Map<PlatformReadDTO>(platformModel);

            try 
            {
                await _commandDataClient.SendPlatformToCommand(platformReadDTO);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformReadDTO.Id}, platformReadDTO);
        }
    }
}