using System;
using System.Collections.Generic;
using System.Text;
using Amazon.SecretsManager;
using Earthquake.Emergency.Domain.Entities.Emergency;
using Microsoft.EntityFrameworkCore;

namespace Earthquake.Emergency.Contexts
{
    public class ApplicationContext : DbContext
    {

        protected async override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(await GetConnectionString());
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        private async Task<string> GetConnectionString()
        {
            var client = new AmazonSecretsManagerClient();
            var connectionString = await client.GetSecretValueAsync(new Amazon.SecretsManager.Model.GetSecretValueRequest
            {
                SecretId = "rds-connection-string"
            });

            return connectionString.SecretString;

        }

        public DbSet<EmergencyEntity> Emergencies;

    }
}

