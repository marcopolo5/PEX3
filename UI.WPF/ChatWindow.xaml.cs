﻿using AccountModule.Controllers;
using ChatModule;
using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private readonly TextChat _textChat = new();
        public ObservableCollection<ConversationPreviewDTO> ConversationPreviews { get; private set; } = new();
        public ObservableCollection<MessageDTO> Messages { get; private set; } = new();

        public ChatWindow()
        {
            // get user's conversations
            var conversations = ApplicationUserController.CurrentUser.Conversations;

            // add conversations previews to ConversationPreviews colection
            foreach (var conversation in conversations)
            {
                var conversationPreview = GetPreviewFromConversation(conversation);
                ConversationPreviews.Add(conversationPreview);
            }

            _textChat.InitializeConnectionAsync(ApplicationUserController.CurrentUser.Id, ApplicationUserController.CurrentUser.Token)
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        MessageBox.Show(task.Exception.ToString());
                    }
                });
            // wire up event:
            _textChat.MessageReceived += OnMessageReceived;

            // initialize the window
            InitializeComponent();

            // add the item source for our list (only after InitializeComponent() is called)
            ConversationList.ItemsSource = ConversationPreviews;


            // add the item source for the current conversation
            ChatConversation.ItemsSource = Messages;

            // tests:
            //ConversationPreviews.Add(new ConversationPreviewDTO { LastMessage = "test", ConversationId = 1, ConversationName = "test" });
            //ConversationPreviews.Add(new ConversationPreviewDTO { LastMessage = "test", ConversationId = 1, ConversationName = "test1" });
            //ConversationPreviews.Add(new ConversationPreviewDTO { LastMessage = "test", ConversationId = 1, ConversationName = "test2" });
        }


        private void OnMessageReceived(Message message)
        {
            var conversation = ConversationPreviews.FirstOrDefault(c => c.ConversationId == message.ConversationId);
            if (conversation == null)
            {
                return;
            }
            conversation.LastMessage = message.TextMessage;

            var messageDto = new MessageDTO
            { 
                IsSent = ApplicationUserController.CurrentUser.Id == message.SenderId ? true : false,
                TextMessage = message.TextMessage 
            };
            if (ApplicationUserController.CurrentUser.CurrentConversationId != 0)
            {
                Messages.Add(messageDto);
                ChatScrollViewer.UpdateLayout();
                ChatScrollViewer.ScrollToVerticalOffset(double.MaxValue);
            }
        }

        /// <summary>
        /// Creates a ConversationPreviewDTO from a Conversation
        /// </summary>
        /// <param name="conversation">Conversation that needs to be mapped to a preview</param>
        /// <returns>The conversation preview</returns>
        private ConversationPreviewDTO GetPreviewFromConversation(Conversation conversation)
        {
            string lastTextMessage;
            string conversationName;

            // if the conversation is a group chat use its title as a conversation name
            if (conversation.Type == Domain.ConversationTypes.Group)
            {
                conversationName = conversation.Title;
            }
            // if the conversation is a private chat get the name of the other participant(friend) and use it as a conversation name
            else if (conversation.Type == Domain.ConversationTypes.Private)
            {
                var friend = conversation.Participants.FirstOrDefault(p => p.Id != ApplicationUserController.CurrentUser.Id);
                conversationName = friend.Profile.DisplayName;
            }
            // if the conversation is a promiximity chat do nothing :)
            else
            {
                conversationName = "";
            }


            // get the last message:
            var lastMessage = conversation.Messages.OrderByDescending(m => m.CreatedAt).FirstOrDefault();
            if (lastMessage == null)
            {
                lastTextMessage = "No recent message";
            }
            else
            {
                lastTextMessage = lastMessage.TextMessage;
            }

            // create the conversation preview
            var conversationPreview = new ConversationPreviewDTO
            {
                ConversationId = conversation.Id,
                ConversationName = conversationName,
                LastMessage = lastTextMessage
            };

            return conversationPreview;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Hide();
            new MainWindow().ShowDialog();
            ShowDialog();
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProfileWindow().ShowDialog();
            ShowDialog();
        }

        private void ListViewItem_Selected_2(object sender, RoutedEventArgs e)
        {
            Hide();
            new AddFriendsWindow().ShowDialog();
            ShowDialog();
        }
        private void ListViewItem_Selected_3(object sender, RoutedEventArgs e)
        {
            Hide();
            new ChatWindow().ShowDialog();
            ShowDialog();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            var currentConversationId = ApplicationUserController.CurrentUser.CurrentConversationId;
            var text = messageText.Text;
            messageText.Clear();
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            await _textChat.SendMessageAsync(currentConversationId, text);
        }

        private void addConversation_Click(object sender, RoutedEventArgs e)
        {
        }


        protected override void OnClosed(EventArgs e)
        {
            _textChat.MessageReceived -= OnMessageReceived;
            base.OnClosed(e);
        }


        // use this to change between convs
        private void ConversationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SendMessageBtn.IsEnabled == false)
            {
                SendMessageBtn.IsEnabled = true;
            }
            var item = (ConversationPreviewDTO)ConversationList.SelectedItem;
            ConversationTitle.Text = item.ConversationName;
            ConversationStatus.Text = item.ConversationName; /// TODO: de schimbat cu status
            Messages.Clear();
            ApplicationUserController.CurrentUser.CurrentConversationId = item.ConversationId;
            PopulateMessages(item.ConversationId);
            //FakePopulateMessages(item.ConversationId);
            ChatScrollViewer.UpdateLayout();
            ChatScrollViewer.ScrollToVerticalOffset(double.MaxValue);
        }

        private void PopulateMessages(int conversationId)
        {
            var conversation = ApplicationUserController.CurrentUser.Conversations.FirstOrDefault(c => c.Id == conversationId);
            if (conversation == null)
            {
                return; ///// throw some err instead of this
            }
            foreach(var message in conversation.Messages.OrderBy(m => m.CreatedAt))
            {
                var messageDto = new MessageDTO 
                { 
                    IsSent = ApplicationUserController.CurrentUser.Id == message.SenderId ? true : false,
                    TextMessage = message.TextMessage
                };

                Messages.Add(messageDto);
            }
        }

        private void FakePopulateMessages(int conversationId)
        {
            Messages.Add(new MessageDTO { IsSent = true, TextMessage = "Hello" });
            Messages.Add(new MessageDTO { IsSent = false, TextMessage = "How are you fam?" });
            Messages.Add(new MessageDTO { IsSent = true, TextMessage = "Im pretty good ngl, wut bout u boii?" });
            Messages.Add(new MessageDTO { IsSent = true, TextMessage = "how's the kids?" });
            Messages.Add(new MessageDTO { IsSent = false, TextMessage = "good good..yours??" });
            Messages.Add(new MessageDTO { IsSent = false, TextMessage = "dcfgvhjkl;'fwafwa??" });
            Messages.Add(new MessageDTO { IsSent = true, TextMessage = "waresthrdtjfykghulji;ko" });
        }
    }
}
