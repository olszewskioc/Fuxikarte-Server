using Moq;
using Xunit;
using System.Threading.Tasks;
using Fuxikarte.Backend.Services;
using Fuxikarte.Backend.Models;
using Fuxikarte.Backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks; // Para usar async/await
using System.Collections.Generic; // Para IEnumerable e listas
using Microsoft.AspNetCore.Identity; // Para IPasswordHasher
using AutoMapper; // Se usar AutoMapper nos testes
using System;
using MockQueryable.Moq;
using Microsoft.Extensions.Configuration;
using Fuxikarte.Backend.DTOs;
using Fuxikarte.Backend.Mappings; // Para Exception, etc.

namespace Fuxikarte.Tests
{
    public class UserServiceTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Garante isolamento entre testes
                .Options;

            return new AppDbContext(options);
        }

        private static IMapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });
            return config.CreateMapper();
        }

        [Theory]
        [InlineData("validUser", "123456", true)]
        [InlineData("validUser", "wrongPassword", false)]
        [InlineData("invalidUser", "123456", false)]
        public async Task ValidateUserCredentials_VariousInputs_ReturnsExpectedResult(string inputUsername, string inputPassword, bool expectedIsValid)
        {
            // Arrange
            using var context = GetInMemoryDbContext();
            var passwordHasher = new PasswordHasher<User>();
            var user = new User { Username = "validUser" };
            user.Password = passwordHasher.HashPassword(user, "123456");
            context.Users.Add(user);
            context.SaveChanges();

            var config = new ConfigurationBuilder().Build(); // ou use um mock se necessário

            var authService = new AuthService(context, config);

            // Act
            var result = await authService.ValidateUserCredentials(inputUsername, inputPassword) != null;

            // Assert
            Assert.True(expectedIsValid == result, "O usuário retornado não corresponde ao esperado.");
        }

        public static IEnumerable<object[]> NewUsers =>
            new List<object[]>
            {
            new object[] { new UserRegistrationDTO { Username = "user1", Password = "123" } },
            new object[] { new UserRegistrationDTO { Username = "user2", Password = "456" } }
            };

        [Theory]
        [MemberData(nameof(NewUsers))]
        public async Task CreateUser_WithValidUser_AddsUser(UserRegistrationDTO newUser)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + newUser.Username)
                .Options;

            // Cria um novo contexto para garantir isolamento do teste
            await using var context = new AppDbContext(options);

            var mapper = GetMapper();

            var service = new UserService(context, mapper);

            // Act
            await service.CreateUser(newUser);

            // Assert
            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Username == newUser.Username);
            Assert.NotNull(userInDb);
            Assert.Equal(newUser.Username, userInDb.Username);
            // Você pode testar se a senha foi 'hasheada' (não é a senha original)
            Assert.NotEqual(newUser.Password, userInDb.Password);
        }

        [Fact]
        public async Task GetAllUsers_WhenUsersExist_ReturnsAllUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllUsers_WithData")
                .Options;

            await using var context = new AppDbContext(options);

            // Adiciona usuários diretamente no banco em memória
            context.Users.Add(new User { Username = "user1", Password = "hash1" });
            context.Users.Add(new User { Username = "user2", Password = "hash2" });
            await context.SaveChangesAsync();

            var mapper = GetMapper();

            var service = new UserService(context, mapper);

            // Act
            var users = await service.GetAllUsers();

            // Assert
            Assert.NotNull(users);
            Assert.Equal(2, users.Count());
            Assert.Contains(users, u => u.Username == "user1");
            Assert.Contains(users, u => u.Username == "user2");
        }

        [Fact]
        public async Task GetAllUsers_WhenNoUsers_ReturnsEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "GetAllUsers_Empty")
                .Options;

            await using var context = new AppDbContext(options);

            var hasher = new PasswordHasher<User>();
            var mapper = GetMapper();

            var service = new UserService(context, mapper);

            // Act
            var users = await service.GetAllUsers();

            // Assert
            Assert.NotNull(users);
            Assert.Empty(users);
        }

        public static IEnumerable<object[]> UpdateUsers =>
            new List<object[]>
            {
            new object[] { new UserUpdateDTO { Username = "UpdateAll", Password = "UpdateAll" } },
            new object[] { new UserUpdateDTO { Username = "UpdateName" } },
            new object[] { new UserUpdateDTO { Password = "UpdatePassword" } },
            new object[] { new UserUpdateDTO { } }
            };

        [Theory]
        [MemberData(nameof(UpdateUsers))]
        public async Task UpdateUser_WithValidUser_ReturnExpected(UserUpdateDTO newUser)
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + newUser.Username)
                .Options;

            // Cria um novo contexto para garantir isolamento do teste
            await using var context = new AppDbContext(options);

            context.Users.Add(new User { Username = "user1", Password = "hash1" });

            await context.SaveChangesAsync();

            var mapper = GetMapper();

            var service = new UserService(context, mapper);
            var userInDbBefore = await context.Users.FirstOrDefaultAsync(u => u.UserId == 1);

            // Act
            bool result = await service.UpdateUser(1, newUser);

            // Assert
            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.UserId == 1);
            Assert.NotNull(userInDbBefore);
            Assert.NotNull(userInDb);
            Assert.True(result);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        public async Task DeleteUser_ResultExpected(int id, bool resultExpected)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDeleteDb_" + id.ToString())
                .Options;

            // Cria um novo contexto para garantir isolamento do teste
            await using var context = new AppDbContext(options);

            context.Users.Add(new User { Username = "user1", Password = "hash1" });

            await context.SaveChangesAsync();

            var mapper = GetMapper();

            var service = new UserService(context, mapper);

            bool result = await service.DeleteUser(id);

            Assert.True(result == resultExpected, $"Usuário não foi deletado corretamente com o id {id}.");
        }

    }
}
