﻿using Dapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    public class UserRepository : GenericRepository<User>
    {
        private readonly ProfileRepository ProfileRepository;

        public UserRepository() : base("Users") {
            ProfileRepository = new();
        }

        public new async Task CreateAsync(User user)
        {
            await base.CreateAsync(user);
            await ProfileRepository.CreateAsync(user.Profile);
        }

        public new async Task<IEnumerable<User>> ReadAllAsync()
        {
            var users = await base.ReadAllAsync();
            await BindUserProfilesAsync(users);
            return users;
        }

        public async Task BindUserProfilesAsync(IEnumerable<User> users) {
            var profiles = await ProfileRepository.ReadAllAsync();

            foreach (User user in users)
            {
                user.Profile = profiles.FirstOrDefault(profile => profile.User.Id == user.Id);
            }
        }

        public async Task<IEnumerable<User>> ReadAllAsync(string displayname)
        {
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User>($"SELECT {TableName}.* FROM {TableName} " +
                    $"INNER JOIN Profiles ON {TableName}.ID = Profiles.User_ID AND Profiles.Display_Name LIKE '%{displayname}%'");
                var profiles = await ProfileRepository.ReadAllAsync();

                foreach(User user in users)
                {
                    user.Profile = profiles.FirstOrDefault(profile => profile.User.Id == user.Id);
                }
                
                return users;
            }
            
        }

        public async Task<User> ReadAsync(string email)
        {
            using (var connection = CreateConnection())
            {
                var entity = await connection.QuerySingleOrDefaultAsync<User>($"SELECT * FROM {TableName} WHERE Email={email}");
                if (entity == null)
                    throw new KeyNotFoundException($"User with email {email} was not found.");
                return entity;
            }
        }

        /*
        User GetUser(object userId);
        IEnumerable<User> FindUsersByDisplayName(string displayName);
        IEnumerable<User> FindUsersByEmail(string email); //IEnumerable ???
        IEnumerable<User> GetAllUsers();
        */
    }
}