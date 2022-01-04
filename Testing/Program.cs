using Flurl;
using Flurl.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;

var host = "http://172.16.26.12";
var port = 3000;
var path = "/login";

var user = "admin";
var password = "qwer1234";

var redMineMng = new RedmineManager($"{host}:{port}", apiKey: "2e0a00918fa104873afad67f9655b4547e1a02f2", MimeFormat.Json, false);
foreach (var proj in redMineMng.GetObjects<Redmine.Net.Api.Types.TimeEntryActivity>(RedmineKeys.ACTIVITY, RedmineKeys.ALL))
    Console.WriteLine($"{proj.Id}:{proj.Name}");
Console.WriteLine(redMineMng.GetCurrentUser().Login);

redMineMng.CreateObject(new Project()
{
    Identifier = "test_proj_07",
    Description = "测试项目7",
    Name = "测试项目7",
    IsPublic = true
});

// 获取登录所需信息
var url = $"{host}:{port}{path}";
var htmlRes = await url.GetAsync();
HtmlDocument doc = new HtmlDocument();
doc.LoadHtml(htmlRes.GetStringAsync().Result);

var cookie = htmlRes.Headers.GetAll("Set-Cookie").FirstOrDefault("Error!!");
Console.WriteLine(cookie);

var node = doc.DocumentNode.SelectSingleNode("//div[@id='login-form']/form/input[@name='authenticity_token']");
var token = node.GetAttributeValue("value", "Error!!");
Console.WriteLine(token);

var content = new FormUrlEncodedContent(new[]
{
    // new KeyValuePair<string, string>("utf8","✓"),
    new KeyValuePair<string, string>("authenticity_token",token),
    // new KeyValuePair<string, string>("back_url","http://192.168.123.170:3000/"),
    new KeyValuePair<string, string>("username",user),
    new KeyValuePair<string, string>("password",password),
    // new KeyValuePair<string, string>("login","登录"),
});

var loginRes = await url
    .AllowAnyHttpStatus()
    // .WithHeader("Accept","text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8")
    // .WithHeader("Content-Type","application/x-www-form-urlencoded")
    // .WithHeader("Accept-Encoding","gzip, deflate")
    // .WithHeader("Accept-Language","zh-CN,zh;q=0.8")
    // .WithHeader("Host","192.168.123.170:3000")
    // .WithHeader("Upgrade-Insecure-Requests","1")
    // .WithHeader("Referer", "http://192.168.123.170:3000/")
    // .WithHeader("Connection","Keep-Alive")
    // .WithHeader("User-Agent","Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36")
    .WithHeader("Cookie", cookie.Substring(0, cookie.IndexOf(';')))
    .PostAsync(content);
if (loginRes.StatusCode == 200)
{
    // test get my account
    var projectsRes = await $"{host}:{port}".AppendPathSegment("issues.json").SetQueryParam("project_id", "test_proj_01").WithHeader("Cookie", loginRes.Headers.FirstOrDefault("Set-Cookie")).GetAsync();
    if (projectsRes.StatusCode == 200)
    {
        var json = await projectsRes.GetStringAsync();
        dynamic obj = JsonConvert.DeserializeObject(json);
        Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
    }
}
else
{
    Console.WriteLine(loginRes.StatusCode);
}
