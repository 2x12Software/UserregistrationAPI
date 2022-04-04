using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using User.BLL;

namespace UserRegistratiomAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    [EnableCors("CorsApi")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserBL _userBL;
        public UserController(ILogger<UserController> logger, UserBL userBL)
        {
            _logger = logger;
            _userBL = userBL;
        }
        [HttpPost("InsertUpdateuser/")]
        public IActionResult InsertUpdate_User([FromBody] UserDTO _user)
        {
            try
            {
                if (!IsValidUser(_user))
                    return StatusCode(500, GenericActionResult.Failure($"An exception occured while trying to create user", null));

                try
                {
                    return Ok(GenericActionResult.Success(null, this._userBL.SaveUserDetailsToFile(_user)));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving to file");
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.ToString(), Array.Empty<Object>());
                return StatusCode(500, GenericActionResult.Success($"An exception occured while trying to Inserting or Updating employee Data", null));
            }
        }
        private bool IsValidUser(UserDTO user)
        {
            if (user != null && !string.IsNullOrEmpty(user.FistName) && !string.IsNullOrEmpty(user.Lastname) && !string.IsNullOrEmpty(user.Emailid) && !string.IsNullOrEmpty(user.ContactNumber))
                return true;

            return false;
        }
    }
}
