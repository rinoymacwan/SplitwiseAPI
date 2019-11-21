using SplitwiseAPI.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SplitwiseAPI.Repository.ActivitiesRepository
{
    public interface IActivitiesRepository
    {
        IEnumerable<Activities> GetActivities();
        IEnumerable<Activities> GetActivitiesByUserId(string id);
        Task<Activities> GetActivity(int id);
        void CreateActivity(Activities Activity);
        void UpdateActivity(Activities Activity);
        Task DeleteActivity(Activities Activity);
        Task DeleteAllActivities(string id);
        Task Save();
        bool ActivityExists(int id);
    }
}
