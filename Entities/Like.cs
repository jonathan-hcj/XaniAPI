using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XaniAPI.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Like
    {
        [Key]
        public long l_id { get; set; }
        public long l_p_id { get; set; }
        public int l_u_id { get; set; }
        public short l_ls_id { get; set; }
    }
}
