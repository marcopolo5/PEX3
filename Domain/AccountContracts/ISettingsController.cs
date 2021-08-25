using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AccountContracts
{
    public interface ISettingsController
    {
        Task<bool> SetProximityRadius(int proximityRadius);

        Task<bool> SetAnonimity(bool anonimity);

        Task<bool> ChangePassword(string newPassword);
    }
}
