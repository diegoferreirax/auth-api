using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthApi.Infraestructure.Conventions;

public class DefaultPropertyTypeConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
            IConventionModelBuilder modelBuilder,
            IConventionContext<IConventionModelBuilder> context)
    {
        foreach (IMutableEntityType entityType in modelBuilder.Metadata.GetEntityTypes())
        {
            foreach (IMutableProperty property in entityType.GetProperties())
            {
                if (property.GetColumnType() == null && property.ClrType == typeof(string))
                {
                    property.SetColumnType("VARCHAR(150)");
                }
            }
        }
    }
}