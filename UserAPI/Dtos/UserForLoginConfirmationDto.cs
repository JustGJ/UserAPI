namespace UserAPI.Dtos
{
    public partial class UserForLoginConfirmationDto
    {
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public UserForLoginConfirmationDto()
        {
            if (PasswordHash == null)
            {
                PasswordHash = Array.Empty<byte>();
            }
            if (PasswordSalt == null)
            {
                PasswordSalt = Array.Empty<byte>();
            }
        }
    }
}
