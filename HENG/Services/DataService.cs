﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HENG.Models;
using Newtonsoft.Json;

namespace HENG.Services
{
    /// <summary>
    /// https://github.com/shengqiangzhang/examples-of-web-crawlers
    /// </summary>
    public class DataService : IDataService
    {
        public async Task<IEnumerable<PaperItem>> GetNewestAsync( int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c68ffb9463b7fbfe72b0db0?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetHottestAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c69251c9b1c011c41bb97be?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetGirlsAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c81087e6aee28c541eefc26?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        public async Task<IEnumerable<PaperItem>> GetSkyAsync(int page = 1, int per_page = 20, CancellationToken cancellationToken = default(CancellationToken))
        {
            var url = $"https://service.paper.meiyuan.in/api/v2/columns/flow/5c81f64c96fad8fe211f5367?page={page}&per_page={per_page}";
            string json = await GetJsonAsync(url, cancellationToken);
            var items = JsonConvert.DeserializeObject<IEnumerable<PaperItem>>(json);
            return items;
        }

        private async Task<string> GetJsonAsync(string url, CancellationToken token)
        {
            HttpClientHandler header = new HttpClientHandler
            {
                AllowAutoRedirect = false
            };
            using (var client = new HttpClient(header))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    string json = string.Empty;

                    if (!token.IsCancellationRequested)
                    {
                        var response = await client.GetAsync(new Uri(url), token).ConfigureAwait(false);
                        response.EnsureSuccessStatusCode();
                        json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    }
                    return json;
                }
                catch (TaskCanceledException)
                {
                    return string.Empty;
                }
                catch (HttpRequestException)
                {
                    return string.Empty;
                }
            }
        }
    }
}
