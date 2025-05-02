var builder = DistributedApplication.CreateBuilder(args);

var productapi = builder.AddProject<Projects.Ecommerce_ProductServices>("apiservice-product");
var orderapi = builder.AddProject<Projects.Ecommerce_OrderServices>("apiservice-order");

builder.AddProject<Projects.Ecommerce_Web>("webfrontend")
    .WithReference(productapi)
    .WithReference(orderapi);


builder.Build().Run();
