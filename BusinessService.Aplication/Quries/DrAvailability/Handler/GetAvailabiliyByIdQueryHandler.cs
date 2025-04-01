using BusinessService.Aplication.Common.DTOs.Availability;
using BusinessService.Aplication.Interfaces.IRepos;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Quries.DrAvailability.Handler
{
    public class GetAvailabiliyByIdQueryHandler : IRequestHandler<GetAvailabiliyByIdQuery, AvailabilityByIdResDto>
    {
        private readonly IDrAvailabilityRepo _repo;
        public GetAvailabiliyByIdQueryHandler(IDrAvailabilityRepo repo)
        {
            _repo = repo;
        }

        public async Task<AvailabilityByIdResDto> Handle(GetAvailabiliyByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var slot =await _repo.GetBySlotId(request.slotid);
                var res = new AvailabilityByIdResDto()
                {
                    Id = slot.Id,
                    IsAvailable = slot.IsAvailable,
                    AppointmentDate = slot.AppointmentDate,
                    AppointmentTime = DateTime.Today.Add(slot.AppointmentTime).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                };

                return res;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
