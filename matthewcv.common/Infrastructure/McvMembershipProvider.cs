using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using matthewcv.common.Service;
using Ninject;

namespace matthewcv.common.Infrastructure
{
    public class McvMembershipProvider:MembershipProvider
    {




        IAuthenticationService _authenticationService;
        public McvMembershipProvider()
        {
            _authenticationService = KernelProvider.Kernel.Get<IAuthenticationService>();
        }



        public override string ApplicationName
        {
            get
            {
                return "mywebsite";
            }
            set
            {
                
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException("15");
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException("17");
        }

        public override System.Web.Security.MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            throw new NotImplementedException("18");
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException("19");
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException("20"); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException("21"); }
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("22");
        }

        public override System.Web.Security.MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("23");
        }

        public override System.Web.Security.MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException("24");
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException("25");
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException("26");
        }

        public override System.Web.Security.MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException("27");
        }

        public override System.Web.Security.MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException("28");
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException("29");
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException("30"); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException("31"); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException("32"); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException("33"); }
        }

        public override System.Web.Security.MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException("34"); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException("35"); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException("36"); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException("37"); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException("38");
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException("39");
        }

        public override void UpdateUser(System.Web.Security.MembershipUser user)
        {
            throw new NotImplementedException("40");
        }

        public override bool ValidateUser(string username, string password)
        {
            throw new NotImplementedException("41");
        }
    }
}