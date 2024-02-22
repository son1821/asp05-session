var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString = "Data Source=localhost,1433;Initial Catalog=webdb;User ID=SA;Password=Password123;Encrypt=False";

    options.SchemaName = "dbo";
    options.TableName = "Session";
});

//builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession((option) =>
{
    option.Cookie.Name = "son1821";
    option.IdleTimeout = new TimeSpan(0, 30, 0);
});

var app = builder.Build();
app.UseSession(); //SessionMiddleware - Cookie (ID Session)

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        int? count; //luu trong session voi key:count
        count = context.Session.GetInt32("count"); //doc session
        if (count == null)
        {
            count = 0;
        }
        await context.Response.WriteAsync($"So lan truy cap vao readwritesession: {count}");
    });
    endpoints.MapGet("/readwritesession",async context =>
    {
        int? count; //luu trong session voi key:count
        count =  context.Session.GetInt32("count"); //doc session
        if(count == null)
        {
            count = 0;
        }
        count += 1;
        context.Session.SetInt32("count",count.Value);//luu sesion
        //context.Session.SetString() - json (Newtonsoft.Json)
        await context.Response.WriteAsync($"So lan truy cap vao readwritesession: {count}");
        
    });
});



app.Run();
//dotnet sql-cache create "Data Source=localhost,1433;Initial Catalog=webdb;User ID=SA;Password=Password123;Encrypt=False" dbo Session