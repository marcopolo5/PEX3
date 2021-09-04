using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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
            var userReport = await _strikesRepository.ReadUserReportAsync(ApplicationUserController.CurrentUser.Id, user.Id);
            if (userReport == null)
            {
                if (up)
                {
                    user.Profile.Reputation += 1;
                }
                else
                {
                    user.Profile.Reputation -= 1;
                    await ApplyStrikes(user);
                }
            }
            await _profileRepository.UpdateAsync(user.Profile);
        }


        private async Task ApplyStrikes(Domain.Models.User user)
        {
            var strikeobj = await _strikesRepository.ReadByUserId(user.Id);
            switch (user.Profile.Reputation)
            {
                case -10:
                    if (!strikeobj.FirstStrike)
                    {
                        user.Profile.Reputation += 5;
                        await _profileRepository.UpdateAsync(user.Profile);
                        strikeobj.FirstStrike = true;
                        strikeobj.UnbanDate = DateTime.Now.AddDays(5);
                        await _strikesRepository.UpdateAsync(strikeobj);
                    }
                    break;
                case -20:
                    if (!strikeobj.SecondStrike)
                    {
                        user.Profile.Reputation += 5;
                        await _profileRepository.UpdateAsync(user.Profile);
                        strikeobj.SecondStrike = true;
                        strikeobj.UnbanDate = DateTime.Now.AddDays(15);
                        await _strikesRepository.UpdateAsync(strikeobj);
                    }
                    break;
                case -50:
                    if (!strikeobj.ThirdStrike)
                    {
                        user.Profile.Reputation += 5;
                        await _profileRepository.UpdateAsync(user.Profile);
                        strikeobj.ThirdStrike = true;
                        strikeobj.UnbanDate = DateTime.Now.AddDays(29200);
                        await _strikesRepository.UpdateAsync(strikeobj);
                    }
                    break;
                default:
                    break;
            }
        }

    }
}
