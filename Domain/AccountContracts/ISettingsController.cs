using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AccountContracts
{
    public interface ISettingsController
    {
        void SetProximityRadius(int proximityRadius);

        void SetAnonimity(bool anonimity);

        Task ChangePassword(string newPassword);
    }
}
