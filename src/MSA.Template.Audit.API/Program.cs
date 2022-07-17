using Amazon.SQS;
using Ardalis.GuardClauses;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MSA.Template.Audit.API.AuditEventHandlers;
using MSA.Template.Audit.API.Configuration;
using MSA.Template.Audit.API.Data;
using System.IO.Compression;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => { options.LowercaseUrls = true; });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            // ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Please enter JWT with Bearer into field",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<AuditDbContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString(nameof(AuditDbContext)));
});
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<EntityPropertyModifiedAuditEventHandler>();

    configurator.UsingAmazonSqs((context, cfg) =>
    {
        var amazonSqsConfig = builder.Configuration.GetSection(nameof(AmazonSqsConfiguration))
            .Get<AmazonSqsConfiguration>();

        cfg.Host(amazonSqsConfig.RegionEndpointSystemName,
            h =>
            {
                h.AccessKey(amazonSqsConfig.AccessKey);
                h.SecretKey(amazonSqsConfig.SecretKey);

                h.Scope(builder.Environment.EnvironmentName, true);
            });

        Guard.Against.NullOrWhiteSpace(amazonSqsConfig.QueueName, nameof(amazonSqsConfig.QueueName));

        cfg.ReceiveEndpoint($"{builder.Environment.EnvironmentName}_{amazonSqsConfig.QueueName}",
            e =>
            {
                e.UseMessageRetry(r =>
                {
                    r.Interval(5, TimeSpan.FromMinutes(1));
                    r.Ignore<ArgumentNullException>();
                });

                e.ConfigureConsumers(context);

                e.QueueAttributes.Add(QueueAttributeName.VisibilityTimeout,
                    TimeSpan.FromMinutes(20).TotalSeconds);
                e.QueueAttributes.Add(QueueAttributeName.ReceiveMessageWaitTimeSeconds, 20);
                e.QueueAttributes.Add(QueueAttributeName.MessageRetentionPeriod,
                    TimeSpan.FromDays(10).TotalSeconds);
                e.WaitTimeSeconds = 20;
            });
    });
});

builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.EnableForHttps = true;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options => { options.Level = CompressionLevel.Fastest; });

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseResponseCompression();

app.MapHealthChecks("/");

await using (var scope = app.Services.CreateAsyncScope())
{
    scope.ServiceProvider.GetRequiredService<AuditDbContext>().Database.Migrate();
}

app.Run();