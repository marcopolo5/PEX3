using NUnit.Framework;
using Domain.Models;
using System;
using System.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using Domain.Repositories;

namespace DAL.Tests
{
    [TestFixture]
    public class UserRepositoryTest
    {
        private UserRepository UserRepositoryTestObject;

        [SetUp]
        public void Setup()
        {
            UserRepositoryTestObject = new("Users", @"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB_Test;Integrated Security=True");
        }

        [TearDown]
        public void ClearDatabase()
        {
            using(var connection = new SqlConnection(@"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB_Test;Integrated Security=True"))
            {
                connection.Query(@"delete from Users");
                return;
            }
        }


        [Test]
        public void CreateAsync_ValidCall()
        {
            //SETUP
            var testuser1_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "testfirstname1",
                LastName = "testlastname1",
                Password = "testpassword1",
                Email = "testmail1@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };
            var testuser2_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result + 1,
                FirstName = "testfirstname2",
                LastName = "testlastname2",
                Password = "testpassword2",
                Email = "testmail2@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token="0"
            };
            var testuser3_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result + 2,
                FirstName = "testfirstname3",
                LastName = "testlastname3",
                Password = "testpassword3",
                Email = "testmail3@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate=DateTime.Now,
                Token="0"
            };

            //ACT
            UserRepositoryTestObject.CreateAsync(testuser1_valid).Wait();
            UserRepositoryTestObject.CreateAsync(testuser2_valid).Wait();
            UserRepositoryTestObject.CreateAsync(testuser3_valid).Wait();
            
            //ASSERT
            Assert.Pass();
        }

        [Test]
        public void CreateAsync_InvalidCall()
        {
            //SETUP
            var testuser1_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "testfirstname1",
                LastName = "testlastname1",
                Password = "testpassword1",
                Email = "samemail@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };
            var testuser2_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result + 1,
                FirstName = "testfirstname2",
                LastName = "testlastname2",
                Password = "testpassword2",
                Email = "samemail@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            //ACT
            UserRepositoryTestObject.CreateAsync(testuser1_valid).Wait();

            //ASSERT
            Assert.Throws<AggregateException>(() => UserRepositoryTestObject.CreateAsync(testuser2_valid).Wait());
        }
        
        [Test]
        public void ReadAsync_ValidCall()
        {
            //SETUP
            var testuser5_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "testfirstname5",
                LastName = "testlastname5",
                Password = "testpassword5",
                Email = "testmail5@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            //ACT
            UserRepositoryTestObject.CreateAsync(testuser5_valid).Wait();
            var testuser5_read = UserRepositoryTestObject.ReadAsync(testuser5_valid.Id).Result;


            //ASSERT            
            Assert.AreEqual(testuser5_read.Id, testuser5_valid.Id);
            Assert.AreEqual(testuser5_read.FirstName, testuser5_valid.FirstName);
            Assert.AreEqual(testuser5_read.LastName, testuser5_valid.LastName);
            Assert.AreEqual(testuser5_read.Password, testuser5_valid.Password);
            Assert.AreEqual(testuser5_read.Email, testuser5_valid.Email);
            Assert.AreEqual(testuser5_read.Verified, testuser5_valid.Verified);
            Assert.IsTrue(testuser5_read.JoinDate.ToShortDateString().Equals(testuser5_valid.JoinDate.ToShortDateString()));
            Assert.IsTrue(testuser5_read.LastUpdate.ToShortDateString().Equals(testuser5_valid.LastUpdate.ToShortDateString()));
            Assert.AreEqual(testuser5_read.Token, testuser5_valid.Token);
        }

        [Test]
        public void ReadAsync_InvalidCall()
        {
            //SETUP
            var testuser5_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "testfirstname5",
                LastName = "testlastname5",
                Password = "testpassword5",
                Email = "testmail5@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            //ACT
            UserRepositoryTestObject.CreateAsync(testuser5_valid).Wait();

            //ASSERT            
            Assert.Throws<AggregateException>(() => UserRepositoryTestObject.ReadAsync(testuser5_valid.Id + 1).Wait());
        }

        [Test]
        public void ReadAllAsync_ValidCall()
        {
            //SETUP
            var testuser6_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "testfirstname6",
                LastName = "testlastname6",
                Password = "testpassword6",
                Email = "testmail6@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };
            var testuser7_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result +1,
                FirstName = "testfirstname7",
                LastName = "testlastname7",
                Password = "testpassword7",
                Email = "testmail7@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            //ACT
            UserRepositoryTestObject.CreateAsync(testuser6_valid).Wait();
            UserRepositoryTestObject.CreateAsync(testuser7_valid).Wait();
            var users = (List<User>)UserRepositoryTestObject.ReadAllAsync().Result;

            //ASSERT
            Assert.AreEqual(testuser6_valid.Id, users[0].Id);
            Assert.AreEqual(testuser6_valid.FirstName, users[0].FirstName);
            Assert.AreEqual(testuser6_valid.LastName, users[0].LastName);
            Assert.AreEqual(testuser6_valid.Password, users[0].Password);
            Assert.AreEqual(testuser6_valid.Email, users[0].Email);
            Assert.AreEqual(testuser6_valid.Verified, users[0].Verified);
            Assert.IsTrue(testuser6_valid.JoinDate.ToShortDateString().Equals(users[0].JoinDate.ToShortDateString()));
            Assert.IsTrue(testuser6_valid.LastUpdate.ToShortDateString().Equals(users[0].LastUpdate.ToShortDateString()));
            Assert.AreEqual(testuser6_valid.Token, users[0].Token);

            Assert.AreEqual(testuser7_valid.Id, users[1].Id);
            Assert.AreEqual(testuser7_valid.FirstName, users[1].FirstName);
            Assert.AreEqual(testuser7_valid.LastName, users[1].LastName);
            Assert.AreEqual(testuser7_valid.Password, users[1].Password);
            Assert.AreEqual(testuser7_valid.Email, users[1].Email);
            Assert.AreEqual(testuser7_valid.Verified, users[1].Verified);
            Assert.IsTrue(testuser7_valid.JoinDate.ToShortDateString().Equals(users[1].JoinDate.ToShortDateString()));
            Assert.IsTrue(testuser7_valid.LastUpdate.ToShortDateString().Equals(users[1].LastUpdate.ToShortDateString()));
            Assert.AreEqual(testuser7_valid.Token, users[1].Token);
        }

        [Test]
        public void Update_ValidCall()
        {
            //SETUP
            var testuser8_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "testfirstname8",
                LastName = "testlastname8",
                Password = "testpassword8",
                Email = "testmail8@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            var testuser8_updated = new User
            {
                Id = testuser8_valid.Id,
                FirstName = "testfirstname8_up",
                LastName = "testlastname8_up",
                Password = "testpassword8_up",
                Email = "testmail8_up@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            }; 

            //ACT
            UserRepositoryTestObject.CreateAsync(testuser8_valid).Wait();
            UserRepositoryTestObject.UpdateAsync(testuser8_updated).Wait();
            var testuser8_updated_read = UserRepositoryTestObject.ReadAsync(testuser8_valid.Id).Result;

            //ASSERT
            Assert.AreEqual(testuser8_updated.Id, testuser8_updated_read.Id);
            Assert.AreEqual(testuser8_updated.FirstName, testuser8_updated_read.FirstName);
            Assert.AreEqual(testuser8_updated.LastName, testuser8_updated_read.LastName);
            Assert.AreEqual(testuser8_updated.Password, testuser8_updated_read.Password);
            Assert.AreEqual(testuser8_updated.Email, testuser8_updated_read.Email);
            Assert.AreEqual(testuser8_updated.Verified, testuser8_updated_read.Verified);
            Assert.IsTrue(testuser8_updated.JoinDate.ToShortDateString().Equals(testuser8_updated_read.JoinDate.ToShortDateString()));
            Assert.IsTrue(testuser8_updated.LastUpdate.ToShortDateString().Equals(testuser8_updated_read.LastUpdate.ToShortDateString()));
            Assert.AreEqual(testuser8_updated.Token, testuser8_updated_read.Token);
        }

        [Test]
        public void Delete_ValidCall()
        {
            //SETUP
            var testuser9_valid = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "testfirstname9",
                LastName = "testlastname9",
                Password = "testpassword9",
                Email = "testmail9@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            //ACT
            UserRepositoryTestObject.CreateAsync(testuser9_valid).Wait();
            UserRepositoryTestObject.DeleteAsync(testuser9_valid.Id).Wait();
            var users = UserRepositoryTestObject.ReadAllAsync().Result;

            //ASSERT
            Assert.IsEmpty(users);
        }
    }
}