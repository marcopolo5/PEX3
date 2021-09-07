using System;
using System.Threading.Tasks;
using Domain.Repositories;


namespace AccountModule.Controllers
{
    public class UserRatingController
    {
        private readonly ProfileRepository _profileRepository = new();
        private readonly MessageRepository _messageRepository = new();
        private readonly UserRepository _userRepository = new();
        private readonly StrikesRepository _strikesRepository = new();


        public async Task RateUser(bool up, int messageid)
        {
            var message = await _messageRepository.ReadAsync(messageid);
            var user = await _userRepository.ReadAsync(message.SenderId);
            if (await _strikesRepository.ReadUserReportAsync(ApplicationUserController.CurrentUser.Id, user.Id) == null)
            {
                if (up)
                {
                    ++user.Profile.Reputation;
                }
                else
                {
                    --user.Profile.Reputation;
                    await ApplyStrikes(user);
                }
                await _profileRepository.UpdateAsync(user.Profile);
            }
        }


        private async Task ApplyStrikes(Domain.Models.User user)
        {
            var strikeobj = await _strikesRepository.ReadByUserId(user.Id);
            switch (user.Profile.Reputation)
            {
                case -10:
                case -20:
                case -50:
                    user.Profile.Reputation += 5;
                    if (!strikeobj.FirstStrike)
                    {
                        strikeobj.FirstStrike = true;
                        strikeobj.UnbanDate = DateTime.Now.AddDays(5);
                        await _strikesRepository.UpdateAsync(strikeobj);
                    }
                    if (!strikeobj.SecondStrike)
                    {
                        strikeobj.SecondStrike = true;
                        strikeobj.UnbanDate = DateTime.Now.AddDays(15);
                        await _strikesRepository.UpdateAsync(strikeobj);
                    }
                    if (!strikeobj.ThirdStrike)
                    {
                        strikeobj.ThirdStrike = true;
                        strikeobj.UnbanDate = DateTime.Now.AddDays(29200);
                        await _strikesRepository.UpdateAsync(strikeobj);
                    }
                    break;
            }
        }

    }
}
