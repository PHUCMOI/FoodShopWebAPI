using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DataAccessInterfaces
{
    public interface IReservationDAO
    {
        Task<List<Reservation>> GetListReservation();
        Task<bool> Create(Reservation resservation);
        Task<bool> Update(Reservation reservation);
        Task<bool> Delete(int reservationId);
        Task<List<Reservation>> GetListReservationByDate(string reservationDate);
        Task<bool> MakeReservation(Reservation reservation);
    }
}
