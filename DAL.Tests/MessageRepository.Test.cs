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
    public class MessageRepositoryTest
    {
        private UserRepository UserRepositoryTestObject;
        private MessageRepository MessageRepositoryTestObject;
        private ConversationRepository ConversationRepositoryTestObject;
        private User user1, user2;
        private Conversation conversation;


        [SetUp]
        public void Setup()
        {
            UserRepositoryTestObject = new("Users", @"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB_Test;Integrated Security=True");
            MessageRepositoryTestObject = new("Messages", @"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB_Test;Integrated Security=True");
            ConversationRepositoryTestObject = new("Conversations",@"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB_Test;Integrated Security=True");
            
            user1 = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result,
                FirstName = "user1firstname",
                LastName = "user1lastname",
                Password = "user1password",
                Email = "user1mail@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            user2 = new User
            {
                Id = UserRepositoryTestObject.GetAvailableId().Result + 1,
                FirstName = "user2firstname",
                LastName = "user2lastname",
                Password = "user2password",
                Email = "user2mail@mail.com",
                JoinDate = DateTime.Now,
                LastUpdate = DateTime.Now,
                Token = "0"
            };

            conversation = new Conversation
            {
                Id = ConversationRepositoryTestObject.GetAvailableId().Result,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = "testconversation",
                Type = Domain.ConversationTypes.Private,
            };

            conversation.Participants.Add(user1);
            conversation.Participants.Add(user2);

            UserRepositoryTestObject.CreateAsync(user1).Wait();
            UserRepositoryTestObject.CreateAsync(user2).Wait();

            ConversationRepositoryTestObject.CreateAsync(conversation).Wait();
        }

        [TearDown]
        public void ClearDatabase()
        {
            using (var connection = new SqlConnection(@"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB_Test;Integrated Security=True"))
            {
                connection.Query(@"delete from Users");
                connection.Query(@"delete from Messages");
                connection.Query(@"delete from Conversations");
                connection.Query(@"delete from Group_Members");
                return;
            }
        }


        [Test]
        public void CreateAsync_ValidCall()
        {
            //SETUP
            var testmessage1_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result,
                SenderId = user1.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent1"
            };
            var testmessage2_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result + 1,
                SenderId = user2.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent2"
            };
            var testmessage3_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result + 2,
                SenderId = user2.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent3"
            };


            //ACT
            MessageRepositoryTestObject.CreateAsync(testmessage1_valid).Wait();
            MessageRepositoryTestObject.CreateAsync(testmessage2_valid).Wait();
            MessageRepositoryTestObject.CreateAsync(testmessage3_valid).Wait();

            //ASSERT
            Assert.Pass();
        }

        [Test]
        public void CreateAsync_InvalidCall()
        {
            //SETUP
            var testmessage4_invalid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result,
                SenderId = -1,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent4"
            };
            var testmessage5_invalid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result+1,
                SenderId = user1.Id,
                ConversationId = -1,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent5"
            };


            //ACT

            //ASSERT
            Assert.Throws<AggregateException>(() => MessageRepositoryTestObject.CreateAsync(testmessage4_invalid).Wait());
            Assert.Throws<AggregateException>(() => MessageRepositoryTestObject.CreateAsync(testmessage5_invalid).Wait());
        }

        [Test]
        public void ReadAsync_ValidCall()
        {
            //SETUP
            var testmessage7_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result,
                SenderId = user1.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent7"
            };

            //ACT
            MessageRepositoryTestObject.CreateAsync(testmessage7_valid).Wait();
            var testmessage7_read = MessageRepositoryTestObject.ReadAsync(testmessage7_valid.Id).Result;

            //ASSERT
            Assert.AreEqual(testmessage7_valid.Id, testmessage7_read.Id);
            Assert.AreEqual(testmessage7_valid.SenderId, testmessage7_read.SenderId);
            Assert.AreEqual(testmessage7_valid.ConversationId, testmessage7_read.ConversationId);
            Assert.IsTrue(testmessage7_valid.CreatedAt.ToShortDateString().Equals(testmessage7_read.CreatedAt.ToShortDateString()));
            Assert.AreEqual(testmessage7_valid.TextMessage, testmessage7_read.TextMessage);
        }

        [Test]
        public void ReadAsync_InvalidCall()
        {
            //SETUP
            var testmessage8_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result,
                SenderId = user1.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent8"
            };

            //ACT
            MessageRepositoryTestObject.CreateAsync(testmessage8_valid).Wait();

            //ASSERT
            Assert.Throws<AggregateException>(() => MessageRepositoryTestObject.ReadAsync(testmessage8_valid.Id + 1).Wait());
        }

        [Test]
        public void ReadAllAsync_ValidCall()
        {
            //SETUP
            var testmessage9_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result,
                SenderId = user1.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent9"
            };
            var testmessage10_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result + 1,
                SenderId = user2.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent10"
            };

            //ACT
            MessageRepositoryTestObject.CreateAsync(testmessage9_valid).Wait();
            MessageRepositoryTestObject.CreateAsync(testmessage10_valid).Wait();
            var messages = (List<Message>)MessageRepositoryTestObject.ReadAllAsync().Result;

            //ASSERT
            Assert.AreEqual(testmessage9_valid.Id, messages[0].Id);
            Assert.AreEqual(testmessage9_valid.SenderId, messages[0].SenderId);
            Assert.AreEqual(testmessage9_valid.ConversationId, messages[0].ConversationId);
            Assert.IsTrue(testmessage9_valid.CreatedAt.ToShortDateString().Equals(messages[0].CreatedAt.ToShortDateString()));
            Assert.AreEqual(testmessage9_valid.TextMessage, messages[0].TextMessage);

            Assert.AreEqual(testmessage10_valid.Id, messages[1].Id);
            Assert.AreEqual(testmessage10_valid.SenderId, messages[1].SenderId);
            Assert.AreEqual(testmessage10_valid.ConversationId, messages[1].ConversationId);
            Assert.IsTrue(testmessage10_valid.CreatedAt.ToShortDateString().Equals(messages[1].CreatedAt.ToShortDateString()));
            Assert.AreEqual(testmessage10_valid.TextMessage, messages[1].TextMessage);
        }

        [Test]
        public void Update_ValidCall()
        {
            //SETUP
            var testmessage11_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result,
                SenderId = user1.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent11"
            };
            var testmessage11_updated = new Message
            {
                Id = testmessage11_valid.Id,
                SenderId = user2.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent11up"
            };

            //ACT
            MessageRepositoryTestObject.CreateAsync(testmessage11_valid).Wait();
            MessageRepositoryTestObject.UpdateAsync(testmessage11_updated).Wait();
            var testmessage11_updated_read = MessageRepositoryTestObject.ReadAsync(testmessage11_updated.Id).Result;

            //ASSERT
            Assert.AreEqual(testmessage11_updated_read.Id, testmessage11_updated.Id);
            Assert.AreEqual(testmessage11_updated_read.SenderId, testmessage11_updated.SenderId);
            Assert.AreEqual(testmessage11_updated_read.ConversationId, testmessage11_updated.ConversationId);
            Assert.IsTrue(testmessage11_updated_read.CreatedAt.ToShortDateString().Equals(testmessage11_updated.CreatedAt.ToShortDateString()));
            Assert.AreEqual(testmessage11_updated_read.TextMessage, testmessage11_updated.TextMessage);}

        [Test]
        public void Delete_ValidCall()
        {
            //SETUP
            var testmessage13_valid = new Message
            {
                Id = MessageRepositoryTestObject.GetAvailableId().Result,
                SenderId = user1.Id,
                ConversationId = conversation.Id,
                CreatedAt = DateTime.Now,
                TextMessage = "testmesasgecontent13"
            };

            //ACT
            MessageRepositoryTestObject.CreateAsync(testmessage13_valid).Wait();
            MessageRepositoryTestObject.DeleteAsync(testmessage13_valid.Id).Wait();
            var messages = MessageRepositoryTestObject.ReadAllAsync().Result;

            //ASSERT
            Assert.IsEmpty(messages);

        }
    }
}