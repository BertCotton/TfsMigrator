﻿using TfsMigrator.Data;
using TfsMigrator.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TfsMigration.Infrastructure;

namespace TfsMigrator.ServiceRequests
{
    public class ProjectServiceRequest
    {
        private static string PROJECT_MEM_KEY = "Projects";
        private readonly AppSettings appSettings;
        private readonly IMemoryCache memoryCache;

        public ProjectServiceRequest(IOptions<AppSettings> appSettings, IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
            this.appSettings = appSettings.Value;
        }

        public Project GetProject(RequestData requestData, Repository repo)
        {
            var projectsResponse = requestData.HttpClient.GetStringAsync(repo.url).Result;
            return JsonConvert.DeserializeObject<Project>(projectsResponse);
        }

        public async Task<List<Project>> GetProjects(RequestData requestData)
        {
            List<Project> cachedProjects;
            memoryCache.TryGetValue(PROJECT_MEM_KEY, out cachedProjects);
            if (cachedProjects != null)
                return cachedProjects;

            var response = await requestData.HttpClient.GetStringAsync($"{requestData.BaseAddress}/_apis/projects?api-version=1.0");
            var responseObject = JsonConvert.DeserializeObject<Response<IEnumerable<Project>>>(response);

            var projects = responseObject.value.ToList();

            projects = projects.Where(p => appSettings.Projects.Contains(p.name)).ToList();
            memoryCache.Set(PROJECT_MEM_KEY, projects);

            return projects;
        }

        public Dictionary<string, KeyValuePair<Repository, Project>> GetProjects(RequestData requestData,
            IList<Repository> repos)
        {
            var projects = new Dictionary<string, KeyValuePair<Repository, Project>>(repos.Count);
            repos.ToList().ForEach(repository =>
            {
                projects.Add(repository.id,
                    new KeyValuePair<Repository, Project>(repository, GetProject(requestData, repository)));
            });

            return projects;
        }
    }
}