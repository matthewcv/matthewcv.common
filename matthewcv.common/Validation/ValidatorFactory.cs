using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ninject;
using Ninject.Parameters;
using Ninject.Planning.Bindings;

namespace matthewcv.common.Validation
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        private readonly IKernel _kernel;

        public ValidatorFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            if (((IList<IBinding>)_kernel.GetBindings(validatorType)).Count == 0)
            {
                return null;
            }

            return _kernel.Get(validatorType) as IValidator;
        }
    }
}
