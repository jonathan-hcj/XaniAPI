using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XaniAPI.Entites
{
    public class Like
    {
        [Key]
        public Int64 l_id { get; set; }
        public Int64 l_p_id { get; set; }
        public Int32 l_u_id { get; set; }
        public Int16 l_ls_id { get; set; }
    }
}
