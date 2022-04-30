using AutoFixture;
using FluentAssertions;
using JE_PracticalTask.Logic;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JE_PracticalTask.Exceptions;

namespace JE_PracticalTask.Tests
{
    public class MovieDetailsLogicTests
    {
        private Mock<HttpClient> _clientMock;
        private MovieDetailsLogic _target;

        public MovieDetailsLogicTests()
        {
            _clientMock = new Mock<HttpClient>();
            _target = new MovieDetailsLogic(_clientMock.Object);
        }

        [Test]
        public void SearchMoviesByImdbId_StringImdbIdIsNull_ArgumentExceptionThrown()
        {
            // Act & Assert
            ArgumentException ex =
                Assert.ThrowsAsync<ArgumentException>(() => _target.SearchMoviesByImdbId(null));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Imdb ID is not provided.");
        }

        [Test]
        public void SearchMoviesByImdbId_StringImdbIdIsEmpty_ArgumentExceptionThrown()
        {
            // Arrange
            var imdbIdToSearch = string.Empty;

            // Act & Assert
            ArgumentException ex =
                Assert.ThrowsAsync<ArgumentException>(() => _target.SearchMoviesByImdbId(imdbIdToSearch));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Imdb ID is not provided.");
        }

        [Test]
        public void SearchMoviesByImdbId_BadRequest_HttpRequestExceptionThrown()
        {
            // Arrange
            string imdbIdToSearch = "Test string";

            // Taken from:
            //https://www.thetechplatform.com/post/how-to-use-and-unit-test-httpclient-in-asp-net-core
            var fixture = new Fixture();

            var delegatingHandlerMock = new Mock<DelegatingHandler>();
            delegatingHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(fixture.Create<string>())
                });

            _clientMock = new Mock<HttpClient>(delegatingHandlerMock.Object);
            _target = new MovieDetailsLogic(_clientMock.Object);

            // Act & Assert
            HttpRequestException ex =
                Assert.ThrowsAsync<HttpRequestException>(() => _target.SearchMoviesByImdbId(imdbIdToSearch));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Network connection problem.");
        }

        [Test]
        public void SearchMoviesByImdbId_StringImdbIdIsImpossibleValue_MovieDetailsSearchExceptionThrown()
        {
            // Arrange
            var imdbIdToSearch = "Test string";

            // Act & Assert
            MovieDetailsSearchException ex =
                Assert.ThrowsAsync<MovieDetailsSearchException>(() => _target.SearchMoviesByImdbId(imdbIdToSearch));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Incorrect IMDb ID.");
        }

        [Test]
        public void SearchMoviesByImdbId_StringImdbIdIsCorrectValue_MovieDetailsSearchExceptionThrown()
        {
            // Arrange
            var imdbIdToSearch = "tt2015381";

            // Act
            var response = _target.SearchMoviesByImdbId(imdbIdToSearch);

            // Assert
            response.Result.Response.Should().Be("True");
            response.Result.Error.Should().BeNull();
            response.Result.Title.Should().Be("Guardians of the Galaxy");
        }
    }
}
