using System;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.IO;
using Microsoft.Extensions.Configuration;


public class RepositoryTestBase : IDisposable
{
    protected readonly IDbConnection _connection;
    protected readonly IConfiguration Configuration;
    private readonly DatabaseHelper _databaseHelper;

    public RepositoryTestBase(DatabaseHelper databaseHelper)
    {

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Test.json")
            .Build();

        _databaseHelper = new DatabaseHelper(Configuration);
        _connection = _databaseHelper.CreateConnection();
        _connection.Open();

        ResetDatabase();
        CreateTables();
    }

    private void ResetDatabase()
    {
        var sql = @"
            SET FOREIGN_KEY_CHECKS = 0;
            DROP TABLE IF EXISTS report;
            DROP TABLE IF EXISTS device;
            DROP TABLE IF EXISTS user;
            DROP TABLE IF EXISTS business;
            SET FOREIGN_KEY_CHECKS = 1;";
        _connection.Execute(sql);
    }

    private void CreateTables()
    {
        var sql = @"
            CREATE TABLE IF NOT EXISTS business (
            BusinessId INT AUTO_INCREMENT PRIMARY KEY,
            Name VARCHAR(255) NOT NULL,
            Address VARCHAR(255) NOT NULL,
            Email VARCHAR(255),
            PhoneNumber VARCHAR(20) UNIQUE
        );

        CREATE TABLE IF NOT EXISTS user (
            UserId INT AUTO_INCREMENT PRIMARY KEY,
            BusinessId INT,
            Email VARCHAR(255) NOT NULL UNIQUE,
            PasswordHash VARCHAR(255) NOT NULL,
            CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            FOREIGN KEY (BusinessId) REFERENCES business(BusinessId) ON DELETE SET NULL
        );


        CREATE TABLE IF NOT EXISTS device (
            DeviceId INT AUTO_INCREMENT PRIMARY KEY,
            BusinessId INT,
            Make VARCHAR(50) NOT NULL, 
            Model VARCHAR(50) NOT NULL, 
            SerialNumber VARCHAR(100) UNIQUE NOT NULL,
            FOREIGN KEY (BusinessId) REFERENCES business(BusinessId) ON DELETE SET NULL
        );

        CREATE TABLE IF NOT EXISTS report (
            ReportId INT AUTO_INCREMENT PRIMARY KEY,
            UserId INT,
            DeviceId INT,
            BusinessId INT,
            ErrorCode VARCHAR(20) NOT NULL, 
            Description TEXT NOT NULL,
            Severity TINYINT NOT NULL CHECK (severity BETWEEN 1 AND 4),
            CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            AttachmentUrl VARCHAR(500),
            FOREIGN KEY (UserId) REFERENCES user(UserId) ON DELETE SET NULL,
            FOREIGN KEY (DeviceId) REFERENCES device(DeviceId) ON DELETE SET NULL,
            FOREIGN KEY (BusinessId) REFERENCES business(BusinessId) ON DELETE SET NULL
        );";
        _connection.Execute(sql);
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
