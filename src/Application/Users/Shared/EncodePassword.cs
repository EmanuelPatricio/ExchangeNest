namespace Application.Users.Shared;
public static class EncodePassword
{
    //code from https://www.c-sharpcorner.com/blogs/how-to-encrypt-or-decrypt-password-using-asp-net-with-c-sharp1
    public static string EncodeToBase64(string plainText)
    {
        var encData_byte = System.Text.Encoding.UTF8.GetBytes(plainText);

        var encodedData = Convert.ToBase64String(encData_byte);
        return encodedData;
    }

    public static string DecodeFrom64(string encodedData)
    {
        var encoder = new System.Text.UTF8Encoding();
        var utf8Decode = encoder.GetDecoder();

        var todecode_byte = Convert.FromBase64String(encodedData);
        var charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        var decoded_char = new char[charCount];

        utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);

        var result = new string(decoded_char);
        return result;
    }
}
