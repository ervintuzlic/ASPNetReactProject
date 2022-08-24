using aspnetserver.Data;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.NET React Project", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(swaggerUiOptions =>
{
    swaggerUiOptions.DocumentTitle = "ASP.NET React Project";
    swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API serving a very simple Post model.");
    swaggerUiOptions.RoutePrefix = String.Empty;
});

app.UseHttpsRedirection();

app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");

app.MapGet("/get-post-by-id/{postId}", async (int postId) =>
 {
     Post postToReturn = await PostsRepository.GetPostByIdAsync(postId);

     if (postToReturn != null)
         return Results.Ok(postToReturn);
     else
         return Results.BadRequest();
 }).WithTags("Posts Endpoints");


app.MapPost("/create-post", async (Post postToCreate) =>
{
    bool createPost = await PostsRepository.createPostAsync(postToCreate);

    if (createPost)
        return Results.Ok("Create successful.");
    else
        return Results.BadRequest();
}).WithTags("Posts Endpoints");


app.MapPut("/update-post", async (Post postToUpdate) =>
{
    bool updatePost = await PostsRepository.updatePostAsync(postToUpdate);
    if (updatePost)
        return Results.Ok("Post updated successfully");
    else
        return Results.BadRequest();
}).WithTags("Posts Endpoints");

app.MapDelete("/delete-post-by-id/{postId}", async (int postId) =>
{
    bool postToDelete = await PostsRepository.deletePostAsync(postId);
    if (postToDelete)
        return Results.Ok("Post deleted successfully");
    else
        return Results.BadRequest();
}).WithTags("Posts Endpoints");

app.Run();
