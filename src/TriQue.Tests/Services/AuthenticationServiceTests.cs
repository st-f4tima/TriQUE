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

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "SeedPasswords:AdminDefault", "admin123" },
                    { "SeedPasswords:DriverDefault", "driver123" }
                })
                .Build();

            var dbInitializer = new DatabaseInitializer(_dbHelper, config);
            dbInitializer.Initialize();

            _auth = new AuthenticationService();
        }

        [TestMethod]
        public void Login_ShouldSucceed_WithValidCredentials()
        {
            bool result = _auth.Login("driver1", "driver123", out string message);

            Assert.IsTrue(result);
            Assert.AreEqual("Login successful!", message);
        }

        [TestMethod]
        public void Login_ShouldFail_WithInvalidUsername()
        {
            bool result = _auth.Login("unknownUser", "password", out string message);

            Assert.IsFalse(result);
            Assert.AreEqual("Invalid username or password.", message);
        }

        [TestMethod]
        public void Login_ShouldFail_WithWrongPassword_AndShowRemainingAttempts()
        {
            bool result = _auth.Login("driver1", "wrong", out string message);

            Assert.IsFalse(result);
            StringAssert.Contains(message, "attempt(s) remaining");
        }

        [TestMethod]
        public void Login_ShouldLockAccount_AfterThreeFailedAttempts()
        {
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
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            bool result = _auth.Login("driver2", "driver123", out string message);

            Assert.IsFalse(result);
            StringAssert.Contains(message, "locked");
        }

        [TestMethod]
        public void GetLockSecondsRemaining_ShouldReturnPositive_WhenLocked()
        {
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            int seconds = _auth.GetLockSecondsRemaining("driver2");

            Assert.IsTrue(seconds > 0);
        }

        [TestMethod]
        public void Audit_ShouldLogSuccess_OnValidLogin()
        {
            bool result = _auth.Login("driver3", "driver123", out string message);

            Assert.IsTrue(result);
            Assert.AreEqual("Login successful!", message);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog WHERE UserID=5"
            );
            Assert.IsTrue(Convert.ToInt32(count) > 0);
        }

        [TestMethod]
        public void Audit_ShouldLogFailed_OnWrongPassword()
        {
            _auth.Login("driver4", "wrong", out string message);

            Assert.AreEqual("Invalid username or password.", message);

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog WHERE UserID=6"
            );
            Assert.IsTrue(Convert.ToInt32(count) > 0);
        }

        [TestMethod]
        public void Audit_ShouldLogLocked_OnAccountLock()
        {
            _auth.Login("driver5", "wrong", out _);
            _auth.Login("driver5", "wrong", out _);
            _auth.Login("driver5", "wrong", out string message);

            Assert.IsFalse(_auth.Login("driver5", "wrong", out message));
            StringAssert.Contains(message, "locked");

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog WHERE UserID=7"
            );
            Assert.IsTrue(Convert.ToInt32(count) > 0);
        }

        [TestMethod]
        public void Audit_ShouldLogLockedAttempt_WhenTryingWhileLocked()
        {
            _auth.Login("driver6", "wrong", out _);
            _auth.Login("driver6", "wrong", out _);
            _auth.Login("driver6", "wrong", out _);

            _auth.Login("driver6", "driver123", out string message);

            StringAssert.Contains(message, "locked");

            var count = _dbHelper.ExecuteScalar(
                "SELECT COUNT(*) FROM AuthenticationLog WHERE UserID=8"
            );
            Assert.IsTrue(Convert.ToInt32(count) > 0);
        }

        [TestMethod]
        public void GetLockSecondsRemaining_ShouldReturnZero_WhenNotLocked()
        {
            int seconds = _auth.GetLockSecondsRemaining("driver7");

            Assert.AreEqual(0, seconds);
        }
    }
}