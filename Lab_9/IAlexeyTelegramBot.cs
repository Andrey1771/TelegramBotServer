using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab_9
{
    public struct Settings
    {
        public bool isLoadData, isSendData, isRegistrationNewUsers;
        public Settings(bool aisLoadData = true, bool aisSendData = true, bool aisRegistrationNewUsers = true)
        {
            isLoadData = aisLoadData;
            isSendData = aisSendData;
            isRegistrationNewUsers = aisRegistrationNewUsers;
        }
        public Settings(Settings asettings)
        {
            isLoadData = asettings.isLoadData;
            isSendData = asettings.isSendData;
            isRegistrationNewUsers = asettings.isRegistrationNewUsers;
        }
    }

    public struct User : IComparable
    {
        public string firstName, lastName;
        public bool mailing;
        public int userId;

        public string FirstName { get { return firstName; } set { firstName = value; } }
        public string LastName { get { return lastName; } set { lastName = value; } }
        public bool Mailing { get { return mailing; } set { mailing = value; } }
        public int UserId { get { return userId; } set { userId = value; } }

        public User(int auserId, string afirstName = "", string alastName = "", bool amailing = false)
        {
            userId = auserId;
            firstName = afirstName;
            lastName = alastName;
            mailing = amailing;
        }

        public User(User user)
        {
            userId = user.userId;
            firstName = user.firstName;
            lastName = user.lastName;
            mailing = user.mailing;
        }

        public int CompareTo(object obj)
        {
            User? user = obj as User?;
            if (user != null)
                return userId.CompareTo(user?.userId);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }
    }

    public interface IAlexeyTelegramBot
    {
        bool Enable { get; }
        Settings Settings { get; set; }
        ICollection<User> Users { get; }
        ICollection Logs { get; }

        void StartBot();

        void StopBot();

        void SendAllMessage(string message);
    }
}
