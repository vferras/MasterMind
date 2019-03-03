using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Xunit;

namespace Lp.Payments.EndToEndTest
{
    public class EndToEndTest
    {
        private static HttpClient _client;
        private static List<int> coloursPattern = new List<int> { 0, 1, 2, 3 };

        public EndToEndTest()
        {
            _client = GetHttpClient();
        }

        [Fact]
        public async Task Initialize_Board_And_Guess_Pattern_At_Last_Try()
        {
            await Initialize();

            await CheckPattern(new List<int> { 0, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 1, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 2, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 3, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 3, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 2, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 0, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 2, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 3, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 0, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int>(coloursPattern), HttpStatusCode.OK);

            await CheckState(BoardState.Discovered);

            await CheckHistoric(12);
        }

        [Fact]
        public async Task Initialize_Board_And_GameOver()
        {
            await Initialize();

            await CheckPattern(new List<int> { 0, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 1, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 2, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 3, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 3, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 2, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 0, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 2, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 3, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 0, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 0, 1, 1, 1 }, HttpStatusCode.OK);

            await CheckState(BoardState.GameOver);

            await CheckHistoric(12);
        }

        [Fact]
        public async Task Initialize_Board_And_Finish()
        {
            await Initialize();

            await CheckPattern(new List<int> { 0, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 1, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 2, 1, 1, 0 }, HttpStatusCode.OK);
            await CheckPattern(new List<int> { 3, 1, 1, 0 }, HttpStatusCode.OK);

            await Finish();

            await CheckPattern(new List<int> { 3, 1, 1, 0 }, HttpStatusCode.BadRequest);

            await CheckState(BoardState.FinishedByUser);

            await CheckHistoric(4);
        }

        private static HttpClient GetHttpClient()
        {
            var serviceCollection = new ServiceCollection();
            var retry = Policy<HttpResponseMessage>
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, _ => TimeSpan.FromSeconds(5));
            var registry = serviceCollection.AddPolicyRegistry();
            registry.Add("retry", retry);

            serviceCollection.AddHttpClient("test").AddPolicyHandlerFromRegistry("retry");

            var client = ((IHttpClientFactory)serviceCollection.BuildServiceProvider().GetService(typeof(IHttpClientFactory))).CreateClient("test");
            client.BaseAddress = new Uri("http://api");
            return client;

        }

        private async Task Initialize()
        {
            var initializeResult = await _client.PostAsJsonAsync("board/initialize", new { Colours = coloursPattern });
            initializeResult.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        private async Task CheckPattern(List<int> colours, HttpStatusCode httpStatusCode)
        {
            var result = await _client.PostAsJsonAsync("/board/pattern/check", new { Colours = colours });
            result.StatusCode.Should().Be(httpStatusCode);
        }

        private async Task CheckState(BoardState state)
        {
            var stateResult = await _client.GetAsync("/board/state");
            stateResult.StatusCode.Should().Be(HttpStatusCode.OK);
            (await stateResult.Content.ReadAsAsync<BoardState>()).Should().Be(state);
        }

        private async Task CheckHistoric(int numberOfChecksStored)
        {
            var stateResult = await _client.GetAsync("/board/historic");
            stateResult.StatusCode.Should().Be(HttpStatusCode.OK);
            (await stateResult.Content.ReadAsAsync<List<object>>()).Count().Should().Be(numberOfChecksStored);
        }

        private async Task Finish()
        {
            var stateResult = await _client.PostAsJsonAsync("/board/finish", new { });
            stateResult.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private enum BoardState
        {
            Initialized,
            Discovered,
            GameOver,
            FinishedByUser
        }
    }
}