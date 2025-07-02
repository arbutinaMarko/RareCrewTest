using System.Text.Json;
using CSharp.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CSharp.Services
{
    public class TimeEntryService
    {
        private readonly HttpClient _httpClient;
        private readonly string _fullApiUrl;

        private readonly IMemoryCache _cache;

        public TimeEntryService(HttpClient httpClient, IConfiguration config, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;

            var baseUrl = config["RareCrewApi:BaseUrl"];
            var apiKey = config["RareCrewApi:ApiKey"];
            _fullApiUrl = $"{baseUrl}{apiKey}";
        }

        public async Task<List<TimeEntry>> GetTimeEntriesAsync()
        {
            const string cacheKey = "TimeEntries";

            if (_cache.TryGetValue(cacheKey, out List<TimeEntry>? cached) && cached != null)
            {
                return cached;
            }

            var response = await _httpClient.GetAsync(_fullApiUrl);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var rawEntries = JsonSerializer.Deserialize<List<RawTimeEntry>>(json, options);

            var entries = rawEntries?
                .Where(e => e.DeletedOn == null && e.EndTimeUtc > e.StarTimeUtc)
                .GroupBy(e => e.EmployeeName)
                .Select(g => new TimeEntry
                {
                    Employee = g.Key ?? "Unnamed Employees",
                    TimeWorked = (int)g.Sum(e => (e.EndTimeUtc - e.StarTimeUtc).TotalHours)
                })
                .OrderByDescending(e => e.TimeWorked)
                .ToList();

            _cache.Set(cacheKey, entries, TimeSpan.FromMinutes(5));

            return entries ?? new List<TimeEntry>();
        }
    }
}
