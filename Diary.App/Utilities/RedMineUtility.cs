using System;
using Diary.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Diary.App.Database;
using Diary.Core.Entities;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace Diary.App.Utilities;

public class RedMineUtility
{
    

    internal class RedMineActivitiesRes
    {
        internal class TimeEntryActivity
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("is_default")]
            public bool IsDefault { get; set; }

            [JsonProperty("active")]
            public bool Active { get; set; }
        }
        
        [JsonProperty("time_entry_activities")]
        public List<TimeEntryActivity> TimeEntryActivities { get; set; }
    }

    internal class Project
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    internal class Tracker
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    internal class Status
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    internal class Priority
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    internal class Author
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    internal class AssignedTo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    internal class Issue
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("project")]
        public Project Project { get; set; }

        [JsonProperty("tracker")]
        public Tracker Tracker { get; set; }

        [JsonProperty("status")]
        public Status Status { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("assigned_to")]
        public AssignedTo AssignedTo { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        [JsonProperty("due_date")]
        public object DueDate { get; set; }

        [JsonProperty("done_ratio")]
        public int DoneRatio { get; set; }

        [JsonProperty("is_private")]
        public bool IsPrivate { get; set; }

        [JsonProperty("estimated_hours")]
        public object EstimatedHours { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("updated_on")]
        public DateTime UpdatedOn { get; set; }

        [JsonProperty("closed_on")]
        public object ClosedOn { get; set; }
    }

    internal class IssueQueryResult
    {
        [JsonProperty("issues")]
        public List<Issue> Issues { get; set; }

        [JsonProperty("total_count")]
        public int TotalCount { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }

    public class DisplayIssue
    {
        public string ProjectName { get; set; }
        public string IssueName { get; set; }
        public int IssueId { get; set; }
        public string AssignedTo { get; set; }

        public string CreateDate { get; set; }
        public string IssueState { get; set; }
    }

    public static async Task<IEnumerable<DisplayIssue>> QueryIssues(string pattern, bool only_me, int page = 0, Action<int>? count_callback = null)
    {
        var cfg = AppSettings.GetConfig();
        if (string.IsNullOrEmpty(cfg.RedMineUserApiKey))
        {
            throw new ArgumentException("Invalid API KEY");
        }

        var url = $@"{cfg.RedMineHost}:{cfg.RedMinePort}/issues.json?page={page}&limit=10";
        if (!string.IsNullOrWhiteSpace(pattern) && !string.IsNullOrEmpty(pattern))
            url = $"{url}&subject=~{pattern}";
        if (only_me)
            url = $"{url}&assigned_to_id=me";
        var http = new RestClient();
        http.UseNewtonsoftJson();
        var res = await http.ExecuteAsync<IssueQueryResult>(
            new RestRequest(url, Method.GET)
                .AddHeader("X-RedMine-API-Key", cfg.RedMineUserApiKey)
        );
        if (res.StatusCode == HttpStatusCode.OK)
        {
            count_callback?.Invoke(res.Data.TotalCount);
            return res.Data.Issues.Select(issue => new DisplayIssue()
            {
                ProjectName = issue.Project.Name,
                IssueName = issue.Subject,
                IssueId = issue.Id,
                AssignedTo = issue.AssignedTo.Name,
                CreateDate = issue.CreatedOn.ToShortDateString(),
                IssueState = issue.Status.Name
            }).ToList();
        }

        return new DisplayIssue[] { };
    }

    public static async void SyncActivities(DiaryDbContext dbContext)
    {
        var cfg = AppSettings.GetConfig();
        if (string.IsNullOrEmpty(cfg.RedMineUserApiKey))
        {
            throw new ArgumentException("Invalid API KEY");
        }

        var url = $@"{cfg.RedMineHost}:{cfg.RedMinePort}/enumerations/time_entry_activities.json";

        var http = new RestClient();
        http.UseNewtonsoftJson();
        var res = http.Execute<RedMineActivitiesRes>(
            new RestRequest(url, Method.GET)
                .AddHeader("X-RedMine-API-Key", cfg.RedMineUserApiKey)
            );
        if (res.StatusCode == HttpStatusCode.OK)
        {
            foreach (var activity in res.Data.TimeEntryActivities)
            {
                try
                {
                    dbContext.RedMineActivities.Add(new RedMineActivity() {Id = activity.Id, Name = activity.Name});
                }
                catch (InvalidOperationException exception)
                {
                    // 已经存在的ID则进行更新
                    dbContext.RedMineActivities.First(x => x.Id == activity.Id).Name = activity.Name;
                }
            }
            await dbContext.SaveChangesAsync();
        }
    }
}