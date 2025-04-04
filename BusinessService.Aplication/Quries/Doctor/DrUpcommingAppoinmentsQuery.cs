using BusinessService.Aplication.Common.DTOs.Appoinment;
using MediatR;


namespace BusinessService.Aplication.Quries.Doctor
{
    public record DrUpcommingAppoinmentsQuery(Guid id, int pageNumber, int pageSize) : IRequest<AppoinmentPaginationResDto>;
   
}
