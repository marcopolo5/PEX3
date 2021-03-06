using AccountModule.Controllers;
using SignalRClientModule;
using Domain;
using Domain.Helpers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UI.WPF.ViewModel;

namespace UI.WPF.View
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ProximityChatControl : UserControl, IDisposable
    {
        private readonly SignalRClient _signalRClient = SignalRClient.GetInstance();
        private readonly UserRatingController _userRatingController= new();
        

        public ObservableCollection<ConversationPreviewViewModel> ConversationPreviews { get; private set; }
        public ObservableCollection<MessageViewModel> Messages { get; private set; }
        public ProximityChatControl()
        {
            ConversationPreviews = new();
            Messages = new();
            // hotfix for RateUpButton and RateDownButton (sometimes content wasnt loaded):
            var dummyMessageViewModel = new MessageViewModel(); 
            Messages.Add(dummyMessageViewModel);
            Messages.Remove(dummyMessageViewModel);

            // wire up events
            _signalRClient.MessageReceived += OnMessageReceived;
            _signalRClient.ConversationsReceived += OnConversationsReceived;
            InitializeComponent();

            // add the item source for our list (only after InitializeComponent() is called)
            ProximityConversationList.ItemsSource = ConversationPreviews;
            
            // add the item source for the current conversation
            ChatProximityConversation.ItemsSource = Messages;
        }


        private void OnMessageReceived(Message message)
        {
            var conversationPreview = ConversationPreviews.FirstOrDefault(c => c.ConversationId == message.ConversationId);
            if (conversationPreview == null)
            {
                return;
            }
            //update the conversation preview:
            conversationPreview.LastMessage = message.TextMessage;
            if (conversationPreview.ConversationId != ApplicationUserController.CurrentUser.CurrentConversationId)
            {
                conversationPreview.UnreadMessage = true;
            }

            var messageDto = new MessageViewModel
            {
                Id = message.Id,
                IsSent = ApplicationUserController.CurrentUser.Id == message.SenderId ? true : false,
                TextMessage = message.TextMessage,
                DisplayName = message.UsersDisplayName
            };

            if (ApplicationUserController.CurrentUser.CurrentConversationId != 0)
            {
                Messages.Add(messageDto);
                ChatScrollViewer.UpdateLayout();
                ChatScrollViewer.ScrollToVerticalOffset(double.MaxValue);
            }
        }
        private void OnConversationsReceived(IEnumerable<Conversation> conversations)
        {
            foreach (var conversation in conversations)
            {
                var conversationPreview = GetPreviewFromConversation(conversation);
                // ApplicationUserController.CurrentUser.Conversations.Add(conversation); //conversations are already added in SignalRClient class
                ConversationPreviews.Add(conversationPreview);
            }
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            await SendMessage();
        }

        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                await SendMessage();
            }
        }

        private async Task SendMessage()
        {
            var currentConversationId = ApplicationUserController.CurrentUser.CurrentConversationId;
            var text = MessageText.Text;
            MessageText.Clear();
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            await _signalRClient.SendMessageAsync(currentConversationId, text);
        }


        // use this to change between conversations
        private void ConversationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SendMessageBtn.IsEnabled == false)
            {
                SendMessageBtn.IsEnabled = true;
                MessageText.IsEnabled = true;
            }
            var item = (ConversationPreviewViewModel)ProximityConversationList.SelectedItem;
            var conversationPreview = ConversationPreviews.FirstOrDefault(cp => cp.ConversationId == item.ConversationId);
            conversationPreview.UnreadMessage = false;

            ConversationTitle.Text = item.ConversationName;
            ConversationStatus.Text = item.StatusMessage;

            Messages.Clear();
            ApplicationUserController.CurrentUser.CurrentConversationId = item.ConversationId;
            PopulateMessages(item.ConversationId);
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
            foreach (var message in conversation.Messages.OrderBy(m => m.CreatedAt))
            {
                var messageDto = new MessageViewModel
                {
                    Id = message.Id,
                    IsSent = ApplicationUserController.CurrentUser.Id == message.SenderId ? true : false,
                    TextMessage = message.TextMessage,
                    DisplayName = message.UsersDisplayName
                };

                Messages.Add(messageDto);
            }
        }

        /// <summary>
        /// Creates a ConversationPreviewDTO from a Conversation
        /// </summary>
        /// <param name="conversation">Conversation that needs to be mapped to a preview</param>
        /// <returns>The conversation preview</returns>
        private ConversationPreviewViewModel GetPreviewFromConversation(Conversation conversation)
        {
            string lastTextMessage;
            string conversationName;
            string statusMessage;
            var profilePictureArray = ImageHelper.GetImageBytes("../../../Assets/profile.png"); // maybe refactor this
            var userStatus = UserStatus.Away; /// momentan folosim away pt group chat

            // if the conversation is a group chat use its title as a conversation name
            if (conversation.Type == Domain.ConversationTypes.Group)
            {
                conversationName = conversation.Title;
                statusMessage = $"Number of participants: {conversation.Participants.Count()}";
            }
            // if the conversation is a private chat get the name of the other participant(friend) and use it as a conversation name
            else if (conversation.Type == Domain.ConversationTypes.Private)
            {
                var friend = conversation.Participants.FirstOrDefault(p => p.Id != ApplicationUserController.CurrentUser.Id);
                conversationName = friend.Profile.DisplayName;
                userStatus = friend.Profile.Status;
                statusMessage = friend.Profile.StatusMessage;
                profilePictureArray = friend.Profile.Image;

            }
            // if the conversation is a promiximity chat do nothing :)
            else
            {
                statusMessage = "ProximityChat";
                conversationName = conversation.Title;
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
            var conversationPreview = new ConversationPreviewViewModel
            {
                ConversationId = conversation.Id,
                ConversationName = conversationName,
                LastMessage = lastTextMessage,
                UserStatus = userStatus,
                StatusMessage = statusMessage,
                AccountProfilePictureArray = profilePictureArray
            };

            return conversationPreview;
        }


        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await _signalRClient.UpdateProximityChats();

        }

        private async void AddNewProximityConversation_Click(object sender, RoutedEventArgs e)
        {
            
            var title = CreateProximityChatName.Text;
            if (string.IsNullOrEmpty(title) || string.IsNullOrWhiteSpace(title) || title == "")
            {
                return;
            }
            CreateProximityChatName.Text = string.Empty; // reset the box
            await _signalRClient.CreateProximityConversationAsync(title);
            await _signalRClient.UpdateProximityChats();
        }

        private async void RateUpButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var id = (int)btn.Content;
            await _userRatingController.RateUser(true, id);
            btn.IsEnabled = false;
            //MessageBox.Show($"id : {id}"); //debugging 
        }
        private async void RateDownButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var id = (int)btn.Content;
            await _userRatingController.RateUser(false, id);
            btn.IsEnabled = false;
            //MessageBox.Show($"id : {id}");
        }
        private void ChatScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
        public void Dispose()
        {
            _signalRClient.MessageReceived -= OnMessageReceived;
            _signalRClient.ConversationsReceived -= OnConversationsReceived;
        }
    }
}
