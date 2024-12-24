using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;


using System.Threading.Tasks.Dataflow;
using WALKWAY_ECommerce.DbContexts;
using WALKWAY_ECommerce.Models.User_Model;
using WALKWAY_ECommerce.Models.User_Model.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using WALKWAY_ECommerce.ApiResponse;

namespace WALKWAY_ECommerce.Services.AuthService
{
    public class AuthService:IAuthService
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IMapper mapper,IConfiguration configuration,AppDbContext context,ILogger<AuthService> logger)
        {
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        public async Task<bool> Register(UserRegistrationDto newUser)
        {
            try
            {
                _logger.LogInformation("Starting Registration Process");

                if (newUser == null)
                {
                    _logger.LogError("User data Cannot be null");
                    throw new ArgumentNullException(nameof(newUser), "User data Cannot be null");
                }

                var ExistingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == newUser.Email);
                if(ExistingUser != null)
                {
                    _logger.LogWarning($"User with EmailId {newUser.Email} Already Exist");
                    return false;
                }
                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);

                var userEntity = _mapper.Map<User>(newUser);

                await _context.AddAsync(userEntity);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User with email {newUser.Email} Registered Successfully");
                return true;
            }
            catch(ArgumentNullException ex)
            {
                _logger.LogError($"Validation Error : {ex.Message}");
                throw new Exception ($"Validation Exception {ex.Message}", ex);
            }
            catch(DbUpdateException ex)
            {
                _logger.LogError($"Database Error :{ex.InnerException?.Message ?? ex.Message}");
                throw new Exception ($"Database Error: {ex.InnerException?.Message?? ex.Message}");
            }
            catch (Exception ex) 
            {
                _logger.LogError($"An error Occured: {ex.Message}");
                throw new Exception($"An error Occured:{ex.Message}", ex);
            }
        }


        public async Task<UserResDto> Login(UserLoginDto userDto)
        {
            try
            {
                _logger.LogInformation("Login attempt started");


                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);

                if (user == null)
                {
                    _logger.LogError("User not found");
                    
                    return new UserResDto { Error = "User not Found" };
                }  

                if (user.IsBlocked==true)
                {
                    _logger.LogWarning("user is Blocked");
                    return  new UserResDto { Error= "User is blocked by admin" };
                }

                bool isValidPassword = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);

                if (!isValidPassword)
                {
                    _logger.LogError($"Invalid Password for {userDto.Email}");
                    return new UserResDto { Error="Invalid Password" };
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,user.Role),
                };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: credentials);

                string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation($"Login successful for {userDto.Email}");

                return new UserResDto
                {
                    Token = jwtToken,
                    Role = user.Role,
                    Email = user.Email,
                    UserName = user.UserName,
                };
                
            }catch(Exception ex)
            {
                _logger.LogError("an error Occur during login");
                throw ex;
            }
        }
    }
}
