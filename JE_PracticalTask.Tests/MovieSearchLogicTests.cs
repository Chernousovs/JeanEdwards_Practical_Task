using AutoFixture;
using FluentAssertions;
using JE_PracticalTask.Logic;
using JE_PracticalTask.Logic.Interfaces;
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
    public class MovieSearchLogicTests
    {
        private Mock<HttpClient> _clientMock;
        private readonly Mock<IHistoryLogic> _historyLogicMock;
        private MovieSearchLogic _target;

        public MovieSearchLogicTests()
        {
            _clientMock = new Mock<HttpClient>();
            _historyLogicMock = new Mock<IHistoryLogic>();
            _target = new MovieSearchLogic(_clientMock.Object, _historyLogicMock.Object);
            _historyLogicMock.Setup(x => x.UpdateQueryHistory(It.IsAny<string>()));
        }

        [Test]
        public void SearchMoviesByTitle_StringMovieToSearchIsEmpty_ArgumentExceptionThrown()
        {
            // Arrange
            var movieToSearch = string.Empty;

            // Act & Assert
            ArgumentException ex = Assert.ThrowsAsync<ArgumentException>(() => _target.SearchMoviesByTitle(movieToSearch));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Search string not provided.");
        }

        [Test]
        public void SearchMoviesByTitle_StringMovieToSearchIsNull_ArgumentExceptionThrown()
        {
            // Act & Assert
            ArgumentException ex = Assert.ThrowsAsync<ArgumentException>(() => _target.SearchMoviesByTitle(null));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Search string not provided.");
        }

        [Test]
        public void SearchMoviesByTitle_BadRequest_HttpRequestExceptionThrown()
        {
            // Arrange
            string movieToSearch = "Test string";

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
            _target = new MovieSearchLogic(_clientMock.Object, _historyLogicMock.Object);

            // Act & Assert
            HttpRequestException ex =
                Assert.ThrowsAsync<HttpRequestException>(() => _target.SearchMoviesByTitle(movieToSearch));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Network connection problem.");
        }

        [Test]
        public void SearchMoviesByTitle_StringMovieToSearchIsImpossibleTitle_MovieListSearchExceptionThrown()
        {
            // Arrange
            string movieToSearch = "Test string";

            // Act & Assert
            MovieListSearchException ex = Assert.ThrowsAsync<MovieListSearchException>(() => _target.SearchMoviesByTitle(movieToSearch));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Movie not found!");
        }

        [Test]
        public void SearchMoviesByTitle_StringMovieToSearchIsTooShort_MovieListSearchExceptionThrown()
        {
            // Arrange
            string movieToSearch = "t";

            // Act & Assert
            MovieListSearchException ex = Assert.ThrowsAsync<MovieListSearchException>(() => _target.SearchMoviesByTitle(movieToSearch));
            ex.Should().NotBeNull();
            ex.Message.Should().Be("Too many results.");
        }

        [Test]
        public void SearchMoviesByTitle_StringMovieToSearchIsCorrect_ResponseSuccessful()
        {
            // Arrange
            string movieToSearch = "Star Wars";

            // Act
            var response = _target.SearchMoviesByTitle(movieToSearch);

            // Assert
            response.Result.Response.Should().Be("True");
            response.Result.Movies.Should().NotBeEmpty();
            response.Result.Error.Should().BeNull();
        }
    }
}
