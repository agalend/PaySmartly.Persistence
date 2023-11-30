using PaySmartly.Persistance.Mongo;
using PaySmartly.Persistance.Repository;
using PaySmartly.Persistance.Services;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.Configure<BookStoreDatabaseSettings>(builder.Configuration.GetSection("BookStoreDatabase"));
builder.Services.AddSingleton<IRepository<MongoRecord>, MongoRepository>();
builder.Services.AddGrpc();

var app = builder.Build();
app.MapGrpcService<PersistanceService>();

app.Run();
