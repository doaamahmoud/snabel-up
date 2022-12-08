using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using snabel_up.DTO;
using snabel_up.Models;

namespace snabel_up.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Login> userManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<Login> userManager,IConfiguration Configuration)
        {
            this.userManager = userManager;
             configuration = Configuration;
        }
     
        [HttpPost("PostLogin")]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            //check identityt  "Create Token" ==Cookie
            Login userModel= await userManager.FindByNameAsync(login.UserName);
            if (userModel != null)
            {
                if(await userManager.CheckPasswordAsync(userModel, login.Password) == true)
                {
                    //toke base on Claims "Name &Roles &id " +Jti ==>unique Key Token "String"
                    var mytoken =await GenerateToke(userModel);
                    return Ok(new { 
                        token=new JwtSecurityTokenHandler().WriteToken(mytoken) ,
                        expiration= mytoken.ValidTo
                    });
                }
                else
                {
                    //return BadRequest("User NAme and PAssword Not Valid");
                    return Unauthorized();//401
                }
            }
            return Unauthorized();
        }

        [HttpPost("GetLogin")]
        public async Task<IActionResult> GetLogin(Login login)
        {
            if (ModelState.IsValid == false)
            {
                return Unauthorized();
            }
            Login userModel = await userManager.FindByNameAsync(login.UserName);
            if (userModel != null)
            {
                if (await userManager.CheckPasswordAsync(userModel, login.Password) == true)

                    return Ok();
            }
            else
            {
                return Unauthorized();
            }
            return Unauthorized();
        }

        [NonAction]
        public async Task<JwtSecurityToken> GenerateToke(Login userModel)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Doaa", "1234"));//Custom
            claims.Add(new Claim(ClaimTypes.Name, userModel.UserName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userModel.Id));
            var roles = await userManager.GetRolesAsync(userModel);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //Jti "Identifier Token
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            //---------------------------------(: Token :)--------------------------------------
            var key =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecrtKey"]));
            var mytoken = new JwtSecurityToken(
                audience: configuration["JWT:ValidAudience"],
                issuer: configuration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials:
                       new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return mytoken;
        }

    }
}
