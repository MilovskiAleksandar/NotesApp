using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SEDC.NotesAppFluentApi.DataAccess.Interfaces;
using SEDC.NotesAppFluentApi.Domain.Models;
using SEDC.NotesAppFluentApi.DTOs.Notes;
using SEDC.NotesAppFluentApi.DTOs.Users;
using SEDC.NotesAppFluentApi.Mappers.Users;
using SEDC.NotesAppFluentApi.Services.Interfaces;
using SEDC.NotesAppFluentApi.Shared;
using SEDC.NotesAppFluentAPi.Shared.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InvalidDataException = SEDC.NotesAppFluentAPi.Shared.Shared.InvalidDataException;

namespace SEDC.NotesAppFluentApi.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        //we retrieve the AppSettings section from appSettings.json file
        private readonly IOptions<AppSettings> _options;

        public UserService(IUserRepository userRepository, IOptions<AppSettings> options)
        {
            _userRepository = userRepository;
            _options = options;
        }

        //return type is string because we will return the generated token
        public string Login(LoginUserDto loginUserDto)
        {
            if (string.IsNullOrEmpty(loginUserDto.UserName) || string.IsNullOrEmpty(loginUserDto.Password))
            {
                throw new InvalidDataException("Username and password are required fields.");
            }

            string hash = GenerateHash(loginUserDto.Password);

            User userDb = _userRepository.GetUserByUsernameAndPassword(loginUserDto.UserName, hash);
            if (userDb == null)
            {
                throw new ResourceNotFoundException($"Invalid login for username {loginUserDto.UserName}");
            }

            //generate JWT token that will be returned to the client

            string userRole = string.IsNullOrEmpty(userDb.Role) ? "noRole" : userDb.Role;

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            //secret key bytes
            byte[] secretKeyBytes = Encoding.ASCII.GetBytes(_options.Value.OurSecretKey);

            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddHours(24), // the token will be valid for one hour
                //signature configuration, signing algorithm that will be used to generate hash (third part of token)
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes),
                                SecurityAlgorithms.HmacSha256Signature),
                //payload
                Subject = new ClaimsIdentity(
                                new[]
                                {
                        new Claim("userFullName", userDb.FirstName + " " + userDb.LastName ),
                        new Claim(ClaimTypes.NameIdentifier, userDb.UserName),
                        new Claim("userRole", userRole)
                                })
            };
            //generate token
            SecurityToken token = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            string resultToken = jwtSecurityTokenHandler.WriteToken(token);
            return resultToken;
        }


        public void RegisterUser(RegisterUserDto registerUserDto)
        {

            ValidationHelper.ValidateRequiredStringColumn(registerUserDto.UserName, "Username", 20);

            if (!string.IsNullOrEmpty(registerUserDto.FirstName))
            {
                ValidationHelper.ValidateStringColumnLenght(registerUserDto.FirstName, "FirstName", 50);
            }

            if (!string.IsNullOrEmpty(registerUserDto.LastName))
            {
                ValidationHelper.ValidateStringColumnLenght(registerUserDto.LastName, "LastName", 50);
            }

            if(string.IsNullOrEmpty(registerUserDto.Password) || string.IsNullOrEmpty(registerUserDto.ConfirmedPassword))
            {
                throw new InvalidDataException("Password fields are required");
            }

            if(registerUserDto.Password != registerUserDto.ConfirmedPassword)
            {
                throw new InvalidDataException("Password do not match");
            }

            User userDb = _userRepository.GetUserByUsername(registerUserDto.UserName);
            if(userDb != null)
            {
                //this means that we have a user with registerUserDto.Username username in the db
                throw new InvalidDataException($"Username {registerUserDto.UserName} EXISTS!");
            }


            string hash = GenerateHash(registerUserDto.Password);

            User newUser = registerUserDto.ToUser(hash);

            _userRepository.Add(newUser);
        }


        private static string GenerateHash(string password)
        {
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();

            //Test123 - get the bytes => 5466879
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);

            //hash the bytes => 5466879 => 6563434
            byte[] hashedBytes = mD5CryptoServiceProvider.ComputeHash(passwordBytes);

            //get a string from the hashed bytes, 6563434 => aqwer246esdg
            return Encoding.ASCII.GetString(hashedBytes);
        }
    }
}
