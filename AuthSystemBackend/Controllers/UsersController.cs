using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystemBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystemBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        Database database = new Database();

        // GET api/values
        [HttpGet, Route("list")]
        public ActionResult Get()
        {
            try
            {
                List<UserModel> data = database.GetUsersList();
                return Ok(new { data });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet, Route("user/"+"{id}")]
        public ActionResult Get(int id)
        {
            try
            {
                UserModel data = database.SearchUser(id);
                return Ok(new { data });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Post para nuevo usuario
        [HttpPost, Route("create")]
        public ActionResult Post([FromBody] UserModel user)
        {
            try
            {
                database.CreateUser(user);
                return Ok(user);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete para eliminar usuario
        [HttpDelete, Route("delete/"+"{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                database.DeleteUser(Convert.ToInt32(id));
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }   
        }

        // Put para editar usuario
        [HttpPut, Route("update/" + "{id}")]
        public ActionResult Put([FromBody] UserModel user)
        {
            try
            {
                Console.WriteLine(user.Password);
                database.UpdateUser(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
