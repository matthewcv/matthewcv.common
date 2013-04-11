using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using Ninject.Web.Common;

namespace matthewcv.common.Infrastructure
{
    public class KernelProvider
    {
        private static IKernel _kernel;
        public static IKernel Kernel { get { return _kernel ?? (_kernel = new Bootstrapper().Kernel); } }
    }
}
