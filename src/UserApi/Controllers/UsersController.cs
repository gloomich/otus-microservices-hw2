using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserApi.DataAccess;
using UserApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UserApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserDbContext _dbContext;
        public UsersController(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<User>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.User>>> Get(CancellationToken cancellationToken = default)
        {
            return (await _dbContext.Users.ToListAsync(cancellationToken))
                .Select(x => new Models.User(x))
                .ToList();
        }

        // GET api/<User>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.User>> Get(int id, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
            return user == null
                ? NotFound("User not found")
                : new Models.User(user);
        }

        // POST api/<User>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUser user, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var entity = new DataAccess.User { FirstName = user.FirstName, LastName = user.LastName };
            _dbContext.Users.Add(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        // PUT api/<User>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Models.User>> Put(int id, [FromBody] Models.User user, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            if (id != user.Id)
            {
                return BadRequest("Wrong ID");
            }

            var entity = await _dbContext.Users.FindAsync(new object[] { id }, cancellationToken);
            if (entity == null)
            {
                return NotFound("User not found");
            }

            entity.FirstName = user.FirstName;
            entity.LastName = user.LastName;
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user;
        }

        // DELETE api/<User>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
    }
}
