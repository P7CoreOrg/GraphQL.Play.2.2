using System;
using System.Collections.Generic;

namespace AppIdentity.Models
{
    public class AppIdentityCreateInputModel
    {
        /// <summary>
        /// The AppId
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// The Machine Id
        /// </summary>
        public string MachineId { get; set; }
        /// <summary>
        /// The Subject, must be authenticated
        /// </summary>
        public string Subject { get; set; }
    }
}