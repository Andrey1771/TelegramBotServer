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
    }

    public struct User : IComparable
    {
        public string firstName, lastName;
        public bool mailing;
        public int userId;

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

    public interface IBillyTelegramBot
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
