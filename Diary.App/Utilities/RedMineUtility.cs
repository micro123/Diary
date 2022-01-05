using System;
using Diary.App.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        var res = http.Execute<RedMineActivitiesRes>(new RestRequest(url, Method.GET).AddHeader("X-RedMine-API-Key", cfg.RedMineUserApiKey));
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