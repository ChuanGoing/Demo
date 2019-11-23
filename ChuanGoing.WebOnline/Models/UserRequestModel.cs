using System.ComponentModel.DataAnnotations;

namespace ChuanGoing.WebOnline.Models
{
    public class UserRequestModel
    {
        [Required(ErrorMessage = "用户名称不可以为空")]
        public string Name { get; set; }

        [Required(ErrorMessage = "用户密码不可以为空")]
        public string Password { get; set; }

        public string Type { get; set; }
        public string Code { get; set; }
    }
}
