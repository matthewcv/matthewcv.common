using System;

namespace matthewcv.common.Entity
{
    /// <summary>
    /// base class for entities.  You don't need to set any of these properties, they're set automatically by stuff.
    /// </summary>
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SiteKey { get; set; }
    }
}
