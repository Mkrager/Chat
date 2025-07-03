﻿using Chat.Api.IntegrationTests.Base;
using Chat.Application.DTOs;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Chat.Api.IntegrationTests.Controllers
{
    public class AccountControllerTests : IClassFixture<CustomIdentityWebApplicationFactory<Program>>
    {
        private readonly CustomIdentityWebApplicationFactory<Program> _factory;

        public AccountControllerTests(CustomIdentityWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Register_ReturnSucces()
        {
            var client = _factory.GetAuthenticatedClient();

            var registrationRequest = new RegistrationRequest()
            {
                Email = "example@gmail.com",
                FirstName = "TestFirtsName",
                LastName = "TestLastName",
                Password = "TestPassword123!",
                UserName = "TestUserName"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(registrationRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Account/register", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<RegistrationResponse>(responseString);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Authentication_ReturnSucces()
        {
            var client = _factory.GetAuthenticatedClient();

            var registrationRequest = new RegistrationRequest()
            {
                Email = "example@gmail.com",
                FirstName = "TestFirtsName",
                LastName = "TestLastName",
                Password = "TestPassword123!",
                UserName = "TestUserName"
            };

            var registrationContent = new StringContent(
                JsonSerializer.Serialize(registrationRequest),
                Encoding.UTF8,
                "application/json"
            );

            var registrationResponse = await client.PostAsync("/api/Account/register", registrationContent);


            var authenticationRequest = new AuthenticationRequest()
            {
                Email = "example@gmail.com",
                Password = "TestPassword123!",
            };

            var content = new StringContent(
                JsonSerializer.Serialize(authenticationRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("/api/Account/authenticate", content);

            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthenticationResponse>(responseString);

            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }
    }
}
