using ModelLayer.ModelRequest;
using ModelLayer.ModelResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.ServiceInterfaces
{
    public interface IReservationService
    {
        Task<List<TimeListResponse>> GetTimeList(GetTimeList getTimeList);
        Task<bool> MakeReservation(ReservationRequest reservationRequest);
    }
}
