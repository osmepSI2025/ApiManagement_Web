namespace SME_WEB_ApiManagement.Models
{
    public class LineLoginSettings
    {
        public string ChannelId { get; set; }
        public string ChannelSecret { get; set; }
        public string CallbackUrl { get; set; }
    }
    public class Member
    {
        public int Id { get; set; }
        public string LineUserId { get; set; }
        public string DisplayName { get; set; }
        public string PictureUrl { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.Now;

        public string EmployeeId { get; set; }
    }

    // Models/LineAccessTokenResponse.cs (สำหรับ Response จาก LINE ตอนแลก Token)
    public class LineAccessTokenResponse
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Id_token { get; set; }
        public string Refresh_token { get; set; }
        public string Scope { get; set; }
        public string Token_type { get; set; }
    }

    // Models/LineProfileResponse.cs (สำหรับ Response จาก LINE ตอนดึงโปรไฟล์)
    public class LineProfileResponse
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string PictureUrl { get; set; }
        public string StatusMessage { get; set; }
    }
}
