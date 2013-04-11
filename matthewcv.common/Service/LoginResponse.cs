
using matthewcv.common.Entity;

namespace matthewcv.common.Service
{
    public class LoginResponse
    {
        public bool NewProfileCreated { get; set; }

        public Profile Profile { get; set; }
    }
}
