using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TriQue.Data.Database;
using TriQue.Helpers;
using TriQue.Services;

namespace TriQue.Tests.Services
{
    [TestClass]
    public class AuthenticationServiceTests
    {
        private AuthenticationService _auth;
        private DatabaseHelper _dbHelper;

        [TestInitialize]
        public void Setup()
        {
            _dbHelper = new DatabaseHelper();

            // test seed passwords
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "SeedPasswords:AdminDefault", "admin123" },
                    { "SeedPasswords:DriverDefault", "driver123" }
                })
                .Build();

            // initialize test database
            var dbInitializer = new DatabaseInitializer(_dbHelper, config);
            dbInitializer.Initialize();

            _auth = new AuthenticationService();
        }

        [TestMethod]
        public void Login_ShouldSucceed_WithValidCredentials()
        {
            // valid login
            bool result = _auth.Login("driver1", "driver123", out string message);

            Console.WriteLine(message);

            Assert.IsTrue(result, message);
            Assert.AreEqual("Login successful!", message);
        }

        [TestMethod]
        public void Login_ShouldFail_WithInvalidUsername()
        {
            // invalid username
            bool result = _auth.Login("unknownUser", "password", out string message);

            Assert.IsFalse(result);
            Assert.AreEqual("Invalid username or password.", message);
        }

        [TestMethod]
        public void Login_ShouldFail_WithWrongPassword_AndShowRemainingAttempts()
        {
            // wrong password
            bool result = _auth.Login("driver1", "wrong", out string message);

            Assert.IsFalse(result);
            StringAssert.Contains(message, "attempt(s) remaining");
        }

        [TestMethod]
        public void Login_ShouldLockAccount_AfterThreeFailedAttempts()
        {
            // trigger account lock
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            bool result = _auth.Login("driver2", "wrong", out string message);

            Assert.IsFalse(result);
            StringAssert.Contains(message, "locked");
        }

        [TestMethod]
        public void LockedAccount_ShouldNotLogin_EvenWithCorrectPassword()
        {
            // lock account first
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            // try correct password while locked
            bool result = _auth.Login("driver2", "driver123", out string message);

            Assert.IsFalse(result);
            StringAssert.Contains(message, "locked");
        }

        [TestMethod]
        public void GetLockSecondsRemaining_ShouldReturnPositive_WhenLocked()
        {
            // lock account
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            int seconds = _auth.GetLockSecondsRemaining("driver2");

            Assert.IsTrue(seconds > 0);
        }

        [TestMethod]
        public void Audit_ShouldLogSuccess_OnValidLogin()
        {
            // successful login audit
            bool result = _auth.Login("driver1", "driver123", out string message);

            Console.WriteLine(message);

            Assert.IsTrue(result, message);
            Assert.AreEqual("Login successful!", message);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog"
            );

            Assert.IsTrue(Convert.ToInt32(count) > 0,
                "Audit log should record a success entry");
        }

        [TestMethod]
        public void Audit_ShouldLogFailed_OnWrongPassword()
        {
            // failed login audit
            _auth.Login("driver1", "wrong", out string message);

            StringAssert.Contains(message, "attempt(s) remaining");

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog"
            );

            Assert.IsTrue(Convert.ToInt32(count) > 0,
                "Audit log should record a failed attempt");
        }

        [TestMethod]
        public void Audit_ShouldLogLocked_OnAccountLock()
        {
            // lock account
            _auth.Login("driver3", "wrong", out _);
            _auth.Login("driver3", "wrong", out _);
            _auth.Login("driver3", "wrong", out _);

            _auth.Login("driver3", "wrong", out string message);

            StringAssert.Contains(message, "locked");

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog"
            );

            Assert.IsTrue(Convert.ToInt32(count) > 0,
                "Audit log should record a lock event");
        }

        [TestMethod]
        public void Audit_ShouldLogLockedAttempt_WhenTryingWhileLocked()
        {
            // lock account first
            _auth.Login("driver4", "wrong", out _);
            _auth.Login("driver4", "wrong", out _);
            _auth.Login("driver4", "wrong", out _);

            // try login while locked
            _auth.Login("driver4", "driver123", out string message);

            StringAssert.Contains(message, "locked");

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog"
            );

            Assert.IsTrue(Convert.ToInt32(count) > 0,
                "Audit log should record a locked attempt");
        }

        [TestMethod]
        public void GetLockSecondsRemaining_ShouldReturnZero_WhenNotLocked()
        {
            // unlocked account
            int seconds = _auth.GetLockSecondsRemaining("driver7");

            Assert.AreEqual(0, seconds);
        }
    }
}