using AuthService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessService.Aplication.Commands.DrCommand
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;
    using static Contarcts.Common.GenderContarct;

    public record DrProfileUpdationCommand : IRequest<string>
    {
        public Guid? drId { get; set; }

        [RegularExpression(@"^\d{10,15}$", ErrorMessage = "Phone must contain only digits and be 10 to 15 characters long.")]
        public string? Phone { get; set; }

        [MaxLength(500, ErrorMessage = "About section cannot exceed 500 characters.")]
        public string? About { get; set; }

        public IFormFile? Profile { get; set; }

        [EnumDataType(typeof(GenderD), ErrorMessage = "Invalid gender selection.")]
        public GenderD? Gender { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Experience must be a positive number.")]
        public double? Field_experience { get; set; }

        [MaxLength(100, ErrorMessage = "Qualification cannot exceed 100 characters.")]
        public string? Qualification { get; set; }
    }

}
