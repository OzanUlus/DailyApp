using AutoMapper;
using DailyApp.Context;
using DailyApp.DTOs;
using DailyApp.Enitities;
using DailyApp.Extentions;
using DailyApp.Token;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DailyApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IValidator<LoginDto> _userLoginDtoValidator;
        private readonly AppDbContext _dbContext;

        public AccountController(UserManager<AppUser> userManager, IMapper mapper, IValidator<LoginDto> userLoginDtoValidator, AppDbContext dbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userLoginDtoValidator = userLoginDtoValidator;
            _dbContext = dbContext;
        }

        [HttpPost("Resgister")]
        public async Task<IActionResult> Resgister(UserCreateDto dto)
        {
            var user = new AppUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,



            };

            
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                return BadRequest("Bir hata meydana geldi");

            }

            return Ok();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            


            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) { return NotFound("Kullanıcı veya şifre hatası"); }

            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return BadRequest();
            }


            var validationResult = _userLoginDtoValidator.Validate(loginDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.CustomValidationErrorList());
            }

            TokenGenerator tokenGenerator = new TokenGenerator();


            var userClaims = (await _userManager.GetClaimsAsync(user)).ToList();
            var RoleClaims = (await _userManager.GetRolesAsync(user)).ToList().Select(x => new Claim(ClaimTypes.Role, x));

            var userInfoClaims = new List<Claim>()
            {

             new(ClaimTypes.NameIdentifier,user.Id),
             new(ClaimTypes.Email,user.Email),



             };

            userClaims.AddRange(RoleClaims);
            userClaims.AddRange(userInfoClaims);

            var token = tokenGenerator.GenerateToken(userClaims);


            return Ok(token);



        }
        [HttpPost]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> AddDaily(DailyDto dailyDto) 
        {
            Daily daily = new Daily()
            {
                Title = dailyDto.Title,
                Content = dailyDto.Content,

            };
            await _dbContext.AddAsync(daily);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);

        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(string id) 
        {
          var daily = _dbContext.Dailies.FirstOrDefault(x=>x.Id==id);
            if (daily != null) 
            {
              _dbContext.Dailies.Remove(daily);
                _dbContext.SaveChangesAsync();
            
            }
            return Ok();
        
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update(string id,UpdateDailyDto updateDaily) 
        {
          var daily = _dbContext.Dailies.FirstOrDefault(x=>x.Id==id);
            if (daily == null) 
            {
                return BadRequest("Bir hata oluştu.");
            
            }
            Daily daily1 = new Daily()
            {
                Title = updateDaily.Title,
                Content = updateDaily.Content

            };
            _dbContext.Dailies.Update(daily1);
            _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);


        }
        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetDailies() 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var dailies = await _dbContext.Dailies.Where(x=>x.AppUserId==userId).ToListAsync();
            return Ok(dailies);
        
        }


        public class DailyDto 
        {
            public string Title { get; set; }
            public string Content { get; set; }

        }
        public class UpdateDailyDto 
        {
            public string Title { get; set; }
            public string Content { get; set; }


        }
    }
}
