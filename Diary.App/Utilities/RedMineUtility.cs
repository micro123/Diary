using System;
using Diary.App.Models;
using Flurl.Http;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Diary.App.Utilities;

public class RedMineUtility
{
    public static async Task<bool> UpdateCookie(string password)
    {
        var cfg = AppSettings.GetConfig();

        var web = new HtmlWeb();
        var url = $"{cfg.RedMineHost}:{cfg.RedMinePort}/login";
        var html = web.Load(url, "GET");

        var node = html.DocumentNode.SelectSingleNode("//div[@id='login-form']/form/input[@name='authenticity_token']");
        var token = node.GetAttributeValue("value", string.Empty);
        if (string.IsNullOrEmpty(token))
        {
            // TODO
            return false;
        }

        var content = new FormUrlEncodedContent(
            new[]
            {
                new KeyValuePair<string, string>("authenticity_token", token),
                new KeyValuePair<string, string>("username", cfg.RedMineUser),
                new KeyValuePair<string, string>("password", password),
            });
        try
        {
            var response = await url.AllowAnyHttpStatus().WithTimeout(3).PostAsync(content);

            var hasCookies = response.Headers.Contains("Set-Cookies");
            if (hasCookies)
            {
                var cookieText = "";
                response.Headers.TryGetFirst("Set-Cookies", out cookieText);
                int a = 0;
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
        }

        return true;
    }
}