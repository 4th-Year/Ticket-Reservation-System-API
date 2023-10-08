using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }


        // GET: api/user
        [HttpGet]
        public async Task<List<User>> Get() => await _userServices.GetAsync();

        // GET api/user/5
        [HttpGet]
        [Route("find/{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            User user = await _userServices.GetAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST api/user
        [HttpPost]
        [Route("add")]
        public async Task<ActionResult<Object>> create(User newUser)
        {
            ApiResponse<object> apiResponse = new ApiResponse<object>();

            await _userServices.CreateAsync(newUser);
            apiResponse.Message = "User Created Successfully";
            apiResponse.Data = newUser;
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, apiResponse);
        }

        // PUT api/user/5
        [HttpPut("{id:length(24)}")]
        public async Task <ActionResult> Update(string id, User updateUser)
        {
            User user = await _userServices.GetAsync(id);

            if(user == null)
            {
                return NotFound("There is no user with this ID: " + id);
            }

            updateUser.Id = user.Id;

            await _userServices.UpdateAsync(id, updateUser);

            return Ok("Updated Successfully");

        }

        // DELETE api/user/5
        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {

            User user = await _userServices.GetAsync(id);

            if (user == null)
            {
                return NotFound("There is no user with this ID: " + id);
            }

            await _userServices.RemoveAsync(id);

            return Ok("Deleted Successfully");
        }
    }
}
