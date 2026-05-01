using Microsoft.VisualStudio.TestTools.UnitTesting;
using TriQue.Data.Database;
using TriQue.Services;
using System;

namespace TriQue.Tests.Services
{
    // This class contains automated tests for the AuthenticationService.
    // Each [TestMethod] checks one specific scenario of logging in or account lockout.
    [TestClass]
    public class AuthenticationServiceTests
    {
        private AuthenticationService _auth;
        private DatabaseHelper _dbHelper;

        [TestInitialize]
        public void Setup()
        {
            // Before each test:
            // 1. Create a DatabaseHelper (handles SQLite connections).
            // 2. Re-seed the database with test users (driver1, driver2, etc.).
            // 3. Create a fresh AuthenticationService instance.
            _dbHelper = new DatabaseHelper();
            var dbInitializer = new DatabaseInitializer(_dbHelper);
            dbInitializer.Initialize();

            _auth = new AuthenticationService();
        }

        [TestMethod]
        public void Login_ShouldSucceed_WithValidCredentials()
        {
            // Logging in with valid username and password
            bool result = _auth.Login("driver1", "hash3", out string message);

            // Expect login to succeed (true) and return the success message
            Assert.IsTrue(result);
            Assert.AreEqual("Login successful!", message);
        }

        [TestMethod]
        public void Login_ShouldFail_WithInvalidUsername()
        {
            // Logging in with username that does not exist
            bool result = _auth.Login("unknownUser", "password", out string message);

            // Expect login to fail (false) and return invalid credentials message
            Assert.IsFalse(result);
            Assert.AreEqual("Invalid username or password.", message);
        }

        [TestMethod]
        public void Login_ShouldFail_WithWrongPassword_AndShowRemainingAttempts()
        {
            // Logging in with the correct username but wrong password
            bool result = _auth.Login("driver1", "wrong", out string message);

            // Expect login to fail (false) and show how many attempts remain
            Assert.IsFalse(result);
            StringAssert.Contains(message, "attempt(s) remaining");
        }

        [TestMethod]
        public void Login_ShouldLockAccount_AfterThreeFailedAttempts()
        {
            // Enter wrong password three times for driver2
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            // After 3 failed attempts, account is locked. 4th attempt should confirm it
            bool result = _auth.Login("driver2", "wrong", out string message);

            // Expect login to fail and the message to mention "locked"
            Assert.IsFalse(result);
            StringAssert.Contains(message, "locked");
        }

        [TestMethod]
        public void LockedAccount_ShouldNotLogin_EvenWithCorrectPassword()
        {
            // Lock driver2 by failing three times
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            // Logging in with the correct password while locked
            bool result = _auth.Login("driver2", "hash4", out string message);

            // Expect login to fail and the message to mention "locked"
            Assert.IsFalse(result);
            StringAssert.Contains(message, "locked");
        }

        [TestMethod]
        public void GetLockSecondsRemaining_ShouldReturnPositive_WhenLocked()
        {
            // Lock driver2 by failing three times
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);
            _auth.Login("driver2", "wrong", out _);

            // Ask how many seconds remain before the lock expires
            int seconds = _auth.GetLockSecondsRemaining("driver2");

            // Expect a positive number (countdown is running)
            Assert.IsTrue(seconds > 0);
        }

        [TestMethod]
        public void Audit_ShouldLogSuccess_OnValidLogin()
        {
            // Perform a successful login with driver3
            _auth.Login("driver3", "hash5", out _);

            // Query the latest log entry for driver3 (UserID=5)
            var outcome = _dbHelper.ExecuteScalar(
                "SELECT AuthOutcome FROM AuthenticationLog WHERE UserID=5 ORDER BY LogID DESC LIMIT 1"
            );

            // Expect the audit trail to record "Success"
            Assert.AreEqual("Success", outcome);
        }

        [TestMethod]
        public void Audit_ShouldLogFailed_OnWrongPassword()
        {
            // Attempt login with driver4 but wrong password
            _auth.Login("driver4", "wrong", out _);

            // Query the latest log entry for driver4
            var outcome = _dbHelper.ExecuteScalar(
                "SELECT AuthOutcome FROM AuthenticationLog WHERE UserID=6 ORDER BY LogID DESC LIMIT 1"
            );

            // Expect the audit trail to record "Failed"
            Assert.AreEqual("Failed", outcome);
        }

        [TestMethod]
        public void Audit_ShouldLogLocked_OnAccountLock()
        {
            // Fail login three times for driver5 to trigger lockout
            _auth.Login("driver5", "wrong", out _);
            _auth.Login("driver5", "wrong", out _);
            _auth.Login("driver5", "wrong", out _);

            // Query the latest log entry for driver5 (UserID=7)
            var outcome = _dbHelper.ExecuteScalar(
                "SELECT AuthOutcome FROM AuthenticationLog WHERE UserID=7 ORDER BY LogID DESC LIMIT 1"
            );

            // Expect the audit trail to record "Locked"
            Assert.AreEqual("Locked", outcome);
        }

        [TestMethod]
        public void Audit_ShouldLogLockedAttempt_WhenTryingWhileLocked()
        {
            // Lock driver6 by failing three times
            _auth.Login("driver6", "wrong", out _);
            _auth.Login("driver6", "wrong", out _);
            _auth.Login("driver6", "wrong", out _);

            // Attempt login with correct password while locked
            _auth.Login("driver6", "hash8", out _);

            // Query the latest log entry for driver6
            var outcome = _dbHelper.ExecuteScalar(
                "SELECT AuthOutcome FROM AuthenticationLog WHERE UserID=8 ORDER BY LogID DESC LIMIT 1"
            );

            // Expect the audit trail to record "Locked Attempt"
            Assert.AreEqual("Locked Attempt", outcome);
        }
        [TestMethod]
        public void GetLockSecondsRemaining_ShouldReturnZero_WhenNotLocked()
        {
            // Ask for lock time on driver7 (never locked)
            int seconds = _auth.GetLockSecondsRemaining("driver7");

            // Expect zero because the account is not locked
            Assert.AreEqual(0, seconds);
        }
    }
}