using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AccountContracts
{
    public interface ISettingsController
    {
        Task<string> SaveChanges(string password, string newPassword, string newRetypedPassword);

        void SetAnonymity(bool anonimity);

        Task ChangePassword(string newPassword);

        void SetProximityRadius(int proximityRadius);
    }
}
