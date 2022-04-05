using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Common.Extensions;
using Project.Common.Helpers;
using Project.Data.Dto;
using Project.Data.Dto.Account;
using Project.Service.Account;
using System;

namespace Project.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }


        [HttpGet("[action]")]
        public UserDto GetDetail()
        {
            var currentUser = HttpContext.User.GetCurrentUser();

            return this._userService.GetDto(currentUser.UserId);
        }


        [HttpPost("[action]")]
        public IActionResult Update([FromBody] UserUpdateDto model)
        {
            var currentUser = HttpContext.User.GetCurrentUser();

            var resultModel = new RequestStateDto();

            try
            {
                #region Validation

                if (model == null)
                {
                    resultModel.Description = "Hatalı işlem";
                    return Ok(value: resultModel);
                }

                #endregion


                var user = this._userService.FindUser(currentUser.UserId);

                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                user.UpdateDateTime = DateTime.Now;

                this._userService.Update(user);

                resultModel.IsSuccess = true;

                return Ok(value: resultModel);
            }
            catch (Exception err)
            {
                resultModel.Description = DebugHelper.GetExceptionErrorMessage(err);
                return Ok(value: resultModel);
            }
        }
    }
}