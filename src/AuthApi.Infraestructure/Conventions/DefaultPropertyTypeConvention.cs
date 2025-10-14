using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Infraestructure.Conventions;

public class DefaultPropertyTypeConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
            IConventionModelBuilder modelBuilder,
            IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.GetColumnType() == null && property.ClrType == typeof(string))
                {
                    property.SetColumnType("VARCHAR(150)");
                }
            }
        }
    }
}