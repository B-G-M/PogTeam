﻿using AuntificationAPI;
using Microsoft.EntityFrameworkCore;
using System;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseMySql("server=localhost;user=root;password=root;database=pong_users;",
            new MySqlServerVersion(new Version(8,0,28)));
    }
}