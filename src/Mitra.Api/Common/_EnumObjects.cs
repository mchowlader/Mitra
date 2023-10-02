namespace Mitra.Api.Common
{
    public class _EnumObjects
    {
        public enum Role
        {
            SuperAdmin,
            Admin,
            User,
            Developer
        }
        public enum EmailOptions
        {
            NoEmail,
            Error,
            SuccessAndError
        }
        public enum HttpStatusCodes
        {
            OK = 200,
            Created = 201,
            Accepted = 202,
            NoContent = 204,
            Found = 302,
            BadRequest = 400,
            Unauthorized = 401,
            Forbidden = 403,
            NotFound = 404,
            InternalServerError = 500
        }
    }
}
