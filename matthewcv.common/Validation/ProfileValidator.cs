using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using matthewcv.common.Entity;

namespace matthewcv.common.Validation
{
    public class ProfileValidator:AbstractValidator<Profile>
    {
        public ProfileValidator()
        {


            RuleFor(p => p.DisplayName).NotEmpty();
            RuleFor(p => p.EmailAddress).EmailAddress();
        }
    }
}
